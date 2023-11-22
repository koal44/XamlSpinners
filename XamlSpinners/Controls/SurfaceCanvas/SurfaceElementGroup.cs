using System.Collections.Specialized;
using System.Windows;

namespace XamlSpinners
{
    public class SurfaceElementGroup : SurfaceElement
    {
        #region Dependency Properties

        public SurfaceElementCollection<SurfaceElement> Children
        {
            get => (SurfaceElementCollection<SurfaceElement>)GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(nameof(Children), typeof(SurfaceElementCollection<SurfaceElement>), typeof(SurfaceElementGroup), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnChildrenChanged));

        private static void OnChildrenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SurfaceElementGroup self) return;
            self.OnChildrenChanged(e);
        }

        private void OnChildrenChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is SurfaceElementCollection<SurfaceElement> oldCollection && !oldCollection.IsFrozen)
            {
                oldCollection.CollectionChanged -= OnCollectionChanged;
            }
            if (e.NewValue is SurfaceElementCollection<SurfaceElement> newCollection && !newCollection.IsFrozen)
            {
                newCollection.CollectionChanged += OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new(ChildrenProperty, e.NewItems, e.OldItems));
            //InvalidateProperty(ChildrenProperty);
        }

        #endregion

        #region Constructors

        public SurfaceElementGroup()
        {
            Children = new SurfaceElementCollection<SurfaceElement>();
        }

        #endregion

        #region Freezable

        public new SurfaceElementGroup Clone() => (SurfaceElementGroup)base.Clone();
        public new SurfaceElementGroup CloneCurrentValue() => (SurfaceElementGroup)base.CloneCurrentValue();
        protected override Freezable CreateInstanceCore() => new SurfaceElementGroup();
        protected override void CloneCore(Freezable sourceFreezable)
        {
            base.CloneCore(sourceFreezable);
            var source = (SurfaceElementGroup)sourceFreezable;

            Children = source.Children.Clone();
        }

        #endregion

    }
}