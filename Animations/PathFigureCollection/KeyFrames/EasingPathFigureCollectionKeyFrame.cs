using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public partial class EasingPathFigureCollectionKeyFrame : PathFigureCollectionKeyFrame
    {
        #region Dependency Properties

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(EasingPathFigureCollectionKeyFrame));

        #endregion

        #region Constructors

        public EasingPathFigureCollectionKeyFrame()
            : base() { }

        public EasingPathFigureCollectionKeyFrame(PathFigureCollection value)
            : this()
        {
            Value = value;
        }

        public EasingPathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime)
            : this(value)
        {
            KeyTime = keyTime;
        }

        public EasingPathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime, IEasingFunction easingFunction)
            : this(value, keyTime)
        {
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new EasingPathFigureCollectionKeyFrame();
        }

        #endregion

        #region PathFigureCollectionKeyFrame 

        protected override PathFigureCollection InterpolateValueCore(PathFigureCollection baseValue, double keyFrameProgress)
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
                return AnimatedTypeHelpers.InterpolatePathFigureCollection(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

    }
}
