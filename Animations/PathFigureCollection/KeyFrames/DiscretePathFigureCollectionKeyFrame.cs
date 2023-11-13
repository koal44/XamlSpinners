using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Animations
{
    public class DiscretePathFigureCollectionKeyFrame : PathFigureCollectionKeyFrame
    {
        #region Constructors 

        public DiscretePathFigureCollectionKeyFrame() : base() { }

        public DiscretePathFigureCollectionKeyFrame(PathFigureCollection value) : base(value) { }

        public DiscretePathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        #region Freezable 

        protected override Freezable CreateInstanceCore()
        {
            return new DiscretePathFigureCollectionKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region PathFigureCollectionKeyFrame 

        protected override PathFigureCollection InterpolateValueCore(PathFigureCollection baseValue, double keyFrameProgress)
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


