using System;
using System.Numerics;
using System.Windows;

namespace ProjectionCanvas
{
    public class AxisAngleRotation3 : Rotation3
    {
        private Quaternion _cachedQuaternionValue = c_dirtyQuaternion;

        // Arbitrary quaternion that will signify that our cached quat is dirty
        internal static readonly Quaternion c_dirtyQuaternion = new((float)Math.E, (float)Math.PI, (float)(Math.E * Math.PI), (float)55.0);

        internal static Vector3 s_Axis = new(0, 1, 0);
        internal const double c_Angle = 0.0;


        public AxisAngleRotation3() { }


        public AxisAngleRotation3(Vector3 axis, double angle)
        {
            Axis = axis;
            Angle = angle;
        }


        internal override Quaternion InternalQuaternion
        {
            get
            {
                if (_cachedQuaternionValue == c_dirtyQuaternion)
                {
                    if (Axis.LengthSquared() <= Utils.FLT_MIN)
                    {
                        _cachedQuaternionValue = Quaternion.Identity;
                    }
                    else
                    {
                        var normalizedAxis = Vector3.Normalize(Axis);
                        var halfAngleRadians = (float)(Math.PI / 180 * Angle / 2);
                        var vectorPart = normalizedAxis * (float)Math.Sin(halfAngleRadians);
                        var scalarPart = (float)Math.Cos(halfAngleRadians);

                        _cachedQuaternionValue = new Quaternion(vectorPart,scalarPart);
                        //_cachedQuaternionValue = new Quaternion(Axis, (float)Angle);
                    }
                }

                return _cachedQuaternionValue;
            }
        }


        internal void AxisPropertyChangedHook(DependencyPropertyChangedEventArgs _)
        {
        }

        public new AxisAngleRotation3 Clone() => (AxisAngleRotation3)base.Clone();

        public new AxisAngleRotation3 CloneCurrentValue() => (AxisAngleRotation3)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new AxisAngleRotation3();


        public Vector3 Axis
        {
            get => (Vector3)GetValue(AxisProperty);
            set => SetValue(AxisProperty, value);
        }

        public static readonly DependencyProperty AxisProperty = DependencyProperty.Register(nameof(Axis), typeof(Vector3), typeof(AxisAngleRotation3), new PropertyMetadata(new Vector3(0,1,0), OnAxisChanged));

        private static void OnAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not AxisAngleRotation3 self) return;
            self.OnAxisChanged(e);
        }

        internal void OnAxisChanged(DependencyPropertyChangedEventArgs e)
        {
            _cachedQuaternionValue = c_dirtyQuaternion;
        }

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(AxisAngleRotation3), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnAngleChanged));

        private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not AxisAngleRotation3 self) return;
            self.OnAngleChanged(e);
        }

        internal void OnAngleChanged(DependencyPropertyChangedEventArgs e)
        {
            _cachedQuaternionValue = c_dirtyQuaternion;
        }
    }
}