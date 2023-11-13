using MS.Internal;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using MS.Internal.PresentationCore;
using MS.Internal.PresentationCore2;

namespace System.Windows.Media.Animation
{


    /// 

    /// This class is used as part of a ByteKeyFrameCollection in
    /// conjunction with a KeyFrameByteAnimation to animate a
    /// Byte property value along a set of key frames.
    /// 
    /// This ByteKeyFrame interpolates between the Byte Value of
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineByteKeyFrame : ByteKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplineByteKeyFrame. 
        /// 

        public SplineByteKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineByteKeyFrame.
        /// 

        public SplineByteKeyFrame(Byte value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineByteKeyFrame.
        /// 

        public SplineByteKeyFrame(Byte value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineByteKeyFrame. 
        /// 

        public SplineByteKeyFrame(Byte value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplineByteKeyFrame();
        }

        #endregion

        #region ByteKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Byte InterpolateValueCore(Byte baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateByte(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineByteKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a ColorKeyFrameCollection in
    /// conjunction with a KeyFrameColorAnimation to animate a 
    /// Color property value along a set of key frames.
    ///
    /// This ColorKeyFrame interpolates between the Color Value of
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineColorKeyFrame : ColorKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplineColorKeyFrame.
        /// 

        public SplineColorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineColorKeyFrame.
        /// 

        public SplineColorKeyFrame(Color value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineColorKeyFrame.
        /// 

        public SplineColorKeyFrame(Color value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineColorKeyFrame.
        /// 

        public SplineColorKeyFrame(Color value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineColorKeyFrame();
        }

        #endregion

        #region ColorKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Color InterpolateValueCore(Color baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateColor(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineColorKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame. 
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a DecimalKeyFrameCollection in 
    /// conjunction with a KeyFrameDecimalAnimation to animate a 
    /// Decimal property value along a set of key frames.
    /// 
    /// This DecimalKeyFrame interpolates between the Decimal Value of
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplineDecimalKeyFrame : DecimalKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineDecimalKeyFrame. 
        /// 

        public SplineDecimalKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineDecimalKeyFrame.
        /// 

        public SplineDecimalKeyFrame(Decimal value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineDecimalKeyFrame.
        /// 

        public SplineDecimalKeyFrame(Decimal value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineDecimalKeyFrame. 
        /// 

        public SplineDecimalKeyFrame(Decimal value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineDecimalKeyFrame();
        }

        #endregion

        #region DecimalKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Decimal InterpolateValueCore(Decimal baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateDecimal(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineDecimalKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame. 
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a DoubleKeyFrameCollection in 
    /// conjunction with a KeyFrameDoubleAnimation to animate a
    /// Double property value along a set of key frames. 
    /// 
    /// This DoubleKeyFrame interpolates between the Double Value of
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineDoubleKeyFrame : DoubleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineDoubleKeyFrame. 
        /// 

        public SplineDoubleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineDoubleKeyFrame. 
        /// 

        public SplineDoubleKeyFrame(Double value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineDoubleKeyFrame. 
        /// 

        public SplineDoubleKeyFrame(Double value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineDoubleKeyFrame. 
        /// 

        public SplineDoubleKeyFrame(Double value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplineDoubleKeyFrame();
        }

        #endregion

        #region DoubleKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Double InterpolateValueCore(Double baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateDouble(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineDoubleKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a Int16KeyFrameCollection in
    /// conjunction with a KeyFrameInt16Animation to animate a
    /// Int16 property value along a set of key frames. 
    ///
    /// This Int16KeyFrame interpolates between the Int16 Value of 
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineInt16KeyFrame : Int16KeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplineInt16KeyFrame.
        /// 

        public SplineInt16KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineInt16KeyFrame. 
        /// 

        public SplineInt16KeyFrame(Int16 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineInt16KeyFrame. 
        /// 

        public SplineInt16KeyFrame(Int16 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineInt16KeyFrame.
        /// 

        public SplineInt16KeyFrame(Int16 value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineInt16KeyFrame();
        }

        #endregion

        #region Int16KeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int16 InterpolateValueCore(Int16 baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateInt16(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineInt16KeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a Int32KeyFrameCollection in 
    /// conjunction with a KeyFrameInt32Animation to animate a
    /// Int32 property value along a set of key frames.
    ///
    /// This Int32KeyFrame interpolates between the Int32 Value of 
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplineInt32KeyFrame : Int32KeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineInt32KeyFrame.
        /// 

        public SplineInt32KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineInt32KeyFrame.
        /// 

        public SplineInt32KeyFrame(Int32 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineInt32KeyFrame.
        /// 

        public SplineInt32KeyFrame(Int32 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineInt32KeyFrame.
        /// 

        public SplineInt32KeyFrame(Int32 value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineInt32KeyFrame();
        }

        #endregion

        #region Int32KeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int32 InterpolateValueCore(Int32 baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateInt32(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineInt32KeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a Int64KeyFrameCollection in 
    /// conjunction with a KeyFrameInt64Animation to animate a
    /// Int64 property value along a set of key frames. 
    ///
    /// This Int64KeyFrame interpolates between the Int64 Value of
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplineInt64KeyFrame : Int64KeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineInt64KeyFrame.
        /// 

        public SplineInt64KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineInt64KeyFrame. 
        /// 

        public SplineInt64KeyFrame(Int64 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineInt64KeyFrame. 
        /// 

        public SplineInt64KeyFrame(Int64 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineInt64KeyFrame.
        /// 

        public SplineInt64KeyFrame(Int64 value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplineInt64KeyFrame();
        }

        #endregion

        #region Int64KeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int64 InterpolateValueCore(Int64 baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateInt64(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineInt64KeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a PointKeyFrameCollection in
    /// conjunction with a KeyFramePointAnimation to animate a 
    /// Point property value along a set of key frames. 
    ///
    /// This PointKeyFrame interpolates between the Point Value of 
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplinePointKeyFrame : PointKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplinePointKeyFrame.
        /// 

        public SplinePointKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplinePointKeyFrame. 
        /// 

        public SplinePointKeyFrame(Point value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplinePointKeyFrame. 
        /// 

        public SplinePointKeyFrame(Point value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplinePointKeyFrame.
        /// 

        public SplinePointKeyFrame(Point value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplinePointKeyFrame();
        }

        #endregion

        #region PointKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Point InterpolateValueCore(Point baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolatePoint(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplinePointKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame. 
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a Point3DKeyFrameCollection in
    /// conjunction with a KeyFramePoint3DAnimation to animate a 
    /// Point3D property value along a set of key frames.
    /// 
    /// This Point3DKeyFrame interpolates between the Point3D Value of 
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplinePoint3DKeyFrame : Point3DKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplinePoint3DKeyFrame. 
        /// 

        public SplinePoint3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplinePoint3DKeyFrame.
        /// 

        public SplinePoint3DKeyFrame(Point3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplinePoint3DKeyFrame.
        /// 

        public SplinePoint3DKeyFrame(Point3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplinePoint3DKeyFrame. 
        /// 

        public SplinePoint3DKeyFrame(Point3D value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplinePoint3DKeyFrame();
        }

        #endregion

        #region Point3DKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Point3D InterpolateValueCore(Point3D baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolatePoint3D(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplinePoint3DKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame. 
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion 
    }



    /// 

    /// This class is used as part of a QuaternionKeyFrameCollection in
    /// conjunction with a KeyFrameQuaternionAnimation to animate a
    /// Quaternion property value along a set of key frames.
    /// 
    /// This QuaternionKeyFrame interpolates between the Quaternion Value of
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineQuaternionKeyFrame : QuaternionKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplineQuaternionKeyFrame. 
        /// 

        public SplineQuaternionKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineQuaternionKeyFrame.
        /// 

        public SplineQuaternionKeyFrame(Quaternion value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineQuaternionKeyFrame.
        /// 

        public SplineQuaternionKeyFrame(Quaternion value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineQuaternionKeyFrame. 
        /// 

        public SplineQuaternionKeyFrame(Quaternion value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplineQuaternionKeyFrame();
        }

        #endregion

        #region QuaternionKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Quaternion InterpolateValueCore(Quaternion baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                throw new NotImplementedException();
                //return AnimatedTypeHelpers.InterpolateQuaternion(baseValue, Value, splineProgress, UseShortestPath);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineQuaternionKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a Rotation3DKeyFrameCollection in
    /// conjunction with a KeyFrameRotation3DAnimation to animate a 
    /// Rotation3D property value along a set of key frames.
    ///
    /// This Rotation3DKeyFrame interpolates between the Rotation3D Value of
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineRotation3DKeyFrame : Rotation3DKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplineRotation3DKeyFrame.
        /// 

        public SplineRotation3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineRotation3DKeyFrame.
        /// 

        public SplineRotation3DKeyFrame(Rotation3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineRotation3DKeyFrame.
        /// 

        public SplineRotation3DKeyFrame(Rotation3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineRotation3DKeyFrame.
        /// 

        public SplineRotation3DKeyFrame(Rotation3D value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineRotation3DKeyFrame();
        }

        #endregion

        #region Rotation3DKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Rotation3D InterpolateValueCore(Rotation3D baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                throw new NotImplementedException();
                //return AnimatedTypeHelpers.InterpolateRotation3D(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineRotation3DKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame. 
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a RectKeyFrameCollection in 
    /// conjunction with a KeyFrameRectAnimation to animate a 
    /// Rect property value along a set of key frames.
    /// 
    /// This RectKeyFrame interpolates between the Rect Value of
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplineRectKeyFrame : RectKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineRectKeyFrame. 
        /// 

        public SplineRectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineRectKeyFrame.
        /// 

        public SplineRectKeyFrame(Rect value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineRectKeyFrame.
        /// 

        public SplineRectKeyFrame(Rect value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineRectKeyFrame. 
        /// 

        public SplineRectKeyFrame(Rect value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineRectKeyFrame();
        }

        #endregion

        #region RectKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Rect InterpolateValueCore(Rect baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateRect(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineRectKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame. 
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a SingleKeyFrameCollection in 
    /// conjunction with a KeyFrameSingleAnimation to animate a
    /// Single property value along a set of key frames. 
    /// 
    /// This SingleKeyFrame interpolates between the Single Value of
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineSingleKeyFrame : SingleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineSingleKeyFrame. 
        /// 

        public SplineSingleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineSingleKeyFrame. 
        /// 

        public SplineSingleKeyFrame(Single value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineSingleKeyFrame. 
        /// 

        public SplineSingleKeyFrame(Single value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineSingleKeyFrame. 
        /// 

        public SplineSingleKeyFrame(Single value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplineSingleKeyFrame();
        }

        #endregion

        #region SingleKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Single InterpolateValueCore(Single baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateSingle(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineSingleKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a SizeKeyFrameCollection in
    /// conjunction with a KeyFrameSizeAnimation to animate a
    /// Size property value along a set of key frames. 
    ///
    /// This SizeKeyFrame interpolates between the Size Value of 
    /// the previous key frame and its own Value to produce its output value. 
    /// 

    public partial class SplineSizeKeyFrame : SizeKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new SplineSizeKeyFrame.
        /// 

        public SplineSizeKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineSizeKeyFrame. 
        /// 

        public SplineSizeKeyFrame(Size value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineSizeKeyFrame. 
        /// 

        public SplineSizeKeyFrame(Size value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineSizeKeyFrame.
        /// 

        public SplineSizeKeyFrame(Size value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineSizeKeyFrame();
        }

        #endregion

        #region SizeKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Size InterpolateValueCore(Size baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateSize(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineSizeKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a VectorKeyFrameCollection in 
    /// conjunction with a KeyFrameVectorAnimation to animate a
    /// Vector property value along a set of key frames.
    ///
    /// This VectorKeyFrame interpolates between the Vector Value of 
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplineVectorKeyFrame : VectorKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineVectorKeyFrame.
        /// 

        public SplineVectorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineVectorKeyFrame.
        /// 

        public SplineVectorKeyFrame(Vector value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineVectorKeyFrame.
        /// 

        public SplineVectorKeyFrame(Vector value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineVectorKeyFrame.
        /// 

        public SplineVectorKeyFrame(Vector value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new SplineVectorKeyFrame();
        }

        #endregion

        #region VectorKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Vector InterpolateValueCore(Vector baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateVector(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineVectorKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }



    /// 

    /// This class is used as part of a Vector3DKeyFrameCollection in 
    /// conjunction with a KeyFrameVector3DAnimation to animate a
    /// Vector3D property value along a set of key frames. 
    ///
    /// This Vector3DKeyFrame interpolates between the Vector3D Value of
    /// the previous key frame and its own Value to produce its output value.
    /// 

    public partial class SplineVector3DKeyFrame : Vector3DKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SplineVector3DKeyFrame.
        /// 

        public SplineVector3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SplineVector3DKeyFrame. 
        /// 

        public SplineVector3DKeyFrame(Vector3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new SplineVector3DKeyFrame. 
        /// 

        public SplineVector3DKeyFrame(Vector3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new SplineVector3DKeyFrame.
        /// 

        public SplineVector3DKeyFrame(Vector3D value, KeyTime keyTime, KeySpline keySpline)
            : this()
        {
            if (keySpline == null)
            {
                throw new ArgumentNullException("keySpline");
            }

            Value = value;
            KeyTime = keyTime;
            KeySpline = keySpline;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new SplineVector3DKeyFrame();
        }

        #endregion

        #region Vector3DKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Vector3D InterpolateValueCore(Vector3D baseValue, double keyFrameProgress)
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
                double splineProgress = KeySpline.GetSplineProgress(keyFrameProgress);

                return AnimatedTypeHelpers.InterpolateVector3D(baseValue, Value, splineProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// KeySpline Property 
        /// 

        public static readonly DependencyProperty KeySplineProperty =
            DependencyProperty.Register(
                "KeySpline",
                typeof(KeySpline),
                typeof(SplineVector3DKeyFrame),
                new PropertyMetadata(new KeySpline()));

        /// 

        /// The KeySpline defines the way that progress will be altered for this 
        /// key frame.
        /// 

        public KeySpline KeySpline
        {
            get
            {
                return (KeySpline)GetValue(KeySplineProperty);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                SetValue(KeySplineProperty, value);
            }
        }

        #endregion
    }


}
