using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Animations
{
    public class AnimatedTypeHelpers
    {
        #region Interpolation Methods

        public static double InterpolateDouble(double from, double to, double progress)
        {
            return from + ((to - from) * progress);
        }


        public static DoubleCollection InterpolateDoubleCollection(DoubleCollection from, DoubleCollection to, double progress)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new DoubleCollection(from.Count);
            for (int i = 0; i < from.Count; i++)
            {
                var interpolatedValue = InterpolateDouble(from[i], to[i], progress);
                result.Add(interpolatedValue);
            }

            return result;
        }


        public static Point InterpolatePoint(Point from, Point to, double progress)
        {
            // from + ((to - from) * progress);
            return AddPoint(from, ScalePoint(SubtractPoint(to, from), progress));
        }


        public static PointCollection InterpolatePointCollection(PointCollection from, PointCollection to, double progress)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PointCollection(from.Count);
            for (int i = 0; i < from.Count; i++)
            {
                var interpolatedValue = InterpolatePoint(from[i], to[i], progress);
                result.Add(interpolatedValue);
            }

            return result;
        }


        public static PathFigureCollection InterpolatePathFigureCollection(PathFigureCollection from, PathFigureCollection to, double progress)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException("Collections must be of the size.");

            var result = new PathFigureCollection(from.Count);
            for (int i = 0; i < from.Count; i++)
            {
                var interpolatedValue = InterpolatePathFigure(from[i], to[i], progress);
                result.Add(interpolatedValue);
            }

            return result;
        }


        public static PathFigure InterpolatePathFigure(PathFigure from, PathFigure to, double progress)
        {
            if (from.IsClosed != to.IsClosed) throw new ArgumentException("PathFigures must have the same IsClosed value.");
            if (from.IsFilled != to.IsFilled) throw new ArgumentException("PathFigures must have the same IsFilled value.");

            return new PathFigure()
            {
                StartPoint = InterpolatePoint(from.StartPoint, to.StartPoint, progress),
                Segments = InterpolatePathSegmentCollection(from.Segments, to.Segments, progress),
                IsClosed = from.IsClosed,
                IsFilled = from.IsFilled
            };
        }


        public static PathSegmentCollection InterpolatePathSegmentCollection(PathSegmentCollection from, PathSegmentCollection to, double progress)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException($"PathSegmentCollections must be of the same size. Expected size: {from.Count}, Actual size: {to.Count}.");

            var result = new PathSegmentCollection(from.Count);
            for (int i = 0; i < from.Count; i++)
            {
                var interpolatedValue = InterpolatePathSegment(from[i], to[i], progress);
                result.Add(interpolatedValue);
            }

            return result;
        }


        public static PathSegment InterpolatePathSegment(PathSegment from, PathSegment to, double progress)
        {
            // from + ((to - from) * progress);
            var result = AddPathSegment(from, ScalePathSegment(SubtractPathSegment(to, from), progress));
            return result;
        }

        #endregion

        #region Additive Methods

        public static Size AddSize(Size value1, Size value2)
        {
            return new Size(
                value1.Width + value2.Width,
                value1.Height + value2.Height);
        }


        public static double AddDouble(double value1, double value2)
        {
            return value1 + value2;
        }


        public static DoubleCollection AddDoubleCollection(DoubleCollection collection1, DoubleCollection collection2)
        {
            if (collection1 == null) throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null) throw new ArgumentNullException(nameof(collection2));
            if (collection1.Count != collection2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new DoubleCollection(collection1.Count);
            for (int i = 0; i < collection1.Count; i++)
            {
                var d1 = collection1[i];
                var d2 = collection2[i];

                result.Add(AddDouble(d1, d2));
            }

            return result;
        }


        public static Point AddPoint(Point value1, Point value2)
        {
            return new Point(
                value1.X + value2.X,
                value1.Y + value2.Y);
        }


        public static PointCollection AddPointCollection(PointCollection collection1, PointCollection collection2)
        {
            if (collection1 == null) throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null) throw new ArgumentNullException(nameof(collection2));
            if (collection1.Count != collection2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PointCollection(collection1.Count);
            for (int i = 0; i < collection1.Count; i++)
            {
                var p1 = collection1[i];
                var p2 = collection2[i];
                result.Add(AddPoint(p1, p2));
            }

            return result;
        }


        public static PathFigureCollection AddPathFigureCollection(PathFigureCollection collection1, PathFigureCollection collection2)
        {
            if (collection1 == null) throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null) throw new ArgumentNullException(nameof(collection2));
            if (collection1.Count != collection2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PathFigureCollection(collection1.Count);
            for (int i = 0; i < collection1.Count; i++)
            {
                var p1 = collection1[i];
                var p2 = collection2[i];
                result.Add(AddPathFigure(p1, p2));
            }

            return result;
        }


        public static PathFigure AddPathFigure(PathFigure p1, PathFigure p2)
        {
            if (p1 == null) throw new ArgumentNullException(nameof(p1));
            if (p2 == null) throw new ArgumentNullException(nameof(p2));
            if (p1.IsClosed != p2.IsClosed) throw new ArgumentException("PathFigures must have the same IsClosed value.");
            if (p1.IsFilled != p2.IsFilled) throw new ArgumentException("PathFigures must have the same IsFilled value.");

            return new PathFigure()
            {
                StartPoint = AddPoint(p1.StartPoint, p2.StartPoint),
                Segments = AddPathSegmentCollection(p1.Segments, p2.Segments),
                IsClosed = p1.IsClosed,
                IsFilled = p1.IsFilled
            };
        }


        private static PathSegmentCollection AddPathSegmentCollection(PathSegmentCollection segments1, PathSegmentCollection segments2)
        {
            if (segments1 == null) throw new ArgumentNullException(nameof(segments1));
            if (segments2 == null) throw new ArgumentNullException(nameof(segments2));
            if (segments1.Count != segments2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PathSegmentCollection(segments1.Count);
            for (int i = 0; i < segments1.Count; i++)
            {
                var s1 = segments1[i];
                var s2 = segments2[i];
                result.Add(AddPathSegment(s1, s2));
            }
            return result;
        }


        public static PathSegment AddPathSegment(PathSegment segment1, PathSegment segment2)
        {
            if (segment1 == null) throw new ArgumentNullException(nameof(segment1));
            if (segment2 == null) throw new ArgumentNullException(nameof(segment2));
            if (segment1.GetType() != segment2.GetType()) throw new ArgumentException("Segments must be of the same type.");

            // svg H, V, L
            if (segment1 is LineSegment ls1 && segment2 is LineSegment ls2)
            {
                return new LineSegment()
                {
                    Point = AddPoint(ls1.Point, ls2.Point)
                };
            }
            // svg C, S
            if (segment1 is BezierSegment bs1 && segment2 is BezierSegment bs2)
            {
                return new BezierSegment()
                {
                    Point1 = AddPoint(bs1.Point1, bs2.Point1),
                    Point2 = AddPoint(bs1.Point2, bs2.Point2),
                    Point3 = AddPoint(bs1.Point3, bs2.Point3)
                };
            }
            // svg Q, T
            if (segment1 is QuadraticBezierSegment qbs1 && segment2 is QuadraticBezierSegment qbs2)
            {
                return new QuadraticBezierSegment()
                {
                    Point1 = AddPoint(qbs1.Point1, qbs2.Point1),
                    Point2 = AddPoint(qbs1.Point2, qbs2.Point2)
                };
            }
            // svg A
            if (segment1 is ArcSegment as1 && segment2 is ArcSegment as2)
            {
                if (as1.IsLargeArc != as2.IsLargeArc) throw new Exception("LargeArcFlag must be the same.");
                if (as1.SweepDirection != as2.SweepDirection) throw new Exception("SweepDirection must be the same.");

                return new ArcSegment()
                {
                    Point = AddPoint(as1.Point, as2.Point),
                    Size = AddSize(as1.Size, as2.Size),
                    RotationAngle = AddDouble(as1.RotationAngle, as2.RotationAngle),
                    IsLargeArc = as1.IsLargeArc,
                    SweepDirection = as1.SweepDirection
                };
            }

            throw new NotSupportedException($"Segment type {segment1.GetType().Name} is not supported.");
        }

        #endregion

        #region Subtractive Methods

        public static Size SubtractSize(Size value1, Size value2)
        {
            return new Size(
                value1.Width - value2.Width,
                value1.Height - value2.Height);
        }


        public static double SubtractDouble(double value1, double value2)
        {
            return value1 - value2;
        }


        public static DoubleCollection SubtractDoubleCollection(DoubleCollection collection1, DoubleCollection collection2)
        {
            if (collection1 == null) throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null) throw new ArgumentNullException(nameof(collection2));
            if (collection1.Count != collection2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new DoubleCollection(collection1.Count);
            for (int i = 0; i < collection1.Count; i++)
            {
                var d1 = collection1[i];
                var d2 = collection2[i];

                result.Add(SubtractDouble(d1, d2));
            }

            return result;
        }


        public static Point SubtractPoint(Point value1, Point value2)
        {
            return new Point(
                value1.X - value2.X,
                value1.Y - value2.Y);
        }

        public static PointCollection SubtractPointCollection(PointCollection collection1, PointCollection collection2)
        {
            if (collection1 == null) throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null) throw new ArgumentNullException(nameof(collection2));
            if (collection1.Count != collection2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PointCollection(collection1.Count);
            for (int i = 0; i < collection1.Count; i++)
            {
                var p1 = collection1[i];
                var p2 = collection2[i];

                result.Add(SubtractPoint(p1, p2));
            }

            return result;
        }


        public static PathFigureCollection SubtractPathFigureCollection(PathFigureCollection collection1, PathFigureCollection collection2)
        {
            if (collection1 == null) throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null) throw new ArgumentNullException(nameof(collection2));
            if (collection1.Count != collection2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PathFigureCollection(collection1.Count);
            for (int i = 0; i < collection1.Count; i++)
            {
                var f1 = collection1[i];
                var f2 = collection2[i];

                result.Add(SubtractPathFigure(f1, f2));
            }
            return result;
        }


        public static PathFigure SubtractPathFigure(PathFigure figure1, PathFigure figure2)
        {
            if (figure1 == null) throw new ArgumentNullException(nameof(figure1));
            if (figure2 == null) throw new ArgumentNullException(nameof(figure2));
            if (figure1.IsClosed != figure2.IsClosed) throw new ArgumentException("PathFigures must have the same IsClosed value.");
            if (figure1.IsFilled != figure2.IsFilled) throw new ArgumentException("PathFigures must have the same IsFilled value.");

            return new PathFigure()
            {
                StartPoint = SubtractPoint(figure1.StartPoint, figure2.StartPoint),
                Segments = SubtractPathSegmentCollection(figure1.Segments, figure2.Segments),
                IsClosed = figure1.IsClosed,
                IsFilled = figure1.IsFilled
            };
        }


        public static PathSegmentCollection SubtractPathSegmentCollection(PathSegmentCollection segments1, PathSegmentCollection segments2)
        {
            if (segments1 == null) throw new ArgumentNullException(nameof(segments1));
            if (segments2 == null) throw new ArgumentNullException(nameof(segments2));
            if (segments1.Count != segments2.Count) throw new ArgumentException("Collections must be of the same size.");

            var result = new PathSegmentCollection(segments1.Count);
            for (int i = 0; i < segments1.Count; i++)
            {
                var s1 = segments1[i];
                var s2 = segments2[i];
                result.Add(SubtractPathSegment(s1, s2));
            }
            return result;
        }


        public static PathSegment SubtractPathSegment(PathSegment segment1, PathSegment segment2)
        {
            if (segment1 == null) throw new ArgumentNullException(nameof(segment1));
            if (segment2 == null) throw new ArgumentNullException(nameof(segment2));
            if (segment1.GetType() != segment2.GetType()) throw new ArgumentException("Segments must be of the same type.");

            // svg H, V, L
            if (segment1 is LineSegment ls1 && segment2 is LineSegment ls2)
            {
                return new LineSegment()
                {
                    Point = SubtractPoint(ls1.Point, ls2.Point)
                };
            }
            // svg C, S
            else if (segment1 is BezierSegment bs1 && segment2 is BezierSegment bs2)
            {
                return new BezierSegment()
                {
                    Point1 = SubtractPoint(bs1.Point1, bs2.Point1),
                    Point2 = SubtractPoint(bs1.Point2, bs2.Point2),
                    Point3 = SubtractPoint(bs1.Point3, bs2.Point3)
                };
            }
            // svg Q, T
            else if (segment1 is QuadraticBezierSegment qbs1 && segment2 is QuadraticBezierSegment qbs2)
            {
                return new QuadraticBezierSegment()
                {
                    Point1 = SubtractPoint(qbs1.Point1, qbs2.Point1),
                    Point2 = SubtractPoint(qbs1.Point2, qbs2.Point2)
                };
            }
            // svg A
            else if (segment1 is ArcSegment as1 && segment2 is ArcSegment as2)
            {
                if (as1.IsLargeArc != as2.IsLargeArc) throw new Exception("LargeArcFlag must be the same.");
                if (as1.SweepDirection != as2.SweepDirection) throw new Exception("SweepDirection must be the same.");

                return new ArcSegment()
                {
                    Point = SubtractPoint(as1.Point, as2.Point),
                    Size = SubtractSize(as1.Size, as2.Size),
                    RotationAngle = SubtractDouble(as1.RotationAngle, as2.RotationAngle),
                    IsLargeArc = as1.IsLargeArc,
                    SweepDirection = as1.SweepDirection
                };
            }
            else
            {
                throw new NotSupportedException($"Segment type {segment1.GetType().Name} is not supported.");
            }
        }

        #endregion

        #region GetSegmentLength Methods

        public static double GetSegmentLengthSize(Size from, Size to)
        {
            var a = GetSegmentLengthDouble(from.Width, to.Width);
            var b = GetSegmentLengthDouble(from.Height, to.Height);
            return Math.Sqrt(a * a + b * b);
        }


        public static double GetSegmentLengthDouble(double from, double to)
        {
            return Math.Abs(to - from);
        }


        public static double GetSegmentLengthDoubleCollection(DoubleCollection from, DoubleCollection to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException("Collections must be of the same size.");

            double sumOfSquares = 0;
            for (int i = 0; i < from.Count; i++)
            {
                var length = GetSegmentLengthDouble(from[i], to[i]);
                sumOfSquares += length * length;
            }

            return Math.Sqrt(sumOfSquares);
        }


        internal static double GetSegmentLengthPoint(Point from, Point to)
        {
            return Math.Abs((to - from).Length);
        }


        public static double GetSegmentLengthPointCollection(PointCollection from, PointCollection to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException("Collections must be of the same size.");

            double sumOfSquares = 0;
            for (int i = 0; i < from.Count; i++)
            {
                double length = GetSegmentLengthPoint(from[i], to[i]);
                sumOfSquares += length * length;
            }

            return Math.Sqrt(sumOfSquares);
        }


        public static double GetSegmentLengthPathFigureCollection(PathFigureCollection from, PathFigureCollection to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Count != to.Count) throw new ArgumentException("Collections must be of the same size.");

            double sumOfSquares = 0;
            for (int i = 0; i < from.Count; i++)
            {
                double length = GetSegmentLengthPathFigure(from[i], to[i]);
                sumOfSquares += length * length;
            }

            return Math.Sqrt(sumOfSquares);
        }


        public static double GetSegmentLengthPathFigure(PathFigure pathFigure1, PathFigure pathFigure2)
        {
            if (pathFigure1 == null) throw new ArgumentNullException(nameof(pathFigure1));
            if (pathFigure2 == null) throw new ArgumentNullException(nameof(pathFigure2));

            int segmentCount = pathFigure1.Segments.Count;

            double lenth1 = GetSegmentLengthPoint(pathFigure1.StartPoint, pathFigure2.StartPoint);
            double weight1 = 1.0 / (segmentCount + 1);

            double length2 = GetSegmentLengthPathSegmentCollection(pathFigure1.Segments, pathFigure2.Segments);
            double weight2 = (double)segmentCount / (segmentCount + 1);

            return (lenth1 * weight1) + (length2 * weight2);
        }


        public static double GetSegmentLengthPathSegmentCollection(PathSegmentCollection segments1, PathSegmentCollection segments2)
        {
            if (segments1 == null) throw new ArgumentNullException(nameof(segments1));
            if (segments2 == null) throw new ArgumentNullException(nameof(segments2));
            if (segments1.Count != segments2.Count) throw new ArgumentException("Collections must be of the same size.");

            double sumOfSquares = 0;
            for (int i = 0; i < segments1.Count; i++)
            {
                double length = GetSegmentLengthPathSegment(segments1[i], segments2[i]);
                sumOfSquares += length * length;
            }
            return Math.Sqrt(sumOfSquares);
        }


        public static double GetSegmentLengthPathSegment(PathSegment segment1, PathSegment segment2)
        {
            if (segment1 == null) throw new ArgumentNullException(nameof(segment1));
            if (segment2 == null) throw new ArgumentNullException(nameof(segment2));
            if (segment1.GetType() != segment2.GetType()) throw new ArgumentException("Segments must be of the same type.");

            // svg H, V, L
            if (segment1 is LineSegment ls1 && segment2 is LineSegment ls2)
            {
                return GetSegmentLengthPoint(ls1.Point, ls2.Point);
            }
            // svg C, S
            if (segment1 is BezierSegment bs1 && segment2 is BezierSegment bs2)
            {
                var a = GetSegmentLengthPoint(bs1.Point1, bs2.Point1);
                var b = GetSegmentLengthPoint(bs1.Point2, bs2.Point2);
                var c = GetSegmentLengthPoint(bs1.Point3, bs2.Point3);
                return Math.Sqrt(a * a + b * b + c * c);
            }
            // svg Q, T
            if (segment1 is QuadraticBezierSegment qbs1 && segment2 is QuadraticBezierSegment qbs2)
            {
                var a = GetSegmentLengthPoint(qbs1.Point1, qbs2.Point1);
                var b = GetSegmentLengthPoint(qbs1.Point2, qbs2.Point2);
                return Math.Sqrt(a * a + b * b);
            }
            // svg A
            if (segment1 is ArcSegment as1 && segment2 is ArcSegment as2)
            {
                if (as1.IsLargeArc != as2.IsLargeArc) throw new Exception("LargeArcFlag must be the same.");
                if (as1.SweepDirection != as2.SweepDirection) throw new Exception("SweepDirection must be the same.");

                var a = GetSegmentLengthPoint(as1.Point, as2.Point);
                var b = GetSegmentLengthSize(as1.Size, as2.Size);
                var c = GetSegmentLengthDouble(as1.RotationAngle, as2.RotationAngle);
                return Math.Sqrt(a * a + b * b + c * c);
            }

            throw new NotSupportedException($"Segment type {segment1.GetType().Name} is not supported.");
        }


        #endregion

        #region Scale Methods

        public static Size ScaleSize(Size value, double factor)
        {
            return (Size)((Vector)value * factor);
        }


        public static double ScaleDouble(double value, double factor)
        {
            return value * factor;
        }


        public static DoubleCollection ScaleDoubleCollection(DoubleCollection collection, double scale)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var result = new DoubleCollection(collection.Count);
            for (int i = 0; i < collection.Count; i++)
            {
                result.Add(ScaleDouble(collection[i], scale));
            }

            return result;
        }


        public static Point ScalePoint(Point value, double factor)
        {
            return new Point(
                value.X * factor,
                value.Y * factor);
        }


        public static PointCollection ScalePointCollection(PointCollection collection, double scale)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var result = new PointCollection(collection.Count);
            for (int i = 0; i < collection.Count; i++)
            {
                result.Add(ScalePoint(collection[i], scale));
            }

            return result;
        }


        public static PathFigureCollection ScalePathFigureCollection(PathFigureCollection collection, double scale)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var result = new PathFigureCollection(collection.Count);
            for (int i = 0; i < collection.Count; i++)
            {
                result.Add(ScalePathFigure(collection[i], scale));
            }

            return result;
        }


        public static PathFigure ScalePathFigure(PathFigure pathFigure, double scale)
        {
            return new PathFigure()
            {
                StartPoint = ScalePoint(pathFigure.StartPoint, scale),
                Segments = ScalePathSegmentCollection(pathFigure.Segments, scale),
                IsClosed = pathFigure.IsClosed,
                IsFilled = pathFigure.IsFilled
            };
        }


        public static PathSegmentCollection ScalePathSegmentCollection(PathSegmentCollection segments, double scale)
        {
            if (segments == null) throw new ArgumentNullException(nameof(segments));

            var result = new PathSegmentCollection(segments.Count);
            for (int i = 0; i < segments.Count; i++)
            {
                result.Add(ScalePathSegment(segments[i], scale));
            }
            return result;
        }


        public static PathSegment ScalePathSegment(PathSegment segment, double scale)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));

            // svg H, V, L
            if (segment is LineSegment ls)
            {
                return new LineSegment()
                {
                    Point = ScalePoint(ls.Point, scale)
                };
            }
            // svg C, S
            else if (segment is BezierSegment bs)
            {
                return new BezierSegment()
                {
                    Point1 = ScalePoint(bs.Point1, scale),
                    Point2 = ScalePoint(bs.Point2, scale),
                    Point3 = ScalePoint(bs.Point3, scale)
                };
            }
            // svg Q, T
            else if (segment is QuadraticBezierSegment qbs)
            {
                return new QuadraticBezierSegment()
                {
                    Point1 = ScalePoint(qbs.Point1, scale),
                    Point2 = ScalePoint(qbs.Point2, scale)
                };
            }
            // svg A
            else if (segment is ArcSegment arc)
            {
                return new ArcSegment()
                {
                    Point = ScalePoint(arc.Point, scale),
                    Size = ScaleSize(arc.Size, scale),
                    RotationAngle = ScaleDouble(arc.RotationAngle, scale),
                    IsLargeArc = arc.IsLargeArc,
                    SweepDirection = arc.SweepDirection
                };
            }
            else
            {
                throw new NotSupportedException($"Segment type {segment.GetType().Name} is not supported.");
            }
        }

        #endregion

        #region IsValidAnimationValue Methods

        public static bool IsValidAnimationValueDouble(double value)
        {
            return !IsInvalidDouble(value);
        }


        public static bool IsValidAnimationValueDoubleCollection(DoubleCollection valueCollection)
        {
            foreach (var item in valueCollection)
            {
                if (!IsValidAnimationValueDouble(item)) return false;
            }
            return true;
        }
        

        internal static bool IsValidAnimationValuePoint(Point value)
        {
            if (IsInvalidDouble(value.X) || IsInvalidDouble(value.Y))
            {
                return false;
            }
            return true;
        }

        public static bool IsValidAnimationValuePointCollection(PointCollection valueCollection)
        {
            foreach (var item in valueCollection)
            {
                if (!IsValidAnimationValuePoint(item)) return false;
            }
            return true;
        }


        public static bool IsValidAnimationValuePathFigureCollection(PathFigureCollection figures)
        {
            foreach (var item in figures)
            {
                if (!IsValidAnimationValuePathFigure(item)) return false;
            }
            return true;
        }


        public static bool IsValidAnimationValuePathFigure(PathFigure figure)
        {
            if (!IsValidAnimationValuePoint(figure.StartPoint)) return false;
            if (!IsValidAnimationValuePathSegmentCollection(figure.Segments)) return false;
            return true;
        }


        private static bool IsValidAnimationValuePathSegmentCollection(PathSegmentCollection segments)
        {
            foreach (var segment in segments)
            {
                if (!IsValidAnimationValuePathSegment(segment)) return false;
            }
            return true;
        }


        private static bool IsValidAnimationValuePathSegment(PathSegment _)
        {
            // TODO: Not sure if we need to check
            return true;
        }

        #endregion

        #region GetZeroValue Methods


        public static Size GetZeroValueSize(Size _)
        {
            return new Size();
        }


        public static double GetZeroValueDouble(double _)
        {
            return 0.0;
        }


        public static DoubleCollection GetZeroValueDoubleCollection(DoubleCollection baseValue)
        {
            var result = new DoubleCollection(baseValue.Count);
            for (int i = 0; i < baseValue.Count; i++)
            {
                result.Add(GetZeroValueDouble(baseValue[i]));
            }
            return result;
        }


        public static Point GetZeroValuePoint(Point _)
        {
            return new Point();
        }


        public static PointCollection GetZeroValuePointCollection(PointCollection baseValue)
        {
            var result = new PointCollection(baseValue.Count);
            for (int i = 0; i < baseValue.Count; i++)
            {
                result.Add(GetZeroValuePoint(baseValue[i]));
            }
            return result;
        }


        public static PathFigureCollection GetZeroValuePathFigureCollection(PathFigureCollection baseValue)
        {
            if (baseValue == null) throw new ArgumentNullException(nameof(baseValue));

            var result = new PathFigureCollection(baseValue.Count);
            for (int i = 0; i < baseValue.Count; i++)
            {
                result.Add(GetZeroValuePathFigure(baseValue[i]));
            }
            return result;
        }


        public static PathFigure GetZeroValuePathFigure(PathFigure pathFigure)
        {
            if (pathFigure == null) throw new ArgumentNullException(nameof(pathFigure));
            return new PathFigure()
            {
                StartPoint = GetZeroValuePoint(pathFigure.StartPoint),
                Segments = GetZeroValuePathSegmentCollection(pathFigure.Segments),
                IsClosed = pathFigure.IsClosed,
                IsFilled = pathFigure.IsFilled
            };
        }


        public static PathSegmentCollection GetZeroValuePathSegmentCollection(PathSegmentCollection segments)
        {
            if (segments == null) throw new ArgumentNullException(nameof(segments));

            var result = new PathSegmentCollection(segments.Count);
            for (int i = 0; i < segments.Count; i++)
            {
                result.Add(GetZeroValuePathSegment(segments[i]));
            }
            return result;
        }


        private static PathSegment GetZeroValuePathSegment(PathSegment segment)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));

            // svg H, V, L
            if (segment is LineSegment ls)
            {
                return new LineSegment()
                {
                    Point = GetZeroValuePoint(ls.Point)
                };
            }
            // svg C, S
            else if (segment is BezierSegment bs)
            {
                return new BezierSegment()
                {
                    Point1 = GetZeroValuePoint(bs.Point1),
                    Point2 = GetZeroValuePoint(bs.Point2),
                    Point3 = GetZeroValuePoint(bs.Point3)
                };
            }
            // svg Q, T
            else if (segment is QuadraticBezierSegment qbs)
            {
                return new QuadraticBezierSegment()
                {
                    Point1 = GetZeroValuePoint(qbs.Point1),
                    Point2 = GetZeroValuePoint(qbs.Point2)
                };
            }
            // svg A
            else if (segment is ArcSegment arc)
            {
                return new ArcSegment()
                {
                    Point = GetZeroValuePoint(arc.Point),
                    Size = GetZeroValueSize(arc.Size),
                    RotationAngle = GetZeroValueDouble(arc.RotationAngle),
                    IsLargeArc = arc.IsLargeArc,
                    SweepDirection = arc.SweepDirection
                };
            }
            else
            {
                throw new NotSupportedException($"Segment type {segment.GetType().Name} is not supported.");
            }
        }

        #endregion

        #region Helper Methods

        private static bool IsInvalidDouble(double value)
        {
            return double.IsInfinity(value) || double.IsNaN(value);
        }

        #endregion

    }
}