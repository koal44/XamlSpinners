using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shapes
{
    public class SegmentedRing : Shape
    {
        #region Data

        private EllipseGeometry _ring;
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

        public static readonly DependencyProperty DashLengthProperty = DependencyProperty.Register(nameof(DashLength), typeof(double), typeof(SegmentedRing), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));


        public double GapLength
        {
            get => (double)GetValue(GapLengthProperty);
            set => SetValue(GapLengthProperty, value);
        }

        public static readonly DependencyProperty GapLengthProperty = DependencyProperty.Register(nameof(GapLength), typeof(double), typeof(SegmentedRing), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));


        //public double RingThickness
        //{
        //    get => (double)GetValue(RingThicknessProperty);
        //    set => SetValue(RingThicknessProperty, value);
        //}

        //public static readonly DependencyProperty RingThicknessProperty = DependencyProperty.Register(nameof(RingThickness), typeof(double), typeof(SegmentedRing), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnRingThicknessChanged), OnValidateRingThickness);

        //private static void OnRingThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is not SegmentedRing self) return;
        //    self.UpdateDashArray();
        //}

        //private static bool OnValidateRingThickness(object value) => (double)value >= 0;


        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(SegmentedRing), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnRadiusChanged));

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SegmentedRing self) return;
            //self._ring.RadiusX = self.Radius;
            //self._ring.RadiusY = self.Radius;
            //self._ring.Center = new Point(self.Radius, self.Radius);
        }



        public int SegmentCount
        {
            get => (int)GetValue(SegmentCountProperty);
            set => SetValue(SegmentCountProperty, value);
        }

        public static readonly DependencyProperty SegmentCountProperty = DependencyProperty.Register(nameof(SegmentCount), typeof(int), typeof(SegmentedRing), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender, OnSegmentCountChanged), ValidateCallback);

        private static bool ValidateCallback(object value)
        {
            return (int)value > 0;
        }

        private static void OnSegmentCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SegmentedRing self) return;
        }



        public double SegmentArcAngle
        {
            get => (double)GetValue(SegmentArcAngleProperty);
            set => SetValue(SegmentArcAngleProperty, value);
        }


        public static readonly DependencyProperty SegmentArcAngleProperty = DependencyProperty.Register(nameof(SegmentArcAngle), typeof(double), typeof(SegmentedRing), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender, OnSegmentArcAngleChanged), OnValidateStrokeDashAngle);



        private static void OnSegmentArcAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SegmentedRing self) return;
        }

        private static bool OnValidateStrokeDashAngle(object value) => (double)value >= 0 && (double)value <= 360;


        #endregion

        #region Ctors

        static SegmentedRing()
        {
            StretchProperty.OverrideMetadata(typeof(SegmentedRing), new FrameworkPropertyMetadata(Stretch.Fill));
        }


        public SegmentedRing()
        {
            _rect = Rect.Empty;
            //_ring = new EllipseGeometry(_rect);
            _ring = new EllipseGeometry();
            //{
            //    RadiusX = Radius,
            //    RadiusY = Radius,
            //    Center = new Point(Radius / 2, Radius / 2),
            //};
            _dashArray = new DoubleCollection() { 56, 19 };
            //_pen = new Pen
            //{
            //    Thickness = StrokeThickness,
            //    Brush = Stroke,
            //    StartLineCap = StrokeStartLineCap,
            //    EndLineCap = StrokeEndLineCap,
            //    DashCap = StrokeDashCap,
            //    LineJoin = StrokeLineJoin,
            //    MiterLimit = StrokeMiterLimit,
            //    DashStyle = new DashStyle(_dashArray, StrokeDashOffset)
            //};
            _pen = new Pen
            {
                Thickness = 6,
                Brush = Brushes.Black,
                DashCap = PenLineCap.Flat,
                DashStyle = new DashStyle(_dashArray, 61)
            };
        }

        #endregion

        #region Shape
        protected override Geometry DefiningGeometry => _rect.IsEmpty ? Geometry.Empty : _ring;

        public override Geometry RenderedGeometry => DefiningGeometry;

        public override Transform GeometryTransform => Transform.Identity;


        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!_rect.IsEmpty)
            {
                //drawingContext.DrawGeometry(Fill, _pen, new EllipseGeometry(_rect));
                drawingContext.DrawGeometry(Fill, _pen, _ring);
            }
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
                    _rect.Width = _rect.Height = 2 * Radius; //original ms code set to 0;
                    break;
                case Stretch.Fill:
                    //break; for ellipse this should be uncommented
                case Stretch.Uniform:
                    _rect.Width = _rect.Height = Math.Min(_rect.Width, _rect.Height);
                    break;
                case Stretch.UniformToFill:
                    _rect.Width = _rect.Height = Math.Max(_rect.Width, _rect.Height);
                    break;
            }
            //_ring = _rect.IsEmpty ? Geometry.Empty : new EllipseGeometry(_rect);
            _ring.Center = new Point(_rect.Width / 2, _rect.Height / 2);
            _ring.RadiusX = _rect.Width / 2;
            _ring.RadiusY = _rect.Height / 2;

            return finalSize;
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
                case nameof(SegmentCount):
                    UpdateDashArray();
                    break;
                case nameof(SegmentArcAngle):
                    UpdateDashArray();
                    break;
                case nameof(DashLength):
                    UpdateDashArray();
                    break;
                case nameof(GapLength):
                    UpdateDashArray();
                    break;
            }
        }

        #endregion

        #region Methods

        //private EllipseGeometry CreateRingGeometry()
        //{
        //    var ring = new EllipseGeometry()
        //    {
        //        RadiusX = Radius,
        //        RadiusY = Radius,
        //        Center = new Point(Radius/2, Radius/2),
        //    };

        //    //var ring = new Ellipse()
        //    //{
        //    //    Width = Radius * 2, 
        //    //    Height = Radius * 2,
        //    //    StrokeThickness = RingThickness,
        //    //    //StrokeDashArray = new DoubleCollection { 0, 0 },
        //    //};

        //    //ring.SetBinding(StrokeProperty, new Binding("Stroke") { Source = this });
        //    //ring.SetBinding(StrokeDashCapProperty, new Binding("StrokeDashCap") { Source = this });
        //    //ring.SetBinding(StrokeStartLineCapProperty, new Binding("StrokeStartLineCap") { Source = this });
        //    //ring.SetBinding(StrokeEndLineCapProperty, new Binding("StrokeEndLineCap") { Source = this });
        //    //ring.SetBinding(StrokeDashOffsetProperty, new Binding("StrokeDashOffset") { Source = this });

        //    return ring;
        //}


        private void UpdateDashArray()
        {
            //if (SegmentCount * SegmentArcAngle > 360)
            //    throw new InvalidOperationException("SegmentCount * SegmentArcAngle must be less than 360");

            double border = 2 * Math.PI * Radius;

            var dashArcAngle = SegmentArcAngle;
            var gapArcAngle = Math.Max(0, (360 - SegmentCount * SegmentArcAngle) / SegmentCount);

            // thickness scales the dash and gap lengths
            double thicknessFactor = 2 / StrokeThickness;

            double dashLength = dashArcAngle / 360 * border * thicknessFactor;
            double gapLength = gapArcAngle / 360 * border * thicknessFactor;

            //_dashArray[0] = dashLength;
            //_dashArray[1] = gapLength;

            _dashArray[0] = DashLength;
            _dashArray[1] = GapLength;

            //_pen.DashStyle = new DashStyle(_dashArray, StrokeDashOffset);
            _pen.DashStyle.Dashes = _dashArray;
            //Debug.WriteLine($"DashArray: {dashLength}, {gapLength}");
        }

        #endregion
    }
}
