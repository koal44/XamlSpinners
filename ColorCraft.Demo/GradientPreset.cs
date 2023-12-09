using System.Collections.Generic;
using System.Windows.Media;

namespace ColorCraft.Demo
{
    public record GradientPreset(string Name, List<MultiColorSpaceGradientStop> Stops)
    {
        public static List<GradientPreset> GetDefaultPresets()
        {
            return new List<GradientPreset>
            {
                new GradientPreset
                ("Red Yellow Green", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.Red, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Yellow, 0.5, true),
                    new MultiColorSpaceGradientStop(Colors.Green, 1.0, true)
                }),
                // Wrap back to start color for a continuous cyclic gradient
                new GradientPreset
                ("Rainbow", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.Red, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Orange, 0.10, true),
                    new MultiColorSpaceGradientStop(Colors.Yellow, 0.28, true),
                    new MultiColorSpaceGradientStop(Colors.Green, 0.42, true),
                    new MultiColorSpaceGradientStop(Colors.Cyan, 0.56, true),
                    new MultiColorSpaceGradientStop(Colors.Blue, 0.70, true),
                    new MultiColorSpaceGradientStop(Colors.Violet, 0.9, true),
                    new MultiColorSpaceGradientStop(Colors.Red, 1.0, true)  
                }),
                new GradientPreset
                ("Sunset Glow", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.DarkOrange, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Orange, 0.25, true),
                    new MultiColorSpaceGradientStop(Colors.Pink, 0.5, true),
                    new MultiColorSpaceGradientStop(Colors.Purple, 0.75, true),
                    new MultiColorSpaceGradientStop(Colors.DarkOrange, 1.0, true),
                }),
                new GradientPreset
                ("Ocean Breeze", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.LightBlue, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.DarkBlue, 0.33, true),
                    new MultiColorSpaceGradientStop(Colors.SeaGreen, 0.67, true),
                    new MultiColorSpaceGradientStop(Colors.LightBlue, 1.0, true)
                }),
                new GradientPreset
                ("Forest Walk", new List<MultiColorSpaceGradientStop>() {
                    new MultiColorSpaceGradientStop(Colors.DarkGreen, 0.0, true),
                    new MultiColorSpaceGradientStop(Colors.Brown, 0.3, true),
                    new MultiColorSpaceGradientStop(Colors.Olive, 0.5, true),
                    new MultiColorSpaceGradientStop(Colors.LightGreen, 0.8, true),
                    new MultiColorSpaceGradientStop(Colors.DarkGreen, 1.0, true),
                }),
            };
        }
    }
}
