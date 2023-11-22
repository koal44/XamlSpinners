using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public partial class SurfaceSphere : UserControl
    {
        public SurfaceSphere()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var spherePoints = GenerateSpherePoints(1000, 200, 12);
            var sphereGroup = GenerateSphereGroup(spherePoints, new Size(2, 2), Brushes.Red);
            rootCanvas.SurfaceGroup = sphereGroup;

            rootCanvas.Camera = new Camera()
            {
                CameraPosition = new Vector3(0, 600, 0),
                TargetPosition = new Vector3(0, -1, 0),
                UpDirection = new Vector3(0, 0, 1),
                FieldOfView = 45,
            };


            var rotateTransform = new RotateTransform(0, new Vector3(0, 0, 1));
            var rotation = rotateTransform.Rotation ?? throw new NullReferenceException("Rotation is null");
            sphereGroup.Transform = rotateTransform;

            // Create and apply the DoubleAnimation
            var rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(15),
                RepeatBehavior = RepeatBehavior.Forever
            };
            rotation.BeginAnimation(AxisAngleRotation.AngleProperty, rotationAnimation);

        }

        private static List<Vector3> GenerateSpherePoints(int sphereCount, double SphereRadius, double azimuthalRotations)
        {
            var spherePoints = new List<Vector3>(sphereCount);

            for (int i = 1; i <= sphereCount; i++)
            {
                double theta = (i / (double)sphereCount) * Math.PI * 2 * azimuthalRotations;
                double delta = (i / (double)sphereCount) * Math.PI;
                double x = SphereRadius * Math.Sin(delta) * Math.Cos(theta);
                double y = SphereRadius * Math.Sin(delta) * Math.Sin(theta);
                double z = SphereRadius * Math.Cos(delta);

                spherePoints.Add(new Vector3((float)x, (float)y, (float)z));
            }

            return spherePoints;
        }


        private static SurfaceElementGroup GenerateSphereGroup(List<Vector3> spherePoints, Size dotSize, Brush brush)
        {
            var group = new SurfaceElementGroup();

            for (int i = 0; i < spherePoints.Count; i++)
            {
                var point = spherePoints[i];
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
    }
}
