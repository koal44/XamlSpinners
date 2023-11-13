using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Animations
{
    public class DiscreteDoubleCollectionKeyFrame : DoubleCollectionKeyFrame
    {
        #region Constructors 

        public DiscreteDoubleCollectionKeyFrame() : base() { }

        public DiscreteDoubleCollectionKeyFrame(DoubleCollection value) : base(value) { }

        public DiscreteDoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        #region Freezable 

        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteDoubleCollectionKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region DoubleCollectionKeyFrame 

        protected override DoubleCollection InterpolateValueCore(DoubleCollection baseValue, double keyFrameProgress)
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


