using System.Windows.Media;

namespace ColorCraft
{
    public class MultiColorSpaceGradientStop
    {
        private readonly bool _useGammaCorrection;
        private readonly double _offset;

        // Cached color spaces
        private readonly Color _color;
        private RgbLinear? _rgbLinear;
        private Hsl? _hsl;
        private Lab? _lab;

        public double Offset => _offset;

        public Color Color => _color;

        public RgbLinear RgbLinear => _rgbLinear ??= RgbLinear.FromColor(_color, _useGammaCorrection);

        public Hsl Hsl => _hsl ??= Hsl.FromColor(_color);

        public Lab Lab => _lab ??= Lab.FromColor(_color, _useGammaCorrection);

        public MultiColorSpaceGradientStop(Color color, double offset, bool useGammaCorrection)
        {
            _color = color;
            _offset = offset;
            _useGammaCorrection = useGammaCorrection;
        }

        public MultiColorSpaceGradientStop(GradientStop stop, bool useGammaCorrection)
        {
            _color = stop.Color;
            _offset = stop.Offset;
            _useGammaCorrection = useGammaCorrection;
        }
    }
}