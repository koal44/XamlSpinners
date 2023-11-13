using System;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{

    public abstract class DoubleCollectionAnimationBase : AnimationTimeline
    {

        protected DoubleCollectionAnimationBase() : base() { }


        public new DoubleCollectionAnimationBase Clone() => (DoubleCollectionAnimationBase)base.Clone();


        public override sealed object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (defaultOriginValue == null) throw new ArgumentNullException(nameof(defaultOriginValue));
            if (defaultDestinationValue == null) throw new ArgumentNullException(nameof(defaultDestinationValue));

            return GetCurrentValue((DoubleCollection)defaultOriginValue, (DoubleCollection)defaultDestinationValue, animationClock);
        }


        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(DoubleCollection);
            }
        }


        public DoubleCollection GetCurrentValue(DoubleCollection defaultOriginValue, DoubleCollection defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            if (animationClock == null) throw new ArgumentNullException(nameof(animationClock));

            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }


        protected abstract DoubleCollection GetCurrentValueCore(DoubleCollection defaultOriginValue, DoubleCollection defaultDestinationValue, AnimationClock animationClock);

    }
}