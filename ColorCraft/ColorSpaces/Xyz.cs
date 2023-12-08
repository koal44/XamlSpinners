using System;

namespace ColorCraft
{
    public record Xyz(double X, double Y, double Z)
    {
        public RgbLinear ToRgbLinear()
        {
            double r = 3.240479 * X - 1.537150 * Y - 0.498535 * Z;
            double g = -0.969256 * X + 1.875992 * Y + 0.041556 * Z;
            double b = 0.055648 * X - 0.204043 * Y + 1.057311 * Z;

            r = Math.Clamp(r, 0.0, 1.0);
            g = Math.Clamp(g, 0.0, 1.0);
            b = Math.Clamp(b, 0.0, 1.0);

            return new RgbLinear(r, g, b);
        }

        public Xyz D65Normalize()
        {
            const double Xn = 0.950456;
            const double Zn = 1.088754;

            return new Xyz(X/Xn, Y, Z/Zn);
        }

        public Xyz D65Denormalize()
        {
            const double Xn = 0.950456;
            const double Zn = 1.088754;

            return new Xyz(X*Xn, Y, Z*Zn);
        }

        public Lab ToLab()
        {
            double L = g(Y); // 0 <= L <= 100
            double a = 500 * (f(X) - f(Y)); // -127 <= a <= 127
            double b = 200 * (f(Y) - f(Z)); // -127 <= b <= 127

            return new Lab(L, a, b);

            static double g(double t) => t > 0.008856
                ? 116 * Math.Pow(t, 1.0 / 3.0) - 16
                : 903.3 * t;

            static double f(double t) => t > 0.008856
                ? Math.Pow(t, 1.0 / 3.0)
                : 7.787 * t + 16.0 / 116.0;
        }

        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";
    }
}
