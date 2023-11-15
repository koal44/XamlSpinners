using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Shapes2
{
    public sealed class Elli : Shape
    {
        private Rect _rect = Rect.Empty;

        public Elli() { }

        static Elli()
        {
            StretchProperty.OverrideMetadata(typeof(Elli), new FrameworkPropertyMetadata(Stretch.Fill));
        }

        public override Geometry RenderedGeometry => DefiningGeometry;

        public override Transform GeometryTransform => Transform.Identity;

        // first pass
        protected override Size MeasureOverride(Size constraint)
        {
            if (Stretch == Stretch.UniformToFill)
            {
                double width = constraint.Width;
                double height = constraint.Height;

                if (Double.IsInfinity(width) && Double.IsInfinity(height))
                {
                    return GetNaturalSize();
                }
                else if (Double.IsInfinity(width) || Double.IsInfinity(height))
                {
                    width = Math.Min(width, height);
                }
                else
                {
                    width = Math.Max(width, height);
                }

                return new Size(width, width);
            }

            return GetNaturalSize();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // We construct the rectangle to fit finalSize with the appropriate Stretch mode.  The rendering
            // transformation will thus be the identity.

            double penThickness = StrokeThickness;
            double margin = penThickness / 2;

            _rect = new Rect(x: margin,
                             y: margin,
                             width: Math.Max(0, finalSize.Width - penThickness),
                             height: Math.Max(0, finalSize.Height - penThickness));

            switch (Stretch)
            {
                case Stretch.None:
                    _rect.Width = _rect.Height = 0;
                    break;
                case Stretch.Fill:
                    // _rect has already been initialized to fill the box
                    break;
                case Stretch.Uniform:
                    _rect.Width = _rect.Height = Math.Min(_rect.Width, _rect.Height);
                    break;
                case Stretch.UniformToFill:
                    _rect.Width = _rect.Height = Math.Max(_rect.Width, _rect.Height);
                    break;
            }

            ResetRenderedGeometry();

            return finalSize;
        }

        protected override Geometry DefiningGeometry => _rect.IsEmpty ? Geometry.Empty : new EllipseGeometry(_rect);

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!_rect.IsEmpty)
            {
                var pen = new Pen()
                {
                    Brush = Stroke,
                    Thickness = StrokeThickness,
                    DashCap = StrokeDashCap,
                    DashStyle = StrokeDashArray != null ? new DashStyle(StrokeDashArray, StrokeDashOffset) : null,
                    StartLineCap = StrokeStartLineCap,
                    EndLineCap = StrokeEndLineCap,
                    LineJoin = StrokeLineJoin,
                    MiterLimit = StrokeMiterLimit,
                };
                drawingContext.DrawGeometry(Fill, pen, new EllipseGeometry(_rect));
            }
        }


        internal override void CacheDefiningGeometry() => _rect = new Rect(StrokeThickness / 2, StrokeThickness / 2, 0, 0);

        internal override Size GetNaturalSize() => new(StrokeThickness, StrokeThickness);

        internal override Rect GetDefiningGeometryBounds() => _rect;

    }
}