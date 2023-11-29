using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlSpinners.Demo
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private SpinnerConfig _selectedSpinnerConfig;

        public Dictionary<Type, SpinnerConfig> SpinnerConfigs { get; }

        public MainWindowViewModel()
        {
            SpinnerConfigs = new()
            {
                [typeof(SpiralSphere)] =
                new SpinnerConfig(typeof(SpiralSphere), new List<UIPropAdjuster>
                {
                    new UIPropAdjuster(Spinner.IsActiveProperty),
                    new UIPropAdjuster(Spinner.SpeedProperty, -3, 3),
                    new UIPropAdjuster(Spinner.PaletteProperty),
                    new UIPropAdjuster(SpiralSphere.SurfacePointCountProperty, 20, 1000, 1, 50),
                    new UIPropAdjuster(SpiralSphere.AzimuthalToInclineRatioProperty, 0, 100),
                    new UIPropAdjuster(SpiralSphere.SpiralPatternProperty),
                    new UIPropAdjuster(SpiralSphere.SurfacePointRelativeSizeProperty, 0.005, 0.1),
                    new UIPropAdjuster(SpiralSphere.CameraDirectionProperty, -1, 1),
                    new UIPropAdjuster(SpiralSphere.UpDirectionProperty, -1, 1),
                    new UIPropAdjuster(SpiralSphere.FieldOfViewProperty, 10, 120),
                    new UIPropAdjuster(SpiralSphere.DepthExaggerationProperty, 0, 3),
                    new UIPropAdjuster(SpiralSphere.AxisOfRationProperty, -1, 1),
                    new UIPropAdjuster(SpiralSphere.StretchProperty),
                }),
            };
            SelectedSpinnerConfig = SpinnerConfigs.First().Value;
        }
    }
}
