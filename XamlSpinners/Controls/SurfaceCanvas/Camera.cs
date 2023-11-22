using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public class Camera : Animatable
    {
        #region Data

        private readonly float _zNear = 3f;
        private readonly float _zFar = float.PositiveInfinity;
        private readonly float _aspectRatio = 1.0f;

        #endregion

        #region Dependency Properties

        // TODO: Add a Transform property if you want the camera to have freedom of movement
        // TODO: Change UpDirection to UpAngle

        public Vector3 CameraPosition
        {
            get => (Vector3)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(CameraPosition), typeof(Vector3), typeof(Camera), new FrameworkPropertyMetadata(new Vector3(), FrameworkPropertyMetadataOptions.AffectsRender, OnPositionChanged));

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not Camera self) return;
            //self.OnPositionChanged(e);
        }


        public Vector3 TargetPosition
        {
            get => (Vector3)GetValue(TargetPositionProperty);
            set => SetValue(TargetPositionProperty, value);
        }

        public static readonly DependencyProperty TargetPositionProperty = DependencyProperty.Register(nameof(TargetPosition), typeof(Vector3), typeof(Camera), new FrameworkPropertyMetadata(default(Vector3), FrameworkPropertyMetadataOptions.AffectsRender, OnTargetPositionChanged));

        private static void OnTargetPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not Camera self) return;
            //self.OnTargetPositionChanged(e);
        }


        public Vector3 UpDirection
        {
            get => (Vector3)GetValue(UpDirectionProperty);
            set => SetValue(UpDirectionProperty, value);
        }

        public static readonly DependencyProperty UpDirectionProperty = DependencyProperty.Register(nameof(UpDirection), typeof(Vector3), typeof(Camera), new FrameworkPropertyMetadata(new Vector3(0, 1, 0), FrameworkPropertyMetadataOptions.AffectsRender, OnUpDirectionChanged));

        private static void OnUpDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not Camera self) return;
            //self.OnUpDirectionChanged(e);
        }


        public float FieldOfView
        {
            get => (float)GetValue(FieldOfViewProperty);
            set => SetValue(FieldOfViewProperty, value);
        }

        public static readonly DependencyProperty FieldOfViewProperty = DependencyProperty.Register(nameof(FieldOfView), typeof(float), typeof(Camera), new FrameworkPropertyMetadata(45.0f, FrameworkPropertyMetadataOptions.AffectsRender, OnFieldOfViewChanged));

        private static void OnFieldOfViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not Camera self) return;
            //self.OnFieldOfViewChanged(e);
        }

        #endregion

        #region Methods

        // Transfrom that moves the world to a camera coordinate system
        // where the camera is at the origin looking down the negative z
        // axis and y is up.
        public Matrix4x4 CreateViewMatrix()
        {
            return Matrix4x4.CreateLookAt(CameraPosition, TargetPosition, UpDirection);
        }


        public Matrix4x4 CreateProjectionMatrix()
        {
            var fov = (float)(FieldOfView * Math.PI / 180);
            return Matrix4x4.CreatePerspectiveFieldOfView(fov, _aspectRatio, _zNear, _zFar);
        }

        #endregion

        #region Freezable

        public new Camera Clone() => (Camera)base.Clone();

        public new Camera CloneCurrentValue() => (Camera)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new Camera();

        #endregion

    }
}
