using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public partial class SpiralSphere : UserControl
    {
        public SpiralSphere()
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

            var rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(15),
                RepeatBehavior = RepeatBehavior.Forever
            };
            rotation.BeginAnimation(AxisAngleRotation.AngleProperty, rotationAnimation);

        }

        private static List<Vector3> GenerateSpherePoints(int sphereCount, double sphereRadius, double azimuthalRotations)
        {
            var spherePoints = new List<Vector3>(sphereCount);

            for (int i = 1; i <= sphereCount; i++)
            {
                var azimuthalAngle = (i / (double)sphereCount) * Math.PI * 2;
                spherePoints.Add(CalculateSpiralSpherePoint(sphereRadius, azimuthalAngle));
            }

            return spherePoints;

            // This method creates a spiral pattern by fixing `θ = n * φ`. The azimuthalRotations, parameter controls the tightness and frequency of the spirals on the sphere's surface.
            Vector3 CalculateSpiralSpherePoint(double radius, double azimuthalAngle)
            {
                var inclinationAngle = azimuthalAngle * azimuthalRotations;
                return CalculateSpherePoint(radius, azimuthalAngle, inclinationAngle);
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
