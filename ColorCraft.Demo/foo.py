import math

# Constants
TwoPi = 2.0 * math.pi
kaleidoscopeCount = 3.5  # Non-integer value for demonstration
modulusAngle = TwoPi / kaleidoscopeCount

# Function to calculate the progress value for a given point
def calculate_direct_progress(x, y, modulusAngle):
    angle = math.atan2(y, x) + math.pi
    print(angle)
    angle %= modulusAngle
    progress = angle / modulusAngle
    return progress

# Bitmap size and center
centerX = 150
centerY = 150

# Points along the positive x-axis near the center y-axis
points = [(299, y) for y in range(146, 155)]

# Calculating the progress values for the points
direct_progress_values_300_updated = [calculate_direct_progress(x - centerX, y - centerY, modulusAngle) for x, y in points]
print(direct_progress_values_300_updated)
