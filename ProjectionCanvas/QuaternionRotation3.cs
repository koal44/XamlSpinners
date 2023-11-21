using System.Numerics;
using System.Windows;

namespace ProjectionCanvas
{
    public class QuaternionRotation3 : Rotation3
    {
        private Quaternion _cachedQuaternionValue = Quaternion.Identity;
        internal static Quaternion s_Quaternion = Quaternion.Identity;


        public Quaternion Quaternion
        {
            get => (Quaternion)GetValue(QuaternionProperty);
            set => SetValue(QuaternionProperty, value);
        }

        public static readonly DependencyProperty QuaternionProperty = DependencyProperty.Register(nameof(Quaternion), typeof(Quaternion), typeof(QuaternionRotation3), new PropertyMetadata(Quaternion.Identity, OnQuaternionChanged));

        private static void OnQuaternionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not QuaternionRotation3 self) return;
            self._cachedQuaternionValue = (Quaternion)e.NewValue;

            //self.OnQuaternionChanged(e);
        }


        public QuaternionRotation3() { }
        

        public QuaternionRotation3(Quaternion quaternion)
        {
            Quaternion = quaternion;
        }


        // Used by animation to get a snapshot of the current rotational
        // configuration for interpolation in Rotation3DAnimations.
        internal override Quaternion InternalQuaternion => _cachedQuaternionValue;


        public new QuaternionRotation3 Clone() => (QuaternionRotation3)base.Clone();

        public new QuaternionRotation3 CloneCurrentValue() => (QuaternionRotation3)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new QuaternionRotation3();

    }
}