using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XamlSpinners.Demo
{
    public class SpinnerConfig
    {
        public Spinner ThumbnailSpinner { get; }
        public Spinner ConfigurableSpinner { get; }
        public Grid AdjusterControlsGrid { get; }

        public SpinnerConfig(Type spinnerType, List<UIPropAdjuster> adjustableProperties)
        {
            ThumbnailSpinner = (Spinner?)Activator.CreateInstance(spinnerType) 
                ?? throw new NullReferenceException(nameof(ThumbnailSpinner));
            ThumbnailSpinner.Palette[0] = Brushes.White;
            ThumbnailSpinner.Palette[1] = Brushes.White;

            ConfigurableSpinner = (Spinner?)Activator.CreateInstance(spinnerType) 
                ?? throw new NullReferenceException(nameof(ConfigurableSpinner));

            AdjusterControlsGrid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
            };


            // spinner name
            var spinnerNameLabel = new TextBlock
            {
                Text = spinnerType.Name,
                TextDecorations = TextDecorations.Underline,
                FontSize = 16,
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 0, 10, 10),
                VerticalAlignment = VerticalAlignment.Center
            };

            AdjusterControlsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            Grid.SetRow(spinnerNameLabel, 0);
            //Grid.SetColumnSpan(spinnerNameLabel, 3);
            AdjusterControlsGrid.Children.Add(spinnerNameLabel);

            // loop over adjustable properties
            int rowStart = 1;
            for (int i = 0; i < adjustableProperties.Count; i++)
            {
                var adjustableProperty = adjustableProperties[i] 
                    ?? throw new NullReferenceException($"adjustableProperty[${i}]");
                AdjusterControlsGrid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                int row = i * 2 + rowStart;

                // label
                var label = new TextBlock
                {
                    Text = adjustableProperty.DpName,
                    TextAlignment = TextAlignment.Right,
                    Margin = new Thickness(0, 0, 10, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(label, row);
                Grid.SetColumn(label, 0);
                AdjusterControlsGrid.Children.Add(label);

                // adjustable control
                var (adjustableControl, valueDisplay) 
                    = adjustableProperty.CreateAdjustableControl(ConfigurableSpinner);
                adjustableControl.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(adjustableControl, row);
                Grid.SetColumn(adjustableControl, 1);
                AdjusterControlsGrid.Children.Add(adjustableControl);

                // value display
                if (valueDisplay != null)
                {
                    valueDisplay.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetRow(valueDisplay, row);
                    Grid.SetColumn(valueDisplay, 2);
                    AdjusterControlsGrid.Children.Add(valueDisplay);
                }

                // spacer
                AdjusterControlsGrid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(10, GridUnitType.Pixel) });
            }
        }

    }
}
