using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace XamlSpinners
{
    public partial class MommySphere2 : UserControl
    {
        private const int SphereCount = 1000;
        private const double SphereRadius = 200;
        private Vector3D SphereCenter = new(0, 0, 0);
        private Size3D DotSize = new(2, 2, 2);
        private readonly Brush DotBrush = Brushes.Red;


        public double AzimuthalRotations
        {
            get => (double)GetValue(AzimuthalRotationsProperty);
            set => SetValue(AzimuthalRotationsProperty, value);
        }

        public static readonly DependencyProperty AzimuthalRotationsProperty = DependencyProperty.Register(nameof(AzimuthalRotations), typeof(double), typeof(MommySphere2), new PropertyMetadata(default(double), OnAzimuthalRotationsChanged));

        private static void OnAzimuthalRotationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MommySphere2 self) return;
            self.SetupSphereAndRunAnimations();
        }



        public MommySphere2()
        {
            DataContext = this;
            InitializeComponent();
            SetupSphereAndRunAnimations();
        }

       
        private void SetupSphereAndRunAnimations()
        {
            viewport.Children.Clear();

            var spherePoints = GenerateSpherePoints(SphereCount, SphereRadius, AzimuthalRotations);
            var model3DGroup = GenerateSphereGroup(spherePoints, DotSize, DotBrush);
            viewport.Children.Add(new ModelVisual3D { Content = model3DGroup });
            var light = new AmbientLight(Colors.White);
            //var light2 = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));
            viewport.Children.Add(new ModelVisual3D { Content = light });
            //viewport.Children.Add(new ModelVisual3D { Content = light2 });

            var camera = new PerspectiveCamera()
            {
                Position = SphereCenter + new Point3D(0,600,0),
                LookDirection = new Vector3D(0, -1, 0),
                UpDirection = new Vector3D(0, 0, 1),
                FieldOfView = 45,
                //FieldOfView = 45,
                //NearPlaneDistance = 1,
                //FarPlaneDistance = 1000,
            };
            viewport.Camera = camera;

            var rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0));
            model3DGroup.Transform = rotateTransform;

            // Create and apply the DoubleAnimation
            var rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(10),
                RepeatBehavior = RepeatBehavior.Forever
            };
            rotateTransform.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnimation);
        }

        private static List<Point3D> GenerateSpherePoints(int sphereCount, double SphereRadius, double azimuthalRotations)
        {
            var spherePoints = new List<Point3D>(sphereCount);

            for (int i = 1; i <= sphereCount; i++)
            {
                double theta = (i / (double)sphereCount) * Math.PI * 2 * azimuthalRotations;
                double delta = (i / (double)sphereCount) * Math.PI;
                double x = SphereRadius * Math.Sin(delta) * Math.Cos(theta);
                double y = SphereRadius * Math.Sin(delta) * Math.Sin(theta);
                double z = SphereRadius * Math.Cos(delta);

                spherePoints.Add(new Point3D(x, y, z));
            }

            return spherePoints;
        }

        private static Model3DGroup GenerateSphereGroup(List<Point3D> spherePoints, Size3D dotSize, Brush brush)
        {
            var group = new Model3DGroup();
            var material = new DiffuseMaterial(brush);


            for (int i = 0; i < spherePoints.Count; i++)
            {
                double hue = (i / (double)spherePoints.Count) * 90 + 180;
                double saturation = 1.0;
                double lightness = (i / (double)spherePoints.Count) * 0.5;

                Color color = Utils.HslToRgb(hue, saturation, lightness);

                //var material2 = new DiffuseMaterial(new SolidColorBrush(color));
                var material2 = new EmissiveMaterial(new SolidColorBrush(color));

                var point = spherePoints[i];
                var dot = new GeometryModel3D()
                {
                    Material = material2,
                    Geometry = Utils.CreateBlockMesh2(point, dotSize)

                };
                group.Children.Add(dot);
            }

            //foreach (var point in spherePoints)
            //{
            //    var dot = new GeometryModel3D()
            //    {
            //        Material = material,
            //        Geometry = Utils.CreateBlockMesh2(point, dotSize)

            //    };
            //    group.Children.Add(dot);
            //}

            return group;
        }

    }
}
