using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public abstract class PointCollectionKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors

        protected PointCollectionKeyFrame() : base() { }

        protected PointCollectionKeyFrame(PointCollection value) : this()
        {
            Value = value;
        }

        protected PointCollectionKeyFrame(PointCollection value, KeyTime keyTime) : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region Dependency Properties


        public KeyTime KeyTime
        {
            get => (KeyTime)GetValue(KeyTimeProperty);
            set => SetValue(KeyTimeProperty, value);
        }

        public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register(nameof(KeyTime), typeof(KeyTime), typeof(PointCollectionKeyFrame), new PropertyMetadata(KeyTime.Uniform));


        public PointCollection Value
        {
            get => (PointCollection)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(PointCollection), typeof(PointCollectionKeyFrame), new PropertyMetadata());

        #endregion

        #region IKeyFrame

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (PointCollection)value;
            }
        }

        #endregion

        #region Public Methods

        public PointCollection InterpolateValue(PointCollection baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(keyFrameProgress));
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods

        protected abstract PointCollection InterpolateValueCore(PointCollection baseValue, double keyFrameProgress);

        #endregion
        
    }
}
