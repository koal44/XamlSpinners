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

        private static List<Vector3> GenerateSpherePoints(int sphereCount, double sphereRadius, double azimuthToInclineRatio, SpiralPattern pattern = SpiralPattern.EqualArea)
        {
            var spherePoints = new List<Vector3>(sphereCount);

            for (int i = 1; i <= sphereCount; i++)
            {
                // Inclination needs to be in the range [0, π] but how to distribute the points? If you do a linear distribution, you get a lot of points near the poles and not many near the equator, same as working with latitude and longitude (polar crowding)
                (var azimuthalAngle, var inclinationAngle) = pattern switch
                {
                    SpiralPattern.LinearSpiral => GetLinearSpiralPoint(i, sphereCount),
                    SpiralPattern.EqualArea => GetEqualAreaSpiralPoint(i, sphereCount),
                    SpiralPattern.GoldenSpiral => GetGoldenSpiralPoint(i, sphereCount),
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

        public enum SpiralPattern
        {
            LinearSpiral,
            EqualArea,
            GoldenSpiral
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
