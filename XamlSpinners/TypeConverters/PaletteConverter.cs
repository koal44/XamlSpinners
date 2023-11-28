using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace XamlSpinners
{
    public class PaletteConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is not string inputString || string.IsNullOrEmpty(inputString))
            {
                return new ObservableCollection<Brush>() { Brushes.Transparent };
            }

            // split string by comma or space
            var brushes = inputString
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new SolidColorBrush((Color)ColorConverter.ConvertFromString(s.Trim())))
                .ToList();

            return new ObservableCollection<Brush>(brushes);
        }
    }

}
