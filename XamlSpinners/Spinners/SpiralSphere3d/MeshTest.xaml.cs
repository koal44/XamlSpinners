using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using XamlSpinners.Utils;

namespace XamlSpinners
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            var dot = new GeometryModel3D()
            {
                Material = new DiffuseMaterial(Brushes.Red),
                Geometry = ThreeDUtils.CreateBlockMesh(new Point3D(1,1,1), new Size3D(2,2,2))
            };
            extraModel.Content = dot;
        }
    }
}
