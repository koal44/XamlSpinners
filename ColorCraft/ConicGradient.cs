using System;
using System.Diagnostics;
using System.Windows;

namespace ColorCraft
{
    public class ConicGradient : Gradient
    {
        #region Dependency Properties

        // AngleOffset [0, 360]
        public float AngleOffset
        {
            get => (float)GetValue(AngleOffsetProperty);
            set => SetValue(AngleOffsetProperty, value);
        }

        public static readonly DependencyProperty AngleOffsetProperty = DependencyProperty.Register(nameof(AngleOffset), typeof(float), typeof(ConicGradient), new FrameworkPropertyMetadata(0f, OnAngleOffsetChanged));

        private static void OnAngleOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onAngleOffsetChangedCount++;
            if (d is not ConicGradient self) return;
            self.OnAngleOffsetChanged(e);
        }

        protected virtual void OnAngleOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // SpiralStrength
        public float SpiralStrength
        {
            get => (float)GetValue(SpiralStrengthProperty);
            set => SetValue(SpiralStrengthProperty, value);
        }

        public static readonly DependencyProperty SpiralStrengthProperty = DependencyProperty.Register(nameof(SpiralStrength), typeof(float), typeof(ConicGradient), new FrameworkPropertyMetadata(0f, OnSpiralStrengthChanged));

        private static void OnSpiralStrengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onSpiralStrengthChangedCount++;
            if (d is not ConicGradient self) return;
            self.OnSpiralStrengthChanged(e);
        }

        protected virtual void OnSpiralStrengthChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // KaleidoscopeCount
        public float KaleidoscopeCount
        {
            get => (float)GetValue(KaleidoscopeCountProperty);
            set => SetValue(KaleidoscopeCountProperty, value);
        }

        public static readonly DependencyProperty KaleidoscopeCountProperty = DependencyProperty.Register(nameof(KaleidoscopeCount), typeof(float), typeof(ConicGradient), new FrameworkPropertyMetadata(1f, OnKaleidoscopeCountChanged));

        private static void OnKaleidoscopeCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onKaleidoscopeCountChangedCount++;
            if (d is not ConicGradient self) return;
            self.OnKaleidoscopeCountChanged(e);
        }

        protected virtual void OnKaleidoscopeCountChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // CenterX
        public float CenterX
        {
            get => (float)GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(float), typeof(ConicGradient), new FrameworkPropertyMetadata(0.5f, OnCenterXChanged), ValidateNormalizedRange);

        private static bool ValidateNormalizedRange(object value)
        {
            if (value is not float f) return false;
            return f >= 0 && f <= 1;
        }

        private static void OnCenterXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onCenterXChangedCount++;
            if (d is not ConicGradient self) return;
            self.OnCenterXChanged(e);
        }

        protected virtual void OnCenterXChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // CenterY
        public float CenterY
        {
            get => (float)GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(float), typeof(ConicGradient), new FrameworkPropertyMetadata(0.5f, OnCenterYChanged), ValidateNormalizedRange);

        private static void OnCenterYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onCenterYChangedCount++;
            if (d is not ConicGradient self) return;
            self.OnCenterYChanged(e);
        }

        protected virtual void OnCenterYChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        #endregion

        #region Methods

        public override void DrawGradient()
        {
            if (_bitmap == null) return;

            const float TwoPi = 2f * MathF.PI;
            int maxRadiusSquared = _bitmap.PixelWidth * _bitmap.PixelWidth + _bitmap.PixelHeight * _bitmap.PixelHeight;

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];

            float cx = _bitmap.PixelWidth * CenterX;
            float cy = _bitmap.PixelHeight * CenterY;

            // Normalize the angleOffsets
            var angleOffset = AngleOffset * MathF.PI / 180;
            angleOffset %= TwoPi;
            angleOffset += angleOffset < 0 ? TwoPi : 0; // angleOffset is now in [0, 2pi)

            float modulusAngle = TwoPi / KaleidoscopeCount;

            // fix the discontinuity ray for non-integral kaleidoscopeCounts to be the same as the angleOffset
            float kaleidoscopeRayOffset = angleOffset;

            bool shouldOffsetKaleidoscopeDiscontinuityRay =
                KaleidoscopeCount != (int)KaleidoscopeCount &&
                kaleidoscopeRayOffset != 0;

            int i = 0;
            float dy = cy;
            float angle;
            for (int y = 0; y < _bitmap.PixelHeight; ++y, --dy)
            {
                float dx = -cx;
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

                    if (SpiralStrength != 0)
                    {
                        float radialProgress = MathF.Sqrt((dy * dy + dx * dx) / maxRadiusSquared);
                        angle += radialProgress * SpiralStrength; // angle is now in (-inf, inf)
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
        }

        #endregion

        private static int onAngleOffsetChangedCount = 0;
        private static int onSpiralStrengthChangedCount = 0;
        private static int onKaleidoscopeCountChangedCount = 0;
        private static int onCenterXChangedCount = 0;
        private static int onCenterYChangedCount = 0;

        public static void DumpConicEventCounts()
        {
            Debug.WriteLine($"onAngleOffsetChangedCount: {onAngleOffsetChangedCount}");
            Debug.WriteLine($"onSpiralStrengthChangedCount: {onSpiralStrengthChangedCount}");
            Debug.WriteLine($"onKaleidoscopeCountChangedCount: {onKaleidoscopeCountChangedCount}");
            Debug.WriteLine($"onCenterXChangedCount: {onCenterXChangedCount}");
            Debug.WriteLine($"onCenterYChangedCount: {onCenterYChangedCount}");
        }

    }
}
