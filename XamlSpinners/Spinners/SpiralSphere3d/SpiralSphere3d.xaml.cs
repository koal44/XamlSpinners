using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using XamlSpinners.Utils;

/*
 * * 
 * * Adpated from the CSS sphere by Mamboleoo.
 * * https://codepen.io/Mamboleoo/post/sphere-css
 * * 
 * */

namespace XamlSpinners
{
    public partial class SpiralSphere3d : Spinner
    {
        private Size3D _blockSize = new(3.5, 3.5, 3.5);
        private const double _sphereRadius = 200;
        //private Vector3D _sphereCenter = new(0, 0, 0);
        private Vector3D _axisOfRatation = new(0, 0, 1);
        private const double _nearPlaneDistance = 1;
        private const double _farPlaneDistance = double.PositiveInfinity;
        private const double _fieldOfView = 45;
        //private Point3D _cameraPosition = new(0, 700, 0);
        private Vector3D _cameraLookDirection = new(0, -1, 0);
        private Vector3D _cameraUpDirection = new(0, 0, 1);

        private readonly Model3DGroup _sphereBlocksGroup;
        private readonly ModelVisual3D _lightVisual;
        private readonly ModelVisual3D _surfaceVisual;
        private readonly PerspectiveCamera _camera;
        private readonly RotateTransform3D _rotateTransform;


        public int BlockCount
        {
            get => (int)GetValue(BlockCountProperty);
            set => SetValue(BlockCountProperty, value);
        }

        public static readonly DependencyProperty BlockCountProperty = DependencyProperty.Register(nameof(BlockCount), typeof(int), typeof(SpiralSphere3d), new FrameworkPropertyMetadata(200, OnBlockCountChanged));

        private static void OnBlockCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere3d self) return;
            self.OnBlockCountChanged(e);
        }

        protected virtual void OnBlockCountChanged(DependencyPropertyChangedEventArgs e)
        {
            SetupSurfaceGroup();
        }

        public double AzimuthalToInclineRatio
        {
            get => (double)GetValue(AzimuthalToInclineRatioProperty);
            set => SetValue(AzimuthalToInclineRatioProperty, value);
        }

        public static readonly DependencyProperty AzimuthalToInclineRatioProperty = DependencyProperty.Register(nameof(AzimuthalToInclineRatio), typeof(double), typeof(SpiralSphere3d), new PropertyMetadata(25.0, OnAzimuthalToInclineRatioChanged));

        private static void OnAzimuthalToInclineRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SpiralSphere3d self) return;
            self.SetupSurfaceGroup();
        }

        public SpiralSphere3d()
        {
            Palette = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(ColorUtils.HslToRgb(180, 1, 0.4)),
                new SolidColorBrush(ColorUtils.HslToRgb(270, 1, 0.8)),
            };

            DataContext = this;
            InitializeComponent();

            // surface
            _sphereBlocksGroup = new Model3DGroup();
            _surfaceVisual = new() { Content = _sphereBlocksGroup };
            RootViewport.Children.Add(_surfaceVisual);
            SetupSurfaceGroup();

            // light
            var light = new AmbientLight(Colors.White); 
            //var light = new DirectionalLight(Colors.White, new Vector3D(0, -1, 0));
            _lightVisual = new() { Content = light };
            RootViewport.Children.Add(_lightVisual);

            // camera
            var fovRad = _fieldOfView * Math.PI / 180;
            var cameraDistance = (float)(_sphereRadius / Math.Tan(fovRad / 2) / Math.Cos(fovRad / 2));
            var cameraPosition = new Point3D(0, cameraDistance, 0);
            _camera = new PerspectiveCamera()
            {
                Position = cameraPosition, // _sphereCenter + _cameraPosition,
                LookDirection = _cameraLookDirection,
                UpDirection = _cameraUpDirection,
                FieldOfView = _fieldOfView,
                NearPlaneDistance = _nearPlaneDistance,
                FarPlaneDistance = _farPlaneDistance,
            };
            RootViewport.Camera = _camera;

            // animation
            _rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(_axisOfRatation, 0));
            _sphereBlocksGroup.Transform = _rotateTransform;

            var angleAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(15),
                RepeatBehavior = RepeatBehavior.Forever
            };

            ActiveStoryboard.AddAnimation(angleAnimation, _rotateTransform.Rotation, AxisAngleRotation3D.AngleProperty, "AnimatableRotation", this);

            UpdateActiveStoryboard();
        }

        private void SetupSurfaceGroup()
        {
            if (_sphereBlocksGroup == null) return;

            var spherePoints = GenerateSpherePoints(BlockCount, _sphereRadius, AzimuthalToInclineRatio);
            UpdateSphereGroup(spherePoints);
        }

        private static List<Point3D> GenerateSpherePoints(int sphereCount, double SphereRadius, double azimuthalToInclineRatio)
        {
            var spherePoints = new List<Point3D>(sphereCount);

            for (int i = 1; i <= sphereCount; i++)
            {
                double azimuthalAngle = (i / (double)sphereCount) * Math.PI * 2 * azimuthalToInclineRatio;
                double inclineAngle = (i / (double)sphereCount) * Math.PI;
                double x = SphereRadius * Math.Sin(inclineAngle) * Math.Cos(azimuthalAngle);
                double y = SphereRadius * Math.Sin(inclineAngle) * Math.Sin(azimuthalAngle);
                double z = SphereRadius * Math.Cos(inclineAngle);

                spherePoints.Add(new Point3D(x, y, z));
            }

            return spherePoints;
        }

        private void UpdateSphereGroup(List<Point3D> blockPositions)
        {
            var altColor = Colors.Red;
            if (Palette.Count < 1 || Palette[0] is not SolidColorBrush b1) { b1 = new SolidColorBrush(altColor); }
            if (Palette.Count < 2 || Palette[1] is not SolidColorBrush b2) { b2 = b1; }
            var (fromHue, fromSat, fromLight) = ColorUtils.RgbToHsl(b1.Color);
            var (toHue, toSat, toLight) = ColorUtils.RgbToHsl(b2.Color);

            _sphereBlocksGroup.Children.Clear();

            for (int i = 0; i < blockPositions.Count; i++)
            {
                var interpolatedColor = ColorUtils.InterpolateHsl(fromHue, toHue, fromSat, toSat, fromLight, toLight, (i / (double)blockPositions.Count));

                //var blockMaterial = new DiffuseMaterial(new SolidColorBrush(interpolatedColor));
                var blockMaterial = new EmissiveMaterial(new SolidColorBrush(interpolatedColor));

                var blockModel = new GeometryModel3D()
                {
                    Material = blockMaterial,
                    Geometry = ThreeDUtils.CreateBlockMesh(blockPositions[i], _blockSize)
                };
                _sphereBlocksGroup.Children.Add(blockModel);
            }
        }

        protected override void OnPaletteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SetupSurfaceGroup();
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var size = Math.Min(arrangeSize.Width, arrangeSize.Height);
            var offsetX = (arrangeSize.Width - size) / 2;
            var offsetY = (arrangeSize.Height - size) / 2;

            var finalRect = new Rect(offsetX, offsetY, size, size);
            RootViewport.Arrange(finalRect);

            return arrangeSize;
        }
    }
}
