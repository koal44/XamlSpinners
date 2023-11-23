using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace XamlSpinners
{
    public class SurfaceCanvas : FrameworkElement
    {
        private Matrix4x4 worldToScreenMatrix;
        private Rect _rect;

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
            if (d is not SurfaceCanvas self) return;
            self.OnCameraChanged(e);
        }

        protected void OnCameraChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateWorldToScreenMatrix();
        }


        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(SurfaceCanvas), new PropertyMetadata(Stretch.Uniform, OnStretchChanged));

        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SurfaceCanvas self) return;
            self.OnStretchChanged(e);
        }

        protected void OnStretchChanged(DependencyPropertyChangedEventArgs e)
        {
            // Handle the property changed event here, e.g., raise a PropertyChanged event.
        }


        #endregion

        #region Method Overrides

        protected override Size MeasureOverride(Size constraint)
        {
            double desiredSize = 0;

            if (Stretch == Stretch.UniformToFill)
            {
                if (double.IsInfinity(constraint.Width) && double.IsInfinity(constraint.Height))
                    desiredSize = 0;
                else if (double.IsInfinity(constraint.Width) || double.IsInfinity(constraint.Height))
                    desiredSize = Math.Min(constraint.Width, constraint.Height);
                else
                    desiredSize = Math.Max(constraint.Width, constraint.Height);
            }

            desiredSize = Math.Max(desiredSize, 16);

            return new Size(desiredSize, desiredSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var margin = 0.0;
            _rect = new Rect(
                x: margin,
                y: margin,
                width: Math.Max(0, finalSize.Width - 2 * margin),
                height: Math.Max(0, finalSize.Height - 2 * margin));

            switch (Stretch)
            {
                case Stretch.None:
                    _rect.Width = _rect.Height = 0;
                    break;
                case Stretch.Fill:
                    break;
                case Stretch.Uniform:
                    _rect.Width = _rect.Height = Math.Min(_rect.Width, _rect.Height);
                    break;
                case Stretch.UniformToFill:
                    _rect.Width = _rect.Height = Math.Max(_rect.Width, _rect.Height);
                    break;
            }

            UpdateWorldToScreenMatrix();

            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (SurfaceGroup == null) return;

            var modelToWorldMatrix = SurfaceGroup.Transform.Value;
            var fullTransformMatrix = modelToWorldMatrix * worldToScreenMatrix;

            // Or get these from Camera.CreateProjectionMatrix(), M11, M22. same same
            var projectionYScale = (1 / Math.Tan(Camera.FieldOfView / 2 * Math.PI / 180));
            var projectionXScale = (projectionYScale / _rect.Height * _rect.Width);

            // Adjust these values as needed
            //float baseScale = 0.01f;  // Base scale at reference Z
            //float scaleRate = 0.001f; // Rate of scale change with depth

            foreach (var shape in SurfaceGroup.Children)
            {
                var projectedPosition = Vector4.Transform(shape.Position, fullTransformMatrix);
                var perspectivePosition = projectedPosition * (1 / projectedPosition.W);

                //var scale = baseScale * (float)Math.Pow(2, (1 - perspectivePosition.Z) / scaleRate);
                var scaleY = projectionXScale / projectedPosition.W * (_rect.Width / 2);
                var scaleX = projectionYScale / projectedPosition.W * (_rect.Height / 2);

                shape.DrawShape(drawingContext, perspectivePosition, scaleX: (float)scaleX, scaleY: (float)scaleY);
            }
        }

        #endregion

        #region Methods

        private void UpdateWorldToScreenMatrix()
        {
            if (Camera == null) return;
            if (_rect.IsEmpty || _rect.Width == 0 || _rect.Height == 0) return;

            var worldToViewMatrix = Camera.CreateViewMatrix();
            var viewToProjectionMatrix = Camera.CreateProjectionMatrix();
            var ndcToScreenMatrix = Matrix4x4.Multiply(
                Matrix4x4.CreateScale((float)_rect.Width / 2, (float)_rect.Height / 2, 1),
                Matrix4x4.CreateTranslation((float)_rect.Right / 2, (float)_rect.Bottom / 2, 0));

            worldToScreenMatrix = worldToViewMatrix * viewToProjectionMatrix * ndcToScreenMatrix;
        }

        #endregion

    }
}
