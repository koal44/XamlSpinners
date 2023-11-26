using FluentAssertions;
using System.Windows.Media;
using XamlSpinners.Utils;

namespace XamlSpinners.Tests
{
    public class ColorUtilsTests
    {
        public record HslAndRgbData(double H, double S, double L, byte R, byte G, byte B);

        public static IEnumerable<Object[]> ColorData => new List<HslAndRgbData>
        {
            new HslAndRgbData(0,   0,   0,   0,   0,   0),   // Black
            new HslAndRgbData(0,   0,   100, 255, 255, 255), // White
            new HslAndRgbData(0,   100, 50,  255, 0,   0),   // Red
            new HslAndRgbData(120, 100, 50,  0,   255, 0),   // Green
            new HslAndRgbData(240, 100, 50,  0,   0,   255), // Blue
            new HslAndRgbData(60,  100, 50,  255, 255, 0),   // Yellow
            new HslAndRgbData(0,   20,  20,  61,  41,  41),
            new HslAndRgbData(0,   50,  50,  191, 64,  64),
            new HslAndRgbData(0,   80,  80,  245, 163, 163),
            new HslAndRgbData(60,  20,  20,  61,  61,  41),
            new HslAndRgbData(60,  50,  50,  191, 191, 64),
            new HslAndRgbData(60,  80,  80,  245, 245, 163),
            new HslAndRgbData(120, 20,  20,  41,  61,  41),
            new HslAndRgbData(120, 50,  50,  64,  191, 64),
            new HslAndRgbData(120, 80,  80,  163, 245, 163),
            new HslAndRgbData(180, 20,  20,  41,  61,  61),
            new HslAndRgbData(180, 50,  50,  64,  191, 191),
            new HslAndRgbData(180, 80,  80,  163, 245, 245),
            new HslAndRgbData(240, 20,  20,  41,  41,  61),
            new HslAndRgbData(240, 50,  50,  64,  64,  191),
            new HslAndRgbData(240, 80,  80,  163, 163, 245),
            new HslAndRgbData(300, 20,  20,  61,  41,  61),
            new HslAndRgbData(300, 50,  50,  191, 64,  191),
            new HslAndRgbData(300, 80,  80,  245, 163, 245),
        }.Select(r => new HslAndRgbData[] {r});


        [Theory]
        [MemberData(nameof(ColorData))]
        public void HslToRgb_ConvertsCorrectly(HslAndRgbData data)
        {
            var result = ColorUtils.HslToRgb(data.H, data.S / 100, data.L / 100);
            result.R.Should().BeCloseTo(data.R, 1);
            result.G.Should().BeCloseTo(data.G, 1);
            result.B.Should().BeCloseTo(data.B, 1);
        }


        [Theory]
        [MemberData(nameof(ColorData))]
        public void HslToRgb2_ConvertsCorrectly(HslAndRgbData data)
        {
            var result = ColorUtils.HslToRgb2(data.H, data.S / 100, data.L / 100);
            result.R.Should().BeCloseTo(data.R, 1);
            result.G.Should().BeCloseTo(data.G, 1);
            result.B.Should().BeCloseTo(data.B, 1);
        }

        [Theory]
        [MemberData(nameof(ColorData))]
        public void RgbToHsl_ConvertsCorrectly(HslAndRgbData data)
        {
            var color = Color.FromRgb(data.R, data.G, data.B);
            var (h, s, l) = ColorUtils.RgbToHsl(color);
            s *= 100;
            l *= 100;
            h.Should().BeApproximately(data.H, 0.5);
            s.Should().BeApproximately(data.S, 0.5);
            l.Should().BeApproximately(data.L, 0.5);
        }

    }
}