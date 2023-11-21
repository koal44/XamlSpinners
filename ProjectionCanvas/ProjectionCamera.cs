using System.Numerics;
using System.Windows;

namespace ProjectionCanvas
{
    public abstract class ProjectionCamera : Camera
    {
        //internal const float c_NearPlaneDistance = 0.125f;
        //internal const float c_FarPlaneDistance = float.PositiveInfinity;
        //internal static Point3 s_Position = new();
        //internal static Vector3 s_LookDirection = new(0, 0, -1);
        //internal static Vector3 s_UpDirection = new(0, 1, 0);

        public ProjectionCamera()
        {
        
        }

        internal override Matrix4x4 GetViewMatrix()
        {
            var position = Position;
            var lookDirection = LookDirection;
            var upDirection = UpDirection;

            return CreateViewMatrix(Transform, ref position, ref lookDirection, ref upDirection);

        }

        // Transfrom that moves the world to a camera coordinate system
        // where the camera is at the origin looking down the negative z
        // axis and y is up.
        //
        // NOTE: We consider camera.Transform to be part of the view matrix.
        //
        internal static Matrix4x4 CreateViewMatrix(Transform3 transform, ref Point3 position, ref Vector3 lookDirection, ref Vector3 upDirection)
        {
            var zaxis = -lookDirection;
            zaxis = Vector3.Normalize(zaxis);

            var xaxis = Vector3.Cross(upDirection, zaxis);
            xaxis = Vector3.Normalize(xaxis);

            var yaxis = Vector3.Cross(zaxis, xaxis);

            var positionVec = (Vector3)position;
            var cx = -Vector3.Dot(xaxis, positionVec);
            var cy = -Vector3.Dot(yaxis, positionVec);
            var cz = -Vector3.Dot(zaxis, positionVec);

            var viewMatrix = new Matrix4x4(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                cx, cy, cz, 1);

            PrependInverseTransform(transform, ref viewMatrix);

            return viewMatrix;
        }

        #region Freezable

        public new ProjectionCamera Clone() => (ProjectionCamera)base.Clone();

        public new ProjectionCamera CloneCurrentValue() => (ProjectionCamera)base.CloneCurrentValue();

        #endregion

        #region Dependency Properties

        public float NearPlaneDistance
        {
            get => (float)GetValue(NearPlaneDistanceProperty);
            set => SetValue(NearPlaneDistanceProperty, value);
        }

        public static readonly DependencyProperty NearPlaneDistanceProperty = DependencyProperty.Register(nameof(NearPlaneDistance), typeof(float), typeof(ProjectionCamera), new PropertyMetadata(0.125f, OnMyPropertyChanged));

        private static void OnMyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionCamera self) return;
            //self.OnMyPropertyChanged(e);
        }


        public float FarPlaneDistance
        {
            get => (float)GetValue(FarPlaneDistanceProperty);
            set => SetValue(FarPlaneDistanceProperty, value);
        }

        public static readonly DependencyProperty FarPlaneDistanceProperty = DependencyProperty.Register(nameof(FarPlaneDistance), typeof(float), typeof(ProjectionCamera), new PropertyMetadata(float.PositiveInfinity, OnFarPlaneDistanceChanged));

        private static void OnFarPlaneDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionCamera self) return;
            //self.OnFarPlaneDistanceChanged(e);
        }


        public Point3 Position
        {
            get => (Point3)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(Point3), typeof(ProjectionCamera), new PropertyMetadata(new Point3(), OnPositionChanged));

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionCamera self) return;
            //self.OnPositionChanged(e);
        }


        public Vector3 LookDirection
        {
            get => (Vector3)GetValue(LookDirectionProperty);
            set => SetValue(LookDirectionProperty, value);
        }

        public static readonly DependencyProperty LookDirectionProperty = DependencyProperty.Register(nameof(LookDirection), typeof(Vector3), typeof(ProjectionCamera), new PropertyMetadata(new Vector3(0,0,-1), OnLookDirectionChanged));

        private static void OnLookDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionCamera self) return;
            //self.OnLookDirectionChanged(e);
        }


        public Vector3 UpDirection
        {
            get => (Vector3)GetValue(UpDirectionProperty);
            set => SetValue(UpDirectionProperty, value);
        }

        public static readonly DependencyProperty UpDirectionProperty = DependencyProperty.Register(nameof(UpDirection), typeof(Vector3), typeof(ProjectionCamera), new PropertyMetadata(new Vector3(0,1,0), OnUpDirectionChanged));

        private static void OnUpDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionCamera self) return;
            //self.OnUpDirectionChanged(e);
        }

        #endregion

    }
}