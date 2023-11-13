import math
import matplotlib.pyplot as plt

# Function to calculate the line equation (m, b) from two points
def line_equation(p1, p2):
    m = (p2[1] - p1[1]) / (p2[0] - p1[0])
    b = (p1[0] * p2[1] - p2[0] * p1[1]) / (p1[0] - p2[0])
    return m, b

# Function to find the intersection point of two lines
def intersection(m1, b1, m2, b2):
    x = (b2 - b1) / (m1 - m2)
    y = (b1 * m2 - b2 * m1) / (m2 - m1)
    return x, y

# Define the function to adjust points for WPF and format them as a string
def wpf_print(star_points, offset=50):
    adjusted_points = [(x + offset, offset - y) for x, y in star_points]
    print( " ".join([f"{x:.2f},{y:.2f}" for x, y in adjusted_points]) )

# Define the points on the circle
radius = 50
p1 = (radius * math.cos(math.radians(90)), radius * math.sin(math.radians(90)))
p2 = (radius * math.cos(math.radians(90 + 72)), radius * math.sin(math.radians(90 + 72)))
p3 = (radius * math.cos(math.radians(90 + 2 * 72)), radius * math.sin(math.radians(90 + 2 * 72)))
p4 = (radius * math.cos(math.radians(90 + 3 * 72)), radius * math.sin(math.radians(90 + 3 * 72)))
p5 = (radius * math.cos(math.radians(90 + 4 * 72)), radius * math.sin(math.radians(90 + 4 * 72)))

# Calculate line equations
m_p1p3, b_p1p3 = line_equation(p1, p3)
m_p1p4, b_p1p4 = line_equation(p1, p4)
m_p2p4, b_p2p4 = line_equation(p2, p4)
m_p2p5, b_p2p5 = line_equation(p2, p5)
m_p3p5, b_p3p5 = line_equation(p3, p5)

# Calculate intersection points
q1 = intersection(m_p1p3, b_p1p3, m_p2p5, b_p2p5)
q2 = intersection(m_p1p3, b_p1p3, m_p2p4, b_p2p4)
q3 = intersection(m_p3p5, b_p3p5, m_p2p4, b_p2p4)
q4 = intersection(m_p3p5, b_p3p5, m_p1p4, b_p1p4)
q5 = intersection(m_p2p5, b_p2p5, m_p1p4, b_p1p4)

# Plotting the outer points (P) and inner points (Q) of the star
plt.figure(figsize=(8, 8))
plt.scatter(*zip(*[p1, p2, p3, p4, p5]), color='blue', label='Outer Points (P)')
plt.scatter(*zip(*[q1, q2, q3, q4, q5]), color='red', label='Inner Points (Q)')

# Drawing lines for the star shape
star_points = [p1, q1, p2, q2, p3, q3, p4, q4, p5, q5, p1]
plt.plot(*zip(*star_points), color='black')
#print([f"({x:.2f}, {y:.2f})" for x, y in star_points])

# Print the points for WPF
wpf_formatted_points = wpf_print([p1, q1, p2, q2, p3, q3, p4, q4, p5, q5])
wpf_formatted_points

wpf_formatted_inverted_points = wpf_print([(-x, -y) for x, y in [p1, q1, p2, q2, p3, q3, p4, q4, p5, q5]])
wpf_formatted_inverted_points

# Plot settings
plt.xlabel('X-axis')
plt.ylabel('Y-axis')
plt.axhline(0, color='black',linewidth=0.5)
plt.axvline(0, color='black',linewidth=0.5)
plt.grid(color = 'gray', linestyle = '--', linewidth = 0.5)
plt.legend()
plt.title('5-Pointed Star with Inner and Outer Points')
plt.gca().set_aspect('equal', adjustable='box')  # Equal aspect ratio
plt.show()
