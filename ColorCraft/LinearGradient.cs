using System;
using System.Diagnostics;
using System.Windows;

namespace ColorCraft
{
    public class LinearGradient : Gradient
    {
        #region Dependency Properties

        // StartPoint
        public Point StartPoint
        {
            get => (Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(nameof(StartPoint), typeof(Point), typeof(LinearGradient), new FrameworkPropertyMetadata(new Point(0, 0), OnStartPointChanged));

        private static void OnStartPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onStartPointChangedCount++;
            if (d is not LinearGradient self) return;
            self.OnStartPointChanged(e);
        }

        protected virtual void OnStartPointChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // EndPoint
        public Point EndPoint
        {
            get => (Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(nameof(EndPoint), typeof(Point), typeof(LinearGradient), new FrameworkPropertyMetadata(new Point(0, 1), OnEndPointChanged));

        private static void OnEndPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onEndPointChangedCount++;
            if (d is not LinearGradient self) return;
            self.OnEndPointChanged(e);
        }

        protected virtual void OnEndPointChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        #endregion

        #region Methods

        public override void DrawGradient()
        {
            if (_bitmap == null) return;

            int[] pixelsBgra = new int[_bitmap.PixelWidth * _bitmap.PixelHeight];
            var gradientVector = new Vector(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);
            var gradientLengthSquared = gradientVector.LengthSquared;

            int i = 0;
            for (int y = 0; y < _bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < _bitmap.PixelWidth; x++)
                {
                    var relativeVector = new Vector(x - StartPoint.X, y - StartPoint.Y);

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
        }

        #endregion

        private static int onStartPointChangedCount = 0;
        private static int onEndPointChangedCount = 0;

        public static void DumpLinearEventCounts()
        {
            Debug.WriteLine($"onStartPointChangedCount: {onStartPointChangedCount}");
            Debug.WriteLine($"onEndPointChangedCount: {onEndPointChangedCount}");
        }

    }
}
