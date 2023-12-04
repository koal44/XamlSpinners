using FluentAssertions;
using System.Windows;
using System.Windows.Media.Animation;


namespace XamlSpinners.Tests
{
    public class StoryboardExtensionsTests
    {

        [Theory]
        [InlineData(0, -5)]
        [InlineData(1, -4)]
        [InlineData(2, -3)]
        [InlineData(3, -2)]
        [InlineData(4, -1)]
        [InlineData(5, 0)]
        [InlineData(6, -9)]
        [InlineData(7, -8)]
        [InlineData(8, -7)]
        [InlineData(9, -6)]
        [InlineData(10, -5)]
        [InlineData(11, -4)]
        public void ReverseDoubleAnimation_ShouldSetCorrectBeginTime(int storyboardSeconds, int expectedBeginTimeSeconds)
        {
            // Arrange
            var ani = new DoubleAnimation
            {
                From = 20,
                To = 25,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Act
            StoryboardExtensions.ReverseDoubleAnimation(ani, TimeSpan.FromSeconds(storyboardSeconds));

            // Assert
            ani.From.Should().Be(25);
            ani.To.Should().Be(20);
            ani.BeginTime.Should().BeCloseTo(TimeSpan.FromSeconds(expectedBeginTimeSeconds), TimeSpan.FromMicroseconds(10));
        }


        [Theory]
        [InlineData(0,-2)]
        [InlineData(1,-1)]
        [InlineData(2, 0)]
        [InlineData(3, -9)]
        [InlineData(4, -8)]
        [InlineData(5, -7)]
        [InlineData(6, -6)]
        [InlineData(7, -5)]
        [InlineData(8, -4)]
        [InlineData(9, -3)]
        [InlineData(10, -2)]
        public void ReverseDoubleAnimation_WithBeginTimeMinus3_ShouldSetCorrectBeginTime(int storyboardSeconds, int expectedBeginTimeSeconds)
        {
            // Arrange
            var ani = new DoubleAnimation
            {
                From = 20,
                To = 25,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                AutoReverse = true,
                BeginTime = TimeSpan.FromSeconds(-3),
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Act
            StoryboardExtensions.ReverseDoubleAnimation(ani, TimeSpan.FromSeconds(storyboardSeconds));

            // Assert
            ani.From.Should().Be(25);
            ani.To.Should().Be(20);
            ani.BeginTime.Should().BeCloseTo(TimeSpan.FromSeconds(expectedBeginTimeSeconds), TimeSpan.FromMicroseconds(10));
        }

        [Theory]
        [InlineData(0, -3)]
        [InlineData(1, -2)]
        [InlineData(2, -1)]
        [InlineData(3, 0)]
        [InlineData(4, -9)]
        [InlineData(5, -8)]
        [InlineData(6, -7)]
        [InlineData(7, -6)]
        [InlineData(8, -5)]
        [InlineData(9, -4)]
        [InlineData(10, -3)]
        public void ReverseDoubleAnimation_WithBeginTimeMinus12_ShouldSetCorrectBeginTime(int storyboardSeconds, int expectedBeginTimeSeconds)
        {
            // Arrange
            var ani = new DoubleAnimation
            {
                From = 20,
                To = 25,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                AutoReverse = true,
                BeginTime = TimeSpan.FromSeconds(-12),
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Act
            StoryboardExtensions.ReverseDoubleAnimation(ani, TimeSpan.FromSeconds(storyboardSeconds));

            // Assert
            ani.From.Should().Be(25);
            ani.To.Should().Be(20);
            ani.BeginTime.Should().BeCloseTo(TimeSpan.FromSeconds(expectedBeginTimeSeconds), TimeSpan.FromMicroseconds(10));
        }

        [Theory]
        [InlineData(0, 17)]
        [InlineData(10, 7)]
        [InlineData(15, 2)]
        [InlineData(17, -5)]
        [InlineData(18, -4)]
        [InlineData(19, -3)]
        [InlineData(20, -2)]
        [InlineData(21, -1)]
        [InlineData(22, 0)]
        [InlineData(23, -9)]
        [InlineData(24, -8)]
        public void ReverseDoubleAnimation_WithBeginTimePlus17_ShouldSetCorrectBeginTime(int storyboardSeconds, int expectedBeginTimeSeconds)
        {
            // Arrange
            var ani = new DoubleAnimation
            {
                From = 20,
                To = 25,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                AutoReverse = true,
                BeginTime = TimeSpan.FromSeconds(+17),
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Act
            StoryboardExtensions.ReverseDoubleAnimation(ani, TimeSpan.FromSeconds(storyboardSeconds));

            // Assert
            ani.From.Should().Be(25);
            ani.To.Should().Be(20);
            ani.BeginTime.Should().BeCloseTo(TimeSpan.FromSeconds(expectedBeginTimeSeconds), TimeSpan.FromMicroseconds(10));
        }




    }
}
