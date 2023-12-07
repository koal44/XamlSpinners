using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XamlSpinners.Demo
{
    public class UIPropAdjuster
    {
        private static readonly List<ColorItem> ColorOptions = new()
        {
            new ColorItem(Colors.Red, "Red"),
            new ColorItem(Colors.Orange, "Orange"),
            new ColorItem(Colors.Yellow, "Yellow"),
            new ColorItem(Colors.Green, "Green"),
            new ColorItem(Colors.Blue, "Blue"),
            new ColorItem(Colors.Purple, "Purple"),
            new ColorItem(Colors.White, "White"),
            new ColorItem(Colors.Black, "Black"),
        };

        private readonly DependencyProperty _dependencyProperty;

        public string DpName => _dependencyProperty.Name;

        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? SmallStep { get; set; }
        public double? BigStep { get; set; }

        public UIPropAdjuster(DependencyProperty dependencyProperty)
        {
            _dependencyProperty = dependencyProperty;
        }

        public UIPropAdjuster(DependencyProperty dependencyProperty, double min, double max)
        {
            _dependencyProperty = dependencyProperty;
            Min = min;
            Max = max;
        }

        public UIPropAdjuster(DependencyProperty dependencyProperty, double min, double max, double smallStep, double bigStep)
        {
            _dependencyProperty = dependencyProperty;
            Min = min;
            Max = max;
            SmallStep = smallStep;
            BigStep = bigStep;
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

            void updateNumericDisplay(object s, RoutedPropertyChangedEventArgs<double> e)
            {
                updateNumericDisplayText();
            }

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
                case var type when type == typeof(ObservableCollection<Brush>):
                    var brushesCollection = (ObservableCollection<Brush>)spinner.GetValue(_dependencyProperty);
                    var stackPanel = new StackPanel { Orientation = Orientation.Vertical };

                    for (int i = 0; i < brushesCollection.Count; i++)
                    {
                        var comboBox = new ComboBox
                        {
                            ItemsSource = ColorOptions,
                            DisplayMemberPath = "Name",
                            SelectedValuePath = "Brush",
                            //SelectedValue = i < ColorOptions.Count ? ColorOptions[i] : ColorOptions[^1]
                        };
                        if (i > 0) comboBox.Margin = new Thickness(0, 5, 0, 0);

                        // in C# closures capture the variable, not the value. so we need to make a copy.
                        int currentIndex = i; 

                        comboBox.SelectionChanged += (s, e) =>
                        {
                            if (e.AddedItems[0] is not ColorItem selectedColorItem) return;

                            if (currentIndex < brushesCollection.Count)
                                brushesCollection[currentIndex] = selectedColorItem.Brush;
                            else
                                brushesCollection.Add(selectedColorItem.Brush);
                        };

                        stackPanel.Children.Add(comboBox);
                    }

                    return (stackPanel, null);
                case var type when type == typeof(Point):
                    var pointValue = (Point)spinner.GetValue(_dependencyProperty);

                    grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var xPointSlider = CreatePointSlider(spinner, "X", pointValue.X, 0);
                    var yPointSlider = CreatePointSlider(spinner, "Y", pointValue.Y, 1);

                    Action updatePointDisplayText = () =>
                    {
                        valueDisplay.Text = $"({xPointSlider.Value:F2}, {yPointSlider.Value:F2})";
                    };

                    RoutedPropertyChangedEventHandler<double> updatePointDisplay = (s, e) =>
                    {
                        updatePointDisplayText();
                    };

                    updatePointDisplayText();

                    xPointSlider.ValueChanged += updatePointDisplay;
                    yPointSlider.ValueChanged += updatePointDisplay;

                    grid.Children.Add(xPointSlider);
                    grid.Children.Add(yPointSlider);

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
                SmallChange = SmallStep ?? 1,
                LargeChange = BigStep ?? 10,
            };
            if (SmallStep.HasValue)
            {
                slider.IsSnapToTickEnabled = true;
                slider.TickFrequency = SmallStep.Value;
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

        private Slider CreatePointSlider(Spinner spinner, string component, double initialValue, int gridColumn)
        {
            var slider = CreateSlider(initialValue);

            slider.ValueChanged += (s, e) =>
            {
                var point = (Point)spinner.GetValue(_dependencyProperty);
                switch (component)
                {
                    case "X": point.X = e.NewValue; break;
                    case "Y": point.Y = e.NewValue; break;
                }
                spinner.SetValue(_dependencyProperty, point);
            };

            Grid.SetColumn(slider, gridColumn);

            return slider;
        }

    }
}
