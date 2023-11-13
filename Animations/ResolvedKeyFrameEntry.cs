using System;

namespace Animations
{
    public struct ResolvedKeyFrameEntry : IComparable
    {
        public int _originalKeyFrameIndex;
        public TimeSpan _resolvedKeyTime;

        public readonly int CompareTo(object? other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (other is not ResolvedKeyFrameEntry otherEntry) throw new ArgumentException($"{nameof(other)} is not a ResolvedKeyFrameEntry");

            if (_resolvedKeyTime == otherEntry._resolvedKeyTime)
            {
                return _originalKeyFrameIndex.CompareTo(otherEntry._originalKeyFrameIndex);
            }

            return _resolvedKeyTime.CompareTo(otherEntry._resolvedKeyTime);
        }
    }
}