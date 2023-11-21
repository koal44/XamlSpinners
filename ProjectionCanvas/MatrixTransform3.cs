using System;
using System.Numerics;
using System.Windows;

namespace ProjectionCanvas
{
    public class MatrixTransform3 : Transform3
    {
        public MatrixTransform3()
        {
        }

        public MatrixTransform3(Matrix4x4 matrix)
        {
            Matrix = matrix;
        }

        public override Matrix4x4 Value => Matrix;

        public override bool IsAffine => Matrix.IsIdentity || (Matrix.M14 == 0.0 && Matrix.M24 == 0.0 && Matrix.M34 == 0.0 && Matrix.M44 == 1.0);

        internal override void Append(ref Matrix4x4 matrix)
        {
            matrix *= Matrix;
        }

        public new MatrixTransform3 Clone() => (MatrixTransform3)base.Clone();

        public new MatrixTransform3 CloneCurrentValue() => (MatrixTransform3)base.CloneCurrentValue();


        protected override Freezable CreateInstanceCore() => new MatrixTransform3();

        internal static Matrix4x4 s_Matrix = Matrix4x4.Identity;

        public Matrix4x4 Matrix
        {
            get => (Matrix4x4)GetValue(MatrixProperty);
            set => SetValue(MatrixProperty, value);
        }

        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register(nameof(Matrix), typeof(Matrix4x4), typeof(MatrixTransform3), new PropertyMetadata(Matrix4x4.Identity, OnMatrixChanged));

        private static void OnMatrixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MatrixTransform3 self) return;
            //self.OnMatrixChanged(e);
        }

    }
}