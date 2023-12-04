using System;
using System.Reflection;
using System.Windows;

namespace XamlSpinners.Extensions
{
    public static class PropertyPathExtensions
    {
        // PropertyPath.Length
        private static readonly Lazy<PropertyInfo> _lengthPropertyInfo = new(
            () => typeof(PropertyPath)
                .GetProperty("Length", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Could not find Length property in PropertyPath"),
            isThreadSafe: true);

        public static int GetLength(this PropertyPath propertyPath)
        {
            return (int)(_lengthPropertyInfo.Value.GetValue(propertyPath)
                ?? throw new InvalidOperationException("Unable to get the Length of the PropertyPath"));
        }

        // PropertyPath.SetContext()
        private static readonly Lazy<MethodInfo> _setContextMethodInfo = new(
            () => typeof(PropertyPath)
                .GetMethod("SetContext", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Could not find SetContext method in PropertyPath"),
            isThreadSafe: true);

        public static IDisposable SetContext(this PropertyPath path, object rootItem)
        {
            var setContextMethod = _setContextMethodInfo.Value;
            return (IDisposable)(setContextMethod.Invoke(path, new[] { rootItem })
                ?? throw new InvalidOperationException("SetContext invocation failed or returned null."));
        }

        // PropertyPath.GetAccessor()
        private static readonly Lazy<MethodInfo> _getAccessorMethodInfo = new(
            () => typeof(PropertyPath)
                .GetMethod("GetAccessor", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Could not find GetAccessor method in PropertyPath"),
            isThreadSafe: true);

        public static object GetAccessor(this PropertyPath path, int k)
        {
            return _getAccessorMethodInfo.Value.Invoke(path, new object[] { k })
                ?? throw new InvalidOperationException("GetAccessor invocation failed or returned null.");
        }

        // PropertyPath.LastItem
        private static readonly Lazy<MethodInfo> _getLastItemMethodInfo = new(
        () => typeof(PropertyPath)
            .GetProperty("LastItem", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetGetMethod(nonPublic: true)
            ?? throw new InvalidOperationException("Could not find LastItem property in PropertyPath"),
        isThreadSafe: true);

        public static object GetLastItem(this PropertyPath path)
        {
            return _getLastItemMethodInfo.Value.Invoke(path, null)
                ?? throw new InvalidOperationException("GetLastItem invocation failed or returned null.");
        }

        // PropertyPath.LastAccessor
        private static readonly Lazy<MethodInfo> _getLastAccessorMethodInfo = new(
            () => typeof(PropertyPath)
                .GetProperty("LastAccessor", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetGetMethod(nonPublic: true)
                ?? throw new InvalidOperationException("Could not find LastAccessor property in PropertyPath"),
            isThreadSafe: true);

        public static object GetLastAccessor(this PropertyPath path)
        {
            return _getLastAccessorMethodInfo.Value.Invoke(path, null)
                ?? throw new InvalidOperationException("GetLastAccessor invocation failed or returned null.");
        }

    }

}
