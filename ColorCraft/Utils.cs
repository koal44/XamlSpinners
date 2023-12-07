using System;
using System.Windows.Media;

namespace ColorCraft
{
    public static class Utils
    {
        public static byte LerpByte(byte from, byte to, double progress)
        {
            return (byte)(from + (to - from) * progress);
        }

        public static double LerpDouble(double from, double to, double progress)
        {
            return from + (to - from) * progress;
        }

        public static Color LerpColor(Color from, Color to, double progress)
        {
            return Color.FromArgb(
                LerpByte(from.A, to.A, progress),
                LerpByte(from.R, to.R, progress),
                LerpByte(from.G, to.G, progress),
                LerpByte(from.B, to.B, progress)
            );
        }
    }
}
