using Shapes;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

/*
 * * This is a port of the PulseRings spinner from https://codepen.io/colinhorn/pen/zdNMVy
 * * 
 * * Colin Horn
 * */

namespace XamlSpinners
{
    public partial class PulseRings : Spinner
    {

        public PenLineCap DashCap
        {
            get => (PenLineCap)GetValue(DashCapProperty);
            set => SetValue(DashCapProperty, value);
        }

        public static readonly DependencyProperty DashCapProperty = DependencyProperty.Register(nameof(DashCap), typeof(PenLineCap), typeof(PulseRings), new FrameworkPropertyMetadata(default(PenLineCap), OnDashCapChanged));

        private static void OnDashCapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PulseRings self) return;
            self.OnDashCapChanged(e);
        }

        protected virtual void OnDashCapChanged(DependencyPropertyChangedEventArgs e) { }




        public double RingThickness
        {
            get => (double)GetValue(RingThicknessProperty);
            set => SetValue(RingThicknessProperty, value);
        }

        public static readonly DependencyProperty RingThicknessProperty = DependencyProperty.Register(nameof(RingThickness), typeof(double), typeof(PulseRings), new FrameworkPropertyMetadata(0.01, FrameworkPropertyMetadataOptions.AffectsRender, OnRingThicknessChanged));

        private static void OnRingThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PulseRings self) return;
            self.OnRingThicknessChanged(e);
        }

        public double RingGap
        {
            get => (double)GetValue(RingGapProperty);
            set => SetValue(RingGapProperty, value);
        }

        public static readonly DependencyProperty RingGapProperty = DependencyProperty.Register(nameof(RingGap), typeof(double), typeof(PulseRings), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.AffectsRender, OnRingGapChanged));

        private static void OnRingGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PulseRings self) return;
            self.OnRingGapChanged(e);
        }

        protected virtual void OnRingGapChanged(DependencyPropertyChangedEventArgs e) { }


        protected virtual void OnRingThicknessChanged(DependencyPropertyChangedEventArgs e) { }


        public PulseRings()
        {
            DataContext = this;
            InitializeComponent();
            Palette = new() { Brushes.White, Brushes.White, Brushes.White };
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var totalRingCount = 3;

            var smallestSize = Math.Min(finalSize.Width, finalSize.Height);

            // Calculate ring thickness
            var ringThickness = RingThickness * smallestSize;
            ringThickness = Math.Min(ringThickness, smallestSize / (2 * totalRingCount));

            // Calculate gaps
            var maxGap = (smallestSize - totalRingCount * 2 * ringThickness) / (2 * (totalRingCount - 1));
            var gap = Math.Min(RingGap * smallestSize, maxGap);

            // Arrange rings
            ArrangeRing(OuterRing, 0, smallestSize, ringThickness, gap);
            ArrangeRing(MiddleRing, 1, smallestSize, ringThickness, gap);
            ArrangeRing(InnerRing1, 2, smallestSize, ringThickness, gap);
            ArrangeRing(InnerRing2, 2, smallestSize, ringThickness, gap);

            base.ArrangeOverride(finalSize);

            return finalSize;
        }

        private static void ArrangeRing(DashedEllipse ring, int outerRingCount, double size, double strokeThickness, double gap)
        {
            var offset = outerRingCount * (strokeThickness + gap);
            var dimension = size - 2 * offset;

            ring.Width = dimension;
            ring.Height = dimension;
            ring.StrokeThickness = strokeThickness;
            //ring.Arrange(new Rect(offset, offset, dimension, dimension));
        }

        protected override void OnPaletteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (OuterRing is null || MiddleRing is null || InnerRing1 is null || InnerRing2 is null) return;

            var altBrush = new SolidColorBrush(Colors.Red);

            OuterRing.Stroke = Palette.Count > 0 ? Palette[0] : altBrush;
            MiddleRing.Stroke = Palette.Count > 1 ? Palette[1] : OuterRing.Stroke;
            InnerRing1.Stroke = Palette.Count > 2 ? Palette[2] : MiddleRing.Stroke;
            InnerRing2.Stroke = Palette.Count > 3 ? Palette[3] : InnerRing1.Stroke;
        }
    }
}
