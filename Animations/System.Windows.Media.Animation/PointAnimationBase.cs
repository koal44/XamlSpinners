using System.Windows.Media.Animation;

namespace System.Windows.Media.Animation2
{
    public abstract class PointAnimationBase : AnimationTimeline
    {
        protected PointAnimationBase() : base() { }

        public new PointAnimationBase Clone() => (PointAnimationBase)base.Clone();


        public override sealed object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (defaultOriginValue == null) throw new ArgumentNullException(nameof(defaultOriginValue));
            if (defaultDestinationValue == null) throw new ArgumentNullException(nameof(defaultDestinationValue));

            return GetCurrentValue((Point)defaultOriginValue, (Point)defaultDestinationValue, animationClock);
        }

        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(Point);
            }
        }


        public Point GetCurrentValue(Point defaultOriginValue, Point defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            if (animationClock == null) throw new ArgumentNullException(nameof(animationClock));
            if (animationClock.CurrentState == ClockState.Stopped) return defaultDestinationValue;

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }

        protected abstract Point GetCurrentValueCore(Point defaultOriginValue, Point defaultDestinationValue, AnimationClock animationClock);

    }
}
