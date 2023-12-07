using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorCraft.Demo
{
    public partial class LinearGradientControl : UserControl
    {
        private readonly int _imgWidth = 400;
        private readonly int _imgHeight = 40;

        private readonly List<(string startGradient, string endGradient)> _gradientPairs = new()
        {
            ("#FF0000", "#00FF00"),
            ("#00FF00", "#0000FF"),
            ("#0000FF", "#FF0000"),
            ("#000000", "#FFFFFF"),
            ("#FF0000", "#FFFFFF"),
            ("#FF0000", "#000000"),
        };

        public LinearGradientControl()
        {
            InitializeComponent();

            foreach (var (startGradient, endGradient) in _gradientPairs)
            {
                var startColor = (Color)ColorConverter.ConvertFromString(startGradient);
                var endColor = (Color)ColorConverter.ConvertFromString(endGradient);

                var panel = CreateGridGradientComparison(startColor, endColor);
                RootContainer.Children.Add(panel);
            }
        }

        private Grid CreateGridGradientComparison(Color startColorString, Color endColorString)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // create a row with gradient image for each lerp mode
            var lerpModes = Enum.GetValues<LerpMode>().ToList();
            for (int i = 0; i < lerpModes.Count; i++)
            {
                var mode = lerpModes[i];

                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // gradient image
                var gradStops = new List<GradientStop>
                {
                    new GradientStop(startColorString, 0),
                    new GradientStop(endColorString, 1)
                };
                var bitmap = LinearGradient.CreateBitmap(_imgWidth, _imgHeight, gradStops, mode);
                var image = new Image
                {
                    Width = _imgWidth,
                    Height = _imgHeight,
                    Source = bitmap,
                };
                Grid.SetRow(image, i);
                Grid.SetColumn(image, 0);
                grid.Children.Add(image);

                // lerp mode label
                var lerpedModeLabel = new Label { Content = mode.ToString(), Margin = new Thickness(0,0,20,0) };
                Grid.SetRow(lerpedModeLabel, i);
                Grid.SetColumn(lerpedModeLabel, 1);
                grid.Children.Add(lerpedModeLabel);
            }

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var margin = new Thickness(0, -5, 0, 10);

            // start color label
            var startColorLabel = new Label { Content = startColorString, HorizontalAlignment = HorizontalAlignment.Left, Margin = margin };
            Grid.SetRow(startColorLabel, grid.RowDefinitions.Count);
            Grid.SetColumn(startColorLabel, 0);
            grid.Children.Add(startColorLabel);

            // end color label
            var endColorLabel = new Label { Content = endColorString, HorizontalAlignment = HorizontalAlignment.Right, Margin = margin };
            Grid.SetRow(endColorLabel, grid.RowDefinitions.Count);
            Grid.SetColumn(endColorLabel, 0);
            grid.Children.Add(endColorLabel);

            return grid;
        }

    }
}
