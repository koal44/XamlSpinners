using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public partial class EasingPointCollectionKeyFrame : PointCollectionKeyFrame
    {
        #region Dependency Properties

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(EasingPointCollectionKeyFrame));

        #endregion

        #region Constructors

        public EasingPointCollectionKeyFrame()
            : base() { }

        public EasingPointCollectionKeyFrame(PointCollection value)
            : this()
        {
            Value = value;
        }

        public EasingPointCollectionKeyFrame(PointCollection value, KeyTime keyTime)
            : this(value)
        {
            KeyTime = keyTime;
        }

        public EasingPointCollectionKeyFrame(PointCollection value, KeyTime keyTime, IEasingFunction easingFunction)
            : this(value, keyTime)
        {
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new EasingPointCollectionKeyFrame();
        }

        #endregion

        #region PointCollectionKeyFrame 

        protected override PointCollection InterpolateValueCore(PointCollection baseValue, double keyFrameProgress)
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
                return AnimatedTypeHelpers.InterpolatePointCollection(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

    }
}
