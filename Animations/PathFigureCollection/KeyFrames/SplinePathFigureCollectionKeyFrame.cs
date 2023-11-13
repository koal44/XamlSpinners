using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public class SplinePathFigureCollectionKeyFrame : PathFigureCollectionKeyFrame
    {
        #region Dependency Properties

        public KeySpline KeySpline
        {
            get => (KeySpline)GetValue(KeySplineProperty);
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                SetValue(KeySplineProperty, value);
            }
        }

        public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register(nameof(KeySpline), typeof(KeySpline), typeof(SplinePathFigureCollectionKeyFrame), new PropertyMetadata(default(KeySpline)));

        #endregion

        #region Constructors 

        public SplinePathFigureCollectionKeyFrame()
            : base() { }
       
        public SplinePathFigureCollectionKeyFrame(PathFigureCollection value)
            : base(value) { }

        public SplinePathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime)
            : base(value, keyTime) { }

        public SplinePathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime, KeySpline keySpline)
            : base(value, keyTime)
        {
            KeySpline = keySpline ?? throw new ArgumentNullException(nameof(keySpline));
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new SplinePathFigureCollectionKeyFrame();
        }

        #endregion

        #region PathFigureCollectionKeyFrame 

        protected override PathFigureCollection InterpolateValueCore(PathFigureCollection baseValue, double keyFrameProgress)
        {
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolatePathFigureCollection(baseValue, Value, splineProgress);
            }
        }

        #endregion
        
    }
}
