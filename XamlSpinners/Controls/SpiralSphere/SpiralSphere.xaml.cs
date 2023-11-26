using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using XamlSpinners.Utils;

namespace XamlSpinners
{
    public partial class SpiralSphere : Spinner
    {
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

        protected void OnSurfacePointCountChanged(DependencyPropertyChangedEventArgs e) { }


        public Size SurfacePointSize
        {
            get => (Size)GetValue(SurfacePointSizeProperty);
            set => SetValue(SurfacePointSizeProperty, value);
        }

        public static readonly DependencyProperty SurfacePointSizeProperty = DependencyProperty.Register(nameof(SurfacePointSize), typeof(Size), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Size(2, 2), FrameworkPropertyMetadataOptions.AffectsRender, OnPointSizeChanged));

        private static void OnPointSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnPointSizeChanged(e);
        }

        protected void OnPointSizeChanged(DependencyPropertyChangedEventArgs e) { }


        public Pattern SpiralPattern
        {
            get => (Pattern)GetValue(SpiralPatternProperty);
            set => SetValue(SpiralPatternProperty, value);
        }

        public static readonly DependencyProperty SpiralPatternProperty = DependencyProperty.Register(nameof(SpiralPattern), typeof(Pattern), typeof(SpiralSphere), new FrameworkPropertyMetadata(Pattern.LinearSpiral, FrameworkPropertyMetadataOptions.AffectsRender, OnSpiralPatternChanged));

        private static void OnSpiralPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnSpiralPatternChanged(e);
        }

        protected virtual void OnSpiralPatternChanged(DependencyPropertyChangedEventArgs e) { }


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

        protected void OnAzimuthalToInclineRatioChanged(DependencyPropertyChangedEventArgs e) { }


        public Vector3 CameraDirection
        {
            get => (Vector3)GetValue(CameraDirectionProperty);
            set => SetValue(CameraDirectionProperty, value);
        }

        public static readonly DependencyProperty CameraDirectionProperty = DependencyProperty.Register(nameof(CameraDirection), typeof(Vector3), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Vector3(0,1,0), FrameworkPropertyMetadataOptions.AffectsRender, OnCameraDirectionChanged));

        private static void OnCameraDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnCameraDirectionChanged(e);
        }

        protected void OnCameraDirectionChanged(DependencyPropertyChangedEventArgs e) { }


        public double CameraDistanceMultiplier
        {
            get => (double)GetValue(CameraDistanceMultiplierProperty);
            set => SetValue(CameraDistanceMultiplierProperty, value);
        }

        public static readonly DependencyProperty CameraDistanceMultiplierProperty = DependencyProperty.Register(nameof(CameraDistanceMultiplier), typeof(double), typeof(SpiralSphere), new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.AffectsRender, OnCameraDistanceMultiplierChanged));

        private static void OnCameraDistanceMultiplierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnCameraDistanceMultiplierChanged(e);
        }

        protected void OnCameraDistanceMultiplierChanged(DependencyPropertyChangedEventArgs e) { }


        public Vector3 UpDirection
        {
            get => (Vector3)GetValue(UpDirectionProperty);
            set => SetValue(UpDirectionProperty, value);
        }

        public static readonly DependencyProperty UpDirectionProperty = DependencyProperty.Register(nameof(UpDirection), typeof(Vector3), typeof(SpiralSphere), new FrameworkPropertyMetadata(new Vector3(0,0,1), FrameworkPropertyMetadataOptions.AffectsRender, OnUpDirectionChanged));

        private static void OnUpDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnUpDirectionChanged(e);
        }

        protected void OnUpDirectionChanged(DependencyPropertyChangedEventArgs e) { }


        public double FieldOfView
        {
            get => (double)GetValue(FieldOfViewProperty);
            set => SetValue(FieldOfViewProperty, value);
        }

        public static readonly DependencyProperty FieldOfViewProperty = DependencyProperty.Register(nameof(FieldOfView), typeof(double), typeof(SpiralSphere), new FrameworkPropertyMetadata(45.0, FrameworkPropertyMetadataOptions.AffectsRender, OnFieldOfViewChanged));

        private static void OnFieldOfViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere self) return;
            self.OnFieldOfViewChanged(e);
        }

        protected void OnFieldOfViewChanged(DependencyPropertyChangedEventArgs e) { }


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
            if (rootCanvas.SurfaceGroup.Transform is not RotateTransform transform) 
                throw new InvalidOperationException("The SurfaceGroup's transform is not a RotateTransform.");
            transform.Rotation.Axis = axis;
        }

        #endregion

        #region Constructors

        public SpiralSphere()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        #endregion

        #region Methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetUpSurface();
            SetUpCamera();
            SetUpAnimation();
        }

        private void SetUpSurface()
        {
            var surfacePoints = CreateSurfacePoints(SurfacePointCount, 100, AzimuthalToInclineRatio);
            var surfaceGroup = CreateSurfaceGroup(surfacePoints, SurfacePointSize, Palette);
            surfaceGroup.Transform = new RotateTransform(0, AxisOfRation);
            rootCanvas.SurfaceGroup = surfaceGroup;
        }

        private void SetUpCamera()
        {
            rootCanvas.Camera = new Camera()
            {
                CameraPosition = new Vector3(0, 600, 0),
                TargetPosition = new Vector3(0, -1, 0),
                UpDirection = new Vector3(0, 0, 1),
                FieldOfView = 45,
            };
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

            ActiveStoryboard.Children.Add(angleAnimation);

            /*
                Method 0: Directly animating the property
                - Works: Easy to understand, but not in line with how Spinners are designed to work.
            */
            // var animatableRotation = ((RotateTransform)rootCanvas.SurfaceGroup.Transform).Rotation;
            // animatableRotation.BeginAnimation(AxisAngleRotation.AngleProperty, angleAnimation);

            /*
                Method 1: Direct Targeting
                - Does not work: 'animatableRotation' is not part of the visual tree, so the Storyboard can't find it.
            */
            // var animatableRotation = ((RotateTransform)rootCanvas.SurfaceGroup.Transform).Rotation;
            //Storyboard.SetTarget(angleAnimation, animatableRotation);
            //Storyboard.SetTargetProperty(angleAnimation, new PropertyPath(AxisAngleRotation.AngleProperty));
            //ActiveStoryboard.Begin();

            /*
                Method 2: Named Targeting
                - Works: 'animatableRotation' is registered in the namescope (this), making it findable by the Storyboard.
            */
            // var animatableRotation = ((RotateTransform)rootCanvas.SurfaceGroup.Transform).Rotation;
            //var name = "AnimatableRotation";
            //RegisterName(name, animatableRotation);
            //Storyboard.SetTargetName(angleAnimation, name);
            //Storyboard.SetTargetProperty(angleAnimation, new PropertyPath(AxisAngleRotation.AngleProperty));
            //ActiveStoryboard.Begin(this);

            /*
                Method 3: Targeting via Property Path on a Visual Element
                - Works: 'rootCanvas' is part of the visual tree and can be targeted by using a more complex property path.
            */
            Storyboard.SetTarget(angleAnimation, rootCanvas);
            Storyboard.SetTargetProperty(angleAnimation, new PropertyPath(
                $"{nameof(SurfaceCanvas.SurfaceGroup)}.{nameof(SurfaceElement.Transform)}.{nameof(RotateTransform.Rotation)}.{nameof(AxisAngleRotation.Angle)}"
            ));

            UpdateActiveStoryboard();
        }

        private static List<Vector3> CreateSurfacePoints(int pointCount, double sphereRadius, double azimuthToInclineRatio, Pattern pattern = Pattern.EqualArea)
        {
            var spherePoints = new List<Vector3>(pointCount);

            for (int i = 1; i <= pointCount; i++)
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

        private static SurfaceElementGroup CreateSurfaceGroup(List<Vector3> spherePoints, Size dotSize, IList<Brush> palette)
        {
            var group = new SurfaceElementGroup();

            if (palette.Count < 1 || palette[0] is not SolidColorBrush brush1) { brush1 = new SolidColorBrush(Colors.Red); }
            if (palette.Count < 2 || palette[1] is not SolidColorBrush brush2) { brush2 = brush1; }

            var (fromHue, fromSaturation, fromLightness) = ColorUtils.RgbToHsl(brush1.Color);
            var (toHue, toSaturation, toLightness) = ColorUtils.RgbToHsl(brush2.Color);

            for (int i = 0; i < spherePoints.Count; i++)
            {
                var point = spherePoints[i];

                var color = ColorUtils.RgbFromHslProgress(fromHue, toHue, fromSaturation, toSaturation, fromLightness, toLightness, (i / (double)spherePoints.Count));
                var brush = new SolidColorBrush(color);

                var dot = new SurfaceElement()
                {
                    Fill = brush,
                    Size = dotSize,
                    Position = point
                };

                group.Children.Add(dot);
            }

            return group;
        }

        #endregion

    }
}
