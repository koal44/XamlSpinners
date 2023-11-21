using System.Windows;

namespace ProjectionCanvas
{
    public class ProjectableShapeGroup : ProjectableShape
    {
        public ProjectableShapeCollection Children
        {
            get => (ProjectableShapeCollection)GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(nameof(Children), typeof(ProjectableShapeCollection), typeof(ProjectableShapeGroup), new PropertyMetadata(null, OnChildrenChanged));

        private static void OnChildrenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProjectableShapeGroup self) return;
            //self.OnChildrenChanged(e);

            if (e.OldValue is ProjectableShapeCollection oldCollection && !oldCollection.IsFrozen)
            {
                //oldCollection.CollectionChanged -= OnCollectionChanged;
            }
            if (e.NewValue is ProjectableShapeCollection newCollection && !newCollection.IsFrozen)
            {
                //newCollection.CollectionChanged += OnCollectionChanged;
            }
        }


        public ProjectableShapeGroup()
        {
            Children = new ProjectableShapeCollection();
        }


        public new ProjectableShapeGroup Clone() => (ProjectableShapeGroup)base.Clone();
        public new ProjectableShapeGroup CloneCurrentValue() => (ProjectableShapeGroup)base.CloneCurrentValue();
        protected override Freezable CreateInstanceCore() => new ProjectableShapeGroup();

        //internal static ProjectableShapeCollection s_Children = ProjectableShapeCollection.Empty;

    }

}