using System.Collections.Generic;
using System.Windows.Media;

namespace ColorCraft.Demo
{
    public record GradientPreset(string Name, List<GradientStop> Stops)
    {
        public static List<GradientPreset> GetDefaultPresets()
        {
            return new List<GradientPreset>
            {
                new GradientPreset
                ("Red Yellow Green", new List<GradientStop>() {
                    new GradientStop(Colors.Red, 0.0),
                    new GradientStop(Colors.Yellow, 0.5),
                    new GradientStop(Colors.Green, 1.0)
                }),
                // Wrap back to start color for a continuous cyclic gradient
                new GradientPreset
                ("Rainbow", new List<GradientStop>() {
                    new GradientStop(Colors.Red, 0.0),
                    new GradientStop(Colors.Orange, 0.10),
                    new GradientStop(Colors.Yellow, 0.28),
                    new GradientStop(Colors.Green, 0.42),
                    new GradientStop(Colors.Cyan, 0.56),
                    new GradientStop(Colors.Blue, 0.70),
                    new GradientStop(Colors.Violet, 0.9),
                    new GradientStop(Colors.Red, 1.0)  
                }),
                new GradientPreset
                ("Sunset Glow", new List<GradientStop>() {
                    new GradientStop(Colors.DarkOrange, 0.0),
                    new GradientStop(Colors.Orange, 0.25),
                    new GradientStop(Colors.Pink, 0.5),
                    new GradientStop(Colors.Purple, 0.75),
                    new GradientStop(Colors.DarkOrange, 1.0),
                }),
                new GradientPreset
                ("Ocean Breeze", new List<GradientStop>() {
                    new GradientStop(Colors.LightBlue, 0.0),
                    new GradientStop(Colors.DarkBlue, 0.33),
                    new GradientStop(Colors.SeaGreen, 0.67),
                    new GradientStop(Colors.LightBlue, 1.0)
                }),
                new GradientPreset
                ("Forest Walk", new List<GradientStop>() {
                    new GradientStop(Colors.DarkGreen, 0.0),
                    new GradientStop(Colors.Brown, 0.3),
                    new GradientStop(Colors.Olive, 0.5),
                    new GradientStop(Colors.LightGreen, 0.8),
                    new GradientStop(Colors.DarkGreen, 1.0),
                }),
            };
        }
    }
}
