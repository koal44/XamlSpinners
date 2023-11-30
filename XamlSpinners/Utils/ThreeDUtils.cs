using System.Windows.Media.Media3D;

namespace XamlSpinners.Utils
{
    public static class ThreeDUtils
    {
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
        public static MeshGeometry3D CreateBlockMesh(Point3D center, Size3D size)
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

        public static MeshGeometry3D ParseMesh(string positions, string triangleIndices)
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
