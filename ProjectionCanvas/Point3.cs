using System;
using System.Numerics;

namespace ProjectionCanvas
{
    public struct Point3
    {
        public static Point3 Zero { get; } = new(0, 0, 0);
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Point3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public static Point3 operator +(Point3 point, Vector3 vector) 
            => new(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);

        public float GetAngle(Point3 b)
        {
            double angle;

            // cos(θ) = a⋅b​ / |a|×|b|, for θ [0,π]
            angle = Math.Acos((X * b.X + Y * b.Y + Z * b.Z) / (Math.Sqrt(X * X + Y * Y + Z * Z) * Math.Sqrt(b.X * b.X + b.Y * b.Y + b.Z * b.Z)));

            return (float)angle;
        }

        public void Transform(Matrix4x4 matrix)
        {
            X = (X * matrix.M11) + (Y * matrix.M21) + (Z * matrix.M31) + matrix.M41;
            Y = (X * matrix.M12) + (Y * matrix.M22) + (Z * matrix.M32) + matrix.M42;
            Z = (X * matrix.M13) + (Y * matrix.M23) + (Z * matrix.M33) + matrix.M43;
        }

        public static Point3 Transform(Point3 position, Matrix4x4 matrix)
        {
            return new Point3(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42,
                (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43
            );
        }

        public static explicit operator Vector3(Point3 point) 
            => new(point.X, point.Y, point.Z);
    }
}