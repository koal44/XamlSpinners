using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace XamlSpinners
{
    public class SurfaceCanvas : FrameworkElement
    {
        private Matrix4x4 _worldToScreenMatrix;
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
            if (d is not SurfaceCanvas self) return;
            self.OnGroupChanged(e);
        }

        protected virtual void OnGroupChanged(DependencyPropertyChangedEventArgs e) { }

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

        protected virtual void OnCameraChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateWorldToScreenMatrix();
        }

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(SurfaceCanvas), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange, OnStretchChanged));

        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SurfaceCanvas self) return;
            self.OnStretchChanged(e);
        }

        protected virtual void OnStretchChanged(DependencyPropertyChangedEventArgs e) { }

        public double DepthExaggeration
        {
            get => (double)GetValue(DepthExaggerationProperty);
            set => SetValue(DepthExaggerationProperty, value);
        }

        public static readonly DependencyProperty DepthExaggerationProperty = DependencyProperty.Register(nameof(DepthExaggeration), typeof(double), typeof(SurfaceCanvas), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange, OnDepthExaggerationChanged));

        private static void OnDepthExaggerationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SurfaceCanvas self) return;
            self.OnDepthExaggerationChanged(e);
        }

        protected virtual void OnDepthExaggerationChanged(DependencyPropertyChangedEventArgs e) { }

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
            var modelToScreenMatrix = modelToWorldMatrix * _worldToScreenMatrix;

            // Or get these from Camera.CreateProjectionMatrix(), M11, M22. same same
            // scale is in range (inf, 0), with fov (0, 180). s = 1, when fov = 90
            var projectionYScale = (1 / Math.Tan(Camera.FieldOfView / 2 * Math.PI / 180));
            var projectionXScale = (projectionYScale / _rect.Height * _rect.Width);

            var depthSortingList = new List<(SurfaceElement shape, Vector4 transformedPosition)>();

            foreach (var shape in SurfaceGroup.Children)
            {
                var transformedPosition = Vector4.Transform(shape.Position, modelToScreenMatrix);
                depthSortingList.Add((shape, transformedPosition));
            }

            depthSortingList.Sort((a, b) => b.transformedPosition.W.CompareTo(a.transformedPosition.W));

            foreach (var (shape, transformedPosition) in depthSortingList)
            {
                var perspectivePosition = transformedPosition * (1 / transformedPosition.W);

                // after projection, transformedPosition.W is effectively position.Z (in camera space)
                var scaleX = projectionXScale / transformedPosition.W * (_rect.Width / 2);
                var scaleY = projectionYScale / transformedPosition.W * (_rect.Height / 2);

                if (DepthExaggeration > 0)
                {
                    // if 's' is scale and 'k' is DepthExaggeration, then we want to scale by
                    // s^(k+1) / (s^k + k)
                    var xPowK = Math.Pow(scaleX, DepthExaggeration);
                    var yPowK = Math.Pow(scaleY, DepthExaggeration);

                    scaleX *= xPowK / (xPowK + DepthExaggeration);
                    scaleY *= yPowK / (yPowK + DepthExaggeration);
                }

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

            _worldToScreenMatrix = worldToViewMatrix * viewToProjectionMatrix * ndcToScreenMatrix;
        }

        #endregion

    }
}
