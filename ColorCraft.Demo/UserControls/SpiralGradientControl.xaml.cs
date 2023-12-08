using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorCraft.Demo
{
    public partial class SpiralGradientControl : UserControl
    {
        public Gradient Gradient
        {
            get => (Gradient)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        public static readonly DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(Gradient), typeof(SpiralGradientControl), new FrameworkPropertyMetadata(default(Gradient), OnGradientChanged));

        private static void OnGradientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralGradientControl self) return;
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

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(LerpMode), typeof(SpiralGradientControl), new FrameworkPropertyMetadata(default(LerpMode), OnModeChanged));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralGradientControl self) return;
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

        public static readonly DependencyProperty AngleOffsetProperty = DependencyProperty.Register(nameof(AngleOffset), typeof(double), typeof(SpiralGradientControl), new FrameworkPropertyMetadata(default(double), OnAngleOffsetChanged));

        private static void OnAngleOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralGradientControl self) return;
            self.OnAngleOffsetChanged(e);
        }

        protected virtual void OnAngleOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public double TurnIntensity
        {
            get => (double)GetValue(TurnIntensityProperty);
            set => SetValue(TurnIntensityProperty, value);
        }

        public static readonly DependencyProperty TurnIntensityProperty = DependencyProperty.Register(nameof(TurnIntensity), typeof(double), typeof(SpiralGradientControl), new FrameworkPropertyMetadata(5.0, OnTurnIntensityChanged));

        private static void OnTurnIntensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralGradientControl self) return;
            self.OnTurnIntensityChanged(e);
        }

        protected virtual void OnTurnIntensityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        public SpiralGradientControl()
        {
            DataContext = this;
            InitGradient();
            UpdateGradient();
            InitializeComponent();
            Loaded += OnLoaded;
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

           Gradient.DrawSpiralGradient(AngleOffset * Math.PI / 180.0, TurnIntensity);
        }
    }
}
