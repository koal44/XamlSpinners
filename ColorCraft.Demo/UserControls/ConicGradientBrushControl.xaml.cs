using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ColorCraft.Demo
{
    public partial class ConicGradientBrushControl : UserControl
    {
        public List<GradientPreset> Presets { get; set; }

        public Gradient Gradient
        {
            get => (Gradient)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        public static readonly DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(Gradient), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(default(Gradient), OnGradientChanged));

        private static void OnGradientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;
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

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(LerpMode), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(default(LerpMode), OnModeChanged));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;
            self.OnModeChanged(e);
        }

        protected virtual void OnModeChanged(DependencyPropertyChangedEventArgs e)
        {
            Gradient.LerpMode = Mode;
            UpdateGradient();
        }

        public float AngleOffset
        {
            get => (float)GetValue(AngleOffsetProperty);
            set => SetValue(AngleOffsetProperty, value);
        }

        public static readonly DependencyProperty AngleOffsetProperty = DependencyProperty.Register(nameof(AngleOffset), typeof(float), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(270f, OnAngleOffsetChanged));

        private static void OnAngleOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;
            self.OnAngleOffsetChanged(e);
        }

        protected virtual void OnAngleOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public float SpiralStrength
        {
            get => (float)GetValue(SpiralStrengthProperty);
            set => SetValue(SpiralStrengthProperty, value);
        }

        public static readonly DependencyProperty SpiralStrengthProperty = DependencyProperty.Register(nameof(SpiralStrength), typeof(float), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(0f, OnSpiralStrengthChanged));

        private static void OnSpiralStrengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;
            self.OnSpiralStrengthChanged(e);
        }

        protected virtual void OnSpiralStrengthChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public float KaleidoscopeCount
        {
            get => (float)GetValue(KaleidoscopeCountProperty);
            set => SetValue(KaleidoscopeCountProperty, value);
        }

        public static readonly DependencyProperty KaleidoscopeCountProperty = DependencyProperty.Register(nameof(KaleidoscopeCount), typeof(float), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(1f, OnKaleidoscopeCountChanged));

        private static void OnKaleidoscopeCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;
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

        public static readonly DependencyProperty SelectedGradientStopPresetIndexProperty = DependencyProperty.Register(nameof(SelectedGradientStopPresetIndex), typeof(int), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(-1, OnSelectedGradientStopPresetIndexChanged));

        private static void OnSelectedGradientStopPresetIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;
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
            Gradient.CreateBrush();
        }

        public ConicGradientBrushControl()
        {
            Presets = GradientPreset.GetDefaultPresets();
            DataContext = this;
            UpdateGradient();
            InitializeComponent();
            Loaded += OnLoaded;
            SelectedGradientStopPresetIndex = 0;
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

            var startOffset = AngleOffset * MathF.PI / 180;
            Gradient.DrawConicGradient(startOffset, SpiralStrength, KaleidoscopeCount);
        }
    }
}
