using System.Windows.Media;

namespace XamlSpinners.Demo
{
    public record ColorItem(Color Color, string Name)
    {
        public Brush Brush => new SolidColorBrush(Color);
    }


}
