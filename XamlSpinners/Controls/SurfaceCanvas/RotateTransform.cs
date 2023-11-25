using System.Numerics;
using System.Windows;

namespace XamlSpinners
{
    public class RotateTransform : Matrix4Transform
    {
        #region Data

        private float _centerX;
        private float _centerY;
        private float _centerZ;
        private AxisAngleRotation _rotation;

        #endregion

        #region Dependency Properties

        public float CenterX
        {
            get { ReadPreamble(); return _centerX; }
            set => SetValue(CenterXProperty, value);
        }

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(float), typeof(RotateTransform), new PropertyMetadata(0.0f, OnCenterXChanged));

        private static void OnCenterXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform self) return;
            self._centerX = (float)e.NewValue;
        }


        public float CenterY
        {
            get { ReadPreamble(); return _centerY; }
            set => SetValue(CenterYProperty, value);
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(float), typeof(RotateTransform), new PropertyMetadata(0.0f, OnCenterYChanged));

        private static void OnCenterYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform self) return;
            self._centerY = (float)e.NewValue;
        }


        public float CenterZ
        {
            get { ReadPreamble(); return _centerZ; }
            set => SetValue(CenterZProperty, value);
        }

        public static readonly DependencyProperty CenterZProperty = DependencyProperty.Register(nameof(CenterZ), typeof(float), typeof(RotateTransform), new PropertyMetadata(0.0f, OnCenterZChanged));

        private static void OnCenterZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform self) return;
            self._centerZ = (float)e.NewValue;
        }


        public AxisAngleRotation Rotation
        {
            get { ReadPreamble(); return _rotation; }
            set => SetValue(RotationProperty, value);
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation), typeof(AxisAngleRotation), typeof(RotateTransform), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnRotationChanged));

        private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform self) return;
            self._rotation = (AxisAngleRotation)e.NewValue;
        }

        #endregion

        #region Constructors

        public RotateTransform()
        {
            Rotation = _rotation = new();
        }

        public RotateTransform(float angle, Vector3 center)
        {
            (CenterX, CenterY, CenterZ) = center;
            Rotation = _rotation = new(center, angle);
        }

        #endregion

        #region Methods

        public override Matrix4x4 Value
        {
            get
            {
                ReadPreamble();

                if (_rotation == null) return Matrix4x4.Identity;

                var quaternion = _rotation.GetQuaternion();
                var center = new Vector3(_centerX, _centerY, _centerZ);

                return CreateRotationMatrix(ref quaternion, ref center);
            }
        }


        public static Matrix4x4 CreateRotationMatrix(ref Quaternion quaternion, ref Vector3 center)
        {
            var rotationMatrix = Matrix4x4.CreateFromQuaternion(quaternion);
            if (center.X != 0 || center.Y != 0 || center.Z != 0)
            {
                var translationToCenter = Matrix4x4.CreateTranslation(-center.X, -center.Y, -center.Z);
                var translationBack = Matrix4x4.CreateTranslation(center.X, center.Y, center.Z);
                rotationMatrix = translationToCenter * rotationMatrix * translationBack;
            }

            return rotationMatrix;
        }

        #endregion

        #region Freezable

        public new RotateTransform Clone() => (RotateTransform)base.Clone();

        public new RotateTransform CloneCurrentValue() => (RotateTransform)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new RotateTransform();

        #endregion

    }
}