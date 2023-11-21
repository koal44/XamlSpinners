using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProjectionCanvas
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            


        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var spherePoints = GenerateSpherePoints(1000, 200, 237);
            var sphereGroup = GenerateSphereGroup(spherePoints, new Size(2, 2), Brushes.Red);
            viewport.ShapeGroup = sphereGroup;
            //viewport.Children.Add(sphereGroup);
            viewport.Camera = new OrthographicCamera()
            {
                Position = new Point3(0, 600, 0),
                LookDirection = new Vector3(0, -1, 0),
                UpDirection = new Vector3(0, 0, 1),
                Width = 1000,
                FarPlaneDistance = 600,
                NearPlaneDistance = 400
            };

            viewport.Camera = new PerspectiveCamera()
            {
                Position = new Point3(0, 600, 0),
                LookDirection = new Vector3(0, -1, 0),
                UpDirection = new Vector3(0, 0, 1),
                FieldOfView = 45,
                FarPlaneDistance = float.PositiveInfinity,
                NearPlaneDistance = 0.125f
            };


            var rotateTransform = new RotateTransform3(new AxisAngleRotation3(new Vector3(0, 0, 1), 0));
            sphereGroup.Transform = rotateTransform;

            // Create and apply the DoubleAnimation
            var rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(10),
                RepeatBehavior = RepeatBehavior.Forever
            };
            rotateTransform.Rotation.BeginAnimation(AxisAngleRotation3.AngleProperty, rotationAnimation);

            // use task.run to set a debug breakpoint here and see the sphere points every 1 second

            //while (true)
            //{
            //    //var foo = rotationAnimation.GetCurrentValueAsFrozen();
            //    await Task.Delay(1000);
            //}
        }

        private static List<Point3> GenerateSpherePoints(int sphereCount, double SphereRadius, double azimuthalRotations)
        {
            var spherePoints = new List<Point3>(sphereCount);

            for (int i = 1; i <= sphereCount; i++)
            {
                double theta = (i / (double)sphereCount) * Math.PI * 2 * azimuthalRotations;
                double delta = (i / (double)sphereCount) * Math.PI;
                double x = SphereRadius * Math.Sin(delta) * Math.Cos(theta);
                double y = SphereRadius * Math.Sin(delta) * Math.Sin(theta);
                double z = SphereRadius * Math.Cos(delta);

                spherePoints.Add(new Point3((float)x, (float)y, (float)z));
            }

            return spherePoints;
        }


        private static ProjectableShapeGroup GenerateSphereGroup(List<Point3> spherePoints, Size dotSize, Brush brush)
        {
            var group = new ProjectableShapeGroup();

            for (int i = 0; i < spherePoints.Count; i++)
            {
                var point = spherePoints[i];
                var dot = new ProjectableShape()
                {
                    Fill = brush,
                    Size = dotSize,
                    Position = point
                };

                group.Children.Add(dot);
            }

            return group;
        }
    }
}
