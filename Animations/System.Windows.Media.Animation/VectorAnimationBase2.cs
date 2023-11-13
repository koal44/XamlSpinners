using System.Windows.Media.Animation;

namespace System.Windows.Media.Animation2
{

    public abstract class VectorAnimationBase2 : AnimationTimeline
    {

        protected VectorAnimationBase2()
            : base()
        {
        }

        public new VectorAnimationBase2 Clone()
        {
            return (VectorAnimationBase2)base.Clone();
        }

        public override sealed object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            // Verify that object arguments are non-null since we are a value type
            if (defaultOriginValue == null)
            {
                throw new ArgumentNullException("defaultOriginValue");
            }
            if (defaultDestinationValue == null)
            {
                throw new ArgumentNullException("defaultDestinationValue");
            }
            return GetCurrentValue((Vector)defaultOriginValue, (Vector)defaultDestinationValue, animationClock);
        }

        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(Vector);
            }
        }


        public Vector GetCurrentValue(Vector defaultOriginValue, Vector defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            if (animationClock == null)
            {
                throw new ArgumentNullException("animationClock");
            }

            // We check for null above but presharp doesn't notice so we suppress the 
            // warning here.

            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }

        protected abstract Vector GetCurrentValueCore(Vector defaultOriginValue, Vector defaultDestinationValue, AnimationClock animationClock);

    }
}