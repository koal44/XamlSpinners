using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace XamlSpinners
{
    public class Matrix4Transform : GeneralTransform
    {
        #region Constructors

        public Matrix4Transform() { }

        public Matrix4Transform(Matrix4x4 matrix)
        {
            Value = matrix;
        }

        #endregion

        #region Methods

        public virtual Matrix4x4 Value { get; }

        public void Append(ref Matrix4x4 matrix)
        {
            matrix *= Value;
        }

        #endregion

        #region Overrides

        public override GeneralTransform? Inverse
        {
            get
            {
                ReadPreamble();

                return Matrix4x4.Invert(Value, out var matrix)
                    ? new Matrix4Transform(matrix)
                    : throw new InvalidOperationException("Matrix is not invertible.");
            }
        }

        public override Rect TransformBounds(Rect rect) => throw new NotImplementedException();

        public override bool TryTransform(Point inPoint, out Point result) => throw new NotImplementedException();

        #endregion

        #region Static

        private static Matrix4Transform? s_identity;

        public static Matrix4Transform Identity
        {
            get
            {
                if (s_identity == null)
                {
                    var identity = new Matrix4Transform(Matrix4x4.Identity);
                    identity.Freeze();
                    s_identity = identity;
                }
                return s_identity;
            }
        }

        #endregion

        #region Freezable

        public new Matrix4Transform Clone() => (Matrix4Transform)base.Clone();

        public new Matrix4Transform CloneCurrentValue() => (Matrix4Transform)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new Matrix4Transform();

        #endregion
    }
}