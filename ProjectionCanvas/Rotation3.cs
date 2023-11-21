using System.Numerics;
using System.Windows.Media.Animation;

namespace ProjectionCanvas
{
    public abstract class Rotation3 : Animatable
    {
        private static readonly Rotation3 s_identity;

        static Rotation3()
        {
            // Create our singleton frozen instance
            s_identity = new QuaternionRotation3();
            s_identity.Freeze();
        }

        internal Rotation3() { }

        public static Rotation3 Identity => s_identity;

        // Used by animation to get a snapshot of the current rotational
        // configuration for interpolation in Rotation3DAnimations.
        internal abstract Quaternion InternalQuaternion { get; }


        public new Rotation3 Clone() => (Rotation3)base.Clone();

        public new Rotation3 CloneCurrentValue() => (Rotation3)base.CloneCurrentValue();

    }
}