using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public partial class LinearPathFigureCollectionKeyFrame : PathFigureCollectionKeyFrame
    {
        #region Constructors

        public LinearPathFigureCollectionKeyFrame() : base() { }

        public LinearPathFigureCollectionKeyFrame(PathFigureCollection value) : base(value) { }

        public LinearPathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        #region Freezable 

        protected override Freezable CreateInstanceCore()
        {
            return new LinearPathFigureCollectionKeyFrame();
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
                return AnimatedTypeHelpers.InterpolatePathFigureCollection(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

    }
}
