using System;
using System.Reflection;
using System.Windows;

namespace XamlSpinners.Extensions
{
    public static class DependencyObjectExtensions
    {
        // DependencyObject.InheritanceContext
        private static readonly Lazy<PropertyInfo> _inheritanceContextProperty = new Lazy<PropertyInfo>(
            () => typeof(DependencyObject)
                .GetProperty("InheritanceContext", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Could not find InheritanceContext property"),
            isThreadSafe: true);

        public static DependencyObject? GetInheritanceContext(this DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return (DependencyObject?)_inheritanceContextProperty.Value.GetValue(obj);
        }
    }

}
