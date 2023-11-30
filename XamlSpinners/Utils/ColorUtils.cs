using System;
using System.Windows.Media;

namespace XamlSpinners.Utils
{
    public static class ColorUtils
    {
        /// <summary>
        /// Converts HSL to RGB color format.
        /// </summary>
        /// <param name="hue">Hue in degrees [0°, 360°).</param>
        /// <param name="saturation">Saturation [0, 1].</param>
        /// <param name="lightness">Lightness [0, 1].</param>
        /// <returns>RGB color.</returns>
        /// <remarks> https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_RGB </remarks>
        public static Color HslToRgb(double hue, double saturation, double lightness)
        {
            if (hue < 0 || hue > 360) throw new ArgumentOutOfRangeException(nameof(hue));
            if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException(nameof(saturation));
            if (lightness < 0 || lightness > 1) throw new ArgumentOutOfRangeException(nameof(lightness));

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

        /// <summary>
        /// Alternative method to convert HSL to RGB based on W3C standard.
        /// </summary>
        /// <param name="hue">Hue in degrees [0°, 360°).</param>
        /// <param name="saturation">Saturation [0, 100].</param>
        /// <param name="lightness">Lightness [0, 100].</param>
        /// <returns>RGB color.</returns>
        /// <remarks> https://www.w3.org/TR/css-color-3/#hsl-color </remarks>
        public static Color HslToRgb2(double hue, double saturation, double lightness)
        {
            if (saturation < 0 || saturation > 100) throw new ArgumentOutOfRangeException(nameof(saturation));
            if (lightness < 0 || lightness > 100) throw new ArgumentOutOfRangeException(nameof(lightness));

            hue %= 360;
            hue = hue < 0 ? hue + 360 : hue;

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

        /// <summary>
        /// Generates an RGB color based on a progression between two HSL colors.
        /// </summary>
        /// <param name="fromHue">Start hue in degrees [0°, 360°).</param>
        /// <param name="toHue">End hue in degrees [0°, 360°).</param>
        /// <param name="fromSaturation">Start saturation [0, 1].</param>
        /// <param name="toSaturation">End saturation [0, 1].</param>
        /// <param name="fromLightness">Start lightness [0, 1].</param>
        /// <param name="toLightness">End lightness [0, 1].</param>
        /// <param name="progress">Progress ratio [0, 1].</param>
        /// <returns>Interpolated RGB color.</returns>
        public static Color InterpolateHsl(double fromHue, double toHue, double fromSaturation, double toSaturation, double fromLightness, double toLightness, double progress)
        {
            var hue = fromHue + (toHue - fromHue) * progress;
            var saturation = fromSaturation + (toSaturation - fromSaturation) * progress;
            var lightness = fromLightness + (toLightness - fromLightness) * progress;

            hue = Math.Clamp(hue, 0, 360);
            saturation = Math.Clamp(saturation, 0, 1);
            lightness = Math.Clamp(lightness, 0, 1);

            return HslToRgb(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts RGB to HSL color format.
        /// </summary>
        /// <param name="color">RGB color.</param>
        /// <returns>HSL, hue ∈ [0°, 360°), saturation ∈ [0, 1], lightness ∈ [0, 1].</returns>
        public static (double Hue, double Saturation, double Lightness) RgbToHsl(Color color)
        {
            var (r, g, b) = (color.R / 255.0, color.G / 255.0, color.B / 255.0);

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min; // delta in [0, 1]

            double hue = 0;
            double saturation = 0;
            double lightness = (max + min) / 2;

            if (delta == 0)
            {
                return (hue, saturation, lightness);
            }

            hue = max switch
            {
                _ when max == r => ((g - b) / delta) % 6,
                _ when max == g => ((b - r) / delta) + 2,
                _ when max == b => ((r - g) / delta) + 4,
                _ => throw new Exception("This should never happen")
            };

            hue *= 60;
            hue = hue < 0 ? hue + 360 : hue;

            saturation = delta / (1 - Math.Abs(2 * lightness - 1));

            return (hue, saturation, lightness);
        }

    }
}
