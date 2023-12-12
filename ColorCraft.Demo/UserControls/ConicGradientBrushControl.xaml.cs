using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ColorCraft.Demo
{
    public partial class ConicGradientBrushControl : UserControl
    {

        public int SelectedGradientStopPresetIndex
        {
            get => (int)GetValue(SelectedGradientStopPresetIndexProperty);
            set => SetValue(SelectedGradientStopPresetIndexProperty, value);
        }

        public static readonly DependencyProperty SelectedGradientStopPresetIndexProperty = DependencyProperty.Register(nameof(SelectedGradientStopPresetIndex), typeof(int), typeof(ConicGradientBrushControl), new FrameworkPropertyMetadata(-1, OnSelectedGradientStopPresetIndexChanged));

        private static void OnSelectedGradientStopPresetIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientBrushControl self) return;

            var stops = self.Presets[self.SelectedGradientStopPresetIndex].Stops;

            var gradient = (ConicGradient)self.Resources["MyGradient"] ?? throw new NullReferenceException();
            gradient.GradientStops = stops;
        }

        public List<GradientPreset> Presets { get; set; }

        public ConicGradientBrushControl()
        {
            DataContext = this;
            Presets = GradientPreset.GetDefaultPresets();
            InitializeComponent();
            SelectedGradientStopPresetIndex = 0;
            LerpSelectionComboBox.ItemsSource = Enum.GetValues(typeof(LerpMode));
            LerpSelectionComboBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var gradient = (ConicGradient)Resources["MyGradient"] ?? throw new NullReferenceException();
            Gradient.DumpEventCounts();
            LinearGradient.DumpLinearEventCounts();
            ConicGradient.DumpConicEventCounts();
        }
    }
}
