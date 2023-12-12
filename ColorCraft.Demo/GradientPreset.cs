using System.Collections.Generic;
using System.Windows.Media;

namespace ColorCraft.Demo
{
    public record GradientPreset(string Name, FreezableCollection<GradientStop> Stops)
    {
        public static List<GradientPreset> GetDefaultPresets()
        {
            return new List<GradientPreset>
            {
                new("Red Yellow Green", new() {
                    new(Colors.Red, 0.0),
                    new(Colors.Yellow, 0.5),
                    new(Colors.Green, 1.0)
                }),
                // Wrap back to start color for a continuous cyclic gradient
                new("Rainbow", new() {
                    new(Colors.Red, 0.0),
                    new(Colors.Orange, 0.10),
                    new(Colors.Yellow, 0.28),
                    new(Colors.Green, 0.42),
                    new(Colors.Cyan, 0.56),
                    new(Colors.Blue, 0.70),
                    new(Colors.Violet, 0.9),
                    new(Colors.Red, 1.0)  
                }),
                new("Sunset Glow", new() {
                    new(Colors.DarkOrange, 0.0),
                    new(Colors.Orange, 0.25),
                    new(Colors.Pink, 0.5),
                    new(Colors.Purple, 0.75),
                    new(Colors.DarkOrange, 1.0),
                }),
                new("Ocean Breeze", new() {
                    new(Colors.LightBlue, 0.0),
                    new(Colors.DarkBlue, 0.33),
                    new(Colors.SeaGreen, 0.67),
                    new(Colors.LightBlue, 1.0)
                }),
                new("Forest Walk", new() {
                    new(Colors.DarkGreen, 0.0),
                    new(Colors.Brown, 0.3),
                    new(Colors.Olive, 0.5),
                    new(Colors.LightGreen, 0.8),
                    new(Colors.DarkGreen, 1.0),
                }),
            };
        }
    }
}
