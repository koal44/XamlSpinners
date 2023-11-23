using FluentAssertions;
using System.Diagnostics;
using XamlSpinners.Utils;

namespace XamlSpinners.Tests
{
    public class ColorUtilsTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)] // Black
        [InlineData(0, 0, 1, 255, 255, 255)] // White
        [InlineData(0, 1, 0.5, 255, 0, 0)] // Red
        [InlineData(120, 1, 0.5, 0, 255, 0)] // Green
        [InlineData(240, 1, 0.5, 0, 0, 255)] // Blue
        [InlineData(60, 1, 0.5, 255, 255, 0)] // Yellow
        public void HslToRgb_ConvertsCorrectly(double h, double s, double l, byte expectedR, byte expectedG, byte expectedB)
        {
            // Arrange
            // (HSL values are already provided as test case parameters)

            //Act
            var result = ColorUtils.HslToRgb(h, s, l);

                // Assert
                result.R.Should().Be(expectedR);
                result.G.Should().Be(expectedG);
                result.B.Should().Be(expectedB);
        }

        [Theory]
        [InlineData(0, 20, 20, 61, 41, 41)]
        [InlineData(0, 50, 50, 191, 64, 64)]
        [InlineData(0, 80, 80, 245, 163, 163)]
        [InlineData(60, 20, 20, 61, 61, 41)]
        [InlineData(60, 50, 50, 191, 191, 64)]
        [InlineData(60, 80, 80, 245, 245, 163)]
        [InlineData(120, 20, 20, 41, 61, 41)]
        [InlineData(120, 50, 50, 64, 191, 64)]
        [InlineData(120, 80, 80, 163, 245, 163)]
        [InlineData(180, 20, 20, 41, 61, 61)]
        [InlineData(180, 50, 50, 64, 191, 191)]
        [InlineData(180, 80, 80, 163, 245, 245)]
        [InlineData(240, 20, 20, 41, 41, 61)]
        [InlineData(240, 50, 50, 64, 64, 191)]
        [InlineData(240, 80, 80, 163, 163, 245)]
        [InlineData(300, 20, 20, 61, 41, 61)]
        [InlineData(300, 50, 50, 191, 64, 191)]
        [InlineData(300, 80, 80, 245, 163, 245)]
        public void HslToRgb_ConvertsCssDataCorrectly(double h, double s, double l, byte expectedR, byte expectedG, byte expectedB)
        {
            // Arrange
            // (HSL values are already provided as test case parameters)

            //Act
            var result = ColorUtils.HslToRgb(h, s/100, l/100);

            // Assert
            result.R.Should().BeCloseTo(expectedR, 1);
            result.G.Should().BeCloseTo(expectedG, 1);
            result.B.Should().BeCloseTo(expectedB, 1);
        }

        [Theory]
        [InlineData(0, 20, 20, 61, 41, 41)]
        [InlineData(0, 50, 50, 191, 64, 64)]
        [InlineData(0, 80, 80, 245, 163, 163)]
        [InlineData(60, 20, 20, 61, 61, 41)]
        [InlineData(60, 50, 50, 191, 191, 64)]
        [InlineData(60, 80, 80, 245, 245, 163)]
        [InlineData(120, 20, 20, 41, 61, 41)]
        [InlineData(120, 50, 50, 64, 191, 64)]
        [InlineData(120, 80, 80, 163, 245, 163)]
        [InlineData(180, 20, 20, 41, 61, 61)]
        [InlineData(180, 50, 50, 64, 191, 191)]
        [InlineData(180, 80, 80, 163, 245, 245)]
        [InlineData(240, 20, 20, 41, 41, 61)]
        [InlineData(240, 50, 50, 64, 64, 191)]
        [InlineData(240, 80, 80, 163, 163, 245)]
        [InlineData(300, 20, 20, 61, 41, 61)]
        [InlineData(300, 50, 50, 191, 64, 191)]
        [InlineData(300, 80, 80, 245, 163, 245)]
        public void HslToRgb2_ConvertsCssDataCorrectly(double h, double s, double l, byte expectedR, byte expectedG, byte expectedB)
        {
            // Arrange
            // (HSL values are already provided as test case parameters)

            //Act
            var result = ColorUtils.HslToRgb2(h, s, l);

            // Assert
            result.R.Should().BeCloseTo(expectedR, 1);
            result.G.Should().BeCloseTo(expectedG, 1);
            result.B.Should().BeCloseTo(expectedB, 1);
        }
    }

}