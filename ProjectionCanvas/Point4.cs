using System;
using System.Numerics;

namespace ProjectionCanvas
{
    public struct Point4
    {
        public static Point4 Zero { get; } = new(0, 0, 0, 0);
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Point4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Point4 operator +(Point4 p, Vector4 v)
            => new(p.X + v.X, p.Y + v.Y, p.Z + v.Z, p.W + v.W);

        public static Point4 operator *(Point4 p, float s)
            => new(p.X * s, p.Y * s, p.Z * s, p.W * s);

        public void Transform(Matrix4x4 m)
        {
            X = (X * m.M11) + (Y * m.M21) + (Z * m.M31) + (W * m.M41);
            Y = (X * m.M12) + (Y * m.M22) + (Z * m.M32) + (W * m.M42);
            Z = (X * m.M13) + (Y * m.M23) + (Z * m.M33) + (W * m.M43);
            W = (X * m.M14) + (Y * m.M24) + (Z * m.M34) + (W * m.M44);
        }

        public static Point4 Transform(Point4 p, Matrix4x4 m)
        {
            return new Point4(
                (p.X * m.M11) + (p.Y * m.M21) + (p.Z * m.M31) + (p.W * m.M41),
                (p.X * m.M12) + (p.Y * m.M22) + (p.Z * m.M32) + (p.W * m.M42),
                (p.X * m.M13) + (p.Y * m.M23) + (p.Z * m.M33) + (p.W * m.M43),
                (p.X * m.M14) + (p.Y * m.M24) + (p.Z * m.M34) + (p.W * m.M44)
            );
        }

        public static explicit operator Vector4(Point4 point)
            => new(point.X, point.Y, point.Z, point.W);
    }
}