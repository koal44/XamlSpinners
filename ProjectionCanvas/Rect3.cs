using System;
using System.Diagnostics;
using System.Numerics;

namespace ProjectionCanvas
{
    public struct Rect3 : IFormattable
    {

        #region Constructors

        public Rect3(Point3 location, Size3 size)
        {
            if (size.IsEmpty)
            {
                this = s_empty;
            }
            else
            {
                _x = location.X;
                _y = location.Y;
                _z = location.Z;
                _sizeX = size.X;
                _sizeY = size.Y;
                _sizeZ = size.Z;
            }
            Debug.Assert(size.IsEmpty == IsEmpty);
        }

       
        public Rect3(float x, float y, float z, float sizeX, float sizeY, float sizeZ)
        {
            if (sizeX < 0 || sizeY < 0 || sizeZ < 0)
                throw new ArgumentException("Size cannot be negative");

            _x = x;
            _y = y;
            _z = z;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _sizeZ = sizeZ;
        }

        internal Rect3(Point3 point1, Point3 point2)
        {
            _x = Math.Min(point1.X, point2.X);
            _y = Math.Min(point1.Y, point2.Y);
            _z = Math.Min(point1.Z, point2.Z);
            _sizeX = Math.Max(point1.X, point2.X) - _x;
            _sizeY = Math.Max(point1.Y, point2.Y) - _y;
            _sizeZ = Math.Max(point1.Z, point2.Z) - _z;
        }

        internal Rect3(Point3 point, Vector3 vector) : this(point, point + vector)
        {

        }

        #endregion Constructors

        #region Public Properties

        public static Rect3 Empty => s_empty;

        public readonly bool IsEmpty => _sizeX < 0;

        public Point3 Location
        {
            readonly get => new(_x, _y, _z);
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");

                _x = value.X;
                _y = value.Y;
                _z = value.Z;
            }
        }

        public Size3 Size
        {
            readonly get => IsEmpty ? Size3.Empty : new Size3(_sizeX, _sizeY, _sizeZ);
            set
            {
                if (value.IsEmpty)
                {
                    this = s_empty;
                }
                else
                {
                    if (IsEmpty)
                        throw new InvalidOperationException("CannotModifyEmptyRect");

                    _sizeX = value.X;
                    _sizeY = value.Y;
                    _sizeZ = value.Z;
                }
            }
        }


        public float SizeX
        {
            readonly get => _sizeX;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                if (value < 0)
                    throw new ArgumentException("Size cannot be negative");

                _sizeX = value;
            }
        }

        public float SizeY
        {
            readonly get => _sizeY;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                if (value < 0)
                    throw new ArgumentException("Size cannot be negative");

                _sizeY = value;
            }
        }

        public float SizeZ
        {
            readonly get => _sizeZ;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                if (value < 0)
                    throw new ArgumentException("Size cannot be negative");

                _sizeZ = value;
            }
        }

        public float X
        {
            readonly get => _x;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");

                _x = value;
            }
        }

        public float Y
        {
            readonly get => _y;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");

                _y = value;
            }
        }

        public float Z
        {
            readonly get => _z;
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");

                _z = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public readonly bool Contains(Point3 point)
            => Contains(point.X, point.Y, point.Z);

        public readonly bool Contains(float x, float y, float z) 
            => !IsEmpty && ContainsInternal(x, y, z);

        public readonly bool Contains(Rect3 rect)
        {
            return !IsEmpty &&
                   !rect.IsEmpty &&
                   (_x <= rect._x) &&
                   (_y <= rect._y) &&
                   (_z <= rect._z) &&
                   (_x + _sizeX >= rect._x + rect._sizeX) &&
                   (_y + _sizeY >= rect._y + rect._sizeY) &&
                   (_z + _sizeZ >= rect._z + rect._sizeZ);
        }

        public readonly bool IntersectsWith(Rect3 rect)
        {
            if (IsEmpty || rect.IsEmpty) return false;

            return (rect._x <= (_x + _sizeX)) &&
                   ((rect._x + rect._sizeX) >= _x) &&
                   (rect._y <= (_y + _sizeY)) &&
                   ((rect._y + rect._sizeY) >= _y) &&
                   (rect._z <= (_z + _sizeZ)) &&
                   ((rect._z + rect._sizeZ) >= _z);
        }

        public void Intersect(Rect3 rect)
        {
            if (IsEmpty || rect.IsEmpty || !this.IntersectsWith(rect))
            {
                this = Empty;
            }
            else
            {
                float x = Math.Max(_x, rect._x);
                float y = Math.Max(_y, rect._y);
                float z = Math.Max(_z, rect._z);
                _sizeX = Math.Min(_x + _sizeX, rect._x + rect._sizeX) - x;
                _sizeY = Math.Min(_y + _sizeY, rect._y + rect._sizeY) - y;
                _sizeZ = Math.Min(_z + _sizeZ, rect._z + rect._sizeZ) - z;

                _x = x;
                _y = y;
                _z = z;
            }
        }

        public static Rect3 Intersect(Rect3 rect1, Rect3 rect2)
        {
            rect1.Intersect(rect2);
            return rect1;
        }

        public void Union(Rect3 rect)
        {
            if (IsEmpty)
            {
                this = rect;
            }
            else if (!rect.IsEmpty)
            {
                float x = Math.Min(_x, rect._x);
                float y = Math.Min(_y, rect._y);
                float z = Math.Min(_z, rect._z);
                _sizeX = Math.Max(_x + _sizeX, rect._x + rect._sizeX) - x;
                _sizeY = Math.Max(_y + _sizeY, rect._y + rect._sizeY) - y;
                _sizeZ = Math.Max(_z + _sizeZ, rect._z + rect._sizeZ) - z;
                _x = x;
                _y = y;
                _z = z;
            }
        }

        public static Rect3 Union(Rect3 rect1, Rect3 rect2)
        {
            rect1.Union(rect2);
            return rect1;
        }

        public void Union(Point3 point)
        {
            Union(new Rect3(point, point));
        }

        public static Rect3 Union(Rect3 rect, Point3 point)
        {
            rect.Union(new Rect3(point, point));
            return rect;
        }

        public void Offset(Vector3 offsetVector)
        {
            Offset(offsetVector.X, offsetVector.Y, offsetVector.Z);
        }

        public void Offset(float offsetX, float offsetY, float offsetZ)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotModifyEmptyRect");

            _x += offsetX;
            _y += offsetY;
            _z += offsetZ;
        }

        public static Rect3 Offset(Rect3 rect, Vector3 offsetVector)
        {
            rect.Offset(offsetVector.X, offsetVector.Y, offsetVector.Z);
            return rect;
        }

        public static Rect3 Offset(Rect3 rect, float offsetX, float offsetY, float offsetZ)
        {
            rect.Offset(offsetX, offsetY, offsetZ);
            return rect;
        }

        #endregion Public Methods

        #region Internal Fields

        internal readonly static Rect3 Infinite = CreateInfiniteRect3();

        #endregion Internal Fields

        #region Private Methods

        private readonly bool ContainsInternal(float x, float y, float z)
        {
            // We include points on the edge as "contained"
            return ((x >= _x) && (x <= _x + _sizeX) &&
                    (y >= _y) && (y <= _y + _sizeY) &&
                    (z >= _z) && (z <= _z + _sizeZ));
        }

        private static Rect3 CreateEmptyRect3()
        {
            Rect3 empty = new()
            {
                _x = float.PositiveInfinity,
                _y = float.PositiveInfinity,
                _z = float.PositiveInfinity,
                // Can't use setters because they throw on negative values
                _sizeX = float.NegativeInfinity,
                _sizeY = float.NegativeInfinity,
                _sizeZ = float.NegativeInfinity
            };
            return empty;
        }

        private static Rect3 CreateInfiniteRect3()
        {
            Rect3 infinite = new()
            {
                _x = -float.MaxValue,
                _y = -float.MaxValue,
                _z = -float.MaxValue,
                _sizeX = float.MaxValue,
                _sizeY = float.MaxValue,
                _sizeZ = float.MaxValue
            };
            return infinite;
        }

        #endregion Private Methods

        #region Private Fields

        private readonly static Rect3 s_empty = CreateEmptyRect3();

        #endregion Private Fields

        #region Public Methods

        public static bool operator ==(Rect3 rect1, Rect3 rect2)
        {
            return rect1.X == rect2.X &&
                   rect1.Y == rect2.Y &&
                   rect1.Z == rect2.Z &&
                   rect1.SizeX == rect2.SizeX &&
                   rect1.SizeY == rect2.SizeY &&
                   rect1.SizeZ == rect2.SizeZ;
        }

        public static bool operator !=(Rect3 rect1, Rect3 rect2)
        {
            return !(rect1 == rect2);
        }

        public static bool Equals(Rect3 rect1, Rect3 rect2)
        {
            if (rect1.IsEmpty)
            {
                return rect2.IsEmpty;
            }
            else
            {
                return rect1.X.Equals(rect2.X) &&
                       rect1.Y.Equals(rect2.Y) &&
                       rect1.Z.Equals(rect2.Z) &&
                       rect1.SizeX.Equals(rect2.SizeX) &&
                       rect1.SizeY.Equals(rect2.SizeY) &&
                       rect1.SizeZ.Equals(rect2.SizeZ);
            }
        }

        public override readonly bool Equals(object? o)
        {
            if ((null == o) || o is not Rect3)
            {
                return false;
            }

            Rect3 value = (Rect3)o;
            return Rect3.Equals(this, value);
        }

        public readonly bool Equals(Rect3 value)
        {
            return Rect3.Equals(this, value);
        }

        public override readonly int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }
            else
            {
                // Perform field-by-field XOR of HashCodes
                return X.GetHashCode() ^
                       Y.GetHashCode() ^
                       Z.GetHashCode() ^
                       SizeX.GetHashCode() ^
                       SizeY.GetHashCode() ^
                       SizeZ.GetHashCode();
            }
        }

        #endregion Public Properties

        #region IFormattable

        public override readonly string ToString() 
            => ConvertToString(null, null);

        public readonly string ToString(IFormatProvider provider) 
            => ConvertToString(null, provider);

        readonly string IFormattable.ToString(string? format, IFormatProvider? provider)
            => ConvertToString(format, provider);

        internal readonly string ConvertToString(string? _, IFormatProvider? _2)
        {
            if (IsEmpty)
            {
                return "Empty";
            }

            return $"location: [x:{_x}, y:{_y}, z:{_z}], size: [width:{_sizeX}, height:{_sizeY}, depth:{_sizeZ}]";
        }

        #endregion

        #region Data

        internal float _x;
        internal float _y;
        internal float _z;
        internal float _sizeX;
        internal float _sizeY;
        internal float _sizeZ;

        #endregion

    }
}