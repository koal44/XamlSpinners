using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace ProjectionCanvas
{
    public class ProjectionViewport : FrameworkElement
    {
        // for now just one group
        public ProjectableShapeGroup ShapeGroup
        {
            get => (ProjectableShapeGroup)GetValue(ShapeGroupProperty);
            set => SetValue(ShapeGroupProperty, value);
        }

        public static readonly DependencyProperty ShapeGroupProperty = DependencyProperty.Register(nameof(ShapeGroup), typeof(ProjectableShapeGroup), typeof(ProjectionViewport), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnGroupChanged));

        private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionViewport self) return;
            //self.OnGroupChanged(e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            if (ShapeGroup == null) return;

            if (Camera is not ProjectionCamera cam) return;
            //cam.FarPlaneDistance = 600;
            //cam.NearPlaneDistance = 400;
            //cam.Position = new Point3(300, 300, 300);
            //cam.LookDirection = new Vector3(-1, -1, -1);

            var modelMatrix = ShapeGroup.Transform.Value;
            

            var viewMatrix = Camera.GetViewMatrix();
            var projectionMatrix = Camera.GetProjectionMatrix(1);
            var foo = projectionMatrix.ToString();
            var w = 200;
            var h = 200;

            foreach (var shape in ShapeGroup.Children)
            {
                var modelPoint = Point3.Transform(shape.Position, modelMatrix);
                var viewPoint = Point3.Transform(modelPoint, viewMatrix);

                Point3 projectedPoint;

                if (Camera is ProjectionCamera)
                {
                    var viewPoint4 = new Point4(viewPoint.X, viewPoint.Y, viewPoint.Z, 1);
                    var projectedPoint4 = Point4.Transform(viewPoint4, projectionMatrix);
                    var perspectivePoint4 = projectedPoint4 * (1 / projectedPoint4.W);
                    projectedPoint = new Point3(perspectivePoint4.X, perspectivePoint4.Y, perspectivePoint4.Z);
                }
                else if (Camera is OrthographicCamera)
                {
                    projectedPoint = Point3.Transform(viewPoint, projectionMatrix);
                }
                else throw new System.Exception("Unknown camera type");

                var screenMatrix = Matrix4x4.Multiply(
                    Matrix4x4.CreateScale(200, 200, 1),
                    Matrix4x4.CreateTranslation(200, 200, 0));

                projectedPoint = Point3.Transform(projectedPoint, screenMatrix);

                if (projectedPoint.Z < 0)
                {
                    var bar = 1;
                    // Math.Pow(projectedPoint.Z, -1)
                }
                shape.DrawShape(drawingContext, projectedPoint, scale: (float)Math.Pow(projectedPoint.Z, -1));
            }
        }

        public Camera Camera
        {
            get => (Camera)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

        public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(nameof(Camera), typeof(Camera), typeof(ProjectionViewport), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnCameraChanged));

        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectionViewport self) return;
            //self.OnCameraChanged(e);
        }


    }
}
