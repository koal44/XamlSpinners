using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ColorCraft
{
    public abstract class Gradient : DependencyObject
    {
        #region Data

        private readonly FreezableCollection<MultiColorSpaceGradientStop> _stops;
        protected WriteableBitmap? _bitmap;

        #endregion

        #region Dependency Properties

        // GradientStops
        public FreezableCollection<GradientStop> GradientStops
        {
            get => (FreezableCollection<GradientStop>)GetValue(GradientStopsProperty);
            set => SetValue(GradientStopsProperty, value);
        }

        public static readonly DependencyProperty GradientStopsProperty = DependencyProperty.Register(nameof(GradientStops), typeof(FreezableCollection<GradientStop>), typeof(Gradient), new FrameworkPropertyMetadata(null, OnGradientStopsChanged));

        private static void OnGradientStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onGradientStopsChangedCount++;
            if (d is not Gradient self) return;
            self.OnGradientStopsChanged(e);
        }

        protected virtual void OnGradientStopsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is FreezableCollection<GradientStop> oldStops)
                oldStops.CollectionChanged -= GradientStops_CollectionChanged;

            if (e.NewValue is FreezableCollection<GradientStop> newStops)
                newStops.CollectionChanged += GradientStops_CollectionChanged;

            UpdateStops();
        }

        private void GradientStops_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateStops();
        }

        private void UpdateStops()
        {
            updateStopsCount++;
            _stops.Clear();

            if (GradientStops == null) return;

            foreach (var stop in GradientStops)
            {
                var multiColorStop = new MultiColorSpaceGradientStop(stop.Color, stop.Offset, UseGammaCorrection);
                _stops.Add(multiColorStop);
            }
            _stops.Sort((a, b) => a.Offset.CompareTo(b.Offset));

            UpdateGradient();
        }

        // UseGammaCorrection
        public bool UseGammaCorrection
        {
            get => (bool)GetValue(UseGammaCorrectionProperty);
            set => SetValue(UseGammaCorrectionProperty, value);
        }

        public static readonly DependencyProperty UseGammaCorrectionProperty = DependencyProperty.Register(nameof(UseGammaCorrection), typeof(bool), typeof(Gradient), new FrameworkPropertyMetadata(true, OnUseGammaCorrectionChanged));

        private static void OnUseGammaCorrectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onUseGammaCorrectionChangedCount++;
            if (d is not Gradient self) return;
            self.OnUseGammaCorrectionChanged(e);
        }

        protected virtual void OnUseGammaCorrectionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // LerpMode
        public LerpMode LerpMode
        {
            get => (LerpMode)GetValue(LerpModeProperty);
            set => SetValue(LerpModeProperty, value);
        }

        public static readonly DependencyProperty LerpModeProperty = DependencyProperty.Register(nameof(LerpMode), typeof(LerpMode), typeof(Gradient), new FrameworkPropertyMetadata(LerpMode.Rgb, OnLerpModeChanged));

        private static void OnLerpModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onLerpModeChangedCount++;
            if (d is not Gradient self) return;
            self.OnLerpModeChanged(e);
        }

        protected virtual void OnLerpModeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // Bitmap
        // Bitmap controls initialization so do not call dp getter internally
        private static readonly DependencyPropertyKey BitmapPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Bitmap), typeof(WriteableBitmap), typeof(Gradient), new FrameworkPropertyMetadata(default(WriteableBitmap), OnBitmapChanged));

        public static readonly DependencyProperty BitmapProperty = BitmapPropertyKey.DependencyProperty;

        public WriteableBitmap Bitmap
        {
            get
            {
                if (_bitmap == null)
                {
                    CreateBitmap();
                }
                return (WriteableBitmap)GetValue(BitmapProperty);
            }
        }

        protected void SetBitmap(WriteableBitmap value)
        {
            _bitmap = value;
            SetValue(BitmapPropertyKey, value);
        }

        private static void OnBitmapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onBitmapChangedCount++;
            if (d is not Gradient self) return;
            self.OnBitmapChanged(e);
        }

        protected virtual void OnBitmapChanged(DependencyPropertyChangedEventArgs e) { }

        // Brush
        private static readonly DependencyPropertyKey BrushPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Brush), typeof(ImageBrush), typeof(Gradient), new FrameworkPropertyMetadata(null, OnBrushChanged));

        public static readonly DependencyProperty BrushProperty = BrushPropertyKey.DependencyProperty;

        public ImageBrush Brush => (ImageBrush)GetValue(BrushProperty);

        protected void SetBrush(ImageBrush value) => SetValue(BrushPropertyKey, value);

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onBrushChangedCount++;
            if (d is not Gradient self) return;
            self.OnBrushChanged(e);
        }

        protected virtual void OnBrushChanged(DependencyPropertyChangedEventArgs e) { }

        // DrawingSize
        public Size DrawingSize
        {
            get => (Size)GetValue(DrawingSizeProperty);
            set => SetValue(DrawingSizeProperty, value);
        }

        public static readonly DependencyProperty DrawingSizeProperty = DependencyProperty.Register(nameof(DrawingSize), typeof(Size), typeof(Gradient), new FrameworkPropertyMetadata(new Size(300,300), OnDrawingSizeChanged), OnValidate);

        private static void OnDrawingSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Gradient self) return;
            self.OnDrawingSizeChanged(e);
        }

        protected virtual void OnDrawingSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            onDrawingSizeChangedCount++;

            // only create a new bitmap if one was already created. lazy creation so we don't want to rush it
            if (_bitmap != null)
            {
                SetBitmap(new WriteableBitmap((int)DrawingSize.Width, (int)DrawingSize.Height, 96, 96, PixelFormats.Pbgra32, null));
            }
            UpdateGradient();

        }

        private static bool OnValidate(object value)
        {
            if (value is not Size size) return false;

            // only allow integer sizes
            return size.Width == (int)size.Width && size.Height == (int)size.Height;
        }

        #endregion

        #region Constructors

        public Gradient()
        {
            _stops = new FreezableCollection<MultiColorSpaceGradientStop>();
            GradientStops = new FreezableCollection<GradientStop>();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                // All properties should be set by now
                if (_bitmap == null)
                {
                    CreateBitmap();
                }
            }), DispatcherPriority.Loaded);
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
                LerpMode.Srgb => RgbLinear.Lerp(startStop.RgbLinear, endStop.RgbLinear, localProgress).ToColor(UseGammaCorrection),
                LerpMode.Hsl => Hsl.Lerp(startStop.Hsl, endStop.Hsl, localProgress).ToColor(),
                LerpMode.Lab => Lab.Lerp(startStop.Lab, endStop.Lab, localProgress).ToColor(UseGammaCorrection),
                LerpMode.SrgbBrightFix => RgbLinear.BrightFixLerp(startStop.RgbLinear, endStop.RgbLinear, localProgress).ToColor(UseGammaCorrection),
                LerpMode.Hard => startStop.Color,
                _ => throw new NotImplementedException(),
            };

            lerpedColor.A = Utils.LerpByte(startStop.Color.A, endStop.Color.A, localProgress);
            return lerpedColor;
        }

        protected void UpdateGradient()
        {
            updateGradientCount++;

            // guard against multiple updates
            if (_bitmap == null) return;

            updateGradientPastGuardCount++;

            _bitmap?.Lock();
            DrawGradient();
            //_bitmap?.AddDirtyRect(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight));
            _bitmap?.Unlock();

            var brush = new ImageBrush(_bitmap);
            //brush.Freeze();
            SetBrush(brush);
        }

        public abstract void DrawGradient();

        private void CreateBitmap()
        {
            SetBitmap(new WriteableBitmap((int)DrawingSize.Width, (int)DrawingSize.Height, 96, 96, PixelFormats.Pbgra32, null));
            UpdateGradient();
        }

        #endregion

        // Static counters for each event
        private static int onGradientStopsChangedCount = 0;
        private static int onUseGammaCorrectionChangedCount = 0;
        private static int onLerpModeChangedCount = 0;
        private static int onBitmapChangedCount = 0;
        private static int onBrushChangedCount = 0;
        private static int onDrawingSizeChangedCount = 0;
        private static int updateGradientCount = 0;
        private static int updateStopsCount = 0;
        private static int updateGradientPastGuardCount = 0;

        public static void DumpEventCounts()
        {
            Debug.Print($"OnGradientStopsChanged Count: {onGradientStopsChangedCount}");
            Debug.Print($"OnUseGammaCorrectionChanged Count: {onUseGammaCorrectionChangedCount}");
            Debug.Print($"OnLerpModeChanged Count: {onLerpModeChangedCount}");
            Debug.Print($"OnBitmapChanged Count: {onBitmapChangedCount}");
            Debug.Print($"OnBrushChanged Count: {onBrushChangedCount}");
            Debug.Print($"OnDrawingSizeChanged Count: {onDrawingSizeChangedCount}");
            Debug.Print($"UpdateGradient Count: {updateGradientCount}");
            Debug.Print($"UpdateStops Count: {updateStopsCount}");
            Debug.Print($"UpdateGradientPastGuard Count: {updateGradientPastGuardCount}");
        }
    }
}
