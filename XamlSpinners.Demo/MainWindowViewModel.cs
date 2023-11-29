using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XamlSpinners.Demo
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private bool _selectedIsActive = true;
        [ObservableProperty] private double _selectedSpeed = 1.0;
        [ObservableProperty] private Brush _selectedForeground = new SolidColorBrush(Colors.Red);
        [ObservableProperty] private SpinnerConfig _selectedSpinnerConfig;

        public Dictionary<Type, SpinnerConfig> SpinnerConfigs { get; }
        [ObservableProperty] private List<ColorItem> _colorOptions;

        public MainWindowViewModel()
        {
            ColorOptions = new List<ColorItem>
            {
                new ColorItem(Colors.Red, "Red"),
                new ColorItem(Colors.Orange, "Orange"),
            };

            SpinnerConfigs = new()
            {
                [typeof(SpiralSphere)] =
                new SpinnerConfig(typeof(SpiralSphere), new List<AdjustableProperty>
                {
                    new AdjustableProperty(Spinner.IsActiveProperty),
                    new AdjustableProperty(SpiralSphere.SurfacePointCountProperty, 20, 1000, 1, true),
                    new AdjustableProperty(SpiralSphere.AzimuthalToInclineRatioProperty, 0, 100),
                    new AdjustableProperty(SpiralSphere.SpiralPatternProperty),
                    new AdjustableProperty(SpiralSphere.SurfacePointRelativeSizeProperty, 0.005, 0.1),
                    new AdjustableProperty(SpiralSphere.CameraDirectionProperty, -1, 1),
                    new AdjustableProperty(SpiralSphere.UpDirectionProperty, -1, 1),
                    new AdjustableProperty(SpiralSphere.FieldOfViewProperty, 10, 120),
                    new AdjustableProperty(SpiralSphere.DepthExaggerationProperty, 0, 3),
                    new AdjustableProperty(SpiralSphere.AxisOfRationProperty, -1, 1),
                    new AdjustableProperty(SpiralSphere.StretchProperty),
                }),
            };
            SelectedSpinnerConfig = SpinnerConfigs.First().Value;
        }
    }

    public class AdjustableProperty
    {
        private readonly DependencyProperty _dependencyProperty;

        public string DpName => _dependencyProperty.Name;

        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Step { get; set; }
        public bool IsInt { get; set; } = false;

        public AdjustableProperty(DependencyProperty dependencyProperty)
        {
            _dependencyProperty = dependencyProperty;
        }

        public AdjustableProperty(DependencyProperty dependencyProperty, double min, double max)
        {
            _dependencyProperty = dependencyProperty;
            Min = min;
            Max = max;
        }

        public AdjustableProperty(DependencyProperty dependencyProperty, double min, double max, double step, bool isInt)
        {
            _dependencyProperty = dependencyProperty;
            Min = min;
            Max = max;
            Step = step;
            IsInt = isInt;
        }

        public (FrameworkElement Control, TextBlock? ValueDisplay) CreateAdjustableControl(Spinner spinner)
        {
            Grid grid;
            Slider? slider = null;
            TextBlock valueDisplay = new() 
            { 
                Margin = new Thickness(10, 0, 0, 0),
            };

            void updateNumericDisplayText()
            {
                valueDisplay.Text = $"{slider?.Value:F2}";
            }

            RoutedPropertyChangedEventHandler<double> updateNumericDisplay = (s, e) =>
            {
                updateNumericDisplayText();
            };

            switch (_dependencyProperty.PropertyType)
            {
                case var type when type == typeof(bool):
                    var checkBoxControl = new CheckBox
                    {
                        IsChecked = (bool)spinner.GetValue(_dependencyProperty),
                    };
                    checkBoxControl.Checked += 
                        (s, e) => spinner.SetValue(_dependencyProperty, true);
                    checkBoxControl.Unchecked +=
                        (s, e) => spinner.SetValue(_dependencyProperty, false);
                    return (checkBoxControl, null);
                case var type when type == typeof(int):
                    slider = CreateNumericSlider(spinner, (int)spinner.GetValue(_dependencyProperty));
                    slider.ValueChanged += updateNumericDisplay;
                    updateNumericDisplayText();
                    return (slider, valueDisplay);
                case var type when type == typeof(float):
                    slider = CreateNumericSlider(spinner, (float)spinner.GetValue(_dependencyProperty));
                    slider.ValueChanged += updateNumericDisplay;
                    updateNumericDisplayText();
                    return (slider, valueDisplay);
                case var type when type == typeof(double):
                    slider = CreateNumericSlider(spinner, (double)spinner.GetValue(_dependencyProperty));
                    slider.ValueChanged += updateNumericDisplay;
                    updateNumericDisplayText();
                    return (slider, valueDisplay);
                case var type when type.IsEnum:
                    var comboBoxControl = new ComboBox
                    {
                        ItemsSource = Enum.GetValues(type),
                        SelectedItem = spinner.GetValue(_dependencyProperty),
                    };
                    comboBoxControl.SelectionChanged +=
                        (s, e) => spinner.SetValue(_dependencyProperty, e.AddedItems[0]);
                    return (comboBoxControl, null);
                case var type when type == typeof(Vector3):
                    var vector3Value = (Vector3)spinner.GetValue(_dependencyProperty);

                    grid = new Grid();
                    for (int i = 0; i < 3; i++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }

                    var xSlider = CreateVector3Slider(spinner, "X", vector3Value.X, 0);
                    var ySlider = CreateVector3Slider(spinner, "Y", vector3Value.Y, 1);
                    var zSlider = CreateVector3Slider(spinner, "Z", vector3Value.Z, 2);

                    Action updateVectorDisplayText = () =>
                    {
                        valueDisplay.Text = $"({xSlider.Value:F2}, {ySlider.Value:F2}, {zSlider.Value:F2})";
                    };

                    RoutedPropertyChangedEventHandler<double> updateVectorDisplay = (s, e) =>
                    {
                        updateVectorDisplayText();
                    };

                    updateVectorDisplayText();
                    valueDisplay.Width = 100;

                    xSlider.ValueChanged += updateVectorDisplay;
                    ySlider.ValueChanged += updateVectorDisplay;
                    zSlider.ValueChanged += updateVectorDisplay;

                    grid.Children.Add(xSlider);
                    grid.Children.Add(ySlider);
                    grid.Children.Add(zSlider);

                    return (grid, valueDisplay);
                case var type when type == typeof(Size):
                    var sizeValue = (Size)spinner.GetValue(_dependencyProperty);

                    grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var widthSlider = CreateSizeSlider(spinner, "Width", sizeValue.Width, 0);
                    var heightSlider = CreateSizeSlider(spinner, "Height", sizeValue.Height, 1);

                    Action updateSizeDisplayText = () =>
                    {
                        valueDisplay.Text = $"({widthSlider.Value:F2}, {heightSlider.Value:F2})";
                    };

                    RoutedPropertyChangedEventHandler<double> updateSizeDisplay = (s, e) =>
                    {
                        updateSizeDisplayText();
                    };

                    updateSizeDisplayText();

                    widthSlider.ValueChanged += updateSizeDisplay;
                    heightSlider.ValueChanged += updateSizeDisplay;

                    grid.Children.Add(widthSlider);
                    grid.Children.Add(heightSlider);

                    return (grid, valueDisplay);
                default:
                    throw new NotSupportedException($"Type {_dependencyProperty.PropertyType} is not supported.");
            }
            
        }

        private Slider CreateSlider(double initialValue)
        {
            var slider = new Slider
            {
                Value = initialValue,
                Minimum = Min ?? 0,
                Maximum = Max ?? 100,
                SmallChange = Step ?? 1,
                LargeChange = Step ?? 10,
            };
            if (IsInt)
            {
                slider.IsSnapToTickEnabled = true;
                slider.TickFrequency = Step ?? 1;
            }
            return slider;
        }

        private Slider CreateNumericSlider(Spinner spinner, double initialValue)
        {
            var slider = CreateSlider(initialValue);

            slider.ValueChanged += (s, e) =>
            {
                switch (_dependencyProperty.PropertyType)
                {
                    case var type when type == typeof(int):
                        spinner.SetValue(_dependencyProperty, Convert.ToInt32(e.NewValue)); break;
                    case var type when type == typeof(float):
                        spinner.SetValue(_dependencyProperty, (float)e.NewValue); break;
                    case var type when type == typeof(double):
                        spinner.SetValue(_dependencyProperty, (double)e.NewValue); break;
                    default:
                        throw new NotSupportedException($"Type {_dependencyProperty.PropertyType} is not supported.");
                }
            };

            return slider;
        }

        private Slider CreateVector3Slider(Spinner spinner, string component, double initialValue, int gridRow)
        {
            var slider = CreateSlider(initialValue);

            slider.ValueChanged += (s, e) =>
            {
                var vector = (Vector3)spinner.GetValue(_dependencyProperty);
                switch (component)
                {
                    case "X": vector.X = (float)e.NewValue; break;
                    case "Y": vector.Y = (float)e.NewValue; break;
                    case "Z": vector.Z = (float)e.NewValue; break;
                }
                spinner.SetValue(_dependencyProperty, vector);
            };

            Grid.SetColumn(slider, gridRow);

            return slider;
        }

        private Slider CreateSizeSlider(Spinner spinner, string component, double initialValue, int gridColumn)
        {
            var slider = CreateSlider(initialValue);

            slider.ValueChanged += (s, e) =>
            {
                var size = (Size)spinner.GetValue(_dependencyProperty);
                switch (component)
                {
                    case "Width": size.Width = e.NewValue; break;
                    case "Height": size.Height = e.NewValue; break;
                }
                spinner.SetValue(_dependencyProperty, size);
            };

            Grid.SetColumn(slider, gridColumn);

            return slider;
        }
    }

    public class SpinnerConfig
    {
        public Spinner ThumbnailSpinner { get; }
        public Spinner ConfigurableSpinner { get; }
        public Grid AdjustablePropertiesGrid { get; }

        public SpinnerConfig(Type spinnerType, List<AdjustableProperty> adjustableProperties)
        {
            ThumbnailSpinner = (Spinner?)Activator.CreateInstance(spinnerType) 
                ?? throw new NullReferenceException(nameof(ThumbnailSpinner));

            ConfigurableSpinner = (Spinner?)Activator.CreateInstance(spinnerType) 
                ?? throw new NullReferenceException(nameof(ConfigurableSpinner));

            AdjustablePropertiesGrid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
            };

            for (int i = 0; i < adjustableProperties.Count; i++)
            {
                var adjustableProperty = adjustableProperties[i] 
                    ?? throw new NullReferenceException($"adjustableProperty[${i}]");
                AdjustablePropertiesGrid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                // label
                var label = new TextBlock
                {
                    Text = adjustableProperty.DpName,
                    TextAlignment = TextAlignment.Right,
                    Margin = new Thickness(0, 0, 10, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(label, i*2);
                Grid.SetColumn(label, 0);
                AdjustablePropertiesGrid.Children.Add(label);

                // adjustable control
                var (adjustableControl, valueDisplay) 
                    = adjustableProperty.CreateAdjustableControl(ConfigurableSpinner);
                adjustableControl.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(adjustableControl, i*2);
                Grid.SetColumn(adjustableControl, 1);
                AdjustablePropertiesGrid.Children.Add(adjustableControl);

                // value display
                if (valueDisplay != null)
                {
                    valueDisplay.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetRow(valueDisplay, i*2);
                    Grid.SetColumn(valueDisplay, 2);
                    AdjustablePropertiesGrid.Children.Add(valueDisplay);
                }

                // spacer
                AdjustablePropertiesGrid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(10, GridUnitType.Pixel) });
            }

        }
    }

    public class ColorItem
    {
        public Color Color { get; }
        public string Name { get; }
        public Brush Brush => new SolidColorBrush(Color);

        public ColorItem(Color color, string name)
        {
            Color = color;
            Name = name;
        }
    }


}
