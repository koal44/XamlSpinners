using System;
using System.Reflection;
using System.Windows.Media.Animation;

namespace XamlSpinners.Extensions
{
    public static class ClockExtensions
    {
        private static readonly Lazy<MethodInfo> _internalSeekMethodInfo = new(
            () => typeof(Clock)
                .GetMethod("InternalSeek", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Could not find InternalSeek method in ClockClass"),
            isThreadSafe: true);

        public static void InternalSeek(this Clock clock, TimeSpan destination)
        {
            if (clock == null)
                throw new ArgumentNullException(nameof(clock), "Clock cannot be null.");

            _internalSeekMethodInfo.Value.Invoke(clock, new object[] { destination });
        }
    }

}
