using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace ColorCraft
{
    public class LinearGradient
    {
        public static WriteableBitmap CreateBitmap(int imgWidth, int imgHeight, List<GradientStop> stops, LerpMode mode)
        {
            bool useGammaCorrection = true;
            var bitmap = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null);
            Color[] pixels = new Color[imgWidth * imgHeight];
            var gradient = new Gradient(mode, stops, useGammaCorrection);

            double dx = imgWidth;
            double dy = imgHeight;
            double lineLength = Math.Sqrt(dx * dx + dy * dy);

            for (int y = 0; y < imgHeight; ++y)
            {
                for (int x = 0; x < imgWidth; ++x)
                {
                    // Calculate the projection of the point onto the gradient line
                    double relativeX = x;
                    double relativeY = y;
                    double projection = (relativeX * dx + relativeY * dy) / lineLength;

                    // Normalize the step value to be between 0 and 1
                    double step = projection / lineLength;
                    step = Math.Clamp(step, 0, 1);

                    pixels[imgWidth * y + x] = gradient.ColorAt(step);
                }
            }

            int stride = imgWidth * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixelBytes = new byte[stride * imgHeight];

            for (int i = 0; i < pixels.Length; i++)
            {
                int pixelIndex = i * 4;
                pixelBytes[pixelIndex] = pixels[i].B;     // Blue
                pixelBytes[pixelIndex + 1] = pixels[i].G; // Green
                pixelBytes[pixelIndex + 2] = pixels[i].R; // Red
                pixelBytes[pixelIndex + 3] = pixels[i].A; // Alpha
            }

            bitmap.WritePixels(new Int32Rect(0, 0, imgWidth, imgHeight), pixelBytes, stride, 0);
            return bitmap;
        }

    }
}
