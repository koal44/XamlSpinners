using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace XamlSpinners
{
    public static class Utils
    {
        //        Y
        //        |
        //        5--------6
        //       /|       /|
        //      / |      / |
        //     1--------2  |
        //     |  |     |  |
        //     |  8-----|--7 --- X
        //     | /      | /
        //     |/       |/
        //     4--------3
        //    /
        //   Z
        public static MeshGeometry3D CreateBlockMesh(Point3D center, Size3D size)
        {
            var mesh = new MeshGeometry3D();

            // define the 8 corners of the block relative to the center
            var c1 = new Point3D(center.X - size.X / 2, center.Y + size.Y / 2, center.Z + size.Z / 2);
            var c2 = new Point3D(center.X + size.X / 2, center.Y + size.Y / 2, center.Z + size.Z / 2);
            var c3 = new Point3D(center.X + size.X / 2, center.Y - size.Y / 2, center.Z + size.Z / 2);
            var c4 = new Point3D(center.X - size.X / 2, center.Y - size.Y / 2, center.Z + size.Z / 2);
            var c5 = new Point3D(center.X - size.X / 2, center.Y + size.Y / 2, center.Z - size.Z / 2);
            var c6 = new Point3D(center.X + size.X / 2, center.Y + size.Y / 2, center.Z - size.Z / 2);
            var c7 = new Point3D(center.X + size.X / 2, center.Y - size.Y / 2, center.Z - size.Z / 2);
            var c8 = new Point3D(center.X - size.X / 2, center.Y - size.Y / 2, center.Z - size.Z / 2);
            
            // front face
            mesh.Positions.Add(c1); mesh.Positions.Add(c2); mesh.Positions.Add(c3); // 0 c1, 1 c2, 2 c3
            mesh.Positions.Add(c1); mesh.Positions.Add(c3); mesh.Positions.Add(c4); // 3 c1, 4 c3, 5 c4
            for (int i = 0; i < 6; i++) mesh.Normals.Add(new Vector3D(0, 0, 1));
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(2); mesh.TriangleIndices.Add(1); // c1, c3, c2
            mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(5); mesh.TriangleIndices.Add(4); // c1, c4, c3
            // back face
            mesh.Positions.Add(c5); mesh.Positions.Add(c8); mesh.Positions.Add(c7); // 6 c5, 7 c8, 8 c7
            mesh.Positions.Add(c5); mesh.Positions.Add(c7); mesh.Positions.Add(c6); // 9 c5, 10 c7, 11 c6
            for (int i = 0; i < 6; i++) mesh.Normals.Add(new Vector3D(0, 0, -1));
            mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(8); mesh.TriangleIndices.Add(7); // c5, c7, c8
            mesh.TriangleIndices.Add(11); mesh.TriangleIndices.Add(10); mesh.TriangleIndices.Add(9); // c6, c7, c5
            // top face
            mesh.Positions.Add(c1); mesh.Positions.Add(c5); mesh.Positions.Add(c6); // 12 c1, 13 c5, 14 c6
            mesh.Positions.Add(c1); mesh.Positions.Add(c6); mesh.Positions.Add(c2); // 15 c1, 16 c6, 17 c2
            for (int i = 0; i < 6; i++) mesh.Normals.Add(new Vector3D(0, 1, 0));
            mesh.TriangleIndices.Add(12); mesh.TriangleIndices.Add(14); mesh.TriangleIndices.Add(13); // c1, c6, c5
            mesh.TriangleIndices.Add(15); mesh.TriangleIndices.Add(17); mesh.TriangleIndices.Add(16); // c1, c2, c6
            // bottom face
            mesh.Positions.Add(c3); mesh.Positions.Add(c7); mesh.Positions.Add(c8); // 18 c3, 19 c7, 20 c8
            mesh.Positions.Add(c3); mesh.Positions.Add(c8); mesh.Positions.Add(c4); // 21 c3, 22 c8, 23 c4
            for (int i = 0; i < 6; i++) mesh.Normals.Add(new Vector3D(0, -1, 0));
            mesh.TriangleIndices.Add(18); mesh.TriangleIndices.Add(20); mesh.TriangleIndices.Add(19); // c3, c8, c7
            mesh.TriangleIndices.Add(21); mesh.TriangleIndices.Add(23); mesh.TriangleIndices.Add(22); // c3, c4, c8
            // left face
            mesh.Positions.Add(c1); mesh.Positions.Add(c4); mesh.Positions.Add(c8); // 24 c1, 25 c4, 26 c8
            mesh.Positions.Add(c1); mesh.Positions.Add(c8); mesh.Positions.Add(c5); // 27 c1, 28 c8, 29 c5
            for (int i = 0; i < 6; i++) mesh.Normals.Add(new Vector3D(-1, 0, 0));
            mesh.TriangleIndices.Add(24); mesh.TriangleIndices.Add(26); mesh.TriangleIndices.Add(25); // c1, c8, c4
            mesh.TriangleIndices.Add(27); mesh.TriangleIndices.Add(29); mesh.TriangleIndices.Add(28); // c1, c5, c8
            // right face
            mesh.Positions.Add(c2); mesh.Positions.Add(c6); mesh.Positions.Add(c7); // 30 c2, 31 c6, 32 c7
            mesh.Positions.Add(c2); mesh.Positions.Add(c7); mesh.Positions.Add(c3); // 33 c2, 34 c7, 35 c3
            for (int i = 0; i < 6; i++) mesh.Normals.Add(new Vector3D(1, 0, 0));
            mesh.TriangleIndices.Add(30); mesh.TriangleIndices.Add(32); mesh.TriangleIndices.Add(31); // c2, c7, c6
            mesh.TriangleIndices.Add(33); mesh.TriangleIndices.Add(35); mesh.TriangleIndices.Add(34); // c2, c3, c7

            return mesh;
        }


        //        Y
        //        |
        //        4--------5
        //       /|       /|
        //      / |      / |
        //     0--------1  |
        //     |  |     |  |
        //     |  7-----|--6 --- X
        //     | /      | /
        //     |/       |/
        //     3--------2
        //    /
        //   Z
        public static MeshGeometry3D CreateBlockMesh2(Point3D center, Size3D size)
        {
            var mesh = new MeshGeometry3D();

            // define the 8 corners of the block relative to the center
            var c0 = new Point3D(center.X - size.X / 2, center.Y + size.Y / 2, center.Z + size.Z / 2);
            var c1 = new Point3D(center.X + size.X / 2, center.Y + size.Y / 2, center.Z + size.Z / 2);
            var c2 = new Point3D(center.X + size.X / 2, center.Y - size.Y / 2, center.Z + size.Z / 2);
            var c3 = new Point3D(center.X - size.X / 2, center.Y - size.Y / 2, center.Z + size.Z / 2);
            var c4 = new Point3D(center.X - size.X / 2, center.Y + size.Y / 2, center.Z - size.Z / 2);
            var c5 = new Point3D(center.X + size.X / 2, center.Y + size.Y / 2, center.Z - size.Z / 2);
            var c6 = new Point3D(center.X + size.X / 2, center.Y - size.Y / 2, center.Z - size.Z / 2);
            var c7 = new Point3D(center.X - size.X / 2, center.Y - size.Y / 2, center.Z - size.Z / 2);

            mesh.Positions.Add(c0);
            mesh.Positions.Add(c1);
            mesh.Positions.Add(c2);
            mesh.Positions.Add(c3);
            mesh.Positions.Add(c4);
            mesh.Positions.Add(c5);
            mesh.Positions.Add(c6);
            mesh.Positions.Add(c7);
            
            // front face
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(2); mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(2);
            // back face
            mesh.TriangleIndices.Add(4); mesh.TriangleIndices.Add(5); mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4); mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(7);
            // top face
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(1); mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(5); mesh.TriangleIndices.Add(4);
            // bottom face
            mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(7); mesh.TriangleIndices.Add(6);
            // left face
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(4); mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(7); mesh.TriangleIndices.Add(3);
            // right face
            mesh.TriangleIndices.Add(1); mesh.TriangleIndices.Add(2); mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(1); mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(5);

            return mesh;
        }


        public static Color HslToRgb(double hue, double saturation, double lightness)
        {
            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double hueSection = hue / 60.0;
            double x = chroma * (1 - Math.Abs(hueSection % 2 - 1));
            double m = lightness - chroma / 2;

            double r = 0, g = 0, b = 0;
            if (hueSection >= 0 && hueSection < 1)
            {
                r = chroma; g = x;
            }
            else if (hueSection >= 1 && hueSection < 2)
            {
                r = x; g = chroma;
            }
            else if (hueSection >= 2 && hueSection < 3)
            {
                g = chroma; b = x;
            }
            else if (hueSection >= 3 && hueSection < 4)
            {
                g = x; b = chroma;
            }
            else if (hueSection >= 4 && hueSection < 5)
            {
                r = x; b = chroma;
            }
            else if (hueSection >= 5 && hueSection < 6)
            {
                r = chroma; b = x;
            }

            r += m; g += m; b += m;
            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }


        public static MeshGeometry3D CreateBlockMesh2(string positions, string triangleIndices)
        {
            var mesh = new MeshGeometry3D();

            var positionValues = positions.Split(' ');
            for (int i = 0; i < positionValues.Length; i += 3)
            {
                mesh.Positions.Add(new Point3D(
                    double.Parse(positionValues[i]),
                    double.Parse(positionValues[i + 1]),
                    double.Parse(positionValues[i + 2])));
            }

            var indexValues = triangleIndices.Split(' ');
            foreach (var index in indexValues)
            {
                mesh.TriangleIndices.Add(int.Parse(index));
            }

            return mesh;
        }



    }
}
