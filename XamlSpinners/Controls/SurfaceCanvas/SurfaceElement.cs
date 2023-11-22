using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public class SurfaceElement : Animatable
    {

        #region Dependency Properties

        public Vector3 Position
        {
            get => (Vector3)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(Vector3), typeof(SurfaceElement), new FrameworkPropertyMetadata(new Vector3(), FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(SurfaceElement), new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.AffectsRender));


        public Size Size
        {
            get => (Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(Size), typeof(SurfaceElement), new FrameworkPropertyMetadata(new Size(1,1), FrameworkPropertyMetadataOptions.AffectsRender));


        public Matrix4Transform Transform
        {
            get => (Matrix4Transform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(nameof(Transform), typeof(Matrix4Transform), typeof(SurfaceElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region Methods

        public void DrawShape(DrawingContext context, Vector4 coords, float scale)
        {
            var rect = new Rect(
                coords.X - scale * (Size.Width / 2),
                coords.Y - scale * (Size.Height / 2),
                scale * Size.Width,
                scale * Size.Height);

            var geometry = new RectangleGeometry(rect);
            context.DrawGeometry(Fill, null, geometry);
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new SurfaceElement();

        public new SurfaceElement Clone() => (SurfaceElement)base.Clone();

        public new SurfaceElement CloneCurrentValue() => (SurfaceElement)base.CloneCurrentValue();

        #endregion

    }
}