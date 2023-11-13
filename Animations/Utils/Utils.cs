using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Animations
{
    public static partial class Utils
    {
        #region SVG

        public static PathFigureCollection ParsePathData(string data)
        {
            data = data.Replace(',', ' ');

            var matches = SvgSegmentRegex().Matches(data);

            var figures = new PathFigureCollection();
            var figure = new PathFigure();
            figures.Add(figure);

            var lastPos = new Point(0, 0);
            Point? lastControlPoint = null;
            string lastControlCommand = "";

            foreach (var match in matches.Cast<Match>())
            {
                string command = match.Groups[1].Value;
                var coords = match.Groups[2].Value.Trim()
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => double.Parse(x))
                    .ToList();

                var nextPos = GetNextPosition(command, coords, lastPos);
                var relativeCoords = GetRelativeCoords(command, coords, lastPos);
                PathSegment? segment = null;

                switch (command.ToUpper())
                {
                    case "M":
                        figure.StartPoint = nextPos;
                        lastControlPoint = null;
                        lastControlCommand = command;
                        break;
                    case "L":
                    case "H":
                    case "V":
                        segment = new LineSegment()
                        {
                            Point = nextPos,
                        };
                        lastControlPoint = null;
                        lastControlCommand = command;
                        break;
                    case "C":
                        if (relativeCoords is null) throw new Exception("Relative coords is null");
                        segment = new BezierSegment()
                        {
                            Point1 = new Point(relativeCoords[0], relativeCoords[1]),
                            Point2 = new Point(relativeCoords[2], relativeCoords[3]),
                            Point3 = nextPos,
                        };
                        lastControlPoint = new Point(relativeCoords[2], relativeCoords[3]);
                        lastControlCommand = command;
                        break;
                    case "S":
                        if (relativeCoords is null) throw new Exception("Relative coords is null");
                        segment = new BezierSegment()
                        {
                            Point1 = lastControlCommand.ToLower() == "c" || lastControlCommand.ToLower() == "s"
                                ? ReflectControlPoint(lastControlPoint!.Value, lastPos)
                                : lastPos,
                            Point2 = new Point(relativeCoords[0], relativeCoords[1]),
                            Point3 = nextPos,
                        };
                        lastControlPoint = new Point(relativeCoords[0], relativeCoords[1]);
                        lastControlCommand = command;
                        break;
                    case "Q":
                        if (relativeCoords is null) throw new Exception("Relative coords is null");
                        segment = new QuadraticBezierSegment()
                        {
                            Point1 = new Point(relativeCoords[0], relativeCoords[1]),
                            Point2 = nextPos,
                        };
                        lastControlPoint = new Point(relativeCoords[0], relativeCoords[1]);
                        lastControlCommand = command;
                        break;
                    case "T":
                        if (relativeCoords is null) throw new Exception("Relative coords is null");
                        segment = new QuadraticBezierSegment()
                        {
                            Point1 = lastControlCommand.ToLower() == "q" || lastControlCommand.ToLower() == "t"
                                ? ReflectControlPoint(lastControlPoint!.Value, lastPos)
                                : lastPos,
                            Point2 = nextPos,
                        };
                        lastControlPoint = new Point(relativeCoords[0], relativeCoords[1]);
                        lastControlCommand = command;
                        break;
                    case "A":
                        segment = new ArcSegment()
                        {
                            Size = new Size(coords[0], coords[1]),
                            RotationAngle = coords[2],
                            IsLargeArc = coords[3] > 0,
                            SweepDirection = (SweepDirection)coords[4],
                            Point = nextPos,
                            IsStroked = true
                        };
                        lastControlPoint = null;
                        lastControlCommand = command;
                        break;
                    case "Z":
                        //segment = new LineSegment()
                        //{
                        //    Point = figure.StartPoint,
                        //};
                        figure.IsClosed = true;
                        break;
                    default:
                        break;
                }

                lastPos = nextPos;

                if (segment is not null)
                {
                    segment.IsStroked = true;
                    figure.Segments.Add(segment);
                }
            }

            return figures;
        }


        [GeneratedRegex("([mzlhvctsqa])([^mzlhvctsqa]*)", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex SvgSegmentRegex();


        private static Point ReflectControlPoint(Point controlPoint, Point mirrorPoint)
        {
            double reflectedX = 2 * mirrorPoint.X - controlPoint.X;
            double reflectedY = 2 * mirrorPoint.Y - controlPoint.Y;
            return new Point(reflectedX, reflectedY);
        }


        private static List<double>? GetRelativeCoords(string command, List<double> coords, Point lastPos)
        {
            return command switch
            {
                "c" => coords.Select((x, i) => i % 2 == 0 ? x + lastPos.X : x + lastPos.Y).ToList(),
                "C" => coords,
                "s" => coords.Select((x, i) => i % 2 == 0 ? x + lastPos.X : x + lastPos.Y).ToList(),
                "S" => coords,
                "q" => coords.Select((x, i) => i % 2 == 0 ? x + lastPos.X : x + lastPos.Y).ToList(),
                "Q" => coords,
                "t" => coords.Select((x, i) => i % 2 == 0 ? x + lastPos.X : x + lastPos.Y).ToList(),
                "T" => coords,
                _ => null
            };
        }


        private static Point GetNextPosition(string command, List<double> coords, Point lastPosition)
        {
            return command switch
            {
                "h" => new Point(coords[0] + lastPosition.X, lastPosition.Y),
                "H" => new Point(coords[0], lastPosition.Y),
                "v" => new Point(lastPosition.X, coords[0] + lastPosition.Y),
                "V" => new Point(lastPosition.X, coords[0]),
                "z" => new Point(lastPosition.X, lastPosition.Y),
                "Z" => new Point(lastPosition.X, lastPosition.Y),
                _ => command == command.ToLower()
                    ? AnimatedTypeHelpers.AddPoint(lastPosition, new Point(coords[^2], coords[^1]))
                    : new Point(coords[^2], coords[^1])
            };
        }

        #endregion

        #region Normalizing BezierSegments

        public static PathFigureCollection NormalizePathFigureCollection(PathFigureCollection figures)
        {
            var normalizedCollection = new PathFigureCollection();
            foreach (var figure in figures)
            {
                normalizedCollection.Add(NormalizePathFigure(figure));
            }
            return normalizedCollection;
        }


        public static PathFigure NormalizePathFigure(PathFigure figure)
        {
            var normalizedFigure = new PathFigure
            {
                StartPoint = figure.StartPoint,
                IsClosed = figure.IsClosed,
                IsFilled = figure.IsFilled
            };

            var normalizedSegments = new PathSegmentCollection();
            PathSegment? previousSegment = null;

            foreach (var segment in figure.Segments)
            {
                normalizedSegments.Add(NormalizePathSegment(segment, previousSegment, figure.StartPoint));
                previousSegment = segment;
            }
            normalizedFigure.Segments = normalizedSegments;

            return normalizedFigure;
        }


        public static PathSegment NormalizePathSegment(PathSegment segment, PathSegment? previousSegment, Point figureStartPoint)
        {
            switch (segment)
            {
                case BezierSegment bezier:
                    return bezier;

                case ArcSegment arc:
                    return arc;

                case LineSegment line:
                    Point startPoint = previousSegment switch
                    {
                        LineSegment prevLine => prevLine.Point,
                        BezierSegment prevBezier => prevBezier.Point3,
                        QuadraticBezierSegment prevQuadratic => prevQuadratic.Point2,
                        PathSegment => throw new NotSupportedException("PathSegment is not supported"), // ??
                        _ => figureStartPoint
                    };
                    return new BezierSegment()
                    {
                        IsStroked = line.IsStroked,
                        Point1 = startPoint + (line.Point - startPoint) / 3,
                        Point2 = startPoint + (line.Point - startPoint) * 2 / 3,
                        Point3 = line.Point
                    };

                case QuadraticBezierSegment qbs:
                    return new BezierSegment()
                    {
                        IsStroked = qbs.IsStroked,
                        Point1 = new Point(qbs.Point1.X / 3, qbs.Point1.Y / 3),
                        Point2 = new Point(qbs.Point1.X / 3 * 2, qbs.Point1.Y / 3 * 2),
                        Point3 = qbs.Point2
                    };

                default:
                    return segment;
            }
        }


        //public static List<LineSegment> ConvertPolyLineSegmentToLineSegments(PolyLineSegment polySegment, Point? startPoint = null)
        //{
        //    var lineSegments = new List<LineSegment>();

        //    if (polySegment.Points.Count < 2)
        //        return lineSegments; // Need at least two points to form a line

        //    // Use provided start point or the first point in the PolyLineSegment
        //    Point previousPoint = startPoint ?? polySegment.Points[0];

        //    // Start from the second point if startPoint is null, else start from the first point
        //    int startIndex = startPoint == null ? 1 : 0;

        //    for (int i = startIndex; i < polySegment.Points.Count; i++)
        //    {
        //        var currentPoint = polySegment.Points[i];
        //        lineSegments.Add(new LineSegment(currentPoint, true));
        //        previousPoint = currentPoint;
        //    }

        //    return lineSegments;
        //}

        #endregion

        #region Debugging

        public static string PrintPathFigureCollection(PathFigureCollection figures)
        {
            var result = new StringBuilder();

            foreach (var figure in figures)
            {
                result.AppendLine($"StartPoint: {figure.StartPoint}");
                foreach (var segment in figure.Segments)
                {
                    var segmentText = segment switch
                    {
                        BezierSegment bs => $"BezierSegment: {bs.Point1}, {bs.Point2}, {bs.Point3}",
                        LineSegment ls => $"LineSegment: {ls.Point}",
                        QuadraticBezierSegment qbs => $"QuadraticBezierSegment: {qbs.Point1}, {qbs.Point2}",
                        PolyBezierSegment pbs => $"PolyBezierSegment: {string.Join(", ", pbs.Points)}",
                        PolyLineSegment pls => $"PolyLineSegment: {string.Join(", ", pls.Points)}",
                        PolyQuadraticBezierSegment pqbs => $"PolyQuadraticBezierSegment: {string.Join(", ", pqbs.Points)}",
                        ArcSegment arc => $"ArcSegment: {arc.Point}, {arc.Size}, {arc.RotationAngle}, {arc.IsLargeArc}, {arc.SweepDirection}",
                        _ => "Unknown Segment"
                    };
                    result.AppendLine(segmentText);
                }
            }
            return result.ToString();
        }

        #endregion

    }
}
