using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace XamlSpinners
{
    public class SurfaceCanvas : FrameworkElement
    {
        #region Dependency Properties

        public SurfaceElementGroup SurfaceGroup
        {
            get => (SurfaceElementGroup)GetValue(SurfaceGroupProperty);
            set => SetValue(SurfaceGroupProperty, value);
        }

        public static readonly DependencyProperty SurfaceGroupProperty = DependencyProperty.Register(nameof(SurfaceGroup), typeof(SurfaceElementGroup), typeof(SurfaceCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSurfaceGroupChanged));

        private static void OnSurfaceGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not SurfaceCanvas self) return;
            //self.OnGroupChanged(e);
        }


        public Camera Camera
        {
            get => (Camera)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

        public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(nameof(Camera), typeof(Camera), typeof(SurfaceCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnCameraChanged));

        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not SurfaceCanvas self) return;
            //self.OnCameraChanged(e);
        }

        #endregion

        #region Method Overrides

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (SurfaceGroup == null) return;

            var modelToWorldMatrix = SurfaceGroup.Transform.Value;
            var worldToViewMatrix = Camera.CreateViewMatrix();
            var viewToProjectionMatrix = Camera.CreateProjectionMatrix();
            var ndcToScreenMatrix = Matrix4x4.Multiply(
                Matrix4x4.CreateScale(200, 200, 1),
                Matrix4x4.CreateTranslation(200, 200, 0));
            var fullTransformMatrix = modelToWorldMatrix * worldToViewMatrix * viewToProjectionMatrix * ndcToScreenMatrix;

            // Adjust these values as needed
            float baseScale = 0.01f;  // Base scale at reference Z
            float scaleRate = 0.001f; // Rate of scale change with depth

            foreach (var shape in SurfaceGroup.Children)
            {
                var projectedPosition = Vector4.Transform(shape.Position, fullTransformMatrix);
                var perspectivePosition = projectedPosition * (1 / projectedPosition.W);

                var scale = baseScale * (float)Math.Pow(2, (1 - perspectivePosition.Z) / scaleRate);

                shape.DrawShape(drawingContext, perspectivePosition, scale: scale);
            }
        }




        #endregion

    }
}
