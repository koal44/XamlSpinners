using System;
using System.Numerics;

namespace ProjectionCanvas
{
    public abstract class Transform3 : GeneralTransform3
    {
        public void Transform(Point3 point) 
            => point.Transform(Value);

        public void Transform(Vector3 v)
        {
            // Do not apply _offset to vectors.
            v.X = v.X * Value.M11 + v.Y * Value.M21 + v.Z * Value.M31;
            v.Y = v.X * Value.M12 + v.Y * Value.M22 + v.Z * Value.M32;
            v.Z = v.X * Value.M13 + v.Y * Value.M23 + v.Z * Value.M33;
        }

        public void Transform(Point4 point)
            => point.Transform(Value);

        public void Transform(Point3[] points)
        {
            foreach (var p in points) { Transform(p); }
        }

        public void Transform(Vector3[] vectors)
        {
            foreach (var v in vectors) { Transform(v); }
        }

        public void Transform(Point4[] points)
        {
            foreach (var p in points) { Transform(p); }
        }

        //public override bool TryTransform(Point3 inPoint, out Point3 result)
        //{
        //    result = Value.Transform(inPoint);
        //    return true;
        //}

        public override Rect3 TransformBounds(Rect3 rect)
        {
            throw new NotImplementedException();
            //return M3DUtil.ComputeTransformedAxisAlignedBoundingBox(ref rect, this);
        }

        public override GeneralTransform3? Inverse
        {
            get
            {
                ReadPreamble();

                Matrix4x4 matrix = Value;

                return Matrix4x4.Invert(matrix, out matrix)
                    ? new MatrixTransform3(matrix)
                    : null;
            }
        }


        internal override Transform3 AffineTransform => this;

        public static Transform3 Identity
        {
            get
            {
                // Make sure identity matrix is initialized.
                if (s_identity == null)
                {
                    var identity = new MatrixTransform3();
                    identity.Freeze();
                    s_identity = identity;
                }
                return s_identity;
            }
        }

        public abstract bool IsAffine { get; }

        public abstract Matrix4x4 Value { get; }

        internal abstract void Append(ref Matrix4x4 matrix);

        private static Transform3? s_identity;

        public new Transform3 Clone() 
            => (Transform3)base.Clone();

        public new Transform3 CloneCurrentValue() 
            => (Transform3)base.CloneCurrentValue();

    }
}