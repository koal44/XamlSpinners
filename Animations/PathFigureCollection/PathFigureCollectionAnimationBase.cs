using System;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{

    public abstract class PathFigureCollectionAnimationBase : AnimationTimeline
    {

        protected PathFigureCollectionAnimationBase() : base() { }


        public new PathFigureCollectionAnimationBase Clone() => (PathFigureCollectionAnimationBase)base.Clone();


        public override sealed object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (defaultOriginValue == null) throw new ArgumentNullException(nameof(defaultOriginValue));
            if (defaultDestinationValue == null) throw new ArgumentNullException(nameof(defaultDestinationValue));

            return GetCurrentValue((PathFigureCollection)defaultOriginValue, (PathFigureCollection)defaultDestinationValue, animationClock);
        }


        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(PathFigureCollection);
            }
        }


        public PathFigureCollection GetCurrentValue(PathFigureCollection defaultOriginValue, PathFigureCollection defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            if (animationClock == null) throw new ArgumentNullException(nameof(animationClock));

            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }


        protected abstract PathFigureCollection GetCurrentValueCore(PathFigureCollection defaultOriginValue, PathFigureCollection defaultDestinationValue, AnimationClock animationClock);

    }
}