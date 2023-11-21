using System;
using System.Windows.Media;

namespace ProjectionCanvas
{
    public static class Utils
    {
        static public Color GetGradientColor(Color startColor, Color endColor, double relativePosition, bool useStartAlphaOnly = true)
        {
            // Check if position is valid
            if (relativePosition < 0 || relativePosition > 1 || double.IsNaN(relativePosition))
                return startColor;

            // Calculate gradient positions for Red, Green, and Blue colors
            var alpha = EnsureInRange(startColor.A + (endColor.A - startColor.A) * relativePosition);
            var red = EnsureInRange(startColor.R + (endColor.R - startColor.R) * relativePosition);
            var green = EnsureInRange(startColor.G + (endColor.G - startColor.G) * relativePosition);
            var blue = EnsureInRange(startColor.B + (endColor.B - startColor.B) * relativePosition);

            // Return the gradient color at the specified position
            return useStartAlphaOnly
                ? Color.FromArgb(startColor.A, red, green, blue)
                : Color.FromArgb(alpha, red, green, blue);

            // Local function to ensure values are within range
            static byte EnsureInRange(double value) => (byte)Math.Min(Math.Max(value, 0), 255);
        }

        internal const double DBL_EPSILON = 2.2204460492503131e-016; /* smallest such that 1.0+DBL_EPSILON != 1.0 */
        internal const float FLT_MIN = 1.175494351e-38F; /* Number close to zero, where float.MinValue is -float.MaxValue */

        public static bool IsZero(double value)
        {
            return Math.Abs(value) < 10.0 * DBL_EPSILON;
        }
    }
}
