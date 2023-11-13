using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{

    /// Animates the value of a PathFigureCollection property using linear interpolation
    /// between two values.  The values are determined by the combination of 
    /// From, To, or By values that are set on the animation. 
    public class PathFigureCollectionAnimation : PathFigureCollectionAnimationBase
    {
        #region Data

        // Used when the user has specified From, To, and/or By. 
        private PathFigureCollection[]? _keyValues;

        private AnimationType _animationType;
        private bool _isAnimationFunctionValid = false;

        #endregion

        #region Dependency Properties

        // Workaround for original MS internal's PropertyChanged strategy: 
        // Invalidate the animation state in the setter to preclude the need for re-triggering 
        // state updates in the DependencyObject.

        public PathFigureCollection From
        {
            get => (PathFigureCollection)GetValue(FromProperty);
            set
            {
                if (ShouldNormalize && value is not null)
                {
                    value = Utils.NormalizePathFigureCollection(value);
                }
                _isAnimationFunctionValid = false;
                SetValue(FromProperty, value);
            }
        }

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(nameof(From), typeof(PathFigureCollection), typeof(PathFigureCollectionAnimation), new PropertyMetadata(null), OnValidateFromToOrByValue);


        public PathFigureCollection? To
        {
            get => (PathFigureCollection)GetValue(ToProperty);
            set
            {
                if (ShouldNormalize && value is not null)
                {
                    value = Utils.NormalizePathFigureCollection(value);
                }
                _isAnimationFunctionValid = false;
                SetValue(ToProperty, value);
            }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(nameof(To), typeof(PathFigureCollection), typeof(PathFigureCollectionAnimation), new PropertyMetadata(null), OnValidateFromToOrByValue);


        public PathFigureCollection? By
        {
            get => (PathFigureCollection)GetValue(ByProperty);
            set
            {
                if (ShouldNormalize && value is not null)
                {
                    value = Utils.NormalizePathFigureCollection(value);
                }
                _isAnimationFunctionValid = false;
                SetValue(ByProperty, value);
            }
        }


        public static readonly DependencyProperty ByProperty = DependencyProperty.Register(nameof(By), typeof(PathFigureCollection), typeof(PathFigureCollectionAnimation), new PropertyMetadata(null), OnValidateFromToOrByValue);


        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(PathFigureCollectionAnimation));


        private static bool OnValidateFromToOrByValue(object value)
        {
            // let From/To/By be nullable
            if (value == null) return true; 
            if (value is not PathFigureCollection valueCollection) return false;

            return AnimatedTypeHelpers.IsValidAnimationValuePathFigureCollection(valueCollection);
        }


        public bool ShouldNormalize
        {
            get => (bool)GetValue(ShouldNormalizeProperty);
            set => SetValue(ShouldNormalizeProperty, value);
        }

        public static readonly DependencyProperty ShouldNormalizeProperty = DependencyProperty.Register(nameof(ShouldNormalize), typeof(bool), typeof(PathFigureCollectionAnimation), new PropertyMetadata(true));


        #endregion

        #region Inherited Dependency Property Accessors

        /// If this property is set to true the animation will add its value to
        /// the base value instead of replacing it entirely. 
        public bool IsAdditive
        {
            get => (bool)GetValue(IsAdditiveProperty);
            set => SetValue(IsAdditiveProperty, value);
        }


        /// It this property is set to true, the animation will accumulate its
        /// value over repeats.  For instance if you have a From value of 0.0 and 
        /// a To value of 1.0, the animation return values from 1.0 to 2.0 over
        /// the second reteat cycle, and 2.0 to 3.0 over the third, etc. 
        public bool IsCumulative
        {
            get => (bool)GetValue(IsCumulativeProperty);
            set => SetValue(IsCumulativeProperty, value);
        }

        #endregion

        #region Constructors 

        public PathFigureCollectionAnimation()
            : base()
        {

        }


        public PathFigureCollectionAnimation(PathFigureCollection toValue, Duration duration)
            : this()
        {
            To = toValue;
            Duration = duration;
        }


        public PathFigureCollectionAnimation(PathFigureCollection toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }


        public PathFigureCollectionAnimation(PathFigureCollection fromValue, PathFigureCollection toValue, Duration duration)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }


        public PathFigureCollectionAnimation(PathFigureCollection fromValue, PathFigureCollection toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        #endregion

        #region Freezable

        // Note that we don't override the Clone virtuals (CloneCore, CloneCurrentValueCore,
        // GetAsFrozenCore, and GetCurrentValueAsFrozenCore) even though this class has state 
        // not stored in a DP. 
        //
        // We don't need to clone _animationType and _keyValues because they are the the cached 
        // results of animation function validation, which can be recomputed.  The other remaining
        // field, isAnimationFunctionValid, defaults to false, which causes this recomputation to happen.

        public new PathFigureCollectionAnimation Clone() => (PathFigureCollectionAnimation)base.Clone();

        protected override Freezable CreateInstanceCore() => new PathFigureCollectionAnimation();

        #endregion

        #region Methods 

        /// 

        /// Calculates the value this animation believes should be the current value for the property.
        /// 

        /// 
        /// This value is the suggested origin value provided to the animation 
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain 
        /// this value will be the snapshot value if one is available or the 
        /// base property value if it is not; otherise this value will be the
        /// value returned by the previous animation in the chain with an 
        /// animationClock that is not Stopped.
        /// 
        /// 
        /// This value is the suggested destination value provided to the animation 
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is 
        /// in the first composition layer of animations on a property; 
        /// otherwise this value will be the output value from the previous
        /// composition layer of animations for the property. 
        /// 
        /// 
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its 
        /// output value.
        ///  
        ///  
        /// The value this animation believes should be the current value for the property.
        ///  
        protected override PathFigureCollection GetCurrentValueCore(PathFigureCollection defaultOriginValue, PathFigureCollection defaultDestinationValue, AnimationClock animationClock)
        {
            Debug.Assert(animationClock.CurrentState != ClockState.Stopped);

            if (!_isAnimationFunctionValid)
            {
                ValidateAnimationFunction();
            }

            Debug.Assert(_keyValues is not null);

            double progress = animationClock.CurrentProgress!.Value;

            var easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                progress = easingFunction.Ease(progress);
            }

            PathFigureCollection? from = null;
            PathFigureCollection? to = null;
            PathFigureCollection? accumulated = null;
            PathFigureCollection? foundation = null;

            // need to validate the default origin and destination values if
            // the animation uses them as the from, to, or foundation values 
            bool validateOrigin = false;
            bool validateDestination = false;

            switch (_animationType)
            {
                case AnimationType.Automatic:
                    from = defaultOriginValue;
                    to = defaultDestinationValue;

                    validateOrigin = true;
                    validateDestination = true;

                    //if (from.Count != to.Count)
                    //{
                    //    from = AnimatedTypeHelpers.NormalizePathFigureCollection(from);
                    //    to = AnimatedTypeHelpers.NormalizePathFigureCollection(to);
                    //}

                    break;
                case AnimationType.From:
                    from = _keyValues[0];
                    to = defaultDestinationValue;

                    validateDestination = true;



                    break;
                case AnimationType.To:
                    from = defaultOriginValue;
                    to = _keyValues[0];

                    validateOrigin = true;

                    break;
                case AnimationType.By:
                    // According to the SMIL specification, a By animation is 
                    // always additive.  But we don't force this so that a 
                    // user can re-use a By animation and have it replace the
                    // animations that precede it in the list without having 
                    // to manually set the From value to the base value.

                    to = _keyValues[0];
                    foundation = defaultOriginValue;

                    validateOrigin = true;

                    break;
                case AnimationType.FromTo:
                    from = _keyValues[0];
                    to = _keyValues[1];

                    if (IsAdditive)
                    {
                        foundation = defaultOriginValue;
                        validateOrigin = true;
                    }

                    break;
                case AnimationType.FromBy:
                    from = _keyValues[0];
                    to = AnimatedTypeHelpers.AddPathFigureCollection(_keyValues[0], _keyValues[1]);

                    if (IsAdditive)
                    {
                        foundation = defaultOriginValue;
                        validateOrigin = true;
                    }

                    break;
                default:
                    Debug.Fail("Unknown animation type.");
                    break;
            }

            if (validateOrigin
                && !AnimatedTypeHelpers.IsValidAnimationValuePathFigureCollection(defaultOriginValue))
            {
                throw new InvalidOperationException("origin");
            }

            if (validateDestination
                && !AnimatedTypeHelpers.IsValidAnimationValuePathFigureCollection(defaultDestinationValue))
            {
                throw new InvalidOperationException("destination");
            }


            if (IsCumulative)
            {
                int currentRepeat = (int)(animationClock.CurrentIteration! - 1);

                if (currentRepeat > 0.0)
                {
                    var accumulator = from is null
                        ? to // AnimationType.By
                        : AnimatedTypeHelpers.SubtractPathFigureCollection(to, from);
                    accumulated = AnimatedTypeHelpers.ScalePathFigureCollection(accumulator, currentRepeat);
                }
            }

            // return foundation + accumulated + from + ((to - from) * progress)

            from ??= AnimatedTypeHelpers.GetZeroValuePathFigureCollection(to);

            var result = AnimatedTypeHelpers.InterpolatePathFigureCollection(from, to, progress);
            if (accumulated is not null)
                result = AnimatedTypeHelpers.AddPathFigureCollection(accumulated, result);
            if (foundation is not null)
                result = AnimatedTypeHelpers.AddPathFigureCollection(foundation, result);

            // print from and result for debugging purposes
            //Debug.WriteLine($"From: \n{Utils.PrintPathFigureCollection(from)}");
            //Debug.WriteLine("");
            //Debug.WriteLine($"Result: \n{Utils.PrintPathFigureCollection(result)}");

            return result;
        }


        private void ValidateAnimationFunction()
        {
            (_animationType, _keyValues) = (From, To, By) switch
            {
                (not null, not null, _) => (AnimationType.FromTo, new PathFigureCollection[2] { From, To }),
                (not null, _, not null) => (AnimationType.FromBy, new PathFigureCollection[2] { From, By }),
                (not null, _, _) => (AnimationType.From, new PathFigureCollection[1] { From }),
                (_, not null, _) => (AnimationType.To, new PathFigureCollection[1] { To }),
                (_, _, not null) => (AnimationType.By, new PathFigureCollection[1] { By }),
                _ => (AnimationType.Automatic, null)
            };

            _isAnimationFunctionValid = true;
        }

        #endregion

    }
} 

