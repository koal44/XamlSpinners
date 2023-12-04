using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlSpinners.Demo
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private SpinnerConfig _selectedSpinnerConfig;

        public Dictionary<Type, SpinnerConfig> SpinnerConfigs { get; }

        public List<Spinner> ThumbnailSpinners => SpinnerConfigs.Values.Select(x => x.ThumbnailSpinner).ToList();

        public MainWindowViewModel()
        {
            SpinnerConfigs = new()
            {
                [typeof(PulseRings)] =
                new SpinnerConfig(typeof(PulseRings), new List<UIPropAdjuster>
                {
                    new UIPropAdjuster(Spinner.IsActiveProperty),
                    new UIPropAdjuster(Spinner.SpeedProperty, -3, 3),
                    new UIPropAdjuster(Spinner.PaletteProperty),
                    new UIPropAdjuster(PulseRings.RingThicknessProperty, 0.01, 0.9),
                    new UIPropAdjuster(PulseRings.RingGapProperty, 0.01, 0.9),
                }),
                [typeof(SpiralSphere3d)] =
                new SpinnerConfig(typeof(SpiralSphere3d), new List<UIPropAdjuster>
                {
                    new UIPropAdjuster(Spinner.IsActiveProperty),
                    new UIPropAdjuster(Spinner.SpeedProperty, -3, 3),
                    new UIPropAdjuster(Spinner.PaletteProperty),
                    new UIPropAdjuster(SpiralSphere3d.BlockCountProperty, 20, 1000, 1, 50),
                    new UIPropAdjuster(SpiralSphere3d.AzimuthalToInclineRatioProperty, 0, 500, 1, 10),
                }),
                [typeof(SpiralSphere)] =
                new SpinnerConfig(typeof(SpiralSphere), new List<UIPropAdjuster>
                {
                    new UIPropAdjuster(Spinner.IsActiveProperty),
                    new UIPropAdjuster(Spinner.SpeedProperty, -3, 3),
                    new UIPropAdjuster(Spinner.PaletteProperty),
                    new UIPropAdjuster(SpiralSphere.SurfacePointCountProperty, 20, 1000, 1, 50),
                    new UIPropAdjuster(SpiralSphere.AzimuthalToInclineRatioProperty, 0, 500, 1, 10),
                    new UIPropAdjuster(SpiralSphere.SpiralPatternProperty),
                    new UIPropAdjuster(SpiralSphere.SurfacePointRelativeSizeProperty, 0.005, 0.1),
                    new UIPropAdjuster(SpiralSphere.CameraDirectionProperty, -1, 1),
                    new UIPropAdjuster(SpiralSphere.UpDirectionProperty, -1, 1),
                    new UIPropAdjuster(SpiralSphere.FieldOfViewProperty, 10, 120),
                    new UIPropAdjuster(SpiralSphere.DepthExaggerationProperty, 0, 3),
                    new UIPropAdjuster(SpiralSphere.AxisOfRationProperty, -1, 1),
                    new UIPropAdjuster(SpiralSphere.StretchProperty),
                }),
                //[typeof(Grad)] =
                //new SpinnerConfig(typeof(Grad), new List<UIPropAdjuster>
                //{
                //    new UIPropAdjuster(Grad.GradientProperty),
                //    new UIPropAdjuster(Grad.BlueOffsetProperty, 0, 1),
                //    new UIPropAdjuster(Grad.RedOffsetProperty, 0, 1),
                //    new UIPropAdjuster(Grad.SpreadMethodProperty),
                //    new UIPropAdjuster(Grad.CenterProperty, 0, 1),
                //    new UIPropAdjuster(Grad.RadiusXProperty, 0, 1),
                //    new UIPropAdjuster(Grad.RadiusYProperty, 0, 1),
                //    new UIPropAdjuster(Grad.GradientOriginProperty, 0, 1),
                //    new UIPropAdjuster(Grad.StartPointProperty, 0, 1),
                //    new UIPropAdjuster(Grad.EndPointProperty, 0, 1),
                //}),
            };
            SelectedSpinnerConfig = SpinnerConfigs.First().Value;
        }

        [RelayCommand]
        private void OnThumbnailClicked(Spinner spinner)
        {
            var spinnerType = spinner.GetType();
            if (SpinnerConfigs.TryGetValue(spinnerType, out var config))
            {
                SelectedSpinnerConfig = config;
            }
        }
    }
}
