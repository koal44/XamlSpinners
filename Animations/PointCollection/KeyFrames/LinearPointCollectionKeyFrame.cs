using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public partial class LinearPointCollectionKeyFrame : PointCollectionKeyFrame
    {
        #region Constructors

        public LinearPointCollectionKeyFrame() : base() { }

        public LinearPointCollectionKeyFrame(PointCollection value) : base(value) { }

        public LinearPointCollectionKeyFrame(PointCollection value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        #region Freezable 

        protected override Freezable CreateInstanceCore()
        {
            return new LinearPointCollectionKeyFrame();
        }

        #endregion

        #region PointCollectionKeyFrame 

        protected override PointCollection InterpolateValueCore(PointCollection baseValue, double keyFrameProgress)
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
                return AnimatedTypeHelpers.InterpolatePointCollection(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

    }
}
