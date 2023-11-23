using System;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media.Media3D;

namespace XamlSpinners.Utils
{
    public static class ColorUtils
    {
        // Given a color with `hue` ∈ [0°, 360°), `saturation` ∈ [0, 1], and `lightness` ∈ [0, 1]
        // from https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_RGB
        public static Color HslToRgb(double hue, double saturation, double lightness)
        {
            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double hueSection = hue / 60.0;
            double x = chroma * (1 - Math.Abs(hueSection % 2 - 1));
            double m = lightness - chroma / 2;

            (double r, double g, double b) rgb = hueSection switch
            {
                < 1 => (chroma, x, 0),
                < 2 => (x, chroma, 0),
                < 3 => (0, chroma, x),
                < 4 => (0, x, chroma),
                < 5 => (x, 0, chroma),
                < 6 => (chroma, 0, x),
                _ => throw new ArgumentOutOfRangeException(nameof(hue))
            };

            return Color.FromRgb(
                (byte)((rgb.r + m) * 255),
            (byte)((rgb.g + m) * 255),
                (byte)((rgb.b + m) * 255));
        }

        // Given a color with `hue` ∈ [0°, 360°), `saturation` ∈ [0, 100], and `lightness` ∈ [0, 100]
        // from https://www.w3.org/TR/css-color-3/#hsl-color
        public static Color HslToRgb2(double hue, double saturation, double lightness)
        {
            hue %= 360;
            hue = hue < 0 ? hue + 360 : hue;
            saturation /= 100;
            lightness /= 100;

            double f(double n)
            {
                double k = (n + hue / 30) % 12;
                double a = saturation * Math.Min(lightness, 1 - lightness);
                return lightness - a * Math.Clamp(Math.Min(k - 3, 9 - k), -1, 1);
            }

            return Color.FromRgb(
                (byte)(f(0) * 255),
                (byte)(f(8) * 255),
                (byte)(f(4) * 255));
        }






    }
}
