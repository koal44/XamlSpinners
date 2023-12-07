using FluentAssertions;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Media;

namespace ColorCraft.Tests
{
    public class ColorUtilsTests
    {
        public record HslAndRgbData(double H, double S, double L, byte R, byte G, byte B);
        public record RgbAndLab(byte R, byte G, byte B, double L, double A, double BB);

        public static IEnumerable<Object[]> HslRgbData => new List<HslAndRgbData>
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
            new HslAndRgbData(180, 100, 0,   0,   0,   0),    // Black due to lightness = 0
        }.Select(r => new HslAndRgbData[] {r});

        public static IEnumerable<Object[]> RgbLabData => new List<RgbAndLab>
        {
            new RgbAndLab(0, 0, 0, 0.00, 0.00, 0.00),
            new RgbAndLab(255, 255, 255, 100.00, 0.00, 0.00),
            new RgbAndLab(255, 0, 0, 53.33, 80.00, 67.00),
            new RgbAndLab(0, 255, 0, 87.84, -86.00, 83.00),
            new RgbAndLab(0, 0, 255, 32.16, 79.00, -108.00),
            new RgbAndLab(255, 255, 0, 97.25, -22.00, 95.00),
            new RgbAndLab(61, 41, 41, 18.82, 9.00, 4.00),
            new RgbAndLab(191, 64, 64, 45.88, 51.00, 28.00),
            new RgbAndLab(245, 163, 163, 74.90, 30.00, 13.00),
            new RgbAndLab(61, 61, 41, 25.10, -3.00, 12.00),
            new RgbAndLab(191, 191, 64, 75.29, -16.00, 61.00),
            new RgbAndLab(245, 245, 163, 94.90, -12.00, 40.00),
            new RgbAndLab(41, 61, 41, 23.53, -13.00, 10.00),
            new RgbAndLab(64, 191, 64, 68.63, -59.00, 52.00),
            new RgbAndLab(163, 245, 163, 89.80, -41.00, 32.00),
            new RgbAndLab(41, 61, 61, 23.92, -8.00, -3.00),
            new RgbAndLab(64, 191, 191, 70.98, -34.00, -10.00),
            new RgbAndLab(163, 245, 245, 91.76, -25.00, -8.00),
            new RgbAndLab(41, 41, 61, 17.65, 5.00, -13.00),
            new RgbAndLab(64, 64, 191, 34.90, 40.00, -66.00),
            new RgbAndLab(163, 163, 245, 69.80, 19.00, -41.00),
            new RgbAndLab(61, 41, 61, 19.61, 14.00, -9.00),
            new RgbAndLab(191, 64, 191, 50.20, 65.00, -41.00),
            new RgbAndLab(245, 163, 245, 77.65, 43.00, -29.00),
            new RgbAndLab(0, 0, 0, 0.00, 0.00, 0.00)
        }.Select(r => new RgbAndLab[] {r});


        [Theory]
        [MemberData(nameof(HslRgbData))]
        public void HslToRgb_ConvertsCorrectly(HslAndRgbData data)
        {
            var hsl = new Hsl(data.H, data.S / 100, data.L / 100);
            var result = hsl.ToColor();
            result.R.Should().BeCloseTo(data.R, 1);
            result.G.Should().BeCloseTo(data.G, 1);
            result.B.Should().BeCloseTo(data.B, 1);
        }


        [Theory]
        [MemberData(nameof(HslRgbData))]
        public void HslToRgb2_ConvertsCorrectly(HslAndRgbData data)
        {
            var hsl = new Hsl(data.H, data.S / 100, data.L / 100);
            var result = hsl.ToColor2();
            result.R.Should().BeCloseTo(data.R, 1);
            result.G.Should().BeCloseTo(data.G, 1);
            result.B.Should().BeCloseTo(data.B, 1);
        }

        [Theory]
        [MemberData(nameof(HslRgbData))]
        public void RgbToHsl_ConvertsCorrectly(HslAndRgbData data)
        {
            var color = Color.FromRgb(data.R, data.G, data.B);
            var (h, s, l) = Hsl.FromColor(color);
            s *= 100;
            l *= 100;

            if (data.R == data.G && data.G == data.B) // Grayscale
            {
                h.Should().BeInRange(0, 360); // Hue can be anything for grayscale
                s.Should().Be(0); // Saturation should be 0 for grayscale
            }
            else
            {
                h.Should().BeApproximately(data.H, 0.5);
                s.Should().BeApproximately(data.S, 0.5);
            }

            l.Should().BeApproximately(data.L, 0.5);
        }

        [Theory]
        [MemberData(nameof(RgbLabData))]
        public void RgbToLab_ConvertsCorrectly(RgbAndLab data)
        {
            var color = Color.FromRgb(data.R, data.G, data.B);
            var lab = Lab.FromColor(color, true);
            Debug.Print($"RGB: {data.R}, {data.G}, {data.B}");
            Debug.Print($"LAB: {lab.L}, {lab.A}, {lab.B}");
            Debug.Print($"LABB: {data.L}, {data.A}, {data.BB}");
            lab.L.Should().BeApproximately(data.L, 0.9);
            lab.A.Should().BeApproximately(data.A, 0.9);
            lab.B.Should().BeApproximately(data.BB, 0.9);
        }

        [Theory]
        [MemberData(nameof(RgbLabData))]
        public void RgbToLabToRgb_ConvertsCorrectly(RgbAndLab data)
        {
            var color = Color.FromRgb(data.R, data.G, data.B);
            var lab = Lab.FromColor(color, true);
            var result = lab.ToColor(true);
            result.R.Should().BeCloseTo(data.R, 1);
            result.G.Should().BeCloseTo(data.G, 1);
            result.B.Should().BeCloseTo(data.B, 1);
        }

        [Fact]
        public void LabLerp_ShouldNotHaveSteepJumpsInHue()
        {
            int maxHueJump = 5;
            var startColor = Color.FromRgb(255, 0, 0);
            var endColor = Color.FromRgb(0, 255, 0);

            var startLab = Lab.FromColor(startColor, true);
            var endLab = Lab.FromColor(endColor, true);

            var lastHsl = Hsl.FromColor(startLab.ToColor(true));
            var lastLab = startLab;
            var lastColor = startColor;

            for (double progress = 0; progress <= 1; progress += 0.01)
            {
                var currentLab = Lab.Lerp(startLab, endLab, progress);
                var currentHsl = Hsl.FromColor(currentLab.ToColor(true));
                var currentColor = currentLab.ToColor(true);

                var hueDiff = Math.Abs(currentHsl.H - lastHsl.H);
                if (hueDiff > maxHueJump)
                {
                    Debug.Print($"Hue diff: {hueDiff} at progress {progress}\nlast hsl: {lastHsl}, current hsl: {currentHsl}\nlastLab: {lastLab}, currentLab: {currentLab}\nlastColor: {lastColor}, currentColor: {currentColor}");
                }

                hueDiff.Should().BeLessThan(maxHueJump);

                lastHsl = currentHsl;
                lastLab = currentLab;
                lastColor = currentColor;
            }
        }

    }
}