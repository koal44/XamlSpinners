using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public abstract class PathFigureCollectionKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors

        protected PathFigureCollectionKeyFrame() : base() { }

        protected PathFigureCollectionKeyFrame(PathFigureCollection value) : this()
        {
            Value = value;
        }

        protected PathFigureCollectionKeyFrame(PathFigureCollection value, KeyTime keyTime) : this()
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

        public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register(nameof(KeyTime), typeof(KeyTime), typeof(PathFigureCollectionKeyFrame), new PropertyMetadata(KeyTime.Uniform));


        public PathFigureCollection Value
        {
            get => (PathFigureCollection)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(PathFigureCollection), typeof(PathFigureCollectionKeyFrame), new PropertyMetadata());

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
                Value = (PathFigureCollection)value;
            }
        }

        #endregion

        #region Public Methods

        public PathFigureCollection InterpolateValue(PathFigureCollection baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(keyFrameProgress));
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods

        protected abstract PathFigureCollection InterpolateValueCore(PathFigureCollection baseValue, double keyFrameProgress);

        #endregion
        
    }
}
