﻿using MS.Internal;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using MS.Internal.PresentationCore;
using MS.Internal.PresentationCore2;
using System.Windows.Media.Animation;

namespace System.Windows.Media.Animation2
{


    /// 

    /// This class is used as part of a ByteKeyFrameCollection in
    /// conjunction with a KeyFrameByteAnimation to animate a
    /// Byte property value along a set of key frames.
    /// 
    /// This ByteKeyFrame interpolates the between the Byte Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingByteKeyFrame : ByteKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingByteKeyFrame. 
        /// 

        public EasingByteKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingByteKeyFrame.
        /// 

        public EasingByteKeyFrame(Byte value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingByteKeyFrame.
        /// 

        public EasingByteKeyFrame(Byte value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingByteKeyFrame. 
        /// 

        public EasingByteKeyFrame(Byte value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingByteKeyFrame();
        }

        #endregion

        #region ByteKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Byte InterpolateValueCore(Byte baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateByte(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingByteKeyFrame));

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

        #endregion 
    }


    /// 

    /// This class is used as part of a ColorKeyFrameCollection in 
    /// conjunction with a KeyFrameColorAnimation to animate a
    /// Color property value along a set of key frames.
    ///
    /// This ColorKeyFrame interpolates the between the Color Value of 
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingColorKeyFrame : ColorKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingColorKeyFrame.
        /// 

        public EasingColorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingColorKeyFrame.
        /// 

        public EasingColorKeyFrame(Color value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingColorKeyFrame.
        /// 

        public EasingColorKeyFrame(Color value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingColorKeyFrame.
        /// 

        public EasingColorKeyFrame(Color value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingColorKeyFrame();
        }

        #endregion

        #region ColorKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Color InterpolateValueCore(Color baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateColor(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingColorKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a DecimalKeyFrameCollection in
    /// conjunction with a KeyFrameDecimalAnimation to animate a 
    /// Decimal property value along a set of key frames.
    ///
    /// This DecimalKeyFrame interpolates the between the Decimal Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingDecimalKeyFrame : DecimalKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingDecimalKeyFrame.
        /// 

        public EasingDecimalKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingDecimalKeyFrame.
        /// 

        public EasingDecimalKeyFrame(Decimal value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingDecimalKeyFrame.
        /// 

        public EasingDecimalKeyFrame(Decimal value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingDecimalKeyFrame.
        /// 

        public EasingDecimalKeyFrame(Decimal value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingDecimalKeyFrame();
        }

        #endregion

        #region DecimalKeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Decimal InterpolateValueCore(Decimal baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateDecimal(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingDecimalKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a DoubleKeyFrameCollection in 
    /// conjunction with a KeyFrameDoubleAnimation to animate a
    /// Double property value along a set of key frames. 
    ///
    /// This DoubleKeyFrame interpolates the between the Double Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingDoubleKeyFrame : DoubleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingDoubleKeyFrame.
        /// 

        public EasingDoubleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingDoubleKeyFrame. 
        /// 

        public EasingDoubleKeyFrame(Double value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingDoubleKeyFrame. 
        /// 

        public EasingDoubleKeyFrame(Double value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingDoubleKeyFrame.
        /// 

        public EasingDoubleKeyFrame(Double value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingDoubleKeyFrame();
        }

        #endregion

        #region DoubleKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Double InterpolateValueCore(Double baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateDouble(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingDoubleKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a Int16KeyFrameCollection in 
    /// conjunction with a KeyFrameInt16Animation to animate a 
    /// Int16 property value along a set of key frames.
    /// 
    /// This Int16KeyFrame interpolates the between the Int16 Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingInt16KeyFrame : Int16KeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingInt16KeyFrame. 
        /// 

        public EasingInt16KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingInt16KeyFrame.
        /// 

        public EasingInt16KeyFrame(Int16 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingInt16KeyFrame.
        /// 

        public EasingInt16KeyFrame(Int16 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingInt16KeyFrame. 
        /// 

        public EasingInt16KeyFrame(Int16 value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingInt16KeyFrame();
        }

        #endregion

        #region Int16KeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int16 InterpolateValueCore(Int16 baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateInt16(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingInt16KeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a Int32KeyFrameCollection in
    /// conjunction with a KeyFrameInt32Animation to animate a 
    /// Int32 property value along a set of key frames. 
    ///
    /// This Int32KeyFrame interpolates the between the Int32 Value of 
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingInt32KeyFrame : Int32KeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingInt32KeyFrame.
        /// 

        public EasingInt32KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingInt32KeyFrame. 
        /// 

        public EasingInt32KeyFrame(Int32 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingInt32KeyFrame. 
        /// 

        public EasingInt32KeyFrame(Int32 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingInt32KeyFrame.
        /// 

        public EasingInt32KeyFrame(Int32 value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingInt32KeyFrame();
        }

        #endregion

        #region Int32KeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int32 InterpolateValueCore(Int32 baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateInt32(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingInt32KeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a Int64KeyFrameCollection in 
    /// conjunction with a KeyFrameInt64Animation to animate a
    /// Int64 property value along a set of key frames. 
    /// 
    /// This Int64KeyFrame interpolates the between the Int64 Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingInt64KeyFrame : Int64KeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingInt64KeyFrame. 
        /// 

        public EasingInt64KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingInt64KeyFrame. 
        /// 

        public EasingInt64KeyFrame(Int64 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingInt64KeyFrame. 
        /// 

        public EasingInt64KeyFrame(Int64 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingInt64KeyFrame. 
        /// 

        public EasingInt64KeyFrame(Int64 value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingInt64KeyFrame();
        }

        #endregion

        #region Int64KeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Int64 InterpolateValueCore(Int64 baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateInt64(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingInt64KeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a PointKeyFrameCollection in
    /// conjunction with a KeyFramePointAnimation to animate a 
    /// Point property value along a set of key frames.
    /// 
    /// This PointKeyFrame interpolates the between the Point Value of 
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingPointKeyFrame : PointKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingPointKeyFrame. 
        /// 

        public EasingPointKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingPointKeyFrame.
        /// 

        public EasingPointKeyFrame(Point value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingPointKeyFrame.
        /// 

        public EasingPointKeyFrame(Point value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingPointKeyFrame. 
        /// 

        public EasingPointKeyFrame(Point value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingPointKeyFrame();
        }

        #endregion

        #region PointKeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Point InterpolateValueCore(Point baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolatePoint(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingPointKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a Point3DKeyFrameCollection in
    /// conjunction with a KeyFramePoint3DAnimation to animate a
    /// Point3D property value along a set of key frames. 
    ///
    /// This Point3DKeyFrame interpolates the between the Point3D Value of 
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingPoint3DKeyFrame : Point3DKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingPoint3DKeyFrame.
        /// 

        public EasingPoint3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingPoint3DKeyFrame. 
        /// 

        public EasingPoint3DKeyFrame(Point3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingPoint3DKeyFrame. 
        /// 

        public EasingPoint3DKeyFrame(Point3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingPoint3DKeyFrame.
        /// 

        public EasingPoint3DKeyFrame(Point3D value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingPoint3DKeyFrame();
        }

        #endregion

        #region Point3DKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Point3D InterpolateValueCore(Point3D baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolatePoint3D(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingPoint3DKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a QuaternionKeyFrameCollection in
    /// conjunction with a KeyFrameQuaternionAnimation to animate a
    /// Quaternion property value along a set of key frames.
    /// 
    /// This QuaternionKeyFrame interpolates the between the Quaternion Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingQuaternionKeyFrame : QuaternionKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingQuaternionKeyFrame. 
        /// 

        public EasingQuaternionKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingQuaternionKeyFrame.
        /// 

        public EasingQuaternionKeyFrame(Quaternion value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingQuaternionKeyFrame.
        /// 

        public EasingQuaternionKeyFrame(Quaternion value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingQuaternionKeyFrame. 
        /// 

        public EasingQuaternionKeyFrame(Quaternion value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingQuaternionKeyFrame();
        }

        #endregion

        #region QuaternionKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Quaternion InterpolateValueCore(Quaternion baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                throw new NotImplementedException();
                //return AnimatedTypeHelpers.InterpolateQuaternion(baseValue, Value, keyFrameProgress, UseShortestPath);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingQuaternionKeyFrame));

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

        #endregion 
    }


    /// 

    /// This class is used as part of a Rotation3DKeyFrameCollection in 
    /// conjunction with a KeyFrameRotation3DAnimation to animate a
    /// Rotation3D property value along a set of key frames.
    ///
    /// This Rotation3DKeyFrame interpolates the between the Rotation3D Value of 
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingRotation3DKeyFrame : Rotation3DKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingRotation3DKeyFrame.
        /// 

        public EasingRotation3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingRotation3DKeyFrame.
        /// 

        public EasingRotation3DKeyFrame(Rotation3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingRotation3DKeyFrame.
        /// 

        public EasingRotation3DKeyFrame(Rotation3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingRotation3DKeyFrame.
        /// 

        public EasingRotation3DKeyFrame(Rotation3D value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingRotation3DKeyFrame();
        }

        #endregion

        #region Rotation3DKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Rotation3D InterpolateValueCore(Rotation3D baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                throw new Exception("Not implemented");
                //return AnimatedTypeHelpers.InterpolateRotation3D(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingRotation3DKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a RectKeyFrameCollection in
    /// conjunction with a KeyFrameRectAnimation to animate a 
    /// Rect property value along a set of key frames.
    ///
    /// This RectKeyFrame interpolates the between the Rect Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingRectKeyFrame : RectKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingRectKeyFrame.
        /// 

        public EasingRectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingRectKeyFrame.
        /// 

        public EasingRectKeyFrame(Rect value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingRectKeyFrame.
        /// 

        public EasingRectKeyFrame(Rect value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingRectKeyFrame.
        /// 

        public EasingRectKeyFrame(Rect value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingRectKeyFrame();
        }

        #endregion

        #region RectKeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Rect InterpolateValueCore(Rect baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateRect(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingRectKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a SingleKeyFrameCollection in 
    /// conjunction with a KeyFrameSingleAnimation to animate a
    /// Single property value along a set of key frames. 
    ///
    /// This SingleKeyFrame interpolates the between the Single Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingSingleKeyFrame : SingleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingSingleKeyFrame.
        /// 

        public EasingSingleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingSingleKeyFrame. 
        /// 

        public EasingSingleKeyFrame(Single value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingSingleKeyFrame. 
        /// 

        public EasingSingleKeyFrame(Single value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingSingleKeyFrame.
        /// 

        public EasingSingleKeyFrame(Single value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingSingleKeyFrame();
        }

        #endregion

        #region SingleKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Single InterpolateValueCore(Single baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateSingle(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingSingleKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a SizeKeyFrameCollection in 
    /// conjunction with a KeyFrameSizeAnimation to animate a 
    /// Size property value along a set of key frames.
    /// 
    /// This SizeKeyFrame interpolates the between the Size Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingSizeKeyFrame : SizeKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingSizeKeyFrame. 
        /// 

        public EasingSizeKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingSizeKeyFrame.
        /// 

        public EasingSizeKeyFrame(Size value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingSizeKeyFrame.
        /// 

        public EasingSizeKeyFrame(Size value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingSizeKeyFrame. 
        /// 

        public EasingSizeKeyFrame(Size value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new EasingSizeKeyFrame();
        }

        #endregion

        #region SizeKeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Size InterpolateValueCore(Size baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateSize(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty 
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingSizeKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a VectorKeyFrameCollection in
    /// conjunction with a KeyFrameVectorAnimation to animate a 
    /// Vector property value along a set of key frames. 
    ///
    /// This VectorKeyFrame interpolates the between the Vector Value of 
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value.
    /// 

    public partial class EasingVectorKeyFrame : VectorKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new EasingVectorKeyFrame.
        /// 

        public EasingVectorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingVectorKeyFrame. 
        /// 

        public EasingVectorKeyFrame(Vector value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingVectorKeyFrame. 
        /// 

        public EasingVectorKeyFrame(Vector value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingVectorKeyFrame.
        /// 

        public EasingVectorKeyFrame(Vector value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingVectorKeyFrame();
        }

        #endregion

        #region VectorKeyFrame 

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Vector InterpolateValueCore(Vector baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateVector(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties 

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingVectorKeyFrame));

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

        #endregion
    }


    /// 

    /// This class is used as part of a Vector3DKeyFrameCollection in 
    /// conjunction with a KeyFrameVector3DAnimation to animate a
    /// Vector3D property value along a set of key frames. 
    /// 
    /// This Vector3DKeyFrame interpolates the between the Vector3D Value of
    /// the previous key frame and its own Value Linearly with an EasingFunction to produce its output value. 
    /// 

    public partial class EasingVector3DKeyFrame : Vector3DKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new EasingVector3DKeyFrame. 
        /// 

        public EasingVector3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new EasingVector3DKeyFrame. 
        /// 

        public EasingVector3DKeyFrame(Vector3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new EasingVector3DKeyFrame. 
        /// 

        public EasingVector3DKeyFrame(Vector3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        /// 

        /// Creates a new EasingVector3DKeyFrame. 
        /// 

        public EasingVector3DKeyFrame(Vector3D value, KeyTime keyTime, IEasingFunction easingFunction)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
            EasingFunction = easingFunction;
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new EasingVector3DKeyFrame();
        }

        #endregion

        #region Vector3DKeyFrame

        /// 

        /// Implemented to Easingly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Vector3D InterpolateValueCore(Vector3D baseValue, double keyFrameProgress)
        {
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);
            }

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
                return AnimatedTypeHelpers.InterpolateVector3D(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion

        #region Public Properties

        /// 

        /// EasingFunctionProperty
        /// 

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(EasingVector3DKeyFrame));

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

        #endregion
    }

}
