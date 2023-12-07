using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorCraft.Demo
{
    public partial class ConicGradientControl : UserControl
    {

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
            UpdateGradient();
        }


        public ConicGradientControl()
        {
            DataContext = this;
            InitializeComponent();
            UpdateGradient();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LerpSelectionComboBox.ItemsSource = Enum.GetValues(typeof(LerpMode));
            LerpSelectionComboBox.SelectedIndex = 0; // Selects the first mode by default
        }

        private void UpdateGradient()
        {
            int width = 300;
            int height = 300;

            var stops = new List<GradientStop>
            {
                new GradientStop(Colors.Red, 0.0),
                new GradientStop(Colors.Yellow, 0.5),
                new GradientStop(Colors.Blue, 1.0)
            };
            double angle = 0;

            WriteableBitmap gradientBitmap = ConicGradient.CreateBitmap(width, height, angle, stops, Mode);

            GradientImage.Source = gradientBitmap;
        }
    }
}
