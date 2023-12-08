using System;
using System.Windows.Media;

namespace ColorCraft
{
    public record Lab(double L, double A, double B)
    {
        public static Lab Lerp(Lab from, Lab to, double progress)
        {
            return new(
                Utils.LerpDouble(from.L, to.L, progress),
                Utils.LerpDouble(from.A, to.A, progress),
                Utils.LerpDouble(from.B, to.B, progress)
            );
        }

        public static Lab FromColor(Color c, bool useGammaCorrection)
        {
            return RgbLinear
                .FromColor(c, useGammaCorrection)
                .ToXyz()
                .D65Normalize()
                .ToLab();
        }

        public Color ToColor(bool useGammaCorrection)
        {
            return this
                .ToXyz()
                .D65Denormalize()
                .ToRgbLinear()
                .ToColor(useGammaCorrection);
        }

        public Xyz ToXyz()
        {
            double y = (L + 16) / 116;
            double x = A / 500 + y;
            double z = y - B / 200;

            x = x > 0.206893
                ? Math.Pow(x, 3.0)
                : (x - 16.0 / 116) / 7.787;

            y = L > (0.008856 * 903.3)
                ? Math.Pow((L + 16) / 116, 3)
                : L / 903.3;

            z = z > 0.206893
                ? Math.Pow(z, 3)
                : (z - 16.0 / 116) / 7.787;

            return new Xyz(x, y, z);
        }

        public override string ToString() => $"L: {L}, A: {A}, B: {B}";
    }
}
