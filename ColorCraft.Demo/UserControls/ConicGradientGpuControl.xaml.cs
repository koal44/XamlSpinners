using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ColorCraft.Demo
{
    public partial class ConicGradientGpuControl : UserControl
    {
        #region Properties

        public List<GradientPreset> Presets { get; set; }

        #endregion

        #region Dependency Properties

        // Gradient
        public ConicGradientGpu Gradient
        {
            get => (ConicGradientGpu)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        public static readonly DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(ConicGradientGpu), typeof(ConicGradientGpuControl), new FrameworkPropertyMetadata(default(ConicGradientGpu), OnGradientChanged));

        private static void OnGradientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientGpuControl self) return;
            self.OnGradientChanged(e);
        }

        protected virtual void OnGradientChanged(DependencyPropertyChangedEventArgs e) { }

        // SelectedGradientStopPresetIndex
        public int SelectedGradientStopPresetIndex
        {
            get => (int)GetValue(SelectedGradientStopPresetIndexProperty);
            set => SetValue(SelectedGradientStopPresetIndexProperty, value);
        }

        public static readonly DependencyProperty SelectedGradientStopPresetIndexProperty = DependencyProperty.Register(nameof(SelectedGradientStopPresetIndex), typeof(int), typeof(ConicGradientGpuControl), new FrameworkPropertyMetadata(-1, OnSelectedGradientStopPresetIndexChanged));

        private static void OnSelectedGradientStopPresetIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ConicGradientGpuControl self) return;
            self.OnSelectedGradientStopPresetIndexChanged(e);
        }

        protected virtual void OnSelectedGradientStopPresetIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            var stops = Presets[SelectedGradientStopPresetIndex].Stops;
            Gradient.GradientStops = stops;
        }

        #endregion

        #region Constructors

        public ConicGradientGpuControl()
        {
            DataContext = this;
            Presets = GradientPreset.GetDefaultPresets();
            Gradient = new ConicGradientGpu();
            InitializeComponent();
            SelectedGradientStopPresetIndex = 0;
            LerpSelectionComboBox.ItemsSource = Enum.GetValues(typeof(LerpMode));
            LerpSelectionComboBox.SelectedIndex = 0;
        }

        #endregion

    }
}
