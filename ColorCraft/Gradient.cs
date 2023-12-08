using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorCraft
{
    public class Gradient
    {
        private readonly bool _useGammaCorrection;
        private readonly List<MultiColorSpaceGradientStop> _stops;
        private WriteableBitmap? _bitmap;

        public Gradient(LerpMode mode, List<GradientStop> stops, bool useGammaCorrection)
        {
            LerpMode = mode;
            _stops = stops.OrderBy(s => s.Offset)
                .Select(x => new MultiColorSpaceGradientStop(x, useGammaCorrection))
                .ToList();
            _useGammaCorrection = useGammaCorrection;
        }

        public WriteableBitmap? Bitmap => _bitmap;

        public LerpMode LerpMode { get; set; }

        public void AddStop(GradientStop stop)
        {
            _stops.Add(new MultiColorSpaceGradientStop(stop, _useGammaCorrection));
            _stops.Sort((a, b) => a.Offset.CompareTo(b.Offset));
        }

        public Color ColorAt(double progress)
        {
            if (progress < 0 || progress > 1) throw new ArgumentOutOfRangeException(nameof(progress));
            if (_stops.Count == 0) return Colors.Transparent;
            if (_stops.Count == 1) return _stops[0].Color;

            MultiColorSpaceGradientStop startStop = _stops[0];
            MultiColorSpaceGradientStop? endStop = null;

            for (int i = 0; i < _stops.Count; i++)
            {
                if (_stops[i].Offset <= progress)
                {
                    startStop = _stops[i];
                }
                else
                {
                    endStop = _stops[i];
                    break;
                }
            }

            if (endStop == null)
            {
                return startStop.Color;
            }

            var localProgress = (progress - startStop.Offset) / (endStop.Offset - startStop.Offset);

            var lerpedColor = LerpMode switch
            {
                LerpMode.Rgb => Utils.LerpColor(startStop.Color, endStop.Color, localProgress),
                LerpMode.Srgb => RgbLinear.Lerp(startStop.RgbLinear, endStop.RgbLinear, localProgress).ToColor(_useGammaCorrection),
                LerpMode.Hsl => Hsl.Lerp(startStop.Hsl, endStop.Hsl, localProgress).ToColor(),
                LerpMode.Lab => Lab.Lerp(startStop.Lab, endStop.Lab, localProgress).ToColor(_useGammaCorrection),
                LerpMode.SrgbBrightFix => RgbLinear.BrightFixLerp(startStop.RgbLinear, endStop.RgbLinear, localProgress).ToColor(_useGammaCorrection),
                _ => throw new NotImplementedException(),
            };

            lerpedColor.A = Utils.LerpByte(startStop.Color.A, endStop.Color.A, localProgress);
            return lerpedColor;
        }

        public void CreateBitmap(int imgWidth, int imgHeight)
        {
            _bitmap = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Pbgra32, null);
        }

        public void DrawLinearGradient(Point startPoint, Point endPoint)
        {
            if (_bitmap == null) throw new InvalidOperationException("Bitmap has not been created yet");

            _bitmap.Lock();

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];
            var gradientVector = new Vector(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            var gradientLengthSquared = gradientVector.LengthSquared;

            int i = 0;
            for (int y = 0; y < _bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < _bitmap.PixelWidth; x++)
                {
                    var relativeVector = new Vector(x - startPoint.X, y - startPoint.Y);

                    // progress = |proj_b(a)| / |b| = (a . b / |b|^2)
                    var progress = Vector.Multiply(relativeVector, gradientVector) / gradientLengthSquared;
                    progress = Math.Clamp(progress, 0, 1);

                    var color = ColorAt(progress);
                    int bgra = (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
                    pixelsBgra[i++] = bgra;
                }
            }

            var sourceRect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            int stride = _bitmap.PixelWidth * (_bitmap.Format.BitsPerPixel / 8);
            _bitmap.WritePixels(sourceRect, pixelsBgra, stride, 0, 0);
            _bitmap.Unlock();
        }

        public void DrawConicGradient(double angleOffset)
        {
            if (_bitmap == null) throw new InvalidOperationException("Bitmap has not been created yet");

            _bitmap.Lock();

            const double InvTwoPi = 1.0 / (2.0 * Math.PI);
            const double TwoPi = 2.0 * Math.PI;

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];

            var centerX = _bitmap.PixelWidth / 2f;
            var centerY = _bitmap.PixelHeight / 2f;

            // Ensure the start angle is within [0, 2pi)
            angleOffset %= TwoPi;
            angleOffset += angleOffset < 0 ? TwoPi : 0;

            int i = 0;
            var dy = centerY;
            for (int y = 0; y < _bitmap.PixelHeight; ++y, --dy)
            {
                var dx = centerX;
                for (int x = 0; x < _bitmap.PixelWidth; ++x, --dx)
                {
                    // Calculate angle from center to pixel; atan2 returns value between [-pi, pi]
                    var angle = (Math.Atan2(dy, dx) + Math.PI) + angleOffset;

                    // Normalize angle: shift atan2 by Pi and an additional 2Pi if in the negative range
                    angle %= TwoPi;

                    // Convert angle to a progress value in the range [0, 1] 
                    var progress = angle * InvTwoPi;

                    // Get the color corresponding to the calculated progress
                    var color = ColorAt(progress);

                    // Convert color to BGRA format
                    int bgra = (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
                    pixelsBgra[i++] = bgra;
                }
            }

            var sourceRect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            int stride = _bitmap.PixelWidth * (_bitmap.Format.BitsPerPixel / 8);
            _bitmap.WritePixels(sourceRect, pixelsBgra, stride, 0, 0);
            _bitmap.Unlock();
        }

        public void DrawSpiralGradient(double angleOffset, double turnIntensity)
        {
            if (_bitmap == null) throw new InvalidOperationException("Bitmap has not been created yet");
            _bitmap.Lock();

            const double InvTwoPi = 1.0 / (2.0 * Math.PI);
            const double TwoPi = 2.0 * Math.PI;
            var maxRadiusSquared = _bitmap.PixelWidth * _bitmap.PixelWidth + _bitmap.PixelHeight * _bitmap.PixelHeight;

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];

            var centerX = _bitmap.PixelWidth / 2f;
            var centerY = _bitmap.PixelHeight / 2f;

            // Normalize the start angle
            angleOffset %= TwoPi;

            int i = 0;
            var dy = centerY;
            for (int y = 0; y < _bitmap.PixelHeight; ++y, --dy)
            {
                var dx = centerX;
                for (int x = 0; x < _bitmap.PixelWidth; ++x, --dx)
                {
                    // Calculate angle for the current pixel
                    var angle = Math.Atan2(dy, dx) + Math.PI + angleOffset;
                    var radialProgress = Math.Sqrt((dy * dy + dx * dx) / maxRadiusSquared);
                    angle += radialProgress * turnIntensity;
                    angle %= TwoPi;
                    angle = angle < 0 ? angle + TwoPi : angle;
                    var progress = angle * InvTwoPi;

                    var color = ColorAt(progress);

                    int bgra = (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
                    pixelsBgra[i++] = bgra;
                }
            }

            var sourceRect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            int stride = _bitmap.PixelWidth * (_bitmap.Format.BitsPerPixel / 8);
            _bitmap.WritePixels(sourceRect, pixelsBgra, stride, 0, 0);
            _bitmap.Unlock();
        }

        public void DrawConicSpiralRayGradient(double angleOffset, double turnIntensity, int armsCount)
        {
            if (_bitmap == null) throw new InvalidOperationException("Bitmap has not been created yet");
            _bitmap.Lock();

            const double TwoPi = 2.0 * Math.PI;
            var maxRadiusSquared = _bitmap.PixelWidth * _bitmap.PixelWidth + _bitmap.PixelHeight * _bitmap.PixelHeight;

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];

            var centerX = _bitmap.PixelWidth / 2f;
            var centerY = _bitmap.PixelHeight / 2f;

            // Normalize the start angle
            angleOffset %= TwoPi;
            angleOffset += angleOffset < 0 ? TwoPi : 0;

            var repeatFactor = Math.PI * 2.0 / armsCount;

            int i = 0;
            var dy = centerY;
            for (int y = 0; y < _bitmap.PixelHeight; ++y, --dy)
            {
                var dx = centerX;
                for (int x = 0; x < _bitmap.PixelWidth; ++x, --dx)
                {
                    // Calculate angle for the current pixel
                    var angle = Math.Atan2(dy, dx) + Math.PI - angleOffset;
                    var radialProgress = Math.Sqrt((dy * dy + dx * dx) / maxRadiusSquared);
                    angle += radialProgress * turnIntensity;
                    angle %= repeatFactor;
                    angle = angle < 0 ? angle + repeatFactor : angle;
                    var progress = angle * 1 / repeatFactor;

                    var color = ColorAt(progress);

                    int bgra = (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
                    pixelsBgra[i++] = bgra;
                }
            }

            var sourceRect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            int stride = _bitmap.PixelWidth * (_bitmap.Format.BitsPerPixel / 8);
            _bitmap.WritePixels(sourceRect, pixelsBgra, stride, 0, 0);
            _bitmap.Unlock();
        }


        //public static Brush CreateBrush(double angle, List<GradientStop> stops, LerpMode mode)
        //{
        //    var imgWidth = 100;
        //    var imgHeight = 100;
        //    var bitmap = CreateBitmap(imgWidth, imgHeight, angle, stops, mode);
        //    var brush = new ImageBrush(bitmap)
        //    {
        //        Viewport = new Rect(0, 0, imgWidth, imgHeight),
        //        ViewboxUnits = BrushMappingMode.Absolute,
        //        Viewbox = new Rect(0, 0, imgWidth, imgHeight),
        //        TileMode = TileMode.None,
        //        Stretch = Stretch.Fill,
        //        AlignmentX = AlignmentX.Left,
        //        AlignmentY = AlignmentY.Top,
        //    };
        //    return brush;
        //}


    }
}
