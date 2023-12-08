using System;
using System.Windows.Media;

namespace ColorCraft
{
    public record Hsl(double H, double S, double L)
    {
        public static Hsl Lerp(Hsl from, Hsl to, double progress, bool useShortestPath = true)
        {
            if (from.H < 0 || from.H > 360) throw new ArgumentOutOfRangeException(nameof(from));
            if (to.H < 0 || to.H > 360) throw new ArgumentOutOfRangeException(nameof(to));

            var fromHue = useShortestPath && to.H > from.H + 180
                ? from.H + 360 
                : from.H;
            var toHue = useShortestPath && from.H > to.H + 180
                ? to.H + 360 
                : to.H;

            var hue = Utils.LerpDouble(fromHue, toHue, progress) % 360;
            var saturation = Utils.LerpDouble(from.S, to.S, progress);
            var lightness = Utils.LerpDouble(from.L, to.L, progress);

            hue = Math.Clamp(hue, 0, 360);
            saturation = Math.Clamp(saturation, 0, 1);
            lightness = Math.Clamp(lightness, 0, 1);

            return new Hsl(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts HSL to RGB color format.
        /// </summary>
        /// <remarks> https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_RGB </remarks>
        public Color ToColor()
        {
            if (H < 0 || H > 360) throw new ArgumentOutOfRangeException(nameof(H));
            if (S < 0 || S > 1) throw new ArgumentOutOfRangeException(nameof(S));
            if (L < 0 || L > 1) throw new ArgumentOutOfRangeException(nameof(L));

            double chroma = (1 - Math.Abs(2 * L - 1)) * S;
            double hueSection = H / 60.0;
            double x = chroma * (1 - Math.Abs(hueSection % 2 - 1));
            double m = L - chroma / 2;

            (double r, double g, double b) rgb = hueSection switch
            {
                < 1 => (chroma, x, 0),
                < 2 => (x, chroma, 0),
                < 3 => (0, chroma, x),
                < 4 => (0, x, chroma),
                < 5 => (x, 0, chroma),
                < 6 => (chroma, 0, x),
                _ => throw new ArgumentOutOfRangeException(nameof(hueSection))
            };

            return Color.FromRgb(
                (byte)((rgb.r + m) * 255),
                (byte)((rgb.g + m) * 255),
                (byte)((rgb.b + m) * 255)
            );
        }

        /// <summary>
        /// Alternative method to convert HSL to RGB based on W3C standard.
        /// </summary>
        /// <remarks> https://www.w3.org/TR/css-color-3/#hsl-color </remarks>
        public Color ToColor2()
        {
            if (S < 0 || S > 1) throw new ArgumentOutOfRangeException(nameof(S));
            if (L < 0 || L > 1) throw new ArgumentOutOfRangeException(nameof(L));

            var hue = H % 360;
            hue += hue < 0 ? 360 : 0;

            double f(double n)
            {
                double k = (n + hue / 30) % 12;
                double a = S * Math.Min(L, 1 - L);
                return L - a * Math.Clamp(Math.Min(k - 3, 9 - k), -1, 1);
            }

            return Color.FromRgb(
                (byte)(f(0) * 255),
                (byte)(f(8) * 255),
                (byte)(f(4) * 255)
            );
        }

        /// <summary>
        /// Converts RGB to HSL color format.
        /// </summary>
        /// <param name="color">RGB color.</param>
        /// <returns>HSL, hue [0, 360), saturation [0, 1], lightness [0, 1].</returns>
        public static Hsl FromColor(Color color)
        {
            var c = RgbLinear.FromColor(color, false); // false??

            double max = Math.Max(c.R, Math.Max(c.G, c.B));
            double min = Math.Min(c.R, Math.Min(c.G, c.B));
            double delta = max - min; // delta in [0, 1]

            double hue = 0;
            double saturation = 0;
            double lightness = (max + min) / 2;

            if (delta == 0)
            {
                return new Hsl(hue, saturation, lightness);
            }

            hue = max switch
            {
                _ when max == c.R => ((c.G - c.B) / delta) % 6,
                _ when max == c.G => ((c.B - c.R) / delta) + 2,
                _ when max == c.B => ((c.R - c.G) / delta) + 4,
                _ => throw new Exception("This should never happen")
            };

            hue *= 60;
            hue = hue < 0 ? hue + 360 : hue;

            saturation = delta / (1 - Math.Abs(2 * lightness - 1));

            return new Hsl(hue, saturation, lightness);
        }

        public override string ToString() => $"H: {H}, S: {S}, L: {L}";
    }
}
