using System;
using System.Numerics;
using System.Windows;

namespace ProjectionCanvas
{
    public class PerspectiveCamera : ProjectionCamera
    {
        internal const double c_FieldOfView = (double)45.0;

        public float FieldOfView
        {
            get => (float)GetValue(FieldOfViewProperty);
            set => SetValue(FieldOfViewProperty, value);
        }

        public static readonly DependencyProperty FieldOfViewProperty = DependencyProperty.Register(nameof(FieldOfView), typeof(float), typeof(PerspectiveCamera), new PropertyMetadata(45.0f, OnFieldOfViewChanged));

        private static void OnFieldOfViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PerspectiveCamera self) return;
            //self.OnFieldOfViewChanged(e);
        }


        public PerspectiveCamera() { }

        public PerspectiveCamera(Point3 position, Vector3 lookDirection, Vector3 upDirection, float fieldOfView)
        {
            Position = position;
            LookDirection = lookDirection;
            UpDirection = upDirection;
            FieldOfView = fieldOfView;
        }

        internal Matrix4x4 GetProjectionMatrix(float aspectRatio, float zNear, float zFar)
        {
            var fov = (float)(FieldOfView * Math.PI / 180);

            return Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, zNear, zFar);

            // Note: h and w are 1/2 of the inverse of the width/height ratios:
            //
            //  h = 1/(heightDepthRatio) * (1/2)
            //  w = 1/(widthDepthRatio) * (1/2)
            //
            // Computation for h is a bit different than what you will find in
            // D3DXMatrixPerspectiveFovRH because we have a horizontal rather
            // than vertical FoV.

            var halfWidthDepthRatio = (float)Math.Tan(fov / 2);
            var h = aspectRatio / halfWidthDepthRatio;
            var w = 1 / halfWidthDepthRatio;

            var m33 = zFar != float.PositiveInfinity ? zFar / (zNear - zFar) : -1;
            var m43 = zNear * m33;

            return new Matrix4x4(
                w, 0, 0,    0,
                0, h, 0,    0,
                0, 0, m33, -1,
                0, 0, m43,  0);
        }

        internal override Matrix4x4 GetProjectionMatrix(float aspectRatio) 
            => GetProjectionMatrix(aspectRatio, NearPlaneDistance, FarPlaneDistance);

        /*
        internal override RayHitTestParameters RayFromViewportPoint(Point p, Size viewSize, Rect3D boundingRect, out double distanceAdjustment)
        {
            // The camera may be animating.  Take a snapshot of the current value
            // and get the property values we need. (Window OS #992662)
            Point3 position = Position;
            Vector3 lookDirection = LookDirection;
            Vector3 upDirection = UpDirection;
            Transform3D transform = Transform;
            double zn = NearPlaneDistance;
            double zf = FarPlaneDistance;
            double fov = M3DUtil.DegreesToRadians(FieldOfView);

            //
            //  Compute rayParameters
            //

            // Find the point on the projection plane in post-projective space where
            // the viewport maps to a 2x2 square from (-1,1)-(1,-1).
            Point np = M3DUtil.GetNormalizedPoint(p, viewSize);

            // Note: h and w are 1/2 of the inverse of the width/height ratios:
            //
            //  h = 1/(heightDepthRatio) * (1/2)
            //  w = 1/(widthDepthRatio) * (1/2)
            //
            // Computation for h is a bit different than what you will find in
            // D3DXMatrixPerspectiveFovRH because we have a horizontal rather
            // than vertical FoV.
            double aspectRatio = M3DUtil.GetAspectRatio(viewSize);
            double halfWidthDepthRatio = Math.Tan(fov / 2);
            double h = aspectRatio / halfWidthDepthRatio;
            double w = 1 / halfWidthDepthRatio;

            // To get from projective space to camera space we apply the
            // width/height ratios to find our normalized point at 1 unit
            // in front of the camera.  (1 is convenient, but has no other
            // special significance.) See note above about the construction
            // of w and h.
            Vector3 rayDirection = new Vector3(np.X / w, np.Y / h, -1);

            // Apply the inverse of the view matrix to our rayDirection vector
            // to convert it from camera to world space.
            //
            // NOTE: Because our construction of the ray assumes that the
            //       viewMatrix translates the position to the origin we pass
            //       null for the Camera.Transform below and account for it
            //       later.

            Matrix4x4 viewMatrix = CreateViewMatrix(transform: null, ref position, ref lookDirection, ref upDirection);
            Matrix4x4 invView = viewMatrix;
            invView.Invert();
            invView.MultiplyVector(ref rayDirection);

            // The we have the ray direction, now we need the origin.  The camera's
            // position would work except that we would intersect geometry between
            // the camera plane and the near plane so instead we must find the
            // point on the project plane where the ray (position, rayDirection)
            // intersect (Windows OS #1005064):
            //
            //                     | _.>       p = camera position
            //                rd  _+"          ld = camera look direction
            //                 .-" |ro         pp = projection plane
            //             _.-"    |           rd = ray direction
            //         p +"--------+--->       ro = desired ray origin on pp
            //                ld   |
            //                     pp
            //
            // Above we constructed the direction such that it's length projects to
            // 1 unit on the lookDirection vector.
            //
            //
            //                rd  _.>
            //                 .-"        rd = unnormalized rayDirection
            //             _.-"           ld = normalized lookDirection (length = 1)
            //           -"--------->
            //                 ld   
            //
            // So to find the desired rayOrigin on the projection plane we simply do:            
            Point3 rayOrigin = position + zn * rayDirection;
            rayDirection.Normalize();

            // Account for the Camera.Transform we ignored during ray construction above.
            if (transform != null && transform != Transform3D.Identity)
            {
                Matrix4x4 m = transform.Value;
                m.MultiplyPoint(ref rayOrigin);
                m.MultiplyVector(ref rayDirection);

                PrependInverseTransform(m, ref viewMatrix);
            }

            RayHitTestParameters rayParameters = new RayHitTestParameters(rayOrigin, rayDirection);

            //
            //  Compute HitTestProjectionMatrix
            //

            Matrix4x4 projectionMatrix = GetProjectionMatrix(aspectRatio, zn, zf);

            // The projectionMatrix takes camera-space 3D points into normalized clip
            // space.

            // The viewportMatrix will take normalized clip space into
            // viewport coordinates, with an additional 2D translation
            // to put the ray at the rayOrigin.
            Matrix4x4 viewportMatrix = new Matrix4x4();
            viewportMatrix.TranslatePrepend(new Vector3(-p.X, viewSize.Height - p.Y, 0));
            viewportMatrix.ScalePrepend(new Vector3(viewSize.Width / 2, -viewSize.Height / 2, 1));
            viewportMatrix.TranslatePrepend(new Vector3(1, 1, 0));

            // `First world-to-camera, then camera's projection, then normalized clip space to viewport.
            rayParameters.HitTestProjectionMatrix =
                viewMatrix *
                projectionMatrix *
                viewportMatrix;

            // 
            // Perspective camera doesn't allow negative NearPlanes, so there's
            // not much point in adjusting the ray origin. Hence, the
            // distanceAdjustment remains 0.
            //
            distanceAdjustment = 0.0;

            return rayParameters;
        }
        */

        public new PerspectiveCamera Clone() => (PerspectiveCamera)base.Clone();

        public new PerspectiveCamera CloneCurrentValue() => (PerspectiveCamera)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new PerspectiveCamera();

    }
}
