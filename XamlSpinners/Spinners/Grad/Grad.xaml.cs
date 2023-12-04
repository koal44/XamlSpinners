using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace XamlSpinners
{
    public partial class Grad : Spinner
    {
        private readonly LinearGradientBrush _linearGradientBrush;
        private readonly RadialGradientBrush _radialGradientBrush;

        readonly GradientStop _blueGradientStop;
        readonly GradientStop _redGradientStop;


        public double BlueOffset
        {
            get => (double)GetValue(BlueOffsetProperty);
            set => SetValue(BlueOffsetProperty, value);
        }

        public static readonly DependencyProperty BlueOffsetProperty = DependencyProperty.Register(nameof(BlueOffset), typeof(double), typeof(Grad), new FrameworkPropertyMetadata(0.0, OnBlueOffsetChanged));

        private static void OnBlueOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnBlueOffsetChanged(e);
        }

        protected virtual void OnBlueOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            _blueGradientStop.Offset = BlueOffset;
        }


        public double RedOffset
        {
            get => (double)GetValue(RedOffsetProperty);
            set => SetValue(RedOffsetProperty, value);
        }

        public static readonly DependencyProperty RedOffsetProperty = DependencyProperty.Register(nameof(RedOffset), typeof(double), typeof(Grad), new FrameworkPropertyMetadata(1.0, OnRedOffsetChanged));

        private static void OnRedOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnRedOffsetChanged(e);
        }

        protected virtual void OnRedOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            _redGradientStop.Offset = RedOffset;
        }


        public GradientSpreadMethod SpreadMethod
        {
            get => (GradientSpreadMethod)GetValue(SpreadMethodProperty);
            set => SetValue(SpreadMethodProperty, value);
        }

        public static readonly DependencyProperty SpreadMethodProperty = DependencyProperty.Register(nameof(SpreadMethod), typeof(GradientSpreadMethod), typeof(Grad), new FrameworkPropertyMetadata(default(GradientSpreadMethod), OnSpreadMethodChanged));

        private static void OnSpreadMethodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnSpreadMethodChanged(e);
        }

        protected virtual void OnSpreadMethodChanged(DependencyPropertyChangedEventArgs e)
        {
            _radialGradientBrush.SpreadMethod = SpreadMethod;
        }


        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(Grad), new FrameworkPropertyMetadata(new Point(0.5, 0.5), OnCenterChanged));

        private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnCenterChanged(e);
        }

        protected virtual void OnCenterChanged(DependencyPropertyChangedEventArgs e)
        {
            _radialGradientBrush.Center = Center;
        }


        public Point GradientOrigin
        {
            get => (Point)GetValue(GradientOriginProperty);
            set => SetValue(GradientOriginProperty, value);
        }

        public static readonly DependencyProperty GradientOriginProperty = DependencyProperty.Register(nameof(GradientOrigin), typeof(Point), typeof(Grad), new FrameworkPropertyMetadata(new Point(0.5, 0.5), OnGradientOriginChanged));

        private static void OnGradientOriginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnGradientOriginChanged(e);
        }

        protected virtual void OnGradientOriginChanged(DependencyPropertyChangedEventArgs e)
        {
            _radialGradientBrush.GradientOrigin = GradientOrigin;
        }


        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(nameof(RadiusX), typeof(double), typeof(Grad), new FrameworkPropertyMetadata(0.5, OnRadiusXChanged));

        private static void OnRadiusXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnRadiusXChanged(e);
        }

        protected virtual void OnRadiusXChanged(DependencyPropertyChangedEventArgs e)
        {
            _radialGradientBrush.RadiusX = RadiusX;
        }


        public double RadiusY
        {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(nameof(RadiusY), typeof(double), typeof(Grad), new FrameworkPropertyMetadata(0.5, OnRadiusYChanged));

        private static void OnRadiusYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnRadiusYChanged(e);
        }

        protected virtual void OnRadiusYChanged(DependencyPropertyChangedEventArgs e)
        {
            _radialGradientBrush.RadiusY = RadiusY;
        }


        public Point StartPoint
        {
            get => (Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(nameof(StartPoint), typeof(Point), typeof(Grad), new FrameworkPropertyMetadata(new Point(0,0), OnStartPointChanged));

        private static void OnStartPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnStartPointChanged(e);
        }

        protected virtual void OnStartPointChanged(DependencyPropertyChangedEventArgs e)
        {
            _linearGradientBrush.StartPoint = StartPoint;
        }


        public Point EndPoint
        {
            get => (Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(nameof(EndPoint), typeof(Point), typeof(Grad), new FrameworkPropertyMetadata(new Point(1,1), OnEndPointChanged));

        private static void OnEndPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnEndPointChanged(e);
        }

        protected virtual void OnEndPointChanged(DependencyPropertyChangedEventArgs e)
        {
            _linearGradientBrush.EndPoint = EndPoint;
        }


        public GradientType Gradient
        {
            get => (GradientType)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }

        public static readonly DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(GradientType), typeof(Grad), new FrameworkPropertyMetadata(default(GradientType), OnGradientChanged));

        private static void OnGradientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grad self) return;
            self.OnGradientChanged(e);
        }

        protected virtual void OnGradientChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradientType(Gradient);
        }

        private void UpdateGradientType(GradientType gradient)
        {
            OuterEllipse.Stroke = gradient switch
            {
                GradientType.Radial => _radialGradientBrush,
                GradientType.Linear => _linearGradientBrush,
                _ => throw new ArgumentOutOfRangeException(nameof(gradient), gradient, null)
            };

            InnerEllipse.Stroke = gradient switch
            {
                GradientType.Radial => _radialGradientBrush,
                GradientType.Linear => _linearGradientBrush,
                _ => throw new ArgumentOutOfRangeException(nameof(gradient), gradient, null)
            };

            CenterEllipse.Fill = gradient switch
            {
                GradientType.Radial => _radialGradientBrush,
                GradientType.Linear => _linearGradientBrush,
                _ => throw new ArgumentOutOfRangeException(nameof(gradient), gradient, null)
            };
        }

        public Grad()
        {
            _blueGradientStop = new GradientStop(Colors.Blue, BlueOffset);
            _redGradientStop = new GradientStop(Colors.Red, RedOffset);

            _radialGradientBrush = new RadialGradientBrush()
            {
                SpreadMethod = SpreadMethod,
                Center = Center,
                GradientOrigin = new Point(0.5, 0.5),
                RadiusX = RadiusX,
                RadiusY = RadiusY,
                GradientStops = { _blueGradientStop, _redGradientStop}
            };

            _linearGradientBrush = new LinearGradientBrush()
            {
                StartPoint = StartPoint,
                EndPoint = EndPoint,
                GradientStops = { _blueGradientStop, _redGradientStop}
            };
            DataContext = this;
            InitializeComponent();
            UpdateGradientType(Gradient);
        }

        protected override void OnPaletteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
        }
    }

    public enum GradientType
    {
        Radial,
        Linear,
    }
}
