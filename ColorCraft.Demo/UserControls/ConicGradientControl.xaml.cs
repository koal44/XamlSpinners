using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorCraft.Demo
{
    public partial class ConicGradientControl : UserControl
    {
        public List<GradientPreset> Presets { get; set; }

        public Gradient Gradient
        {
            get => (Gradient)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        public static readonly DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(Gradient), typeof(ConicGradientControl), new FrameworkPropertyMetadata(default(Gradient), OnGradientChanged));

        private static void OnGradientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientControl self) return;
            self.OnGradientChanged(e);
        }

        protected virtual void OnGradientChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public LerpMode Mode
        {
            get => (LerpMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(LerpMode), typeof(ConicGradientControl), new FrameworkPropertyMetadata(default(LerpMode), OnModeChanged));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientControl self) return;
            self.OnModeChanged(e);
        }

        protected virtual void OnModeChanged(DependencyPropertyChangedEventArgs e)
        {
            Gradient.LerpMode = Mode;
            UpdateGradient();
        }

        public double AngleOffset
        {
            get => (double)GetValue(AngleOffsetProperty);
            set => SetValue(AngleOffsetProperty, value);
        }

        public static readonly DependencyProperty AngleOffsetProperty = DependencyProperty.Register(nameof(AngleOffset), typeof(double), typeof(ConicGradientControl), new FrameworkPropertyMetadata(default(double), OnAngleOffsetChanged));

        private static void OnAngleOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientControl self) return;
            self.OnAngleOffsetChanged(e);
        }

        protected virtual void OnAngleOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public double SpiralStrength
        {
            get => (double)GetValue(SpiralStrengthProperty);
            set => SetValue(SpiralStrengthProperty, value);
        }

        public static readonly DependencyProperty SpiralStrengthProperty = DependencyProperty.Register(nameof(SpiralStrength), typeof(double), typeof(ConicGradientControl), new FrameworkPropertyMetadata(0.0, OnSpiralStrengthChanged));

        private static void OnSpiralStrengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientControl self) return;
            self.OnSpiralStrengthChanged(e);
        }

        protected virtual void OnSpiralStrengthChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public int KaleidoscopeCount
        {
            get => (int)GetValue(KaleidoscopeCountProperty);
            set => SetValue(KaleidoscopeCountProperty, value);
        }

        public static readonly DependencyProperty KaleidoscopeCountProperty = DependencyProperty.Register(nameof(KaleidoscopeCount), typeof(int), typeof(ConicGradientControl), new FrameworkPropertyMetadata(1, OnKaleidoscopeCountChanged));

        private static void OnKaleidoscopeCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientControl self) return;
            self.OnKaleidoscopeCountChanged(e);
        }

        protected virtual void OnKaleidoscopeCountChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public int SelectedGradientStopPresetIndex
        {
            get => (int)GetValue(SelectedGradientStopPresetIndexProperty);
            set => SetValue(SelectedGradientStopPresetIndexProperty, value);
        }

        public static readonly DependencyProperty SelectedGradientStopPresetIndexProperty = DependencyProperty.Register(nameof(SelectedGradientStopPresetIndex), typeof(int), typeof(ConicGradientControl), new FrameworkPropertyMetadata(-1, OnSelectedGradientStopPresetIndexChanged));

        private static void OnSelectedGradientStopPresetIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientControl self) return;
            self.OnSelectedGradientStopPresetIndexChanged(e);
        }

        protected virtual void OnSelectedGradientStopPresetIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            var stops = Presets[SelectedGradientStopPresetIndex].Stops;
            Gradient = new Gradient(Mode, stops, true);

            int width = 300;
            int height = 300;
            Gradient.CreateBitmap(width, height);
            UpdateGradient();
        }

        public ConicGradientControl()
        {
            Presets = new List<GradientPreset>()
            {
                new GradientPreset
                ("Red Yellow Blue", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.Red, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Yellow, 0.5, true),
                    new MultiColorSpaceGradientStop(Colors.Blue, 1.0, true)
                }),
                new GradientPreset
                ("Rainbow", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.Red, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Orange, 0.10, true),
                    new MultiColorSpaceGradientStop(Colors.Yellow, 0.28, true),
                    new MultiColorSpaceGradientStop(Colors.Green, 0.42, true),
                    new MultiColorSpaceGradientStop(Colors.Cyan, 0.56, true),
                    new MultiColorSpaceGradientStop(Colors.Blue, 0.70, true),
                    new MultiColorSpaceGradientStop(Colors.Violet, 0.9, true),
                    new MultiColorSpaceGradientStop(Colors.Red, 1.0, true)  // Wrap back to Red
                }),
                new GradientPreset
                ("Sunset Glow", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.DarkOrange, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Orange, 0.25, true),
                    new MultiColorSpaceGradientStop(Colors.Pink, 0.5, true),
                    new MultiColorSpaceGradientStop(Colors.Purple, 1.0, true)
                }),
                new GradientPreset
                ("Ocean Breeze", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.LightBlue, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.DarkBlue, 0.33, true),
                    new MultiColorSpaceGradientStop(Colors.SeaGreen, 0.67, true),
                    new MultiColorSpaceGradientStop(Colors.LightBlue, 1.0, true)
                }),
                new GradientPreset
                ("Forest Walk", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.DarkGreen, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Brown, 0.4, true),
                    new MultiColorSpaceGradientStop(Colors.Olive, 0.7, true),
                    new MultiColorSpaceGradientStop(Colors.LightGreen, 1.0, true)
                }),
            };

            DataContext = this;
            //InitGradient();
            UpdateGradient();
            InitializeComponent();
            Loaded += OnLoaded;
            SelectedGradientStopPresetIndex = 0;

            //GradientStopsPresetsComboBox.ItemsSource = Presets.Select(p => p.Name);
        }

        private void InitGradient()
        {
            int width = 300;
            int height = 300;

            var stops = new List<GradientStop>
            {
                new GradientStop(Colors.Red, 0.0),
                new GradientStop(Colors.Yellow, 0.33),
                new GradientStop(Colors.Blue, 0.67),
                new GradientStop(Colors.Red, 1.0) // return to first color for a smooth transition
            };

            Gradient = new Gradient(Mode, stops, true);
            Gradient.CreateBitmap(width, height);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LerpSelectionComboBox.ItemsSource = Enum.GetValues(typeof(LerpMode));
            LerpSelectionComboBox.SelectedIndex = 0; // Selects the first mode by default
        }

        private void UpdateGradient()
        {
           if (Gradient == null) return;
           if (Gradient.Bitmap == null) return;

           Gradient.DrawConicGradient(AngleOffset * Math.PI / 180.0, SpiralStrength, KaleidoscopeCount);
        }
    }

    public record GradientPreset(string Name, List<MultiColorSpaceGradientStop> Stops);
}
