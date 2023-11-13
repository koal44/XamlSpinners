using System.Windows.Media.Animation;

namespace System.Windows.Media.Animation2
{
    public abstract class VectorKeyFrame2 : Freezable, IKeyFrame
    {
        protected VectorKeyFrame2()
        {
        }

        protected VectorKeyFrame2(Vector value)
        {
        }

        protected VectorKeyFrame2(Vector value, KeyTime keyTime)
        {
        }

        public KeyTime KeyTime
        {
            get
            {
                throw null;
            }
            set
            {
            }
        }

        object IKeyFrame.Value
        {
            get
            {
                throw null;
            }
            set
            {
            }
        }

        public Vector Value
        {
            get
            {
                throw null;
            }
            set
            {
            }
        }


        public Vector InterpolateValue(Vector baseValue, double keyFrameProgress)
        {
            throw null;
        }

        protected abstract Vector InterpolateValueCore(Vector baseValue, double keyFrameProgress);

        public static readonly DependencyProperty KeyTimeProperty;

        public static readonly DependencyProperty ValueProperty;
    }
}
