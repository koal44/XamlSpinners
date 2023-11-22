import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import numpy as np

# Function to compute the point and vector at given t and w
def compute_point_and_vector(t, w, r=1):
    sin_t = np.sin(np.radians(t))
    cos_t = np.cos(np.radians(t))
    sin_w = np.sin(np.radians(w))
    cos_w = np.cos(np.radians(w))

    # Calculate the point on the sphere
    point_x = r * cos_t * sin_w
    point_y = r * sin_t * sin_w
    point_z = r * cos_w

    # Calculate the vector
    vector_x = r * cos_t * cos_w
    vector_y = r * sin_t * cos_w
    vector_z = -r * sin_w

    return (point_x, point_y, point_z), (vector_x, vector_y, vector_z)

# Generate points and vectors for different t and w values
points, vectors = [], []
for w in range(0, 181, 30):
    for t in range(0, 361, 30):
        point, vector = compute_point_and_vector(t, w)
        points.append(point)
        vectors.append(vector)

# Extract point and vector components
points_x, points_y, points_z = zip(*points)
vectors_x, vectors_y, vectors_z = zip(*vectors)

# Plotting
fig = plt.figure(figsize=(10, 8))
ax = fig.add_subplot(111, projection='3d')

# Draw the unit sphere
u_sphere = np.linspace(0, 2 * np.pi, 50)
v_sphere = np.linspace(0, np.pi, 50)
x_sphere = np.outer(np.cos(u_sphere), np.sin(v_sphere))
y_sphere = np.outer(np.sin(u_sphere), np.sin(v_sphere))
z_sphere = np.outer(np.ones(np.size(u_sphere)), np.cos(v_sphere))

ax.plot_surface(x_sphere, y_sphere, z_sphere, color='c', alpha=0.1)

# Plot the vectors at the specified points
ax.quiver(points_x, points_y, points_z, vectors_x, vectors_y, vectors_z, color='blue', length=0.1, normalize=True)

ax.set_xlabel('X axis')
ax.set_ylabel('Z axis')
ax.set_zlabel('Y axis')
ax.set_title('3D Vectors on Unit Sphere')
plt.show()

