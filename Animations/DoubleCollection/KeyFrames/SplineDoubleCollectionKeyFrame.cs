using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public class SplineDoubleCollectionKeyFrame : DoubleCollectionKeyFrame
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

        public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register(nameof(KeySpline), typeof(KeySpline), typeof(SplineDoubleCollectionKeyFrame), new PropertyMetadata(default(KeySpline)));

        #endregion

        #region Constructors 

        public SplineDoubleCollectionKeyFrame()
            : base() { }
       
        public SplineDoubleCollectionKeyFrame(DoubleCollection value)
            : base(value) { }

        public SplineDoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime)
            : base(value, keyTime) { }

        public SplineDoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime, KeySpline keySpline)
            : base(value, keyTime)
        {
            KeySpline = keySpline ?? throw new ArgumentNullException(nameof(keySpline));
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new SplineDoubleCollectionKeyFrame();
        }

        #endregion

        #region DoubleCollectionKeyFrame 

        protected override DoubleCollection InterpolateValueCore(DoubleCollection baseValue, double keyFrameProgress)
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

                return AnimatedTypeHelpers.InterpolateDoubleCollection(baseValue, Value, splineProgress);
            }
        }

        #endregion
        
    }
}
