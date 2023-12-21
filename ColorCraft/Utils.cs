using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public static void DebugBitmap(BitmapSource? bitmap)
        {
            if (bitmap == null)
            {
                Debug.WriteLine("Bitmap is null");
                return;
            }

            int stride = bitmap.PixelWidth * (bitmap.Format.BitsPerPixel / 8);

            string bitmapDescription = $"Bitmap:\n" +
                $"pixelwidth:{bitmap.PixelWidth},\n" +
                $"pixelheight:{bitmap.PixelHeight},\n" +
                $"dpi:{bitmap.DpiX},\n" +
                $"dpi:{bitmap.DpiY},\n" +
                $"format:{bitmap.Format},\n" +
                $"palette:{bitmap.Palette},\n" +
                $"bitcount:{bitmap.Format.BitsPerPixel},\n" +
                $"stride:{stride},\n" +
                $"size:{stride * bitmap.PixelHeight},\n";

            Debug.WriteLine(bitmapDescription);

            // print first few bytes of the bitmap


            var rect = new Int32Rect(0, 0, 10, 1);
            var pixels = new int[10];
            bitmap.CopyPixels(rect, pixels, stride, 0);
            Debug.WriteLine("First few pixels of the bitmap:\n");
            foreach (int pixel in pixels)
            {
                string hex = $"#{pixel:X8}, ";
                Debug.Write(hex);
            }
            Debug.WriteLine("\n");
        }

    }
}
