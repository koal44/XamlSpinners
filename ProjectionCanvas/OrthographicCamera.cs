using System;
using System.Numerics;
using System.Windows;

namespace ProjectionCanvas
{
    public class OrthographicCamera : ProjectionCamera
    {
        public OrthographicCamera() { }

        public OrthographicCamera(Point3 position, Vector3 lookDirection, Vector3 upDirection, float width)
        {
            Position = position;
            LookDirection = lookDirection;
            UpDirection = upDirection;
            Width = width;
        }

        internal Matrix4x4 GetProjectionMatrix0(float aspectRatio, float zNear, float zFar)
        {
            var w = Width;
            var h = w / aspectRatio;

            var m22 = 1 / (zNear - zFar);
            var m32 = zNear * m22;

            return new Matrix4x4(
                2/w, 0,   0,   0,
                0,   2/h, 0,   0,
                0,   0,   m22, 0,
                0,   0,   m32, 1);
        }


        internal Matrix4x4 GetProjectionMatrix2(float aspectRatio, float zNear, float zFar)
        {
            var w = Width;
            var h = w / aspectRatio;

            var m22 = 2 / (zFar - zNear);
            var m32 = -(zFar + zNear) / (zFar - zNear);

            return new Matrix4x4(
                2/w, 0,   0,   0,
                0,   2/h, 0,   0,
                0,   0,   m22, m32,
                0,   0,   0,   1);
        }


        internal Matrix4x4 GetProjectionMatrix(float aspectRatio, float zNear, float zFar)
        {
            var w = Width;
            var h = w / aspectRatio;

            var m33 = -2 / (zFar - zNear);
            var m43 = -(zFar + zNear) / (zFar - zNear);

            return new Matrix4x4(
                2/w, 0,   0,   0,
                0,   2/h, 0,   0,
                0,   0,   m33, 0,
                0,   0,   m43, 1);
        }

        internal override Matrix4x4 GetProjectionMatrix(float aspectRatio)
        {
            return GetProjectionMatrix(aspectRatio, NearPlaneDistance, FarPlaneDistance);
        }

        /*
        internal override RayHitTestParameters RayFromViewportPoint(Point p, Size viewSize, Rect3 boundingRect, out float distanceAdjustment)
        {
            // The camera may be animating.  Take a snapshot of the current value
            // and get the property values we need. (Window OS #992662)
            var position = Position;
            var lookDirection = LookDirection;
            var upDirection = UpDirection;
            var zn = NearPlaneDistance;
            var zf = FarPlaneDistance;
            var width = Width;

            //
            //  Compute rayParameters
            //

            // Find the point on the projection plane in post-projective space where
            // the viewport maps to a 2x2 square from (-1,1)-(1,-1).
            Point np = M3DUtil.GetNormalizedPoint(p, viewSize);

            var aspectRatio = M3DUtil.GetAspectRatio(viewSize);
            var w = width;
            var h = w / aspectRatio;

            // Direction is always perpendicular to the viewing surface.
            var direction = new Vector3(0, 0, -1);

            // Apply the inverse of the view matrix to our ray.
            Matrix4x4 viewMatrix = CreateViewMatrix(Transform, ref position, ref lookDirection, ref upDirection);
            Matrix4x4 invView = viewMatrix;
            Matrix4x4.Invert(invView, out invView);

            // We construct our ray such that the origin resides on the near
            // plane.  If our near plane is too far from our the bounding box
            // of our scene then the results will be inaccurate.  (e.g.,
            // OrthographicCameras permit negative near planes, so the near
            // plane could be at -Inf.)
            // 
            // However, it is permissable to move the near plane nearer to
            // the scene bounds without changing what the ray intersects.
            // If the near plane is sufficiently far from the scene bounds
            // we make this adjustment below to increase precision.

            Rect3 transformedBoundingBox =
                M3DUtil.ComputeTransformedAxisAlignedBoundingBox(
                    ref boundingRect,
                    ref viewMatrix);

            // DANGER:  The NearPlaneDistance property is specified as a
            //          distance from the camera position along the
            //          LookDirection with (Near < Far).  
            //
            //          However, when we transform our scene bounds so that
            //          the camera is aligned with the negative Z-axis the
            //          relationship inverts (Near > Far) as illustrated
            //          below:
            //
            //            NearPlane    Y                      FarPlane
            //                |        ^                          |
            //                |        |                          |
            //                |        | (rect.Z + rect.SizeZ)    |
            //                |        |           o____          |
            //                |        |           |    |         |
            //                |        |           |    |         |
            //                |        |            ____o         |
            //                |        |             (rect.Z)     |
            //                |     Camera ->                     |
            //          +Z  <----------+----------------------------> -Z
            //                |        0                          |
            //
            //          It is surprising, but its the "far" side of the
            //          transformed scene bounds that determines the near
            //          plane distance.

            var zn2 = -AddEpsilon(transformedBoundingBox.Z + transformedBoundingBox.SizeZ);

            if (zn2 > zn)
            {
                //
                // Our near plane is far from our children. Construct a new
                // near plane that's closer. Note that this will modify our
                // distance computations, so we have to be sure to adjust our
                // distances appropriately.
                //
                distanceAdjustment = zn2 - zn;

                zn = zn2;
            }
            else
            {
                //
                // Our near plane is either close to or in front of our
                // children, so let's keep it -- no distance adjustment needed.
                //
                distanceAdjustment = 0.0f;
            }

            // Our origin is the point normalized to the front of our viewing volume.
            // To find our origin's x/y we just need to scale the normalize point by our
            // width/height.  In camera space we are looking down the negative Z axis
            // so we just set Z to be -zn which puts us on the projection plane
            // (Windows OS #1005064).
            var origin = new Point3((float)(np.X * (w / 2)), (float)(np.Y * (h / 2)), (float)-zn);

            invView.MultiplyPoint(ref origin);
            invView.MultiplyVector(ref direction);

            RayHitTestParameters rayParameters = new RayHitTestParameters(origin, direction);

            //
            //  Compute HitTestProjectionMatrix
            //

            Matrix4x4 projectionMatrix = GetProjectionMatrix(aspectRatio, zn, zf);

            // The projectionMatrix takes camera-space 3D points into normalized clip
            // space.

            // The viewportMatrix will take normalized clip space into
            // viewport coordinates, with an additional 2D translation
            // to put the ray at the origin.
            var viewportMatrix = new Matrix4x4();
            viewportMatrix.TranslatePrepend(new Vector3((float)-p.X, (float)(viewSize.Height - p.Y), 0f));
            viewportMatrix.ScalePrepend(new Vector3((float)(viewSize.Width / 2), (float)(-viewSize.Height / 2), 1));
            viewportMatrix.TranslatePrepend(new Vector3(1, 1, 0));

            // `First world-to-camera, then camera's projection, then normalized clip space to viewport.
            rayParameters.HitTestProjectionMatrix =
                viewMatrix *
                projectionMatrix *
                viewportMatrix;

            return rayParameters;

        }
        */


        private static float AddEpsilon(float x)
        {
            //
            // x is either close to 0 or not. If it's close to 0, then 1.0 is
            // sufficiently large to act as an epsilon.  If it's not, then
            // 0.1*Math.Abs(x) sufficiently large.
            //
            return x + 0.1f * Math.Abs(x) + 1.0f;
        }



        public float Width
        {
            get => (float)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(float), typeof(OrthographicCamera), new PropertyMetadata(2.0f, OnWidthChanged));

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not OrthographicCamera self) return;
            //self.OnWidthChanged(e);
        }


        public new OrthographicCamera Clone()
            => (OrthographicCamera)base.Clone();

        public new OrthographicCamera CloneCurrentValue()
            => (OrthographicCamera)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore()
            => new OrthographicCamera();

        /*
        internal override void UpdateResource(DUCE.Channel channel, bool skipOnChannelCheck)
        {
            // If we're told we can skip the channel check, then we must be on channel
            Debug.Assert(!skipOnChannelCheck || _duceResource.IsOnChannel(channel));

            if (skipOnChannelCheck || _duceResource.IsOnChannel(channel))
            {
                base.UpdateResource(channel, skipOnChannelCheck);

                // Read values of properties into local variables
                Transform3D vTransform = Transform;

                // Obtain handles for properties that implement DUCE.IResource
                DUCE.ResourceHandle hTransform;
                if (vTransform == null ||
                    Object.ReferenceEquals(vTransform, Transform3D.Identity)
                    )
                {
                    hTransform = DUCE.ResourceHandle.Null;
                }
                else
                {
                    hTransform = ((DUCE.IResource)vTransform).GetHandle(channel);
                }

                // Obtain handles for animated properties
                DUCE.ResourceHandle hNearPlaneDistanceAnimations = GetAnimationResourceHandle(NearPlaneDistanceProperty, channel);
                DUCE.ResourceHandle hFarPlaneDistanceAnimations = GetAnimationResourceHandle(FarPlaneDistanceProperty, channel);
                DUCE.ResourceHandle hPositionAnimations = GetAnimationResourceHandle(PositionProperty, channel);
                DUCE.ResourceHandle hLookDirectionAnimations = GetAnimationResourceHandle(LookDirectionProperty, channel);
                DUCE.ResourceHandle hUpDirectionAnimations = GetAnimationResourceHandle(UpDirectionProperty, channel);
                DUCE.ResourceHandle hWidthAnimations = GetAnimationResourceHandle(WidthProperty, channel);

                // Pack & send command packet
                DUCE.MILCMD_ORTHOGRAPHICCAMERA data;
                unsafe
                {
                    data.Type = MILCMD.MilCmdOrthographicCamera;
                    data.Handle = _duceResource.GetHandle(channel);
                    data.htransform = hTransform;
                    if (hNearPlaneDistanceAnimations.IsNull)
                    {
                        data.nearPlaneDistance = NearPlaneDistance;
                    }
                    data.hNearPlaneDistanceAnimations = hNearPlaneDistanceAnimations;
                    if (hFarPlaneDistanceAnimations.IsNull)
                    {
                        data.farPlaneDistance = FarPlaneDistance;
                    }
                    data.hFarPlaneDistanceAnimations = hFarPlaneDistanceAnimations;
                    if (hPositionAnimations.IsNull)
                    {
                        data.position = CompositionResourceManager.Point3ToMilPoint3F(Position);
                    }
                    data.hPositionAnimations = hPositionAnimations;
                    if (hLookDirectionAnimations.IsNull)
                    {
                        data.lookDirection = CompositionResourceManager.Vector3ToMilPoint3F(LookDirection);
                    }
                    data.hLookDirectionAnimations = hLookDirectionAnimations;
                    if (hUpDirectionAnimations.IsNull)
                    {
                        data.upDirection = CompositionResourceManager.Vector3ToMilPoint3F(UpDirection);
                    }
                    data.hUpDirectionAnimations = hUpDirectionAnimations;
                    if (hWidthAnimations.IsNull)
                    {
                        data.width = Width;
                    }
                    data.hWidthAnimations = hWidthAnimations;

                    // Send packed command structure
                    channel.SendCommand(
                        (byte*)&data,
                        sizeof(DUCE.MILCMD_ORTHOGRAPHICCAMERA));
                }
            }
        }
        */
        


    }
}