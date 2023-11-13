using System;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{

    public abstract class PointCollectionAnimationBase : AnimationTimeline
    {

        protected PointCollectionAnimationBase() : base() { }


        public new PointCollectionAnimationBase Clone() => (PointCollectionAnimationBase)base.Clone();


        public override sealed object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (defaultOriginValue == null) throw new ArgumentNullException(nameof(defaultOriginValue));
            if (defaultDestinationValue == null) throw new ArgumentNullException(nameof(defaultDestinationValue));

            return GetCurrentValue((PointCollection)defaultOriginValue, (PointCollection)defaultDestinationValue, animationClock);
        }


        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(PointCollection);
            }
        }


        public PointCollection GetCurrentValue(PointCollection defaultOriginValue, PointCollection defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            if (animationClock == null) throw new ArgumentNullException(nameof(animationClock));

            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }


        protected abstract PointCollection GetCurrentValueCore(PointCollection defaultOriginValue, PointCollection defaultDestinationValue, AnimationClock animationClock);

    }
}