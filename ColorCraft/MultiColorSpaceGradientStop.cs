using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ColorCraft
{
    public class MultiColorSpaceGradientStop : Animatable
    {
        #region Fields

        // Cached color spaces
        private RgbLinear? _rgbLinear;
        private Hsl? _hsl;
        private Lab? _lab;

        #endregion

        #region Dependency Properties

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(MultiColorSpaceGradientStop), new FrameworkPropertyMetadata(default(Color), OnColorChanged));

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MultiColorSpaceGradientStop self) return;
            self.OnColorChanged(e);
        }

        protected virtual void OnColorChanged(DependencyPropertyChangedEventArgs e) { }

        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(nameof(Offset), typeof(double), typeof(MultiColorSpaceGradientStop), new FrameworkPropertyMetadata(default(double), OnOffsetChanged));

        private static void OnOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MultiColorSpaceGradientStop self) return;
            self.OnOffsetChanged(e);
        }

        protected virtual void OnOffsetChanged(DependencyPropertyChangedEventArgs e) { }

        public bool UseGammaCorrection
        {
            get => (bool)GetValue(UseGammaCorrectionProperty);
            set => SetValue(UseGammaCorrectionProperty, value);
        }

        public static readonly DependencyProperty UseGammaCorrectionProperty = DependencyProperty.Register(nameof(UseGammaCorrection), typeof(bool), typeof(MultiColorSpaceGradientStop), new FrameworkPropertyMetadata(default(bool), OnUseGammaCorrectionChanged));

        private static void OnUseGammaCorrectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MultiColorSpaceGradientStop self) return;
            self.OnUseGammaCorrectionChanged(e);
        }

        protected virtual void OnUseGammaCorrectionChanged(DependencyPropertyChangedEventArgs e) { }

        #endregion

        #region Properties

        public RgbLinear RgbLinear => _rgbLinear ??= RgbLinear.FromColor(Color, UseGammaCorrection);

        public Hsl Hsl => _hsl ??= Hsl.FromColor(Color);

        public Lab Lab => _lab ??= Lab.FromColor(Color, UseGammaCorrection);

        #endregion

        #region Constructors

        public MultiColorSpaceGradientStop() { }

        public MultiColorSpaceGradientStop(Color color, double offset, bool useGammaCorrection)
        {
            Color = color;
            Offset = offset;
            UseGammaCorrection = useGammaCorrection;
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new MultiColorSpaceGradientStop();

        public new MultiColorSpaceGradientStop Clone() => (MultiColorSpaceGradientStop)base.Clone();

        public new MultiColorSpaceGradientStop CloneCurrentValue() => (MultiColorSpaceGradientStop)base.CloneCurrentValue();

        #endregion
    }
}