using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public partial class EasingDoubleCollectionKeyFrame : DoubleCollectionKeyFrame
    {
        #region Dependency Properties

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(EasingDoubleCollectionKeyFrame));

        #endregion

        #region Constructors

        public EasingDoubleCollectionKeyFrame()
            : base() { }

        public EasingDoubleCollectionKeyFrame(DoubleCollection value)
            : this()
        {
            Value = value;
        }

        public EasingDoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime)
            : this(value)
        {
            KeyTime = keyTime;
        }

        public EasingDoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime, IEasingFunction easingFunction)
            : this(value, keyTime)
        {
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new EasingDoubleCollectionKeyFrame();
        }

        #endregion

        #region DoubleCollectionKeyFrame 

        protected override DoubleCollection InterpolateValueCore(DoubleCollection baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

            if (keyFrameProgress == 0.0)
            {
                return baseValue;
            }
            else if (keyFrameProgress == 1.0)
            {
                return Value;
            }
            else
            {
                return AnimatedTypeHelpers.InterpolateDoubleCollection(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

    }
}
