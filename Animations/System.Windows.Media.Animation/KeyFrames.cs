using MS.Internal;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using MS.Internal.PresentationCore;
using System.Windows.Media.Animation;

namespace System.Windows.Media.Animation2
{

    /// 

    /// This class is used as part of a BooleanKeyFrameCollection in
    /// conjunction with a KeyFrameBooleanAnimation to animate a
    /// Boolean property value along a set of key frames.
    /// 

    public abstract class BooleanKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new BooleanKeyFrame.
        /// 

        protected BooleanKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new BooleanKeyFrame. 
        /// 

        protected BooleanKeyFrame(Boolean value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteBooleanKeyFrame. 
        /// 

        protected BooleanKeyFrame(Boolean value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(BooleanKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Boolean),
                    typeof(BooleanKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Boolean)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Boolean Value
        {
            get
            {
                return (Boolean)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Boolean InterpolateValue(
            Boolean baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Boolean InterpolateValueCore(
            Boolean baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a ByteKeyFrameCollection in
    /// conjunction with a KeyFrameByteAnimation to animate a
    /// Byte property value along a set of key frames.
    /// 

    public abstract class ByteKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new ByteKeyFrame.
        /// 

        protected ByteKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new ByteKeyFrame. 
        /// 

        protected ByteKeyFrame(Byte value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteByteKeyFrame. 
        /// 

        protected ByteKeyFrame(Byte value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(ByteKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Byte),
                    typeof(ByteKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Byte)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Byte Value
        {
            get
            {
                return (Byte)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Byte InterpolateValue(
            Byte baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Byte InterpolateValueCore(
            Byte baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a CharKeyFrameCollection in
    /// conjunction with a KeyFrameCharAnimation to animate a
    /// Char property value along a set of key frames.
    /// 

    public abstract class CharKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new CharKeyFrame.
        /// 

        protected CharKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new CharKeyFrame. 
        /// 

        protected CharKeyFrame(Char value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteCharKeyFrame. 
        /// 

        protected CharKeyFrame(Char value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(CharKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Char),
                    typeof(CharKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Char)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Char Value
        {
            get
            {
                return (Char)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Char InterpolateValue(
            Char baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Char InterpolateValueCore(
            Char baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a ColorKeyFrameCollection in
    /// conjunction with a KeyFrameColorAnimation to animate a
    /// Color property value along a set of key frames.
    /// 

    public abstract class ColorKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new ColorKeyFrame.
        /// 

        protected ColorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new ColorKeyFrame. 
        /// 

        protected ColorKeyFrame(Color value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteColorKeyFrame. 
        /// 

        protected ColorKeyFrame(Color value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(ColorKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Color),
                    typeof(ColorKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Color)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Color Value
        {
            get
            {
                return (Color)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Color InterpolateValue(
            Color baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Color InterpolateValueCore(
            Color baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a DecimalKeyFrameCollection in
    /// conjunction with a KeyFrameDecimalAnimation to animate a
    /// Decimal property value along a set of key frames.
    /// 

    public abstract class DecimalKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DecimalKeyFrame.
        /// 

        protected DecimalKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DecimalKeyFrame. 
        /// 

        protected DecimalKeyFrame(Decimal value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteDecimalKeyFrame. 
        /// 

        protected DecimalKeyFrame(Decimal value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(DecimalKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Decimal),
                    typeof(DecimalKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Decimal)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Decimal Value
        {
            get
            {
                return (Decimal)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Decimal InterpolateValue(
            Decimal baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Decimal InterpolateValueCore(
            Decimal baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a DoubleKeyFrameCollection in
    /// conjunction with a KeyFrameDoubleAnimation to animate a
    /// Double property value along a set of key frames.
    /// 

    public abstract class DoubleKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DoubleKeyFrame.
        /// 

        protected DoubleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DoubleKeyFrame. 
        /// 

        protected DoubleKeyFrame(Double value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteDoubleKeyFrame. 
        /// 

        protected DoubleKeyFrame(Double value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(DoubleKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Double),
                    typeof(DoubleKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Double)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Double Value
        {
            get
            {
                return (Double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Double InterpolateValue(
            Double baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Double InterpolateValueCore(
            Double baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a Int16KeyFrameCollection in
    /// conjunction with a KeyFrameInt16Animation to animate a
    /// Int16 property value along a set of key frames.
    /// 

    public abstract class Int16KeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new Int16KeyFrame.
        /// 

        protected Int16KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new Int16KeyFrame. 
        /// 

        protected Int16KeyFrame(Int16 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteInt16KeyFrame. 
        /// 

        protected Int16KeyFrame(Int16 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(Int16KeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Int16),
                    typeof(Int16KeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Int16)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Int16 Value
        {
            get
            {
                return (Int16)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Int16 InterpolateValue(
            Int16 baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Int16 InterpolateValueCore(
            Int16 baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a Int32KeyFrameCollection in
    /// conjunction with a KeyFrameInt32Animation to animate a
    /// Int32 property value along a set of key frames.
    /// 

    public abstract class Int32KeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new Int32KeyFrame.
        /// 

        protected Int32KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new Int32KeyFrame. 
        /// 

        protected Int32KeyFrame(Int32 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteInt32KeyFrame. 
        /// 

        protected Int32KeyFrame(Int32 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(Int32KeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Int32),
                    typeof(Int32KeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Int32)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Int32 Value
        {
            get
            {
                return (Int32)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Int32 InterpolateValue(
            Int32 baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Int32 InterpolateValueCore(
            Int32 baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a Int64KeyFrameCollection in
    /// conjunction with a KeyFrameInt64Animation to animate a
    /// Int64 property value along a set of key frames.
    /// 

    public abstract class Int64KeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new Int64KeyFrame.
        /// 

        protected Int64KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new Int64KeyFrame. 
        /// 

        protected Int64KeyFrame(Int64 value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteInt64KeyFrame. 
        /// 

        protected Int64KeyFrame(Int64 value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(Int64KeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Int64),
                    typeof(Int64KeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Int64)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Int64 Value
        {
            get
            {
                return (Int64)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Int64 InterpolateValue(
            Int64 baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Int64 InterpolateValueCore(
            Int64 baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a MatrixKeyFrameCollection in
    /// conjunction with a KeyFrameMatrixAnimation to animate a
    /// Matrix property value along a set of key frames.
    /// 

    public abstract class MatrixKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new MatrixKeyFrame.
        /// 

        protected MatrixKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new MatrixKeyFrame. 
        /// 

        protected MatrixKeyFrame(Matrix value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteMatrixKeyFrame. 
        /// 

        protected MatrixKeyFrame(Matrix value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(MatrixKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Matrix),
                    typeof(MatrixKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Matrix)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Matrix Value
        {
            get
            {
                return (Matrix)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Matrix InterpolateValue(
            Matrix baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Matrix InterpolateValueCore(
            Matrix baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a ObjectKeyFrameCollection in
    /// conjunction with a KeyFrameObjectAnimation to animate a
    /// Object property value along a set of key frames.
    /// 

    public abstract class ObjectKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new ObjectKeyFrame.
        /// 

        protected ObjectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new ObjectKeyFrame. 
        /// 

        protected ObjectKeyFrame(Object value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteObjectKeyFrame. 
        /// 

        protected ObjectKeyFrame(Object value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(ObjectKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Object),
                    typeof(ObjectKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Object)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Object Value
        {
            get
            {
                return (Object)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Object InterpolateValue(
            Object baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Object InterpolateValueCore(
            Object baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a PointKeyFrameCollection in
    /// conjunction with a KeyFramePointAnimation to animate a
    /// Point property value along a set of key frames.
    /// 

    public abstract class PointKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new PointKeyFrame.
        /// 

        protected PointKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new PointKeyFrame. 
        /// 

        protected PointKeyFrame(Point value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscretePointKeyFrame. 
        /// 

        protected PointKeyFrame(Point value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(PointKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Point),
                    typeof(PointKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Point)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Point Value
        {
            get
            {
                return (Point)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Point InterpolateValue(
            Point baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Point InterpolateValueCore(
            Point baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a Point3DKeyFrameCollection in
    /// conjunction with a KeyFramePoint3DAnimation to animate a
    /// Point3D property value along a set of key frames.
    /// 

    public abstract class Point3DKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new Point3DKeyFrame.
        /// 

        protected Point3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new Point3DKeyFrame. 
        /// 

        protected Point3DKeyFrame(Point3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscretePoint3DKeyFrame. 
        /// 

        protected Point3DKeyFrame(Point3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(Point3DKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Point3D),
                    typeof(Point3DKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Point3D)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Point3D Value
        {
            get
            {
                return (Point3D)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Point3D InterpolateValue(
            Point3D baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Point3D InterpolateValueCore(
            Point3D baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a QuaternionKeyFrameCollection in
    /// conjunction with a KeyFrameQuaternionAnimation to animate a
    /// Quaternion property value along a set of key frames.
    /// 

    public abstract class QuaternionKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new QuaternionKeyFrame.
        /// 

        protected QuaternionKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new QuaternionKeyFrame. 
        /// 

        protected QuaternionKeyFrame(Quaternion value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteQuaternionKeyFrame. 
        /// 

        protected QuaternionKeyFrame(Quaternion value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(QuaternionKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Quaternion),
                    typeof(QuaternionKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Quaternion)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Quaternion Value
        {
            get
            {
                return (Quaternion)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Quaternion InterpolateValue(
            Quaternion baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Quaternion InterpolateValueCore(
            Quaternion baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a Rotation3DKeyFrameCollection in
    /// conjunction with a KeyFrameRotation3DAnimation to animate a
    /// Rotation3D property value along a set of key frames.
    /// 

    public abstract class Rotation3DKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new Rotation3DKeyFrame.
        /// 

        protected Rotation3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new Rotation3DKeyFrame. 
        /// 

        protected Rotation3DKeyFrame(Rotation3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteRotation3DKeyFrame. 
        /// 

        protected Rotation3DKeyFrame(Rotation3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(Rotation3DKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Rotation3D),
                    typeof(Rotation3DKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Rotation3D)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Rotation3D Value
        {
            get
            {
                return (Rotation3D)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Rotation3D InterpolateValue(
            Rotation3D baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Rotation3D InterpolateValueCore(
            Rotation3D baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a RectKeyFrameCollection in
    /// conjunction with a KeyFrameRectAnimation to animate a
    /// Rect property value along a set of key frames.
    /// 

    public abstract class RectKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new RectKeyFrame.
        /// 

        protected RectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new RectKeyFrame. 
        /// 

        protected RectKeyFrame(Rect value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteRectKeyFrame. 
        /// 

        protected RectKeyFrame(Rect value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(RectKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Rect),
                    typeof(RectKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Rect)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Rect Value
        {
            get
            {
                return (Rect)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Rect InterpolateValue(
            Rect baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Rect InterpolateValueCore(
            Rect baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a SingleKeyFrameCollection in
    /// conjunction with a KeyFrameSingleAnimation to animate a
    /// Single property value along a set of key frames.
    /// 

    public abstract class SingleKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SingleKeyFrame.
        /// 

        protected SingleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SingleKeyFrame. 
        /// 

        protected SingleKeyFrame(Single value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteSingleKeyFrame. 
        /// 

        protected SingleKeyFrame(Single value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(SingleKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Single),
                    typeof(SingleKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Single)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Single Value
        {
            get
            {
                return (Single)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Single InterpolateValue(
            Single baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Single InterpolateValueCore(
            Single baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a SizeKeyFrameCollection in
    /// conjunction with a KeyFrameSizeAnimation to animate a
    /// Size property value along a set of key frames.
    /// 

    public abstract class SizeKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new SizeKeyFrame.
        /// 

        protected SizeKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new SizeKeyFrame. 
        /// 

        protected SizeKeyFrame(Size value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteSizeKeyFrame. 
        /// 

        protected SizeKeyFrame(Size value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(SizeKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Size),
                    typeof(SizeKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Size)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Size Value
        {
            get
            {
                return (Size)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Size InterpolateValue(
            Size baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Size InterpolateValueCore(
            Size baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a StringKeyFrameCollection in
    /// conjunction with a KeyFrameStringAnimation to animate a
    /// String property value along a set of key frames.
    /// 

    public abstract class StringKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new StringKeyFrame.
        /// 

        protected StringKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new StringKeyFrame. 
        /// 

        protected StringKeyFrame(String value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteStringKeyFrame. 
        /// 

        protected StringKeyFrame(String value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(StringKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(String),
                    typeof(StringKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (String)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public String Value
        {
            get
            {
                return (String)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public String InterpolateValue(
            String baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract String InterpolateValueCore(
            String baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a VectorKeyFrameCollection in
    /// conjunction with a KeyFrameVectorAnimation to animate a
    /// Vector property value along a set of key frames.
    /// 

    public abstract class VectorKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new VectorKeyFrame.
        /// 

        protected VectorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new VectorKeyFrame. 
        /// 

        protected VectorKeyFrame(Vector value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteVectorKeyFrame. 
        /// 

        protected VectorKeyFrame(Vector value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(VectorKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Vector),
                    typeof(VectorKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Vector)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Vector Value
        {
            get
            {
                return (Vector)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Vector InterpolateValue(
            Vector baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Vector InterpolateValueCore(
            Vector baseValue,
            double keyFrameProgress);

        #endregion
    }


    /// 

    /// This class is used as part of a Vector3DKeyFrameCollection in
    /// conjunction with a KeyFrameVector3DAnimation to animate a
    /// Vector3D property value along a set of key frames.
    /// 

    public abstract class Vector3DKeyFrame : Freezable, IKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new Vector3DKeyFrame.
        /// 

        protected Vector3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new Vector3DKeyFrame. 
        /// 

        protected Vector3DKeyFrame(Vector3D value)
            : this()
        {
            Value = value;
        }

        /// 

        /// Creates a new DiscreteVector3DKeyFrame. 
        /// 

        protected Vector3DKeyFrame(Vector3D value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// 

        /// KeyTime Property
        /// 

        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(Vector3DKeyFrame),
                    new PropertyMetadata(KeyTime.Uniform));

        /// 

        /// The time at which this KeyFrame's value should be equal to the Value 
        /// property. 
        /// 

        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty);
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// 

        /// Value Property 
        /// 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Vector3D),
                    typeof(Vector3DKeyFrame),
                    new PropertyMetadata());

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        object IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (Vector3D)value;
            }
        }

        /// 

        /// The value of this key frame at the KeyTime specified.
        /// 

        public Vector3D Value
        {
            get
            {
                return (Vector3D)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods 

        /// 

        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// 

        public Vector3D InterpolateValue(
            Vector3D baseValue,
            double keyFrameProgress)
        {
            if (keyFrameProgress < 0.0
                || keyFrameProgress > 1.0)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods 

        /// 

        /// This method should be implemented by derived classes to calculate 
        /// the value of this key frame at the progress value provided.
        /// 

        protected abstract Vector3D InterpolateValueCore(
            Vector3D baseValue,
            double keyFrameProgress);

        #endregion
    }

}
