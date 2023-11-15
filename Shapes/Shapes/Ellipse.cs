using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shapes
{
    public class Ellipse : Shape
    {
        #region Data

        private EllipseGeometry _geometry;
        private readonly Pen _pen;
        private readonly DoubleCollection _dashArray;
        private Rect _rect;

        #endregion

        #region Dependency Properties

        public double DashLength
        {
            get => (double)GetValue(DashLengthProperty);
            set => SetValue(DashLengthProperty, value);
        }

        public static readonly DependencyProperty DashLengthProperty = DependencyProperty.Register(nameof(DashLength), typeof(double), typeof(Ellipse), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));


        public double GapLength
        {
            get => (double)GetValue(GapLengthProperty);
            set => SetValue(GapLengthProperty, value);
        }

        public static readonly DependencyProperty GapLengthProperty = DependencyProperty.Register(nameof(GapLength), typeof(double), typeof(Ellipse), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region Constructors

        static Ellipse()
        {
            StretchProperty.OverrideMetadata(typeof(Ellipse), new FrameworkPropertyMetadata(Stretch.Fill));
        }


        public Ellipse()
        {
            _rect = Rect.Empty;
            _geometry = new EllipseGeometry();
            _dashArray = new DoubleCollection(2) { DashLength, GapLength };
            _pen = new Pen
            {
                Thickness = StrokeThickness,
                Brush = Stroke,
                DashCap = StrokeDashCap,
                DashStyle = new DashStyle(_dashArray, StrokeDashOffset)
            };
        }

        #endregion

        #region Shape

        protected override Geometry DefiningGeometry => _rect.IsEmpty ? Geometry.Empty : _geometry;


        public override Geometry RenderedGeometry => DefiningGeometry;


        public override Transform GeometryTransform => Transform.Identity;


        protected override Size MeasureOverride(Size constraint)
        {
            double size = 0;

            if (Stretch == Stretch.UniformToFill)
            {
                if (double.IsInfinity(constraint.Width) && double.IsInfinity(constraint.Height))
                    size = 0;
                else if (double.IsInfinity(constraint.Width) || double.IsInfinity(constraint.Height))
                    size = Math.Min(constraint.Width, constraint.Height);
                else
                    size = Math.Max(constraint.Width, constraint.Height);
            }

            size = Math.Max(size, StrokeThickness);

            return new Size(size, size);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            _rect = new Rect(x: StrokeThickness / 2,
                             y: StrokeThickness / 2,
                             width: Math.Max(0, finalSize.Width - StrokeThickness),
                             height: Math.Max(0, finalSize.Height - StrokeThickness));

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
            _geometry.Center = new Point(_rect.Width / 2, _rect.Height / 2);
            _geometry.RadiusX = _rect.Width / 2;
            _geometry.RadiusY = _rect.Height / 2;

            return finalSize;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!_rect.IsEmpty)
            {
                drawingContext.DrawGeometry(Fill, _pen, _geometry);
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.Property.Name)
            {
                case nameof(Stroke):
                    _pen.Brush = Stroke;
                    break;
                case nameof(StrokeThickness):
                    _pen.Thickness = StrokeThickness;
                    break;
                case nameof(StrokeDashCap):
                    _pen.DashCap = StrokeDashCap;
                    break;
                case nameof(StrokeStartLineCap):
                    _pen.StartLineCap = StrokeStartLineCap;
                    break;
                case nameof(StrokeEndLineCap):
                    _pen.EndLineCap = StrokeEndLineCap;
                    break;
                case nameof(StrokeDashOffset):
                    _pen.DashStyle.Offset = StrokeDashOffset;
                    break;
                case nameof(DashLength):
                    _pen.DashStyle.Dashes[0] = DashLength;
                    break;
                case nameof(GapLength):
                    _pen.DashStyle.Dashes[1] = GapLength;
                    break;
            }
        }

        #endregion

    }
}
