using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{

    /// Animates the value of a PointCollection property using linear interpolation
    /// between two values.  The values are determined by the combination of 
    /// From, To, or By values that are set on the animation. 
    public class PointCollectionAnimation : PointCollectionAnimationBase
    {
        #region Data

        // Used when the user has specified From, To, and/or By. 
        private PointCollection[]? _keyValues;

        private AnimationType _animationType;
        private bool _isAnimationFunctionValid = false;

        #endregion

        #region Dependency Properties

        // Workaround for original MS internal's PropertyChanged strategy: 
        // Invalidate the animation state in the setter to preclude the need for re-triggering 
        // state updates in the DependencyObject.

        public PointCollection From
        {
            get => (PointCollection)GetValue(FromProperty);
            set
            {
                _isAnimationFunctionValid = false;
                SetValue(FromProperty, value);
            }
        }

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(nameof(From), typeof(PointCollection), typeof(PointCollectionAnimation), new PropertyMetadata(null), OnValidateFromToOrByValue);


        public PointCollection? To
        {
            get => (PointCollection)GetValue(ToProperty);
            set
            {
                _isAnimationFunctionValid = false;
                SetValue(ToProperty, value);
            }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(nameof(To), typeof(PointCollection), typeof(PointCollectionAnimation), new PropertyMetadata(null), OnValidateFromToOrByValue);


        public PointCollection? By
        {
            get => (PointCollection)GetValue(ByProperty);
            set
            {
                _isAnimationFunctionValid = false;
                SetValue(ByProperty, value);
            }
        }

        public static readonly DependencyProperty ByProperty = DependencyProperty.Register(nameof(By), typeof(PointCollection), typeof(PointCollectionAnimation), new PropertyMetadata(null), OnValidateFromToOrByValue);


        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(PointCollectionAnimation));


        private static bool OnValidateFromToOrByValue(object value)
        {
            // let From/To/By be nullable
            if (value == null) return true; 
            if (value is not PointCollection valueCollection) return false;

            return AnimatedTypeHelpers.IsValidAnimationValuePointCollection(valueCollection);
        }


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

        public PointCollectionAnimation() : base() { }


        public PointCollectionAnimation(PointCollection toValue, Duration duration)
            : this()
        {
            To = toValue;
            Duration = duration;
        }


        public PointCollectionAnimation(PointCollection toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }


        public PointCollectionAnimation(PointCollection fromValue, PointCollection toValue, Duration duration)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }


        public PointCollectionAnimation(PointCollection fromValue, PointCollection toValue, Duration duration, FillBehavior fillBehavior)
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
        public new PointCollectionAnimation Clone() => (PointCollectionAnimation)base.Clone();

        protected override Freezable CreateInstanceCore() => new PointCollectionAnimation();

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
        protected override PointCollection GetCurrentValueCore(PointCollection defaultOriginValue, PointCollection defaultDestinationValue, AnimationClock animationClock)
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

            PointCollection? from = null;
            PointCollection? to = null;
            PointCollection? accumulated = null;
            PointCollection? foundation = null;

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
                    to = AnimatedTypeHelpers.AddPointCollection(_keyValues[0], _keyValues[1]);

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
                && !AnimatedTypeHelpers.IsValidAnimationValuePointCollection(defaultOriginValue))
            {
                throw new InvalidOperationException("origin");
            }

            if (validateDestination
                && !AnimatedTypeHelpers.IsValidAnimationValuePointCollection(defaultDestinationValue))
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
                        : AnimatedTypeHelpers.SubtractPointCollection(to, from);
                    accumulated = AnimatedTypeHelpers.ScalePointCollection(accumulator, currentRepeat);
                }
            }

            // return foundation + accumulated + from + ((to - from) * progress)

            from ??= AnimatedTypeHelpers.GetZeroValuePointCollection(to);

            var result = AnimatedTypeHelpers.InterpolatePointCollection(from, to, progress);
            if (accumulated is not null)
                result = AnimatedTypeHelpers.AddPointCollection(accumulated, result);
            if (foundation is not null)
                result = AnimatedTypeHelpers.AddPointCollection(foundation, result);

            return result;
        }


        private void ValidateAnimationFunction()
        {
            (_animationType, _keyValues) = (From, To, By) switch
            {
                (not null, not null, _       ) => (AnimationType.FromTo, new PointCollection[2] { From, To }),
                (not null, null,     not null) => (AnimationType.FromBy, new PointCollection[2] { From, By }),
                (not null, null,     null    ) => (AnimationType.From, new PointCollection[1] { From }),
                (null,     not null, _       ) => (AnimationType.To, new PointCollection[1] { To }),
                (null,     null,     not null) => (AnimationType.By, new PointCollection[1] { By }),
                _ => (AnimationType.Automatic, null)
            };

            _isAnimationFunctionValid = true;
        }

        #endregion

    }
} 

