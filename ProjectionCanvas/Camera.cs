using System.Numerics;
using System.Windows;
using System.Windows.Media.Animation;

namespace ProjectionCanvas
{
    public abstract class Camera : Animatable
    {
        internal static Transform3 s_Transform = Transform3.Identity;


        public Transform3 Transform
        {
            get => (Transform3)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(nameof(Transform), typeof(Transform3), typeof(Camera), new PropertyMetadata(Transform3.Identity, OnTransformChanged));

        private static void OnTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Camera self) return;
            //self.OnTransformChanged(e);
        }


        public Camera() { }


        // Creates a ray by projecting the given point on the viewport into the scene.
        // Used for bridging 2D -> 3D hit testing.
        //
        // The latter two parameters in this method are used to deal with the
        // case where the camera's near plane is far away from the viewport
        // contents. In these cases, we can sometimes construct a new, closer,
        // near plane and start the ray on that plane. To do this, we need an
        // axis-aligned bounding box of the viewport's contents (boundingRect).
        // We also need to return the distance between the original an new near
        // planes (distanceAdjustment), so we can correct the hit-test
        // distances before handing them back to the user. For more
        // information, see WindowsOS Bug #1329733.
        //
        //internal abstract RayHitTestParameters RayFromViewportPoint(Point point, Size viewSize, Rect3 boundingRect, out double distanceAdjustment);
        internal abstract Matrix4x4 GetViewMatrix();
        internal abstract Matrix4x4 GetProjectionMatrix(float aspectRatio);

        internal static void PrependInverseTransform(Transform3 transform, ref Matrix4x4 viewMatrix)
        {
            if (transform != null && transform != Transform3.Identity)
            {
                PrependInverseTransform(transform.Value, ref viewMatrix);
            }
        }

        // Helper method to prepend the inverse of Camera.Transform to the
        // the given viewMatrix.  This is used by the various GetViewMatrix()
        // and RayFromViewportPoint implementations.
        // 
        // Transforming the camera is equivalent to applying the inverse
        // transform to the scene.  We invert the transform and prepend it to
        // the result of viewMatrix:
        //
        //                                  -1
        //     viewMatrix = Camera.Transform   x viewMatrix
        //
        // If the matrix is non-invertable we set the viewMatrix to NaNs which
        // will result in nothing being rendered.  This is the correct behavior
        // since the near and far planes will have collapsed onto each other.
        internal static void PrependInverseTransform(Matrix4x4 matrix, ref Matrix4x4 viewMatrix)
        {
            if (!Matrix4x4.Invert(matrix, out _))
            {
                // If the matrix is non-invertable we return a NaN matrix.
                viewMatrix = new Matrix4x4(
                    float.NaN, float.NaN, float.NaN, float.NaN,
                    float.NaN, float.NaN, float.NaN, float.NaN,
                    float.NaN, float.NaN, float.NaN, float.NaN,
                    float.NaN, float.NaN, float.NaN, float.NaN);
            }
            else
            {
                viewMatrix = Matrix4x4.Multiply(viewMatrix, matrix);
            }
        }

        public new Camera Clone() => (Camera)base.Clone();

        public new Camera CloneCurrentValue() => (Camera)base.CloneCurrentValue();

    }
}