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

            OnPaletteCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
                ActiveStoryboard.Reverse(this);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateActiveStoryboard();
                }), DispatcherPriority.ContextIdle);
            }

            // set minimum speed to 0.0001 to avoid divide by zero errors
            ActiveStoryboard.SetSpeedRatio(this, Math.Max(Math.Abs(Speed), 0.0001));
        }

        #endregion

        #region Overrides

        // set and activate any storyboard defined in xaml
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("ActiveStoryboard") is Storyboard xamlStoryboard)
            {
                ActiveStoryboard = xamlStoryboard;
                UpdateActiveStoryboard();
            }

            var storyboardResource = this.TryFindResource("ActiveStoryboard");
            if (storyboardResource is Storyboard resourceStoryboard)
            {
                ActiveStoryboard = resourceStoryboard;
                UpdateActiveStoryboard();
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

        #endregion
    }
}
