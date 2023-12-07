using ColorCraft;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public partial class SpiralSphere : Spinner
    {
        #region Data

        private readonly SurfaceElementGroup _surface;

        #endregion

        #region Dependency Properties

        public int SurfacePointCount
        {
            get => (int)GetValue(SurfacePointCountProperty);
            set => SetValue(SurfacePointCountProperty, value);
        }

        public static readonly DependencyProperty SurfacePointCountProperty = DependencyProperty.Register(nameof(SurfacePointCount), typeof(int), typeof(SpiralSphere), new FrameworkPropertyMetadata(200, FrameworkPropertyMetadataOptions.AffectsRender, OnSurfacePointCountChanged));

        private static void OnSurfacePointCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnSurfacePointCountChanged(e);
        }

        protected virtual void OnSurfacePointCountChanged(DependencyPropertyChangedEventArgs e)
        {
            SetUpSurface();
        }

        public Size SurfacePointRelativeSize
        {
            get => (Size)GetValue(SurfacePointRelativeSizeProperty);
            set => SetValue(SurfacePointRelativeSizeProperty, value);
        }

        public static readonly DependencyProperty SurfacePointRelativeSizeProperty = DependencyProperty.Register(nameof(SurfacePointRelativeSize), typeof(Size), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Size(0.02, 0.02), FrameworkPropertyMetadataOptions.AffectsRender, OnPointSizeChanged));

        private static void OnPointSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnPointSizeChanged(e);
        }
        protected virtual void OnPointSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            SetUpSurface();
        }

        public Pattern SpiralPattern
        {
            get => (Pattern)GetValue(SpiralPatternProperty);
            set => SetValue(SpiralPatternProperty, value);
        }

        public static readonly DependencyProperty SpiralPatternProperty = DependencyProperty.Register(nameof(SpiralPattern), typeof(Pattern), typeof(SpiralSphere), new FrameworkPropertyMetadata(Pattern.LinearSpiral, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange, OnSpiralPatternChanged));

        private static void OnSpiralPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnSpiralPatternChanged(e);
        }

        protected virtual void OnSpiralPatternChanged(DependencyPropertyChangedEventArgs e)
        {
            SetUpSurface();
        }

        public double AzimuthalToInclineRatio
        {
            get => (double)GetValue(AzimuthalToInclineRatioProperty);
            set => SetValue(AzimuthalToInclineRatioProperty, value);
        }

        public static readonly DependencyProperty AzimuthalToInclineRatioProperty = DependencyProperty.Register(nameof(AzimuthalToInclineRatio), typeof(double), typeof(SpiralSphere), new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender, OnAzimuthalToInclineRatioChanged));

        private static void OnAzimuthalToInclineRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnAzimuthalToInclineRatioChanged(e);
        }

        protected virtual void OnAzimuthalToInclineRatioChanged(DependencyPropertyChangedEventArgs e)
        {
            SetUpSurface();
        }

        public Vector3 CameraDirection
        {
            get => (Vector3)GetValue(CameraDirectionProperty);
            set => SetValue(CameraDirectionProperty, value);
        }

        public static readonly DependencyProperty CameraDirectionProperty = DependencyProperty.Register(nameof(CameraDirection), typeof(Vector3), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Vector3(0, 1, 0), FrameworkPropertyMetadataOptions.AffectsRender, OnCameraDirectionChanged));

        private static void OnCameraDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnCameraDirectionChanged(e);
        }
        protected virtual void OnCameraDirectionChanged(DependencyPropertyChangedEventArgs e)
        {
            SetUpCameraPosition();
        }

        public double DepthExaggeration
        {
            get => (double)GetValue(DepthExaggerationProperty);
            set => SetValue(DepthExaggerationProperty, value);
        }

        public static readonly DependencyProperty DepthExaggerationProperty = DependencyProperty.Register(nameof(DepthExaggeration), typeof(double), typeof(SpiralSphere), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnDepthExaggerationChanged));

        private static void OnDepthExaggerationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnDepthExaggerationChanged(e);
        }

        protected virtual void OnDepthExaggerationChanged(DependencyPropertyChangedEventArgs e)
        {
            RootCanvas.DepthExaggeration = DepthExaggeration;
        }

        public Vector3 UpDirection
        {
            get => (Vector3)GetValue(UpDirectionProperty);
            set => SetValue(UpDirectionProperty, value);
        }

        public static readonly DependencyProperty UpDirectionProperty = DependencyProperty.Register(nameof(UpDirection), typeof(Vector3), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Vector3(0, 0, 1), FrameworkPropertyMetadataOptions.AffectsRender, OnUpDirectionChanged));

        private static void OnUpDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnUpDirectionChanged(e);
        }

        protected virtual void OnUpDirectionChanged(DependencyPropertyChangedEventArgs e)
        {
            RootCanvas.Camera.UpDirection = UpDirection;
        }

        public float FieldOfView
        {
            get => (float)GetValue(FieldOfViewProperty);
            set => SetValue(FieldOfViewProperty, value);
        }

        public static readonly DependencyProperty FieldOfViewProperty = DependencyProperty.Register(nameof(FieldOfView), typeof(float), typeof(SpiralSphere), new FrameworkPropertyMetadata(40.0f, FrameworkPropertyMetadataOptions.AffectsRender, OnFieldOfViewChanged));

        private static void OnFieldOfViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnFieldOfViewChanged(e);
        }

        protected virtual void OnFieldOfViewChanged(DependencyPropertyChangedEventArgs e)
        {
            RootCanvas.Camera.FieldOfView = FieldOfView;
            SetUpCameraPosition();
        }

        public Vector3 AxisOfRation
        {
            get => (Vector3)GetValue(AxisOfRationProperty);
            set => SetValue(AxisOfRationProperty, value);
        }

        public static readonly DependencyProperty AxisOfRationProperty = DependencyProperty.Register(nameof(AxisOfRation), typeof(Vector3), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Vector3(0,0,1), FrameworkPropertyMetadataOptions.AffectsRender, OnAxisOfRationChanged));

        private static void OnAxisOfRationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnAxisOfRationChanged(e);
        }

        protected void OnAxisOfRationChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not Vector3 axis) return;
            if (RootCanvas.SurfaceGroup.Transform is not RotateTransform transform) 
                throw new InvalidOperationException("The SurfaceGroup's transform is not a RotateTransform.");
            transform.Rotation.Axis = axis;
        }

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(SpiralSphere), new PropertyMetadata(Stretch.Uniform, OnStretchChanged));

        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnStretchChanged(e);
        }

        protected void OnStretchChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not Stretch newStretch) return;
            RootCanvas.Stretch = newStretch;
        }

        #endregion

        #region Constructors

        public SpiralSphere()
        {
            Loaded += OnLoaded;
            InitializeComponent();

            _surface = new SurfaceElementGroup()
            {
                Transform = new RotateTransform(AxisOfRation, 0, new Vector3(0, 0, 0))
            };

            RootCanvas.SurfaceGroup = _surface;
            RootCanvas.Camera = new Camera()
            {
                TargetPosition = new Vector3(0, 0, 0),
                UpDirection = UpDirection,
                FieldOfView = FieldOfView,
            };
        }

        #endregion

        #region Methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetUpCameraPosition();
            SetUpSurface();
            SetUpAnimation();
        }

        private void SetUpCameraPosition()
        {
            var size = Math.Max(ActualWidth, ActualHeight);
            var radius = size / 2;

            var fovRad = FieldOfView * Math.PI / 180;

            var cameraDistance = (float)(radius / Math.Tan(fovRad / 2) / Math.Cos(fovRad / 2));
            var cameraPosition = Vector3.Multiply(-cameraDistance, Vector3.Normalize(CameraDirection));

            RootCanvas.Camera.CameraPosition = cameraPosition; 
        }

        private void SetUpSurface()
        {
            var size = Math.Max(ActualWidth, ActualHeight);
            var radius = size / 2;

            var surfacePoints = CreateSurfacePoints(SurfacePointCount, radius, AzimuthalToInclineRatio, SpiralPattern);

            var surfacePointSize = new Size(SurfacePointRelativeSize.Width * radius, SurfacePointRelativeSize.Height * radius);

            UpdateSurfaceGroup(surfacePoints, surfacePointSize);
        }

        private void SetUpAnimation()
        {

            var angleAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(15),
                RepeatBehavior = RepeatBehavior.Forever
            };


            /*
                Method 0: Directly animating the property
                - Works: Easy to understand, but not in line with how Spinners are designed to work.
            */
            //var animatableRotation = ((RotateTransform)RootCanvas.SurfaceGroup.Transform).Rotation;
            //animatableRotation.BeginAnimation(AxisAngleRotation.AngleProperty, angleAnimation);

            /*
                Method 1: Direct Targeting
                - Does not work: 'animatableRotation' is not part of the visual tree, so the Storyboard can't find it.
            */
            // var animatableRotation = ((RotateTransform)RootCanvas.SurfaceGroup.Transform).Rotation;
            //Storyboard.SetTarget(angleAnimation, animatableRotation);
            //Storyboard.SetTargetProperty(angleAnimation, new PropertyPath(AxisAngleRotation.AngleProperty));
            //ActiveStoryboard.Begin();

            /*
                Method 2: Named Targeting
                - Works: 'animatableRotation' is registered in the namescope (this), making it findable by the Storyboard.
            */
            // var animatableRotation = ((RotateTransform)RootCanvas.SurfaceGroup.Transform).Rotation;
            //var name = "AnimatableRotation";
            //RegisterName(name, animatableRotation);
            //Storyboard.SetTargetName(angleAnimation, name);
            //Storyboard.SetTargetProperty(angleAnimation, new PropertyPath(AxisAngleRotation.AngleProperty));
            //ActiveStoryboard.Begin(this);

            /*
                Method 3: Targeting via Property Path on a Visual Element
                - Works: 'rootCanvas' is part of the visual tree and can be targeted by using a more complex property path.
            */
            //var path = $"{nameof(SurfaceCanvas.SurfaceGroup)}.{nameof(SurfaceElement.Transform)}.{nameof(RotateTransform.Rotation)}.{nameof(AxisAngleRotation.Angle)}";
            //ActiveStoryboard.AddAnimation(angleAnimation, RootCanvas, path, "foo", this);

            ActiveStoryboard.Children.Add(angleAnimation);
            Storyboard.SetTarget(angleAnimation, RootCanvas);
            Storyboard.SetTargetProperty(angleAnimation, new PropertyPath(
                $"{nameof(SurfaceCanvas.SurfaceGroup)}.{nameof(SurfaceElement.Transform)}.{nameof(RotateTransform.Rotation)}.{nameof(AxisAngleRotation.Angle)}"
            ));

            UpdateActiveStoryboard();
        }

        private static List<Vector3> CreateSurfacePoints(int pointCount, double sphereRadius, double azimuthToInclineRatio, Pattern pattern = Pattern.EqualArea)
        {
            var spherePoints = new List<Vector3>(pointCount);
            pointCount += 1; // exclude the last point to avoid a gap in the surface

            for (int i = 1; i < pointCount; i++)
            {
                // Inclination needs to be in the range [0, π] but how to distribute the points? If you do a linear distribution, you get a lot of points near the poles and not many near the equator, same as working with latitude and longitude (polar crowding)
                (var azimuthalAngle, var inclinationAngle) = pattern switch
                {
                    Pattern.LinearSpiral => GetLinearSpiralPoint(i, pointCount),
                    Pattern.EqualArea => GetEqualAreaSpiralPoint(i, pointCount),
                    Pattern.GoldenSpiral => GetGoldenSpiralPoint(i, pointCount),
                    _ => throw new NotImplementedException(),
                };

                spherePoints.Add(CalculateSpherePoint(sphereRadius, azimuthalAngle, inclinationAngle));
            }

            return spherePoints;

            // This method creates a spiral pattern by fixing `φ = n * θ`. The azimuthToInclineRatio,
            // parameter controls the tightness and frequency of the spirals on the sphere's surface.
            (double azimuthal, double incline) GetLinearSpiralPoint(double i, double n)
            {
                var inclinationAngle = (i / n) * Math.PI;
                var azimuthalAngle = inclinationAngle * azimuthToInclineRatio;
                return (azimuthalAngle, inclinationAngle);
            }

            (double azimuthal, double incline) GetEqualAreaSpiralPoint(double i, double n)
            {
                var inclinationAngle = Math.Acos(1 - 2 * i / n);
                var azimuthalAngle = inclinationAngle * azimuthToInclineRatio;
                return (azimuthalAngle, inclinationAngle);
            }

            (double azimuthal, double incline) GetGoldenSpiralPoint(double i, double n)
            {
                var phi = (1 + Math.Sqrt(5)) / 2; // Golden Ratio
                var inclinationAngle = Math.Acos(1 - 2 * i / n); // Equal area inclination
                var azimuthalAngle = 2 * Math.PI * i / (phi * n); // Golden Spiral azimuthal
                azimuthalAngle *= azimuthToInclineRatio;
                return (azimuthalAngle, inclinationAngle);
            }

            // Textbook math
            Vector3 CalculateSpherePoint(double radius, double azimuthalAngle, double inclinationAngle)
            {
                var x = radius * Math.Sin(inclinationAngle) * Math.Cos(azimuthalAngle);
                var y = radius * Math.Sin(inclinationAngle) * Math.Sin(azimuthalAngle);
                var z = radius * Math.Cos(inclinationAngle);

                return new Vector3((float)x, (float)y, (float)z);
            }
        }

        private void UpdateSurfaceGroup(List<Vector3> sufacePoints, Size surfacePointSize)
        {
            if (_surface == null) return;

            var altColor = Foreground is SolidColorBrush brush ? brush.Color : Colors.Red;
            if (Palette.Count < 1 || Palette[0] is not SolidColorBrush brush1) { brush1 = new SolidColorBrush(altColor); }
            if (Palette.Count < 2 || Palette[1] is not SolidColorBrush brush2) { brush2 = brush1; }

            var fromHsl = Hsl.FromColor(brush1.Color);
            var toHsl = Hsl.FromColor(brush2.Color);

            _surface.Children.Clear();

            for (int i = 0; i < sufacePoints.Count; i++)
            {
                var point = sufacePoints[i];

                var color = Hsl.Lerp(fromHsl, toHsl, (i / (double)sufacePoints.Count), false).ToColor();

                var progressBrush = new SolidColorBrush(color);

                var dot = new SurfaceElement()
                {
                    Fill = progressBrush,
                    Size = surfacePointSize,
                    Position = point
                };

                _surface.Children.Add(dot);
            }
        }

        #endregion

        #region Method Overrides

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            SetUpCameraPosition();
            SetUpSurface();
        }

        protected override void OnPaletteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SetUpSurface();
        }

        #endregion

    }
}
