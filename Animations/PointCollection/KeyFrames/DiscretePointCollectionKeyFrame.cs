using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public class DiscretePointCollectionKeyFrame : PointCollectionKeyFrame
    {
        #region Constructors 

        public DiscretePointCollectionKeyFrame() : base() { }

        public DiscretePointCollectionKeyFrame(PointCollection value) : base(value) { }

        public DiscretePointCollectionKeyFrame(PointCollection value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        #region Freezable 

        protected override Freezable CreateInstanceCore()
        {
            return new DiscretePointCollectionKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region PointCollectionKeyFrame 

        protected override PointCollection InterpolateValueCore(PointCollection baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }
}


