using System;
using System.Windows.Media.Animation;

namespace ProjectionCanvas
{
    public abstract class GeneralTransform3 : Animatable
    {
        public GeneralTransform3() { }

        //public abstract bool TryTransform(Point3 inPoint, out Point3 result);

        //public Point3 Transform(Point3 point)
        //{
        //    if (!TryTransform(point, out Point3 transformedPoint))
        //        throw new InvalidOperationException("GeneralTransform3D.TransformFailed");

        //    return transformedPoint;
        //}

        /// <summary>
        /// Transforms the bounding box to the smallest axis aligned bounding box
        /// that contains all the points in the original bounding box
        /// </summary>
        public abstract Rect3 TransformBounds(Rect3 rect);

        public abstract GeneralTransform3? Inverse { get; }

        internal abstract Transform3 AffineTransform { get; }

        public new GeneralTransform3 Clone() 
            => (GeneralTransform3)base.Clone();

        public new GeneralTransform3 CloneCurrentValue() 
            => (GeneralTransform3)base.CloneCurrentValue();

    }
}