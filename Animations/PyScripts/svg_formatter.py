import re

# Original path data
path_data = "m 122.87844,114.48724 h -19.99258 v -3.77031 h 7.68945 V 98.339392 92.150623 89.056238 85.961853 h -3.84473 -1.92236 -0.96118 -0.96118 v -3.373437 c 1.0418,0 2.15801,-0.08268 3.34863,-0.248047 1.19063,-0.181901 2.09186,-0.438216 2.70371,-0.768945 0.76068,-0.413411 1.35599,-0.93431 1.78594,-1.562695 0.44649,-0.644922 0.7028,-1.504818 0.76895,-2.579688 h 3.84472 v 8.321972 4.160986 4.160987 4.160987 4.160987 4.16099 4.16098 h 7.54063 z"

# Function to split chained commands in the SVG path data
def split_chained_commands(data):
    # Replace commas with spaces to standardize the separation
    data = data.replace(',', ' ')

    # Split the data into commands and their parameters
    commands = re.findall(r'([mzlhvctsq])([^mzlhvctsq]*)', data, re.I)

    new_path = []
    for command, params in commands:
        params = ' '.join(params.split())
        coords = params.strip().split()

        if command.lower() in 'vh':
            for coord in coords:
                new_path.append(command + ' ' + coord)
        elif command.lower() in 'lt':
            for i in range(0, len(coords), 2):
                new_path.append(command + ' ' + ' '.join(coords[i:i + 2]))
        elif command.lower() in 'qs':
            for i in range(0, len(coords), 4):
                new_path.append(command + ' ' + ' '.join(coords[i:i + 4]))
        elif command.lower() in 'c':
            for i in range(0, len(coords), 6):
                new_path.append(command + ' ' + ' '.join(coords[i:i + 6]))
        else: # m, a, z
            new_path.append(command + ' ' + params.strip())

    return ' '.join(new_path)

split_path_data = split_chained_commands(path_data)
#print(split_path_data)

def convert_to_relative2(data):
    svg_commands = ['m','M','l','L','h','H','v','V','c','C','s','S','q','Q','t','T','a','A','z','Z']
    pattern = '|'.join(f'(?={command})' for command in svg_commands)
    split_data = re.split(pattern, data)
    split_data = list(filter(None, split_data))

    current_pos = [0, 0]
    result = ""

    for item in split_data:
        command = item[0]
        params = item[1:].strip().split()

        # Assert that there are no chained commands in the data
        # Handle line, cubic, and quadratic Bézier curves
        if command in 'LCSQT':
            new_coords = []
            for i in range(0, len(params), 2):
                x, y = map(float, params[i:i + 2])
                rx, ry = x - current_pos[0], y - current_pos[1]
                new_coords.append(f'{rx},{ry}')
                if i == len(params) - 2:  # Update current position at the end point
                    current_pos = [x, y]
            result += command.lower() + ' ' + ' '.join(new_coords) + ' '

        # Handle elliptical arc commands
        elif command in 'A':
            # Only the last two parameters (end point) are converted
            new_coords = params[:-2]
            x, y = map(float, params[-2:])
            rx, ry = x - current_pos[0], y - current_pos[1]
            new_coords.append(f'{rx},{ry}')
            current_pos = [x, y]
            result += 'a ' + ' '.join(new_coords) + ' '

        elif command in 'Hh':
            new_coords = []
            for p in params:
                val = float(p)
                if command == 'H':  # Absolute horizontal line
                    relative_val = val - current_pos[0]
                    current_pos[0] = val
                else:  # Relative horizontal line
                    relative_val = val
                    current_pos[0] += val
                new_coords.append(str(relative_val))
            result += 'h ' + ' '.join(new_coords) + ' '

        elif command in 'Vv':
            new_coords = []
            for p in params:
                val = float(p)
                if command == 'V':  # Absolute vertical line
                    relative_val = val - current_pos[1]
                    current_pos[1] = val
                else:  # Relative vertical line
                    relative_val = val
                    current_pos[1] += val
                new_coords.append(str(relative_val))
            result += 'v ' + ' '.join(new_coords) + ' '

        elif command in 'zZ':
            result += command + ' '

        elif command.islower():
            if params:
                last_coord = params[-1].split(',')
                current_pos[0] += float(last_coord[0])
                if len(last_coord) > 1:
                    current_pos[1] += float(last_coord[1])
            result += command + ' ' + ' '.join(params) + ' '

        else:
            if params:
                last_coord = params[-1].split(',')
                current_pos = [float(last_coord[0]), float(last_coord[1]) if len(last_coord) > 1 else current_pos[1]]
            result += command + ' ' + ' '.join(params) + ' '

    return result


def convert_to_relative(data):
    data = data.replace(',', ' ')
    current_pos = [0, 0]
    result = ""

    commands = re.findall(r'([mzlhvctsq])([^mzlhvctsq]*)', data, re.I)
    for command, params in commands:
        params = params.strip().split()
        print(f"Command: {command}, Params: {params}")

        # Handle line, cubic, and quadratic Bézier curves
        # Assert that there are no chained commands in the data
        if command in 'LCSQT':
            new_coords = []
            for i in range(0, len(params), 2):
                x, y = map(float, params[i:i + 2])
                rx, ry = x - current_pos[0], y - current_pos[1]
                new_coords.append(f'{rx} {ry}')
                if i == len(params) - 2:  # Update current position at the end point
                    current_pos = [x, y]
            result += command.lower() + ' ' + ' '.join(new_coords) + ' '

        # Handle elliptical arc commands
        elif command in 'A':
            # Convert elliptical arc commands to relative
            # Only the last two parameters (end point) are converted
            new_coords = params[:-2]
            x, y = map(float, params[-2:])
            rx, ry = x - current_pos[0], y - current_pos[1]
            new_coords.append(f'{rx} {ry}')
            current_pos = [x, y]
            result += 'a ' + ' '.join(new_coords) + ' '

        elif command in 'Hh':
            # Convert horizontal line commands to relative
            val = float(params[0])
            if command == 'H':  # # Absolute horizontal line
                relative_val = val - current_pos[0]
                current_pos[0] = val
            else:  # Relative horizontal line
                relative_val = val
                current_pos[0] += val
            result += 'h ' + str(relative_val) + ' '

        elif command in 'Vv':
            # Convert vertical line commands to relative
            val = float(params[0])
            if command == 'V':  # Absolute vertical line
                relative_val = val - current_pos[1]
                current_pos[1] = val
            else:  # Relative vertical line
                relative_val = val
                current_pos[1] += val
            result += 'v ' + str(relative_val) + ' '

        elif command in 'zZ':
            # Close path command
            result += command + ' '

        elif command.islower():
            # Lowercase commands are relative, update current position
            if params:
                current_pos[0] += float(params[-2])
                current_pos[1] += float(params[-1])
            result += command + ' ' + ' '.join(params) + ' '

        else:
            # Uppercase commands are absolute, set current position
            if params:
                current_pos = [float(params[-2]), float(params[-1])]
            result += command + ' ' + ' '.join(params) + ' '

    return result



# Test the function with the provided path data
converted_path = convert_to_relative(split_path_data)


# Function to round numbers in the path data
def round_path_data(data, precision=1):
    # Function to round a matched number
    def round_number(match):
        # Round the number to the specified precision
        return format(float(match.group()), f".{precision}f")

    # Use regular expression to find and round each number
    rounded_data = re.sub(r"[-+]?[0-9]*\.?[0-9]+", round_number, data)

    return rounded_data


# Apply rounding to the path data
rounded_path_data = round_path_data(converted_path)
print(rounded_path_data)


