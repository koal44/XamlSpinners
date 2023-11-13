using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public abstract class DoubleCollectionKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors

        protected DoubleCollectionKeyFrame() : base() { }

        protected DoubleCollectionKeyFrame(DoubleCollection value) : this()
        {
            Value = value;
        }

        protected DoubleCollectionKeyFrame(DoubleCollection value, KeyTime keyTime) : this()
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

        public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register(nameof(KeyTime), typeof(KeyTime), typeof(DoubleCollectionKeyFrame), new PropertyMetadata(KeyTime.Uniform));


        public DoubleCollection Value
        {
            get => (DoubleCollection)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(DoubleCollection), typeof(DoubleCollectionKeyFrame), new PropertyMetadata());

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
                Value = (DoubleCollection)value;
            }
        }

        #endregion

        #region Public Methods

        public DoubleCollection InterpolateValue(DoubleCollection baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(keyFrameProgress));
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods

        protected abstract DoubleCollection InterpolateValueCore(DoubleCollection baseValue, double keyFrameProgress);

        #endregion
        
    }
}
