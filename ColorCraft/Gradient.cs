using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace ColorCraft
{
    public class Gradient
    {
        private readonly LerpMode _lerpMode;
        private readonly List<MultiColorSpaceGradientStop> _stops;
        private readonly bool _useGammaCorrection;

        public Gradient(LerpMode mode, List<GradientStop> stops, bool useGammaCorrection)
        {
            _lerpMode = mode;
            _stops = stops.OrderBy(s => s.Offset)
                .Select(x => new MultiColorSpaceGradientStop(x, useGammaCorrection))
                .ToList();
            _useGammaCorrection = useGammaCorrection;
        }

        public Color ColorAt(double progress)
        {
            if (progress < 0 || progress > 1) throw new ArgumentOutOfRangeException(nameof(progress));
            if (_stops.Count == 0) return Colors.Transparent;
            if (_stops.Count == 1) return _stops[0].Color;

            MultiColorSpaceGradientStop startStop = _stops[0];
            MultiColorSpaceGradientStop? endStop = null;

            for (int i = 0; i < _stops.Count; i++)
            {
                if (_stops[i].Offset <= progress)
                {
                    startStop = _stops[i];
                }
                else
                {
                    endStop = _stops[i];
                    break;
                }
            }

            if (endStop == null)
            {
                return startStop.Color;
            }

            var localProgress = (progress - startStop.Offset) / (endStop.Offset - startStop.Offset);

            var lerpedColor = _lerpMode switch
            {
                LerpMode.Rgb => Utils.LerpColor(startStop.Color, endStop.Color, localProgress),
                LerpMode.Srgb => RgbLinear.Lerp(startStop.RgbLinear, endStop.RgbLinear, localProgress).ToColor(_useGammaCorrection),
                LerpMode.Hsl => Hsl.Lerp(startStop.Hsl, endStop.Hsl, localProgress).ToColor(),
                LerpMode.Lab => Lab.Lerp(startStop.Lab, endStop.Lab, localProgress).ToColor(_useGammaCorrection),
                LerpMode.SrgbBrightFix => RgbLinear.BrightFixLerp(startStop.RgbLinear, endStop.RgbLinear, localProgress).ToColor(_useGammaCorrection),
                _ => throw new NotImplementedException(),
            };

            lerpedColor.A = Utils.LerpByte(startStop.Color.A, endStop.Color.A, localProgress);
            return lerpedColor;
        }
    }
}
