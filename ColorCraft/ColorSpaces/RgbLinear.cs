using System;
using System.Windows.Media;

namespace ColorCraft
{
    public record RgbLinear(double R, double G, double B)
    {
        public static RgbLinear Lerp(RgbLinear from, RgbLinear to, double progress)
        {
            return new RgbLinear(
                Utils.LerpDouble(from.R, to.R, progress),
                Utils.LerpDouble(from.G, to.G, progress),
                Utils.LerpDouble(from.B, to.B, progress)
            );
        }

        // Mark'sLerp method
        // https://stackoverflow.com/questions/22607043/color-gradient-algorithm/39924008#39924008
        public static RgbLinear BrightFixLerp(RgbLinear from, RgbLinear to, double progress)
        {
            const double Gamma = 0.43;
            const double GammaInv = 1 / Gamma;

            // Lerp the colors normally first
            var rgbLerped = Lerp(from, to, progress);

            // Calculate brightness using gamma correction
            var fromGammaBright = Math.Pow(from.R + from.G + from.B, Gamma);
            var toGammaBright = Math.Pow(to.R + to.G + to.B, Gamma);
            var bright = Math.Pow(Utils.LerpDouble(fromGammaBright, toGammaBright, progress), GammaInv);

            // Adjusting the brightness of the lerped color
            var sum = rgbLerped.R + rgbLerped.G + rgbLerped.B;
            if (sum != 0)
            {
                bright /= sum;
                rgbLerped = new RgbLinear(rgbLerped.R * bright, rgbLerped.G * bright, rgbLerped.B * bright);
            }

            return rgbLerped;
        }

        public Color ToColor(bool useGammaCorrection)
        {
            return useGammaCorrection
                ? Color.FromRgb(Gamma(R), Gamma(G), Gamma(B))
                : Color.FromRgb((byte)(255.0 * R), (byte)(255.0 * G), (byte)(255.0 * B));

            static byte Gamma(double color)
            {
                return color <= 0.0031308
                    ? (byte)(255.0 * 12.92 * color)
                    : (byte)(255.0 * (1.055 * Math.Pow(color, 1.0 / 2.4) - 0.055));
            }
        }

        public static RgbLinear FromColor(Color c, bool useGammaCorrection)
        {
            return useGammaCorrection
                ? new RgbLinear(InverseGamma(c.R), InverseGamma(c.G), InverseGamma(c.B))
                : new RgbLinear(c.R / 255.0, c.G / 255.0, c.B / 255.0);

            static double InverseGamma(byte color)
            {
                double c = color / 255.0;
                return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
            }
        }

        public Xyz ToXyz()
        {
            double X = 0.412453 * R + 0.357580 * G + 0.180423 * B;
            double Y = 0.212671 * R + 0.715160 * G + 0.072169 * B;
            double Z = 0.019334 * R + 0.119193 * G + 0.950227 * B;

            return new Xyz(X, Y, Z);
        }

        public override string ToString() => $"R: {R}, G: {G}, B: {B}";

    }
}
