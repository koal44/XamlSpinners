using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shapes
{
    public class SegmentedEllipse : Shape
    {
        #region Data

        private readonly EllipseGeometry _ring;
        private readonly Pen _pen;
        private Rect _rect;

        #endregion

        #region Dependency Properties

        public int SegmentCount
        {
            get => (int)GetValue(SegmentCountProperty);
            set => SetValue(SegmentCountProperty, value);
        }

        public static readonly DependencyProperty SegmentCountProperty = DependencyProperty.Register(nameof(SegmentCount), typeof(int), typeof(SegmentedEllipse), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender, OnSegmentCountChanged), ValidateCallback);

        private static bool ValidateCallback(object value) => (int)value > 0;

        private static void OnSegmentCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SegmentedEllipse self) return;
            self.UpdateDashArray();
        }


        public double DashGapRatio
        {
            get => (double)GetValue(DashGapRatioProperty);
            set => SetValue(DashGapRatioProperty, value);
        }

        public static readonly DependencyProperty DashGapRatioProperty = DependencyProperty.Register(nameof(DashGapRatio), typeof(double), typeof(SegmentedEllipse), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender, OnDashGapRatioChanged));

        private static void OnDashGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SegmentedEllipse self) return;
            self.UpdateDashArray();
        }


        public bool IsGapAnchored
        {
            get => (bool)GetValue(IsGapAnchoredProperty);
            set => SetValue(IsGapAnchoredProperty, value);
        }

        public static readonly DependencyProperty IsGapAnchoredProperty = DependencyProperty.Register(nameof(IsGapAnchored), typeof(bool), typeof(SegmentedEllipse), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, OnIsGapAnchoredChanged));

        private static void OnIsGapAnchoredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SegmentedEllipse self) return;
            self.UpdateDashArray();
        }

        #endregion

        #region Ctors

        static SegmentedEllipse()
        {
            StretchProperty.OverrideMetadata(typeof(SegmentedEllipse), new FrameworkPropertyMetadata(Stretch.Fill));
        }


        public SegmentedEllipse()
        {
            _rect = Rect.Empty;
            _ring = new EllipseGeometry();
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

        protected override Geometry DefiningGeometry => _rect.IsEmpty ? Geometry.Empty : _ring;


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

            _ring.Center = new Point(_rect.X + _rect.Width / 2, _rect.Y + _rect.Height / 2);
            _ring.RadiusX = _rect.Width / 2;
            _ring.RadiusY = _rect.Height / 2;

            UpdateDashArray();

            return finalSize;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!_rect.IsEmpty)
            {
                drawingContext.DrawGeometry(Fill, _pen, _ring);
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
                    UpdateDashArray();
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
                    //UpdateDashArray();
                    break;
            }
        }

        #endregion

        #region Methods

        private void UpdateDashArray()
        {
            var a = _rect.Width / 2;
            var b = _rect.Height / 2;

            double perimeter;

            if (a == b)
            {
                perimeter = 2 * Math.PI * a;
            }
            else
            {
                perimeter = Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b))); // Ramanujan approximation
            }

            // thickness scales the dash and gap lengths which we must adjust for
            double thicknessFactor = 1 / StrokeThickness;

            double segment = perimeter / SegmentCount * thicknessFactor;

            // dash + gap = segment; d/g = DashGapRatio
            // g * ratio + g = segment
            var gapLength = segment / (DashGapRatio + 1);
            var dashLength = segment - gapLength;

            _pen.DashStyle.Dashes[0] = dashLength;
            _pen.DashStyle.Dashes[1] = gapLength;
        }

        #endregion
    }
}
