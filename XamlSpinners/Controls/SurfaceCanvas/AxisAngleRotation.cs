using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public class AxisAngleRotation : Animatable
    {
        #region Data

        // Arbitrary quaternion that will signify that our cached quat is dirty
        private static readonly Quaternion c_dirtyQuaternion = new((float)Math.E, (float)Math.PI, (float)(Math.E * Math.PI), (float)55.0);
        private static readonly AxisAngleRotation s_identity;

        private Quaternion _quat = c_dirtyQuaternion;
        private Vector3 _normalizedAxis;

        #endregion

        #region Dependency Properties

        public Vector3 Axis
        {
            get => (Vector3)GetValue(AxisProperty);
            set => SetValue(AxisProperty, value);
        }

        public static readonly DependencyProperty AxisProperty = DependencyProperty.Register(nameof(Axis), typeof(Vector3), typeof(AxisAngleRotation), new FrameworkPropertyMetadata(new Vector3(0, 1, 0), FrameworkPropertyMetadataOptions.AffectsRender, OnAxisChanged));

        private static void OnAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not AxisAngleRotation self) return;
            self.UpdateNormalizedAxis();
            self.MarkQuaternionDirty();
        }


        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(AxisAngleRotation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnAngleChanged));

        private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not AxisAngleRotation self) return;
            self.MarkQuaternionDirty();
        }

        #endregion

        #region Constructors

        static AxisAngleRotation()
        {
            // Create a singleton frozen instance
            s_identity = new AxisAngleRotation();
            s_identity.Freeze();
        }


        public AxisAngleRotation() { }


        public AxisAngleRotation(Vector3 axis, double angle)
        {
            Axis = axis;
            Angle = angle;
        }

        #endregion

        #region Methods

        public static AxisAngleRotation Identity => s_identity;


        public Quaternion GetQuaternion()
        {
            if (_quat == c_dirtyQuaternion)
            {
                _quat = _normalizedAxis == Vector3.Zero
                    ? Quaternion.Identity
                    : Quaternion.CreateFromAxisAngle(_normalizedAxis, (float)(Angle * Math.PI / 180));
            }

            return _quat;
        }

        private void UpdateNormalizedAxis()
        {
            _normalizedAxis = Axis.LengthSquared() < float.Epsilon * 10
                ? Vector3.Zero
                : Vector3.Normalize(Axis);
        }

        private void MarkQuaternionDirty()
        {
            _quat = c_dirtyQuaternion;
        }

        #endregion

        #region Clone
        public new AxisAngleRotation Clone() => (AxisAngleRotation)base.Clone();

        public new AxisAngleRotation CloneCurrentValue() => (AxisAngleRotation)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new AxisAngleRotation();

        #endregion

    }
}