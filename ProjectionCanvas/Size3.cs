using System;
using System.Numerics;

namespace ProjectionCanvas
{
    public struct Size3 : IEquatable<Size3>, IFormattable
    {
        private float _x;
        private float _y;
        private float _z;

        public Size3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Size3 Empty => s_empty;

        public readonly bool IsEmpty => X < 0;

        public float X
        {
            readonly get => _x;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Cannot modify empty size");
                if (value < 0)
                    throw new ArgumentException("Dimension cannot be negative");

                _x = value;
            }
        }

        public float Y
        {
            readonly get => _y;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Cannot modify empty size");
                if (value < 0)
                    throw new ArgumentException("Dimension cannot be negative");

                _y = value;
            }
        }

        public float Z
        {
            readonly get => _z;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Cannot modify empty size");
                if (value < 0)
                    throw new ArgumentException("Dimension cannot be negative");

                _z = value;
            }
        }

        public static explicit operator Vector3(Size3 size)
            => new(size._x, size._y, size._z);

        public static explicit operator Point3(Size3 size) 
            => new(size._x, size._y, size._z);


        private static Size3 CreateEmptySize3D()
            => new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        private readonly static Size3 s_empty = CreateEmptySize3D();

        public static bool operator ==(Size3 size1, Size3 size2) 
            => size1.X == size2.X && size1.Y == size2.Y && size1.Z == size2.Z;

        public static bool operator !=(Size3 size1, Size3 size2) 
            => !(size1 == size2);
        
        public static bool Equals(Size3 size1, Size3 size2)
        {
            return size1.IsEmpty 
                ? size2.IsEmpty 
                : size1.X.Equals(size2.X) && size1.Y.Equals(size2.Y) && size1.Z.Equals(size2.Z);
        }

        public override readonly bool Equals(object? o)
        {
            return o switch
            {
                null or not Size3 => false,
                _ => Size3.Equals(this, (Size3)o)
            };
        }

        public readonly bool Equals(Size3 value) => Equals(this, value);

        public override readonly int GetHashCode() 
            => IsEmpty
                ? 0
                : X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();

        public override readonly string ToString() => ConvertToString(null, null);

        public readonly string ToString(IFormatProvider provider) => ConvertToString(null, provider);

        readonly string IFormattable.ToString(string? format, IFormatProvider? provider) 
            => ConvertToString(format, provider);

        internal readonly string ConvertToString(string? _, IFormatProvider? _2)
            => IsEmpty ? "Empty" : $"[{X}, {Y}, {Z}]";

    }
}
