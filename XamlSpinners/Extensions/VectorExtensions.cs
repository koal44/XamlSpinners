using System.Numerics;

namespace XamlSpinners
{
    public static class Vector3Extensions
    {
        public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z)
        {
            x = vector.X;
            y = vector.Y;
            z = vector.Z;
        }
    }

    public static class Vector4Extensions
    {
        public static void Deconstruct(this Vector4 vector, out float x, out float y, out float z, out float w)
        {
            x = vector.X;
            y = vector.Y;
            z = vector.Z;
            w = vector.W;
        }
    }
}
