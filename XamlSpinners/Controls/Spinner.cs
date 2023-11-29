using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace XamlSpinners
{
    public abstract class Spinner : UserControl
    {
        #region Data

        public Storyboard ActiveStoryboard { get; set; }
        public bool HasClock { get; set;} = false;

        #endregion

        #region Constructors

        static Spinner()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(Spinner), new FrameworkPropertyMetadata(typeof(Spinner)));
            TypeDescriptor.AddAttributes(typeof(SpiralSphere), new TypeConverterAttribute(typeof(PaletteConverter)));
        }

        public Spinner()
        {
            Palette ??= new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Utils.ColorUtils.HslToRgb(270, 1, 0.7)),
                new SolidColorBrush(Utils.ColorUtils.HslToRgb(10, 1, 0.3)),
            };
            ActiveStoryboard = new Storyboard();
        }

        #endregion

        #region Dependency Properties

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(Spinner), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, OnIsActiveChanged));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnIsActiveChanged(e);
        }

        protected virtual void OnIsActiveChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateActiveStoryboard();
        }

        [TypeConverter(typeof(PaletteConverter))]
        public ObservableCollection<Brush> Palette
        {
            get => (ObservableCollection<Brush>)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(nameof(Palette), typeof(ObservableCollection<Brush>), typeof(Spinner), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnPaletteChanged));

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnPaletteChanged(e);
        }

        private void OnPaletteChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ObservableCollection<Brush> oldCollection)
            {
                oldCollection.CollectionChanged -= OnPaletteCollectionChanged;
            }

            if (e.NewValue is ObservableCollection<Brush> newCollection)
            {
                newCollection.CollectionChanged += OnPaletteCollectionChanged;
            }
        }

        protected abstract void OnPaletteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e);

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(Spinner), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, OnIsIndeterminateChanged));

        private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnIsIndeterminateChanged(e);
        }

        protected virtual void OnIsIndeterminateChanged(DependencyPropertyChangedEventArgs e) { }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(Spinner), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnMinimumChanged));

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnMinimumChanged(e);
        }

        protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs e) { }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(Spinner), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender, OnMaximumChanged));

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnMaximumChanged(e);
        }

        protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs e) { }

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(Spinner), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnProgressChanged));

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnProgressChanged(e);
        }

        protected virtual void OnProgressChanged(DependencyPropertyChangedEventArgs e) { }

        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(nameof(Speed), typeof(double), typeof(Spinner), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender, OnSpeedChanged));

        private static void OnSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Spinner self) return;
            self.OnSpeedChanged(e);
        }

        protected virtual void OnSpeedChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is not double oldSpeed || e.NewValue is not double newSpeed || oldSpeed == newSpeed)
            {
                return;
            }

            if (oldSpeed * newSpeed < 0)
            {
                ReverseStoryboard();
            }

            // set minimum speed to 0.0001 to avoid divide by zero errors
            ActiveStoryboard.SetSpeedRatio(this, Math.Max(Math.Abs(Speed), 0.0001));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("ActiveStoryboard") is Storyboard xamlStoryboard)
            {
                ActiveStoryboard = xamlStoryboard;
            }
        }

        #endregion

        #region Methods

        protected void UpdateActiveStoryboard()
        {
            if (ActiveStoryboard == null) return;

            if (IsActive)
            {
                if (!HasClock)
                {
                    ActiveStoryboard.Begin(this, true);
                    HasClock = true;
                    return;
                }

                ActiveStoryboard.Resume(this);

            }
            else
            {
                if (!HasClock) return;
                var state = ActiveStoryboard.GetCurrentState(this);
                if (state == ClockState.Stopped) return;
                ActiveStoryboard.Pause(this);
            }
        }

        private void ReverseStoryboard()
        {
            if (ActiveStoryboard is null) return;
            if (ActiveStoryboard.Children.Count == 0) return;

            if (ActiveStoryboard.GetCurrentTime(this) is not TimeSpan storyTime)
                throw new Exception($"storyTime was null");

            var duration = TimeSpan.Zero;
            var reversedStoryboard = new Storyboard();

            foreach (var child in ActiveStoryboard.Children)
            {
                if (child is not AnimationTimeline animation)
                    throw new Exception($"child was not an AnimationTimeline");
                if (duration == TimeSpan.Zero)
                    duration = animation.Duration.TimeSpan;
                if (duration != TimeSpan.Zero && duration != animation.Duration.TimeSpan)
                    throw new Exception($"duration was not consistent");

                switch (animation)
                {
                    case DoubleAnimation doubleAnimation:
                        var reversedDoubleAnimation = doubleAnimation.Clone();
                        reversedDoubleAnimation.From = doubleAnimation.To;
                        reversedDoubleAnimation.To = doubleAnimation.From;

                        Storyboard.SetTarget(reversedDoubleAnimation, Storyboard.GetTarget(doubleAnimation));
                        Storyboard.SetTargetProperty(reversedDoubleAnimation, Storyboard.GetTargetProperty(doubleAnimation));
                        reversedStoryboard.Children.Add(reversedDoubleAnimation);

                        break;
                    case IKeyFrameAnimation keyFrameAnimation:
                        //keyFrameAnimation.KeyFrames.Reverse();
                        break;
                    default:
                        throw new Exception($"animation reversing is not supported");
                        // TODO: Add support for other animations:
                        // ColorAnimation, PointAnimation, RectAnimation, SizeAnimation, ThicknessAnimation,
                        // DoubleCollection, PathFigureCollection, PointCollection, Point3Collection, VectorCollection
                }
            }

            if (duration == TimeSpan.Zero) return;

            var progress = TimeSpan.FromTicks(storyTime.Ticks % duration.Ticks);
            var reversedProgress = duration - progress;

            ActiveStoryboard.Stop(this);
            ActiveStoryboard = reversedStoryboard;

            ActiveStoryboard.Begin(this, true);
            ActiveStoryboard.Seek(this, reversedProgress, TimeSeekOrigin.BeginTime);

            // if isActive is false then reversing triggers it the animation to start again. so we
            // must delay the update until after the begin/seek have had a chance to change state.
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateActiveStoryboard();
            }), DispatcherPriority.ContextIdle);
        }

        #endregion

    }
}
