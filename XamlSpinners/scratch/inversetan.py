import matplotlib.pyplot as plt
import numpy as np

# Define the range for x (field of view in radians) from 30 to 180 degrees
x = np.linspace(np.deg2rad(30), np.pi, 400)  # 30 degrees to 180 degrees in radians

# Calculate y as tan(fov/2)
y = np.power(np.tan(x * 0.5), -1)

# Plotting the graph
plt.figure(figsize=(10, 6))
plt.plot(np.rad2deg(x), y)  # Converting radians back to degrees for the x-axis using np.rad2deg
plt.title('Plot of tan(FOV/2) for FOV between 30 and 180 Degrees')
plt.xlabel('Field of View (Degrees)')
plt.ylabel('tan(FOV/2)')
plt.grid(True)
plt.show()
