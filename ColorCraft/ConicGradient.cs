using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorCraft
{
    public static class ConicGradient
    {
        private const double InvTwoPi = 1.0 / (2.0 * Math.PI);
        private const double TwoPi = 2.0 * Math.PI;

        public static Brush CreateBrush(double angle, List<GradientStop> stops, LerpMode mode)
        {
            var imgWidth = 100;
            var imgHeight = 100;
            var bitmap = CreateBitmap(imgWidth, imgHeight, angle, stops, mode);
            var brush = new ImageBrush(bitmap)
            {
                Viewport = new Rect(0, 0, imgWidth, imgHeight),
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(0, 0, imgWidth, imgHeight),
                TileMode = TileMode.None,
                Stretch = Stretch.Fill,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
            };
            return brush;
        }

        public static WriteableBitmap CreateBitmap(int imgWidth, int imgHeight, double angle, List<GradientStop> stops, LerpMode mode)
        {
            bool useGammaCorrection = true;
            var bitmap = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null);
            Color[] pixels = new Color[imgWidth * imgHeight];
            var midPoint = new Point(imgWidth / 2, imgHeight / 2);
            var gradient = new Gradient(mode, stops, useGammaCorrection);

            for (int y = 0, x; y < imgHeight; ++y)
            {
                double rise = midPoint.Y - y;
                double run = midPoint.X;
                for (x = 0; x < imgWidth; ++x)
                {
                    double progress = Math.Atan2(rise, run) + Math.PI - angle;
                    progress = Math.Clamp(progress, 0, TwoPi);
                    progress *= InvTwoPi; // get value in range 0...1

                    pixels[imgWidth * y + x] = gradient.ColorAt(progress); // pixels is 1D pixel array

                    run -= 1;
                }
            }

            int stride = imgWidth * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelBytes = new byte[stride * imgHeight];

            for (int i = 0; i < pixels.Length; i++)
            {
                int pixelIndex = i * 4;
                pixelBytes[pixelIndex + 0] = pixels[i].B; // Blue
                pixelBytes[pixelIndex + 1] = pixels[i].G; // Green
                pixelBytes[pixelIndex + 2] = pixels[i].R; // Red
                pixelBytes[pixelIndex + 3] = pixels[i].A; // Alpha
            }

            bitmap.WritePixels(new Int32Rect(0, 0, imgWidth, imgHeight), pixelBytes, stride, 0);


            //bitmap.WritePixels(new Int32Rect(0, 0, imgWidth, imgHeight), pixels, imgWidth * 4, 0);
            return bitmap;
        }

    }
}
