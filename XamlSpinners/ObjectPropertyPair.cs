using System.Windows;

namespace XamlSpinners
{
    public record ObjectPropertyPair(DependencyObject DependencyObject, DependencyProperty DependencyProperty)
    {
        public override int GetHashCode() => DependencyObject.GetHashCode() ^ DependencyProperty.GetHashCode();
    }
}
