using System.Numerics;
using System.Windows;


namespace ProjectionCanvas
{
    public class RotateTransform3 : AffineTransform3
    {
        private float _cachedCenterXValue = 0.0f;
        private float _cachedCenterYValue = 0.0f;
        private float _cachedCenterZValue = 0.0f;
        private Rotation3 _cachedRotationValue = Rotation3.Identity;

        internal const float c_CenterX = 0.0f;
        internal const float c_CenterY = 0.0f;
        internal const float c_CenterZ = 0.0f;
        internal static Rotation3 s_Rotation = Rotation3.Identity;


        public float CenterX
        {
            //get => (float)GetValue(CenterXProperty);
            get { ReadPreamble(); return _cachedCenterXValue; }
            set => SetValue(CenterXProperty, value);
        }

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(float), typeof(RotateTransform3), new PropertyMetadata(0.0f, OnCenterXChanged));

        private static void OnCenterXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform3 self) return;
            self._cachedCenterXValue = (float)e.NewValue;
            //self.OnCenterXChanged(e);
        }


        public float CenterY
        {
            //get => (float)GetValue(CenterYProperty);
            get { ReadPreamble(); return _cachedCenterYValue; }
            set => SetValue(CenterYProperty, value);
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(float), typeof(RotateTransform3), new PropertyMetadata(0.0f, OnCenterYChanged));

        private static void OnCenterYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform3 self) return;
            self._cachedCenterYValue = (float)e.NewValue;
            //self.OnCenterYChanged(e);
        }


        public float CenterZ
        {
            //get => (float)GetValue(CenterZProperty);
            get { ReadPreamble(); return _cachedCenterZValue; }
            set => SetValue(CenterZProperty, value);
        }

        public static readonly DependencyProperty CenterZProperty = DependencyProperty.Register(nameof(CenterZ), typeof(float), typeof(RotateTransform3), new PropertyMetadata(0.0f, OnCenterZChanged));

        private static void OnCenterZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform3 self) return;
            self._cachedCenterZValue = (float)e.NewValue;
            //self.OnCenterYChanged(e);
        }


        public Rotation3 Rotation
        {
            //get => (Rotation3)GetValue(RotationProperty);
            get { ReadPreamble(); return _cachedRotationValue; }
            set => SetValue(RotationProperty, value);
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation), typeof(Rotation3), typeof(RotateTransform3), new PropertyMetadata(Rotation3.Identity, OnRotationChanged));

        private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RotateTransform3 self) return;
            self._cachedRotationValue = (Rotation3)e.NewValue;
            //self.OnRotationChanged(e);
        }


        public RotateTransform3() { }

        public RotateTransform3(Rotation3 rotation)
        {
            Rotation = rotation;
        }

        public RotateTransform3(Rotation3 rotation, Point3 center)
        {
            Rotation = rotation;
            CenterX = center.X;
            CenterY = center.Y;
            CenterZ = center.Z;
        }

        public RotateTransform3(Rotation3 rotation, float centerX, float centerY, float centerZ)
        {
            Rotation = rotation;
            CenterX = centerX;
            CenterY = centerY;
            CenterZ = centerZ;
        }

        public override Matrix4x4 Value
        {
            get
            {
                ReadPreamble();

                Rotation3 rotation = _cachedRotationValue;

                if (rotation == null)
                {
                    return Matrix4x4.Identity;
                }

                var quaternion = rotation.InternalQuaternion;
                var center = new Point3(_cachedCenterXValue, _cachedCenterYValue, _cachedCenterZValue);

                return CreateRotationMatrix(ref quaternion, ref center);
            }
        }

        public static Matrix4x4 CreateRotationMatrix2(ref Quaternion quaternion, ref Point3 center)
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

        public static Matrix4x4 CreateRotationMatrix(ref Quaternion quaternion, ref Point3 center)
        {
            var m = Matrix4x4.Identity;

            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;
            var xx = quaternion.X * x2;
            var xy = quaternion.X * y2;
            var xz = quaternion.X * z2;
            var yy = quaternion.Y * y2;
            var yz = quaternion.Y * z2;
            var zz = quaternion.Z * z2;
            var wx = quaternion.W * x2;
            var wy = quaternion.W * y2;
            var wz = quaternion.W * z2;

            m.M11 = 1.0f - (yy + zz);
            m.M12 = xy + wz;
            m.M13 = xz - wy;
            m.M21 = xy - wz;
            m.M22 = 1.0f - (xx + zz);
            m.M23 = yz + wx;
            m.M31 = xz + wy;
            m.M32 = yz - wx;
            m.M33 = 1.0f - (xx + yy);

            if (center.X != 0 || center.Y != 0 || center.Z != 0)
            {
                m.M41 = -center.X * m.M11 - center.Y * m.M21 - center.Z * m.M31 + center.X; // offset.X
                m.M42 = -center.X * m.M12 - center.Y * m.M22 - center.Z * m.M32 + center.Y; // offset.Y
                m.M43 = -center.X * m.M13 - center.Y * m.M23 - center.Z * m.M33 + center.Z; // offset.Z
            }

            var foo = m.ToString();
            return m;
        }

        internal override void Append(ref Matrix4x4 matrix)
        {
            matrix *= Value;
        }
        public new RotateTransform3 Clone() => (RotateTransform3)base.Clone();

        public new RotateTransform3 CloneCurrentValue() => (RotateTransform3)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new RotateTransform3();

    }
}