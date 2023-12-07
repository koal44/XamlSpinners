from cv2 import cvtColor, COLOR_RGB2Lab
import numpy as np


# List of RGB values from the provided data
rgb_values = [
    [0, 0, 0],        # Black
    [255, 255, 255],  # White
    [255, 0, 0],      # Red
    [0, 255, 0],      # Green
    [0, 0, 255],      # Blue
    [255, 255, 0],    # Yellow
    [61, 41, 41],
    [191, 64, 64],
    [245, 163, 163],
    [61, 61, 41],
    [191, 191, 64],
    [245, 245, 163],
    [41, 61, 41],
    [64, 191, 64],
    [163, 245, 163],
    [41, 61, 61],
    [64, 191, 191],
    [163, 245, 245],
    [41, 41, 61],
    [64, 64, 191],
    [163, 163, 245],
    [61, 41, 61],
    [191, 64, 191],
    [245, 163, 245],
    [0, 0, 0],        # Black (due to lightness = 0)
]

# Function to convert RGB to 32-bit Lab
# Adjusting the function to output the results in the format (r, g, b, l, a, b) and formatting the decimals
def rgb_to_lab_formatted(rgb_list):
    formatted_lab_values = []

    for bgr in rgb_list:
        # Convert RGB to LAB using OpenCV with 32-bit float
        foo = np.uint8([[bgr]])
        lab = cvtColor(foo, COLOR_RGB2Lab)

        # Convert the 8-bit LAB to 32-bit values as per OpenCV's scaling
        lab_32 = lab[0][0].astype(np.float32)
        lab_32[0] = lab_32[0] * 100 / 255  # L from [0, 255] to [0, 100]
        lab_32[1] = lab_32[1] - 128        # a from [0, 255] to [-127, 128]
        lab_32[2] = lab_32[2] - 128        # b from [0, 255] to [-127, 128]

        # Formatting the LAB values and appending them with RGB values
        l, a, b = lab_32
        formatted_lab_values.append((bgr[0], bgr[1], bgr[2], f"{l:.2f}", f"{a:.2f}", f"{b:.2f}"))

    return formatted_lab_values

# Convert the list of RGB colors to formatted LAB color space
formatted_lab = rgb_to_lab_formatted(rgb_values)
formatted_lab
