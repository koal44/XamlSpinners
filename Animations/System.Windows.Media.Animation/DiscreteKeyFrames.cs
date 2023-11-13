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
    /// This BooleanKeyFrame changes from the Boolean Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteBooleanKeyFrame : BooleanKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteBooleanKeyFrame.
        /// 

        public DiscreteBooleanKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteBooleanKeyFrame. 
        /// 

        public DiscreteBooleanKeyFrame(Boolean value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteBooleanKeyFrame.
        /// 

        public DiscreteBooleanKeyFrame(Boolean value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteBooleanKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region BooleanKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Boolean InterpolateValueCore(Boolean baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a ByteKeyFrameCollection in
    /// conjunction with a KeyFrameByteAnimation to animate a 
    /// Byte property value along a set of key frames. 
    ///
    /// This ByteKeyFrame changes from the Byte Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteByteKeyFrame : ByteKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteByteKeyFrame. 
        /// 

        public DiscreteByteKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteByteKeyFrame.
        /// 

        public DiscreteByteKeyFrame(Byte value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteByteKeyFrame. 
        /// 

        public DiscreteByteKeyFrame(Byte value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteByteKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region ByteKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Byte InterpolateValueCore(Byte baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion 
    }


    /// 

    /// This class is used as part of a CharKeyFrameCollection in 
    /// conjunction with a KeyFrameCharAnimation to animate a
    /// Char property value along a set of key frames.
    ///
    /// This CharKeyFrame changes from the Char Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteCharKeyFrame : CharKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteCharKeyFrame. 
        /// 

        public DiscreteCharKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteCharKeyFrame.
        /// 

        public DiscreteCharKeyFrame(Char value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteCharKeyFrame.
        /// 

        public DiscreteCharKeyFrame(Char value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteCharKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region CharKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Char InterpolateValueCore(Char baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a ColorKeyFrameCollection in 
    /// conjunction with a KeyFrameColorAnimation to animate a
    /// Color property value along a set of key frames. 
    /// 
    /// This ColorKeyFrame changes from the Color Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteColorKeyFrame : ColorKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteColorKeyFrame.
        /// 

        public DiscreteColorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteColorKeyFrame. 
        /// 

        public DiscreteColorKeyFrame(Color value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteColorKeyFrame. 
        /// 

        public DiscreteColorKeyFrame(Color value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteColorKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region ColorKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Color InterpolateValueCore(Color baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a DecimalKeyFrameCollection in
    /// conjunction with a KeyFrameDecimalAnimation to animate a 
    /// Decimal property value along a set of key frames.
    ///
    /// This DecimalKeyFrame changes from the Decimal Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteDecimalKeyFrame : DecimalKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteDecimalKeyFrame.
        /// 

        public DiscreteDecimalKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteDecimalKeyFrame.
        /// 

        public DiscreteDecimalKeyFrame(Decimal value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteDecimalKeyFrame.
        /// 

        public DiscreteDecimalKeyFrame(Decimal value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteDecimalKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region DecimalKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Decimal InterpolateValueCore(Decimal baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a DoubleKeyFrameCollection in
    /// conjunction with a KeyFrameDoubleAnimation to animate a 
    /// Double property value along a set of key frames.
    /// 
    /// This DoubleKeyFrame changes from the Double Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteDoubleKeyFrame : DoubleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteDoubleKeyFrame. 
        /// 

        public DiscreteDoubleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteDoubleKeyFrame. 
        /// 

        public DiscreteDoubleKeyFrame(Double value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteDoubleKeyFrame.
        /// 

        public DiscreteDoubleKeyFrame(Double value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteDoubleKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region DoubleKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Double InterpolateValueCore(Double baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a Int16KeyFrameCollection in 
    /// conjunction with a KeyFrameInt16Animation to animate a
    /// Int16 property value along a set of key frames. 
    ///
    /// This Int16KeyFrame changes from the Int16 Value of
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteInt16KeyFrame : Int16KeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteInt16KeyFrame.
        /// 

        public DiscreteInt16KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteInt16KeyFrame.
        /// 

        public DiscreteInt16KeyFrame(Int16 value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteInt16KeyFrame. 
        /// 

        public DiscreteInt16KeyFrame(Int16 value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteInt16KeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region Int16KeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int16 InterpolateValueCore(Int16 baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a Int32KeyFrameCollection in
    /// conjunction with a KeyFrameInt32Animation to animate a
    /// Int32 property value along a set of key frames. 
    ///
    /// This Int32KeyFrame changes from the Int32 Value of 
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteInt32KeyFrame : Int32KeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteInt32KeyFrame. 
        /// 

        public DiscreteInt32KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteInt32KeyFrame.
        /// 

        public DiscreteInt32KeyFrame(Int32 value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteInt32KeyFrame. 
        /// 

        public DiscreteInt32KeyFrame(Int32 value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteInt32KeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region Int32KeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Int32 InterpolateValueCore(Int32 baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a Int64KeyFrameCollection in 
    /// conjunction with a KeyFrameInt64Animation to animate a 
    /// Int64 property value along a set of key frames.
    /// 
    /// This Int64KeyFrame changes from the Int64 Value of
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteInt64KeyFrame : Int64KeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteInt64KeyFrame.
        /// 

        public DiscreteInt64KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteInt64KeyFrame. 
        /// 

        public DiscreteInt64KeyFrame(Int64 value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteInt64KeyFrame.
        /// 

        public DiscreteInt64KeyFrame(Int64 value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteInt64KeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region Int64KeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Int64 InterpolateValueCore(Int64 baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a MatrixKeyFrameCollection in
    /// conjunction with a KeyFrameMatrixAnimation to animate a
    /// Matrix property value along a set of key frames.
    /// 
    /// This MatrixKeyFrame changes from the Matrix Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteMatrixKeyFrame : MatrixKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteMatrixKeyFrame.
        /// 

        public DiscreteMatrixKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteMatrixKeyFrame. 
        /// 

        public DiscreteMatrixKeyFrame(Matrix value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteMatrixKeyFrame.
        /// 

        public DiscreteMatrixKeyFrame(Matrix value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteMatrixKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region MatrixKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Matrix InterpolateValueCore(Matrix baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a ObjectKeyFrameCollection in
    /// conjunction with a KeyFrameObjectAnimation to animate a 
    /// Object property value along a set of key frames. 
    ///
    /// This ObjectKeyFrame changes from the Object Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteObjectKeyFrame : ObjectKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteObjectKeyFrame. 
        /// 

        public DiscreteObjectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteObjectKeyFrame.
        /// 

        public DiscreteObjectKeyFrame(Object value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteObjectKeyFrame. 
        /// 

        public DiscreteObjectKeyFrame(Object value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteObjectKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region ObjectKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Object InterpolateValueCore(Object baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion 
    }


    /// 

    /// This class is used as part of a PointKeyFrameCollection in 
    /// conjunction with a KeyFramePointAnimation to animate a
    /// Point property value along a set of key frames.
    ///
    /// This PointKeyFrame changes from the Point Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime. 
    /// 

    public class DiscretePointKeyFrame : PointKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscretePointKeyFrame. 
        /// 

        public DiscretePointKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscretePointKeyFrame.
        /// 

        public DiscretePointKeyFrame(Point value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscretePointKeyFrame.
        /// 

        public DiscretePointKeyFrame(Point value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscretePointKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region PointKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Point InterpolateValueCore(Point baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a Point3DKeyFrameCollection in 
    /// conjunction with a KeyFramePoint3DAnimation to animate a
    /// Point3D property value along a set of key frames. 
    /// 
    /// This Point3DKeyFrame changes from the Point3D Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime.
    /// 

    public class DiscretePoint3DKeyFrame : Point3DKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscretePoint3DKeyFrame.
        /// 

        public DiscretePoint3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscretePoint3DKeyFrame. 
        /// 

        public DiscretePoint3DKeyFrame(Point3D value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscretePoint3DKeyFrame. 
        /// 

        public DiscretePoint3DKeyFrame(Point3D value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscretePoint3DKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region Point3DKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Point3D InterpolateValueCore(Point3D baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a QuaternionKeyFrameCollection in
    /// conjunction with a KeyFrameQuaternionAnimation to animate a 
    /// Quaternion property value along a set of key frames.
    ///
    /// This QuaternionKeyFrame changes from the Quaternion Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteQuaternionKeyFrame : QuaternionKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteQuaternionKeyFrame.
        /// 

        public DiscreteQuaternionKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteQuaternionKeyFrame.
        /// 

        public DiscreteQuaternionKeyFrame(Quaternion value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteQuaternionKeyFrame.
        /// 

        public DiscreteQuaternionKeyFrame(Quaternion value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteQuaternionKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region QuaternionKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Quaternion InterpolateValueCore(Quaternion baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    public class DiscreteRotation3DKeyFrame : Rotation3DKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteRotation3DKeyFrame. 
        /// 

        public DiscreteRotation3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteRotation3DKeyFrame. 
        /// 

        public DiscreteRotation3DKeyFrame(Rotation3D value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteRotation3DKeyFrame.
        /// 

        public DiscreteRotation3DKeyFrame(Rotation3D value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteRotation3DKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region Rotation3DKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Rotation3D InterpolateValueCore(Rotation3D baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    public class DiscreteRectKeyFrame : RectKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteRectKeyFrame.
        /// 

        public DiscreteRectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteRectKeyFrame.
        /// 

        public DiscreteRectKeyFrame(Rect value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteRectKeyFrame. 
        /// 

        public DiscreteRectKeyFrame(Rect value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteRectKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region RectKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Rect InterpolateValueCore(Rect baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a SingleKeyFrameCollection in
    /// conjunction with a KeyFrameSingleAnimation to animate a
    /// Single property value along a set of key frames. 
    ///
    /// This SingleKeyFrame changes from the Single Value of 
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteSingleKeyFrame : SingleKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteSingleKeyFrame. 
        /// 

        public DiscreteSingleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteSingleKeyFrame.
        /// 

        public DiscreteSingleKeyFrame(Single value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteSingleKeyFrame. 
        /// 

        public DiscreteSingleKeyFrame(Single value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteSingleKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region SingleKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Single InterpolateValueCore(Single baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a SizeKeyFrameCollection in 
    /// conjunction with a KeyFrameSizeAnimation to animate a 
    /// Size property value along a set of key frames.
    /// 
    /// This SizeKeyFrame changes from the Size Value of
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteSizeKeyFrame : SizeKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteSizeKeyFrame.
        /// 

        public DiscreteSizeKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteSizeKeyFrame. 
        /// 

        public DiscreteSizeKeyFrame(Size value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteSizeKeyFrame.
        /// 

        public DiscreteSizeKeyFrame(Size value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteSizeKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region SizeKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override Size InterpolateValueCore(Size baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a StringKeyFrameCollection in
    /// conjunction with a KeyFrameStringAnimation to animate a
    /// String property value along a set of key frames.
    /// 
    /// This StringKeyFrame changes from the String Value of
    /// the previous key frame to its own Value without interpolation.  The 
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteStringKeyFrame : StringKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteStringKeyFrame.
        /// 

        public DiscreteStringKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteStringKeyFrame. 
        /// 

        public DiscreteStringKeyFrame(String value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteStringKeyFrame.
        /// 

        public DiscreteStringKeyFrame(String value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteStringKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything

        #endregion

        #region StringKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress. 
        /// 

        protected override String InterpolateValueCore(String baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }


    /// 

    /// This class is used as part of a VectorKeyFrameCollection in
    /// conjunction with a KeyFrameVectorAnimation to animate a 
    /// Vector property value along a set of key frames. 
    ///
    /// This VectorKeyFrame changes from the Vector Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime.
    /// 

    public class DiscreteVectorKeyFrame : VectorKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new DiscreteVectorKeyFrame. 
        /// 

        public DiscreteVectorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteVectorKeyFrame.
        /// 

        public DiscreteVectorKeyFrame(Vector value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteVectorKeyFrame. 
        /// 

        public DiscreteVectorKeyFrame(Vector value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable 

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable.
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteVectorKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region VectorKeyFrame 

        /// 

        /// Implemented to linearly interpolate between the baseValue and the 
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Vector InterpolateValueCore(Vector baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion 
    }


    /// 

    /// This class is used as part of a Vector3DKeyFrameCollection in 
    /// conjunction with a KeyFrameVector3DAnimation to animate a
    /// Vector3D property value along a set of key frames.
    ///
    /// This Vector3DKeyFrame changes from the Vector3D Value of 
    /// the previous key frame to its own Value without interpolation.  The
    /// change occurs at the KeyTime. 
    /// 

    public class DiscreteVector3DKeyFrame : Vector3DKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new DiscreteVector3DKeyFrame. 
        /// 

        public DiscreteVector3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new DiscreteVector3DKeyFrame.
        /// 

        public DiscreteVector3DKeyFrame(Vector3D value)
            : base(value)
        {
        }

        /// 

        /// Creates a new DiscreteVector3DKeyFrame.
        /// 

        public DiscreteVector3DKeyFrame(Vector3D value, KeyTime keyTime)
            : base(value, keyTime)
        {
        }

        #endregion

        #region Freezable

        /// 

        /// Implementation of Freezable.CreateInstanceCore.
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new DiscreteVector3DKeyFrame();
        }

        // We don't need to override CloneCore because it won't do anything 

        #endregion

        #region Vector3DKeyFrame

        /// 

        /// Implemented to linearly interpolate between the baseValue and the
        /// Value of this KeyFrame using the keyFrameProgress.
        /// 

        protected override Vector3D InterpolateValueCore(Vector3D baseValue, double keyFrameProgress)
        {
            if (keyFrameProgress < 1.0)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }

        #endregion
    }

}
