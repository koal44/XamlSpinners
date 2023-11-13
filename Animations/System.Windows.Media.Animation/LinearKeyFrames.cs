using MS.Internal;

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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearByteKeyFrame : ByteKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearByteKeyFrame. 
        /// 

        public LinearByteKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearByteKeyFrame.
        /// 

        public LinearByteKeyFrame(Byte value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearByteKeyFrame.
        /// 

        public LinearByteKeyFrame(Byte value, KeyTime keyTime)
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
            return new LinearByteKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateByte(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearColorKeyFrame : ColorKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearColorKeyFrame. 
        /// 

        public LinearColorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearColorKeyFrame.
        /// 

        public LinearColorKeyFrame(Color value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearColorKeyFrame. 
        /// 

        public LinearColorKeyFrame(Color value, KeyTime keyTime)
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
            return new LinearColorKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateColor(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearDecimalKeyFrame : DecimalKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearDecimalKeyFrame.
        /// 

        public LinearDecimalKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearDecimalKeyFrame. 
        /// 

        public LinearDecimalKeyFrame(Decimal value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearDecimalKeyFrame.
        /// 

        public LinearDecimalKeyFrame(Decimal value, KeyTime keyTime)
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
            return new LinearDecimalKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateDecimal(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearDoubleKeyFrame : DoubleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearDoubleKeyFrame.
        /// 

        public LinearDoubleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearDoubleKeyFrame. 
        /// 

        public LinearDoubleKeyFrame(Double value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearDoubleKeyFrame.
        /// 

        public LinearDoubleKeyFrame(Double value, KeyTime keyTime)
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
            return new LinearDoubleKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateDouble(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearInt16KeyFrame : Int16KeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearInt16KeyFrame. 
        /// 

        public LinearInt16KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearInt16KeyFrame.
        /// 

        public LinearInt16KeyFrame(Int16 value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearInt16KeyFrame. 
        /// 

        public LinearInt16KeyFrame(Int16 value, KeyTime keyTime)
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
            return new LinearInt16KeyFrame();
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
                return AnimatedTypeHelpers.InterpolateInt16(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearInt32KeyFrame : Int32KeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearInt32KeyFrame.
        /// 

        public LinearInt32KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearInt32KeyFrame.
        /// 

        public LinearInt32KeyFrame(Int32 value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearInt32KeyFrame. 
        /// 

        public LinearInt32KeyFrame(Int32 value, KeyTime keyTime)
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
            return new LinearInt32KeyFrame();
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
                return AnimatedTypeHelpers.InterpolateInt32(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearInt64KeyFrame : Int64KeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearInt64KeyFrame. 
        /// 

        public LinearInt64KeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearInt64KeyFrame. 
        /// 

        public LinearInt64KeyFrame(Int64 value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearInt64KeyFrame.
        /// 

        public LinearInt64KeyFrame(Int64 value, KeyTime keyTime)
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
            return new LinearInt64KeyFrame();
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
                return AnimatedTypeHelpers.InterpolateInt64(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearPointKeyFrame : PointKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearPointKeyFrame.
        /// 

        public LinearPointKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearPointKeyFrame.
        /// 

        public LinearPointKeyFrame(Point value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearPointKeyFrame.
        /// 

        public LinearPointKeyFrame(Point value, KeyTime keyTime)
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
            return new LinearPointKeyFrame();
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
                return AnimatedTypeHelpers.InterpolatePoint(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearPoint3DKeyFrame : Point3DKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearPoint3DKeyFrame.
        /// 

        public LinearPoint3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearPoint3DKeyFrame. 
        /// 

        public LinearPoint3DKeyFrame(Point3D value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearPoint3DKeyFrame. 
        /// 

        public LinearPoint3DKeyFrame(Point3D value, KeyTime keyTime)
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
            return new LinearPoint3DKeyFrame();
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
                return AnimatedTypeHelpers.InterpolatePoint3D(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearQuaternionKeyFrame : QuaternionKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearQuaternionKeyFrame. 
        /// 

        public LinearQuaternionKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearQuaternionKeyFrame.
        /// 

        public LinearQuaternionKeyFrame(Quaternion value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearQuaternionKeyFrame.
        /// 

        public LinearQuaternionKeyFrame(Quaternion value, KeyTime keyTime)
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
            return new LinearQuaternionKeyFrame();
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
                throw new NotImplementedException();
                //return AnimatedTypeHelpers.InterpolateQuaternion(baseValue, Value, keyFrameProgress, UseShortestPath);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearRotation3DKeyFrame : Rotation3DKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearRotation3DKeyFrame. 
        /// 

        public LinearRotation3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearRotation3DKeyFrame.
        /// 

        public LinearRotation3DKeyFrame(Rotation3D value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearRotation3DKeyFrame. 
        /// 

        public LinearRotation3DKeyFrame(Rotation3D value, KeyTime keyTime)
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
            return new LinearRotation3DKeyFrame();
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
                throw new NotImplementedException();
                //return AnimatedTypeHelpers.InterpolateRotation3D(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearRectKeyFrame : RectKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearRectKeyFrame.
        /// 

        public LinearRectKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearRectKeyFrame. 
        /// 

        public LinearRectKeyFrame(Rect value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearRectKeyFrame.
        /// 

        public LinearRectKeyFrame(Rect value, KeyTime keyTime)
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
            return new LinearRectKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateRect(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearSingleKeyFrame : SingleKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearSingleKeyFrame.
        /// 

        public LinearSingleKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearSingleKeyFrame. 
        /// 

        public LinearSingleKeyFrame(Single value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearSingleKeyFrame.
        /// 

        public LinearSingleKeyFrame(Single value, KeyTime keyTime)
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
            return new LinearSingleKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateSingle(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value.
    /// 

    public partial class LinearSizeKeyFrame : SizeKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearSizeKeyFrame. 
        /// 

        public LinearSizeKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearSizeKeyFrame.
        /// 

        public LinearSizeKeyFrame(Size value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearSizeKeyFrame. 
        /// 

        public LinearSizeKeyFrame(Size value, KeyTime keyTime)
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
            return new LinearSizeKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateSize(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearVectorKeyFrame : VectorKeyFrame
    {
        #region Constructors

        /// 

        /// Creates a new LinearVectorKeyFrame.
        /// 

        public LinearVectorKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearVectorKeyFrame.
        /// 

        public LinearVectorKeyFrame(Vector value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearVectorKeyFrame. 
        /// 

        public LinearVectorKeyFrame(Vector value, KeyTime keyTime)
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
            return new LinearVectorKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateVector(baseValue, Value, keyFrameProgress);
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
    /// the previous key frame and its own Value linearly to produce its output value. 
    /// 

    public partial class LinearVector3DKeyFrame : Vector3DKeyFrame
    {
        #region Constructors 

        /// 

        /// Creates a new LinearVector3DKeyFrame. 
        /// 

        public LinearVector3DKeyFrame()
            : base()
        {
        }

        /// 

        /// Creates a new LinearVector3DKeyFrame. 
        /// 

        public LinearVector3DKeyFrame(Vector3D value)
            : base(value)
        {
        }

        /// 

        /// Creates a new LinearVector3DKeyFrame.
        /// 

        public LinearVector3DKeyFrame(Vector3D value, KeyTime keyTime)
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
            return new LinearVector3DKeyFrame();
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
                return AnimatedTypeHelpers.InterpolateVector3D(baseValue, Value, keyFrameProgress);
            }
        }

        #endregion 
    }

} 
