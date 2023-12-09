using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorCraft
{
    public class Gradient : INotifyPropertyChanged
    {
        #region Data

        private readonly bool _useGammaCorrection;
        private readonly List<MultiColorSpaceGradientStop> _stops;
        private WriteableBitmap? _bitmap;
        private ImageBrush? _brush;

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Properties

        public WriteableBitmap? Bitmap
        {
            get => _bitmap;
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;
                    OnPropertyChanged(nameof(Bitmap));
                }
            }
        }

        public ImageBrush? Brush
        {
            get => _brush;
            set
            {
                if (_brush != value)
                {
                    _brush = value;
                    OnPropertyChanged(nameof(Brush));
                }
            }
        }

        public LerpMode LerpMode { get; set; }

        #endregion

        #region Constructors

        public Gradient(List<GradientStop> stops, LerpMode mode = LerpMode.Rgb, bool useGammaCorrection = true)
        {
            LerpMode = mode;
            _stops = stops.OrderBy(s => s.Offset)
                .Select(x => new MultiColorSpaceGradientStop(x, useGammaCorrection))
                .ToList();
            _useGammaCorrection = useGammaCorrection;
        }

        #endregion

        #region Methods

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

        public void InitBitmap(int imgWidth = 200, int imgHeight = 200)
        {
            Bitmap = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Pbgra32, null);
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
            Brush = new ImageBrush(_bitmap);
        }

        public void DrawConicGradient(float angleOffset, float spiralStrength, float kaleidoscopeCount)
        {
            if (_bitmap == null) throw new InvalidOperationException("Bitmap has not been created yet");
            _bitmap.Lock();

            const float TwoPi = 2f * MathF.PI;
            int maxRadiusSquared = _bitmap.PixelWidth * _bitmap.PixelWidth + _bitmap.PixelHeight * _bitmap.PixelHeight;

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];

            float centerX = _bitmap.PixelWidth / 2f;
            float centerY = _bitmap.PixelHeight / 2f;

            // Normalize the angleOffsets
            angleOffset %= TwoPi;
            angleOffset += angleOffset < 0 ? TwoPi : 0; // angleOffset is now in [0, 2pi)

            float modulusAngle = TwoPi / kaleidoscopeCount;

            // fix the discontinuity ray for non-integral kaleidoscopeCounts to be the same as the angleOffset
            float kaleidoscopeRayOffset = angleOffset;

            bool shouldOffsetKaleidoscopeDiscontinuityRay =
                kaleidoscopeCount != (int)kaleidoscopeCount &&
                kaleidoscopeRayOffset != 0;

            int i = 0;
            float dy = centerY;
            float angle;
            for (int y = 0; y < _bitmap.PixelHeight; ++y, --dy)
            {
                float dx = -centerX;
                for (int x = 0; x < _bitmap.PixelWidth; ++x, ++dx)
                {
                    // Calculate angle for the current pixel
                    if (shouldOffsetKaleidoscopeDiscontinuityRay)
                    {
                        // Atan2 has an inherent discontinuity at the negative x-axis, which becomes apparent when
                        // using a non integral kaleidoscope count. We can choose where the discontinuity
                        // ray is by rotating before calculating atan2.
                        float dxRot = dx * MathF.Cos(kaleidoscopeRayOffset) - dy * MathF.Sin(kaleidoscopeRayOffset);
                        float dyRot = dx * MathF.Sin(kaleidoscopeRayOffset) + dy * MathF.Cos(kaleidoscopeRayOffset);
                        angle = MathF.Atan2(dyRot, dxRot) + MathF.PI + 0; // angleOffset - kaleidoscopeRayOffset == 0;
                    }
                    else
                    {
                        angle = MathF.Atan2(dy, dx) + MathF.PI + angleOffset; // angle is now in [0, 4pi)
                    }

                    if (spiralStrength != 0)
                    {
                        float radialProgress = MathF.Sqrt((dy * dy + dx * dx) / maxRadiusSquared);
                        angle += radialProgress * spiralStrength; // angle is now in (-inf, inf)
                    }
                    angle %= modulusAngle; // angle is now in (-modulusAngle, modulusAngle)
                    angle = angle < 0 ? angle + modulusAngle : angle; // angle is now in [0, modulusAngle)
                    float progress = angle / modulusAngle; // progress is [0, 1)

                    var color = ColorAt(progress);

                    int bgra = (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
                    pixelsBgra[i++] = bgra;
                }
            }

            var sourceRect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            int stride = _bitmap.PixelWidth * (_bitmap.Format.BitsPerPixel / 8);
            _bitmap.WritePixels(sourceRect, pixelsBgra, stride, 0, 0);
            _bitmap.Unlock();
            Brush = new ImageBrush(_bitmap);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
