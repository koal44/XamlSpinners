using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shapes
{
    public class DashedEllipse : Shape
    {
        #region Data

        private readonly EllipseGeometry _geometry;
        private readonly Pen _pen;
        private Rect _rect;

        #endregion

        #region Dependency Properties

        public int DashCount
        {
            get => (int)GetValue(DashCountProperty);
            set => SetValue(DashCountProperty, value);
        }

        public static readonly DependencyProperty DashCountProperty = DependencyProperty.Register(nameof(DashCount), typeof(int), typeof(DashedEllipse), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender, OnDashCountChanged), OnValidateDashCountCallback);

        private static void OnDashCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DashedEllipse self) return;
            self.UpdateDashStyle();
        }

        private static bool OnValidateDashCountCallback(object value) => (int)value > 0;


        public double DashLengthRatio
        {
            get => (double)GetValue(DashLengthRatioProperty);
            set => SetValue(DashLengthRatioProperty, value);
        }

        public static readonly DependencyProperty DashLengthRatioProperty = DependencyProperty.Register(nameof(DashLengthRatio), typeof(double), typeof(DashedEllipse), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender, OnDashLengthRatioChanged), OnValidateDashLengthRatio);

        private static void OnDashLengthRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DashedEllipse self) return;
            self.UpdateDashStyle();
        }

        private static bool OnValidateDashLengthRatio(object value) => (double)value >= 0 && (double)value <= 1;


        public double DashFractionalOffset
        {
            get => (double)GetValue(DashFractionalOffsetProperty);
            set => SetValue(DashFractionalOffsetProperty, value);
        }

        public static readonly DependencyProperty DashFractionalOffsetProperty = DependencyProperty.Register(nameof(DashFractionalOffset), typeof(double), typeof(DashedEllipse), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnDashFractionalOffsetChanged), OnValidateDashFractionalOffset);

        private static void OnDashFractionalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DashedEllipse self) return;
            self.UpdateDashStyle();
        }

        private static bool OnValidateDashFractionalOffset(object value) => (double)value >= -1 && (double)value <= 1;


        public Anchor DashAnchor
        {
            get => (Anchor)GetValue(DashAnchorProperty);
            set => SetValue(DashAnchorProperty, value);
        }

        public static readonly DependencyProperty DashAnchorProperty = DependencyProperty.Register(nameof(DashAnchor), typeof(Anchor), typeof(DashedEllipse), new FrameworkPropertyMetadata(Anchor.Symmetric, FrameworkPropertyMetadataOptions.AffectsRender, OnDashAnchorChanged));

        private static void OnDashAnchorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DashedEllipse self) return;
            self.UpdateDashStyle();
        }

        #endregion

        #region Ctors

        static DashedEllipse()
        {
            StretchProperty.OverrideMetadata(typeof(DashedEllipse), new FrameworkPropertyMetadata(Stretch.Fill));
        }


        public DashedEllipse()
        {
            _rect = Rect.Empty;
            _geometry = new EllipseGeometry();
            var dashArray = new DoubleCollection(2) { 1, 0 };
            _pen = new Pen
            {
                Thickness = StrokeThickness,
                Brush = Stroke,
                DashCap = StrokeDashCap,
                DashStyle = new DashStyle(dashArray, StrokeDashOffset)
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

            _geometry.Center = new Point(_rect.X + _rect.Width / 2, _rect.Y + _rect.Height / 2);
            _geometry.RadiusX = _rect.Width / 2;
            _geometry.RadiusY = _rect.Height / 2;

            UpdateDashStyle();

            return finalSize;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!_rect.IsEmpty)
            {
                drawingContext.DrawGeometry(Fill, _pen, _geometry);
            }
        }


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
                    UpdateDashStyle();
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
                    UpdateDashStyle();
                    break;
                case nameof(Stretch):
                    InvalidateVisual();
                    break;
            }
        }

        #endregion

        #region Methods

        private void UpdateDashStyle()
        {
            var a = _rect.Width / 2;
            var b = _rect.Height / 2;

            double circumference = a == b
                ? 2 * Math.PI * a
                : Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b))); // Ramanujan approximation

            double normalizedCircumference = circumference / StrokeThickness;

            // let segment = dash + gap; DashRatio = dash / segment; circumference = segment * dashCount
            var segmentLength = normalizedCircumference / DashCount;
            var dashLength = segmentLength * DashLengthRatio;
            var gapLength = segmentLength - dashLength;

            _pen.DashStyle.Dashes[0] = dashLength;
            _pen.DashStyle.Dashes[1] = gapLength;

            // anchor the dashes by normalizing the offset and adjusting for dash growth
            // anchored dashes appear to be fixed with respect to: size, thickness, and dash/gap changes.
            double gapAdjustment = - gapLength / 2;
            double offset = DashFractionalOffset * normalizedCircumference;

            offset += DashAnchor switch
            {
                Anchor.Symmetric => gapAdjustment,
                Anchor.Start => 0,
                _ => throw new NotImplementedException()
            };

            _pen.DashStyle.Offset = offset;
        }

        #endregion
    }

    public enum Anchor
    {
        Symmetric,
        Start
    }
}
