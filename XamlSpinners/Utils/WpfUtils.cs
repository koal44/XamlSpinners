using System;
using System.Windows;
using System.Windows.Markup;
using XamlSpinners.Extensions;

namespace XamlSpinners.Utils
{
    public static class WpfUtils
    {
        public static DependencyObject ResolveTargetName(string targetName, INameScope? nameScope, DependencyObject element)
        {
            object? nameScopeUsed = element switch
            {
                FrameworkElement fe => ((object?)(FrameworkTemplate?)nameScope) ?? (object?)fe,
                FrameworkContentElement fce => fce,
                _ => throw new InvalidOperationException($"Storyboard_NoNameScope, {targetName}")
            };

            object? namedObject = nameScopeUsed switch
            {
                FrameworkElement fe when nameScope is FrameworkTemplate template => template.FindName(targetName, fe),
                FrameworkElement fe => fe.FindName(targetName),
                FrameworkContentElement fce => fce.FindName(targetName),
                _ => throw new InvalidOperationException($"Invalid nameScope type for {targetName}")
            } ?? throw new InvalidOperationException($"Storyboard_NameNotFound, {targetName}, {nameScopeUsed.GetType()}");

            if (namedObject is not DependencyObject targetObject)
                throw new InvalidOperationException($"Storyboard_TargetNameNotDependencyObject, {targetName}");

            return targetObject;
        }

        public static DependencyObject? FindMentor(DependencyObject? d)
        {
            // Find the nearest FE/FCE InheritanceContext
            while (d != null)
            {
                var (fe, fce) = DowncastToFEorFCE(d, false);

                if (fe != null) return fe;
                if (fce != null) return fce;

                d = d.GetInheritanceContext();
            }

            return null;
        }

        public static (FrameworkElement? fe, FrameworkContentElement? fce) DowncastToFEorFCE(DependencyObject d, bool throwIfNeither)
        {
            return d switch
            {
                FrameworkElement fe => (fe, null),
                FrameworkContentElement fce => (null, fce),
                _ when throwIfNeither => throw new InvalidOperationException($"Must be a FrameworkElement or FrameworkContentElement, but was {d.GetType()}"),
                _ => (null, null),
            };
        }
    }

}
