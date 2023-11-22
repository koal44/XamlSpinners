using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProjectionCanvas
{
    public class ProjectableShape : Animatable
    {
        internal static Transform3 s_Transform = Transform3.Identity;


        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(
                nameof(Position), typeof(Point3), typeof(ProjectableShape));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(
                nameof(Fill), typeof(Brush), typeof(ProjectableShape));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(
                nameof(Size), typeof(Size), typeof(ProjectableShape));


        public static readonly DependencyProperty TransformProperty =
            DependencyProperty.Register(
                nameof(Transform), typeof(Transform3), typeof(ProjectableShape), new FrameworkPropertyMetadata(Transform3.Identity, FrameworkPropertyMetadataOptions.AffectsRender, OnTransformChanged));

        private static void OnTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectableShape self) return;
            //self.OnTransformChanged(e);
        }

        public Point3 Position
        {
            get => (Point3)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public Size Size
        {
            get => (Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public Transform3 Transform
        {
            get => (Transform3)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        public ProjectableShape()
        {

        }

        public void DrawShape(DrawingContext context, Point3 coords, float scale)
        {
            var rect = new Rect(
                coords.X - scale * (Size.Width / 2),
                coords.Y - scale * (Size.Height / 2),
                scale * Size.Width,
                scale * Size.Height);

            var geometry = new RectangleGeometry(rect);
            context.DrawGeometry(Fill, null, geometry);
        }

        protected override Freezable CreateInstanceCore() => new ProjectableShape();

        public new ProjectableShape Clone() => (ProjectableShape)base.Clone();

        public new ProjectableShape CloneCurrentValue() => (ProjectableShape)base.CloneCurrentValue();

    }
}