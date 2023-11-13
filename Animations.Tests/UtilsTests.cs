using FluentAssertions;
using System.Windows;
using System.Windows.Media;

namespace Animations.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void ParsePathData_ShouldHandleMoveCommandCorrectly()
        {
            // Arrange
            string pathData = "M 10 10";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            result.First().StartPoint.Should().Be(new Point(10, 10));
        }


        [Fact]
        public void ParsePathData_ShouldHandleLineCommandCorrectly()
        {
            // Arrange
            string pathData = "M 10 10 L 20 20";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            var lineSegment = result.First().Segments.First() as LineSegment;
            lineSegment.Should().NotBeNull();
            lineSegment!.Point.Should().Be(new Point(20, 20));
        }


        [Fact]
        public void ParsePathData_ShouldHandleHorizontalAndVerticalCommandsCorrectly()
        {
            // Arrange
            string pathData = "M 10 10 H 20 V 30";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            var lineSegmentH = result.First().Segments[0] as LineSegment;
            lineSegmentH.Should().NotBeNull();
            lineSegmentH!.Point.Should().Be(new Point(20, 10));

            var lineSegmentV = result.First().Segments[1] as LineSegment;
            lineSegmentV.Should().NotBeNull();
            lineSegmentV!.Point.Should().Be(new Point(20, 30));
        }


        [Fact]
        public void ParsePathData_ShouldHandleClosePathCommandCorrectly()
        {
            // Arrange
            string pathData = "M 10 10 L 20 20 Z";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            result.First().IsClosed.Should().BeTrue();
        }


        [Fact]
        public void ParsePathData_ShouldHandleQuadraticBezierCommandCorrectly()
        {
            // Arrange
            string pathData = "M 10 10 Q 20 20 30 10";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            var quadraticBezierSegment = result.First().Segments.First() as QuadraticBezierSegment;
            quadraticBezierSegment.Should().NotBeNull();
            quadraticBezierSegment!.Point1.Should().Be(new Point(20, 20));
            quadraticBezierSegment.Point2.Should().Be(new Point(30, 10));
        }


        [Fact]
        public void ParsePathData_ShouldHandleCubicBezierCommandCorrectly()
        {
            // Arrange
            string pathData = "M 10 10 C 20 20, 30 30, 40 10";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            var bezierSegment = result.First().Segments.First() as BezierSegment;
            bezierSegment.Should().NotBeNull();
            bezierSegment!.Point1.Should().Be(new Point(20, 20));
            bezierSegment.Point2.Should().Be(new Point(30, 30));
            bezierSegment.Point3.Should().Be(new Point(40, 10));
        }



        [Fact]
        public void ParsePathData_ShouldHandleArcCommandCorrectly()
        {
            // Arrange
            string pathData = "M 10 10 A 20 20 0 0 1 30 30";

            // Act
            var result = Utils.ParsePathData(pathData);

            // Assert
            var arcSegment = result.First().Segments.First() as ArcSegment;
            arcSegment.Should().NotBeNull();
            arcSegment!.Size.Should().Be(new Size(20, 20));
            arcSegment.SweepDirection.Should().Be(SweepDirection.Clockwise);
            arcSegment.Point.Should().Be(new Point(30, 30));
        }


        [Fact]
        public void NormalizePathSegment_ShouldConvertLineSegmentToBezierSegment()
        {
            // Arrange
            var lineSegment = new LineSegment(new Point(30, 30), true);

            // Act
            var result = Utils.NormalizePathSegment(lineSegment, null, new Point(0,0));

            // Assert
            var bezierSegment = Assert.IsType<BezierSegment>(result);
            bezierSegment.Point1.Should().Be(new Point(10, 10));
            bezierSegment.Point2.Should().Be(new Point(20, 20));
            bezierSegment.Point3.Should().Be(new Point(30, 30));
            bezierSegment.IsStroked.Should().BeTrue();
        }


        [Fact]
        public void NormalizePathSegment_ShouldConvertQuadraticBezierSegmentToBezierSegment()
        {
            // Arrange
            var quadraticBezierSegment = new QuadraticBezierSegment(new Point(15, 15), new Point(30, 30), true);

            // Act
            var result = Utils.NormalizePathSegment(quadraticBezierSegment, null, new Point(0, 0));

            // Assert
            var bezierSegment = Assert.IsType<BezierSegment>(result);
            bezierSegment.Point1.Should().Be(new Point(5, 5));
            bezierSegment.Point2.Should().Be(new Point(10, 10));
            bezierSegment.Point3.Should().Be(new Point(30, 30));
            bezierSegment.IsStroked.Should().BeTrue();
        }


        [Fact]
        public void NormalizePathSegment_ShouldNotAlterBezierSegment()
        {
            // Arrange
            var bezierSegment = new BezierSegment(new Point(10, 10), new Point(20, 20), new Point(30, 30), true);

            // Act
            var result = Utils.NormalizePathSegment(bezierSegment, null, new Point(0, 0));

            // Assert
            Assert.Same(bezierSegment, result);
        }


        [Fact]
        public void NormalizePathSegment_ShouldNotAlterArcSegment()
        {
            // Arrange
            var arcSegment = new ArcSegment(new Point(30, 30), new Size(10, 20), 45, true, SweepDirection.Clockwise, true);

            // Act
            var result = Utils.NormalizePathSegment(arcSegment, null, new Point(0, 0));

            // Assert
            Assert.Same(arcSegment, result);
        }


        [Fact]
        public void NormalizePathFigure_ShouldNormalizeTriangleCorrectly()
        {
            // Arrange
            var startPoint = new Point(0, 0);
            var lineSegments = new PathSegmentCollection
            {
                new LineSegment(new Point(0, 30), true),
                new LineSegment(new Point(30, 30), true),
                new LineSegment(startPoint, true) // Closing the triangle
            };

                    var triangleFigure = new PathFigure
                    {
                        StartPoint = startPoint,
                        Segments = lineSegments,
                        IsClosed = true,
                        IsFilled = true
                    };

                    var expectedBezierSegments = new[]
                    {
                new BezierSegment(new Point(0, 10), new Point(0, 20), new Point(0, 30), true),
                new BezierSegment(new Point(10, 30), new Point(20, 30), new Point(30, 30), true),
                new BezierSegment(new Point(20, 20), new Point(10, 10), startPoint, true)
            };

            // Act
            var result = Utils.NormalizePathFigure(triangleFigure);

            // Assert
            result.StartPoint.Should().Be(startPoint);
            result.IsClosed.Should().BeTrue();
            result.IsFilled.Should().BeTrue();

            var resultBezierSegments = result.Segments.OfType<BezierSegment>().ToArray();
            resultBezierSegments.Should().BeEquivalentTo(expectedBezierSegments, options => options.ComparingByMembers<BezierSegment>());
        }



    }
}