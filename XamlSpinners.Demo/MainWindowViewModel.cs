using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                    new AdjustableProperty(SpiralSphere.SurfacePointCountProperty, 20, 1000),
                    new AdjustableProperty(SpiralSphere.AzimuthalToInclineRatioProperty, 0, 100),
                    new AdjustableProperty(SpiralSphere.SpiralPatternProperty),
                    new AdjustableProperty(SpiralSphere.CameraDirectionProperty, -1, 1),
                    new AdjustableProperty(SpiralSphere.UpDirectionProperty, -1, 1),
                    new AdjustableProperty(SpiralSphere.FieldOfViewProperty, 10, 120),
                    new AdjustableProperty(SpiralSphere.AxisOfRationProperty, -1, 1),
                    new AdjustableProperty(SpiralSphere.StretchProperty),
                }),
            };
            SelectedSpinnerConfig = SpinnerConfigs.First().Value;

            PropertyChanged += OnPropertyChanged;

            //UpdateAllSpinners();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case nameof(SelectedSpeed):
                //case nameof(SelectedIsActive):
                //case nameof(SelectedForeground):
                //    //UpdateAllSpinners();
                //    break;
            }
        }

    }

    public class AdjustableProperty
    {
        private readonly DependencyProperty _dependencyProperty;

        public string DpName => _dependencyProperty.Name;

        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Step { get; set; }

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

        public FrameworkElement CreateAdjustableControl(Spinner spinner)
        {
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
                    return checkBoxControl;
                case var type when type == typeof(int):
                    return CreateSlider(spinner, _dependencyProperty, (int)spinner.GetValue(_dependencyProperty), type);
                case var type when type == typeof(float):
                    return CreateSlider(spinner, _dependencyProperty, (float)spinner.GetValue(_dependencyProperty), type);
                case var type when type == typeof(double):
                    return CreateSlider(spinner, _dependencyProperty, (double)spinner.GetValue(_dependencyProperty), type);
                case var type when type.IsEnum:
                    var comboBoxControl = new ComboBox
                    {
                        ItemsSource = Enum.GetValues(type),
                        SelectedItem = spinner.GetValue(_dependencyProperty),
                    };
                    comboBoxControl.SelectionChanged +=
                        (s, e) => spinner.SetValue(_dependencyProperty, e.AddedItems[0]);
                    return comboBoxControl;
                case var type when type == typeof(Vector3):
                    var vectorValue = (Vector3)spinner.GetValue(_dependencyProperty);

                    var grid = new Grid();
                    for (int i = 0; i < 3; i++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }

                    grid.Children.Add(CreateVectorComponentSlider(spinner, "X", vectorValue.X, 0));
                    grid.Children.Add(CreateVectorComponentSlider(spinner, "Y", vectorValue.Y, 1));
                    grid.Children.Add(CreateVectorComponentSlider(spinner, "Z", vectorValue.Z, 2));

                    return grid;
                default:
                    throw new NotSupportedException($"Type {_dependencyProperty.PropertyType} is not supported.");
            }
            
        }

        private Slider CreateVectorComponentSlider(Spinner spinner, string component, double initialValue, int gridRow)
        {
            var slider = new Slider
            {
                Value = initialValue,
                Minimum = Min ?? -100,
                Maximum = Max ?? 100,
                SmallChange = Step ?? 1,
                LargeChange = Step ?? 10,
            };

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

        private Slider CreateSlider(Spinner spinner, DependencyProperty property, double initialValue, Type propertyType)
        {
            var slider = new Slider
            {
                Value = initialValue,
                Minimum = Min ?? 0,
                Maximum = Max ?? 100,
                SmallChange = Step ?? 1,
                LargeChange = Step ?? 10,
            };

            slider.ValueChanged += (s, e) =>
            {
                switch (propertyType)
                {
                    case var type when type == typeof(int):
                        spinner.SetValue(property, Convert.ToInt32(e.NewValue));
                        break;
                    case var type when type == typeof(float):
                        spinner.SetValue(property, (float)e.NewValue);
                        break;
                    case var type when type == typeof(double):
                        spinner.SetValue(property, (double)e.NewValue);
                        break;
                    default:
                        throw new NotSupportedException($"Type {propertyType} is not supported.");
                }
            };

            return slider;
        }

    }


    public class SpinnerConfig
    {
        // This class uses reflection to update properties dynamically, as not all LoadingIndicators share the same set of properties.

        private readonly Dictionary<string, Action<Spinner, object>> _settersCache = new();

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
                },
            };

            for (int i = 0; i < adjustableProperties.Count; i++)
            {
                var adjustableProperty = adjustableProperties[i] 
                    ?? throw new NullReferenceException($"adjustableProperty[${i}]");
                AdjustablePropertiesGrid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

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

                var adjustableControl = adjustableProperty.CreateAdjustableControl(ConfigurableSpinner);
                adjustableControl.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(adjustableControl, i*2);
                Grid.SetColumn(adjustableControl, 1);
                AdjustablePropertiesGrid.Children.Add(adjustableControl);

                // spacer
                AdjustablePropertiesGrid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(10, GridUnitType.Pixel) });
            }

            //CacheSetters(ConfigurableSpinner);
        }

        private void CacheSetters(Spinner indicator)
        {
            var properties = indicator.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite && property.GetSetMethod() != null)
                {
                    _settersCache[property.Name] = (indicator, value) => property.SetValue(indicator, value);
                }
            }
        }

        private void SetPropertyValue(string propertyName, object value)
        {
            if (_settersCache.TryGetValue(propertyName, out var setter))
            {
                var currentValue = ConfigurableSpinner.GetType().GetProperty(propertyName)?.GetValue(ConfigurableSpinner);
                if (!Equals(currentValue, value))
                {
                    setter(ConfigurableSpinner, value);
                }
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
