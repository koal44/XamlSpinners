using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public partial class LinearDoubleCollectionKeyFrame : DoubleCollectionKeyFrame
    {
        #region Constructors

        public LinearDoubleCollectionKeyFrame() : base() { }

        public LinearDoubleCollectionKeyFrame(DoubleCollection value) : base(value) { }

        public LinearDoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        #region Freezable 

        protected override Freezable CreateInstanceCore()
        {
            return new LinearDoubleCollectionKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateDoubleCollection(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

    }
}
