using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public abstract class Spinner : UserControl
    {
        #region Data

        internal Storyboard ActiveStoryboard { get; set; }

        #endregion

        #region Constructors

        public Spinner()
        {
            Palette ??= new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Utils.ColorUtils.HslToRgb(270, 1, 0.7)),
                new SolidColorBrush(Utils.ColorUtils.HslToRgb(210, 1, 0.3)),
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

        internal virtual void OnIsActiveChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateActiveStoryboard();
        }

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

        protected virtual void OnPaletteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

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
            ActiveStoryboard.SetSpeedRatio(Speed);
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

        internal void UpdateActiveStoryboard()
        {
            if (IsActive)
                ActiveStoryboard?.Begin();
            else
                ActiveStoryboard?.Stop();
        }

        #endregion

    }
}
