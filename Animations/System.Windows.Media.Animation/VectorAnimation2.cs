using MS.Internal.PresentationCore2;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace System.Windows.Media.Animation2
{

    /// 

    /// Animates the value of a Vector property using linear interpolation
    /// between two values.  The values are determined by the combination of 
    /// From, To, or By values that are set on the animation. 
    /// 

    public class VectorAnimation2 :
        VectorAnimationBase2
    {
        #region Data

        /// 

        /// This is used if the user has specified From, To, and/or By values. 
        /// 

        private Vector[] _keyValues;

        private AnimationType _animationType;
        private bool _isAnimationFunctionValid;

        #endregion

        #region Constructors 

        /// 

        /// Static ctor for VectorAnimation establishes 
        /// dependency properties, using as much shared data as possible.
        /// 

        static VectorAnimation2()
        {
            Type typeofProp = typeof(Vector?);
            Type typeofThis = typeof(VectorAnimation2);
            PropertyChangedCallback propCallback = new PropertyChangedCallback(AnimationFunction_Changed);
            ValidateValueCallback validateCallback = new ValidateValueCallback(ValidateFromToOrByValue);

            FromProperty = DependencyProperty.Register(
                "From",
                typeofProp,
                typeofThis,
                new PropertyMetadata((Vector?)null, propCallback),
                validateCallback);

            ToProperty = DependencyProperty.Register(
                "To",
                typeofProp,
                typeofThis,
                new PropertyMetadata((Vector?)null, propCallback),
                validateCallback);

            ByProperty = DependencyProperty.Register(
                "By",
                typeofProp,
                typeofThis,
                new PropertyMetadata((Vector?)null, propCallback),
                validateCallback);

            EasingFunctionProperty = DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeofThis);
        }


        /// 

        /// Creates a new VectorAnimation with all properties set to
        /// their default values. 
        /// 

        public VectorAnimation2()
            : base()
        {
        }

        /// 

        /// Creates a new VectorAnimation that will animate a
        /// Vector property from its base value to the value specified 
        /// by the "toValue" parameter of this constructor.
        /// 

        public VectorAnimation2(Vector toValue, Duration duration)
            : this()
        {
            To = toValue;
            Duration = duration;
        }

        /// 

        /// Creates a new VectorAnimation that will animate a 
        /// Vector property from its base value to the value specified 
        /// by the "toValue" parameter of this constructor.
        /// 

        public VectorAnimation2(Vector toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        /// 

        /// Creates a new VectorAnimation that will animate a
        /// Vector property from the "fromValue" parameter of this constructor
        /// to the "toValue" parameter.
        /// 

        public VectorAnimation2(Vector fromValue, Vector toValue, Duration duration)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }

        /// 

        /// Creates a new VectorAnimation that will animate a
        /// Vector property from the "fromValue" parameter of this constructor 
        /// to the "toValue" parameter. 
        /// 

        public VectorAnimation2(Vector fromValue, Vector toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        #endregion

        #region Freezable

        /// 

        /// Creates a copy of this VectorAnimation
        /// 

        /// The copy 
        public new VectorAnimation2 Clone()
        {
            return (VectorAnimation2)base.Clone();
        }

        // 
        // Note that we don't override the Clone virtuals (CloneCore, CloneCurrentValueCore,
        // GetAsFrozenCore, and GetCurrentValueAsFrozenCore) even though this class has state 
        // not stored in a DP. 
        //
        // We don't need to clone _animationType and _keyValues because they are the the cached 
        // results of animation function validation, which can be recomputed.  The other remaining
        // field, isAnimationFunctionValid, defaults to false, which causes this recomputation to happen.
        //

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new VectorAnimation2();
        }

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
        protected override Vector GetCurrentValueCore(Vector defaultOriginValue, Vector defaultDestinationValue, AnimationClock animationClock)
        {
            Debug.Assert(animationClock.CurrentState != ClockState.Stopped);

            if (!_isAnimationFunctionValid)
            {
                ValidateAnimationFunction();
            }

            double progress = animationClock.CurrentProgress.Value;

            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                progress = easingFunction.Ease(progress);
            }

            Vector from = new Vector();
            Vector to = new Vector();
            Vector accumulated = new Vector();
            Vector foundation = new Vector();

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
                    to = AnimatedTypeHelpers.AddVector(_keyValues[0], _keyValues[1]);

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
                && !AnimatedTypeHelpers.IsValidAnimationValueVector(defaultOriginValue))
            {
                throw new InvalidOperationException("origin");
            }

            if (validateDestination
                && !AnimatedTypeHelpers.IsValidAnimationValueVector(defaultDestinationValue))
            {
                throw new InvalidOperationException("destination");
            }


            if (IsCumulative)
            {
                double currentRepeat = (double)(animationClock.CurrentIteration - 1);

                if (currentRepeat > 0.0)
                {
                    Vector accumulator = AnimatedTypeHelpers.SubtractVector(to, from);

                    accumulated = AnimatedTypeHelpers.ScaleVector(accumulator, currentRepeat);
                }
            }

            // return foundation + accumulated + from + ((to - from) * progress) 

            return AnimatedTypeHelpers.AddVector(
                foundation,
                AnimatedTypeHelpers.AddVector(
                    accumulated,
                    AnimatedTypeHelpers.InterpolateVector(from, to, progress)));
        }

        private void ValidateAnimationFunction()
        {
            _animationType = AnimationType.Automatic;
            _keyValues = null;

            if (From.HasValue)
            {
                if (To.HasValue)
                {
                    _animationType = AnimationType.FromTo;
                    _keyValues = new Vector[2];
                    _keyValues[0] = From.Value;
                    _keyValues[1] = To.Value;
                }
                else if (By.HasValue)
                {
                    _animationType = AnimationType.FromBy;
                    _keyValues = new Vector[2];
                    _keyValues[0] = From.Value;
                    _keyValues[1] = By.Value;
                }
                else
                {
                    _animationType = AnimationType.From;
                    _keyValues = new Vector[1];
                    _keyValues[0] = From.Value;
                }
            }
            else if (To.HasValue)
            {
                _animationType = AnimationType.To;
                _keyValues = new Vector[1];
                _keyValues[0] = To.Value;
            }
            else if (By.HasValue)
            {
                _animationType = AnimationType.By;
                _keyValues = new Vector[1];
                _keyValues[0] = By.Value;
            }

            _isAnimationFunctionValid = true;
        }

        #endregion

        #region Properties 

        private static void AnimationFunction_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VectorAnimation2 a = (VectorAnimation2)d;

            a._isAnimationFunctionValid = false;

            // KOAL: what's this??
            //a.PropertyChanged(e.Property);
        }

        private static bool ValidateFromToOrByValue(object value)
        {
            Vector? typedValue = (Vector?)value;

            if (typedValue.HasValue)
            {
                return AnimatedTypeHelpers.IsValidAnimationValueVector(typedValue.Value);
            }
            else
            {
                return true;
            }
        }

        /// 

        /// FromProperty
        /// 

        public static readonly DependencyProperty FromProperty;

        /// 

        /// From
        /// 

        public Vector? From
        {
            get
            {
                return (Vector?)GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }

        /// 

        /// ToProperty 
        /// 

        public static readonly DependencyProperty ToProperty;

        /// 

        /// To
        /// 

        public Vector? To
        {
            get
            {
                return (Vector?)GetValue(ToProperty);
            }
            set
            {
                SetValue(ToProperty, value);
            }
        }

        /// 

        /// ByProperty 
        /// 

        public static readonly DependencyProperty ByProperty;

        /// 

        /// By
        /// 

        public Vector? By
        {
            get
            {
                return (Vector?)GetValue(ByProperty);
            }
            set
            {
                SetValue(ByProperty, value);
            }
        }


        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty;

        /// 

        /// EasingFunction
        /// 

        public IEasingFunction EasingFunction
        {
            get
            {
                return (IEasingFunction)GetValue(EasingFunctionProperty);
            }
            set
            {
                SetValue(EasingFunctionProperty, value);
            }
        }

        /// 

        /// If this property is set to true the animation will add its value to
        /// the base value instead of replacing it entirely. 
        /// 

        public bool IsAdditive
        {
            get
            {
                return (bool)GetValue(IsAdditiveProperty);
            }
            set
            {
                SetValue(IsAdditiveProperty, value);
            }
        }

        /// 

        /// It this property is set to true, the animation will accumulate its
        /// value over repeats.  For instance if you have a From value of 0.0 and 
        /// a To value of 1.0, the animation return values from 1.0 to 2.0 over
        /// the second reteat cycle, and 2.0 to 3.0 over the third, etc. 
        /// 

        public bool IsCumulative
        {
            get
            {
                return (bool)GetValue(IsCumulativeProperty);
            }
            set
            {
                SetValue(IsCumulativeProperty, value);
            }
        }

        #endregion
    }

    public enum AnimationType
    {
        Automatic,
        From,
        To,
        By,
        FromTo,
        FromBy
    }
} 

