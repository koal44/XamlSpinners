using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Animations.Demo
{
    public partial class AnimatedDashOnCircle : UserControl
    {
        private readonly Storyboard _storyboard;
        private readonly Shape _animatedEllipse;

        public AnimatedDashOnCircle()
        {
            _storyboard = new Storyboard();
            _animatedEllipse = new Ellipse
            {
                Stroke = Foreground,
                StrokeThickness = 5,
                Width = 100,
                Height = 100,
                StrokeDashArray = new DoubleCollection { 5, 5 },
                StrokeDashOffset = 0,
                StrokeDashCap = PenLineCap.Flat,
            };

            DataContext = this;
            Loaded += OnUserControlLoaded;

            InitializeComponent();
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            RootContainer.Children.Add(_animatedEllipse);
            SetupAnimations();
            _storyboard.Begin();
        }

        private void SetupAnimations()
        {
            var dashAnimation = new DoubleCollectionAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromSeconds(2),
                RepeatBehavior = RepeatBehavior.Forever
            };
            dashAnimation.KeyFrames.Add(new LinearDoubleCollectionKeyFrame(new DoubleCollection() { 5, 5}, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
            dashAnimation.KeyFrames.Add(new LinearDoubleCollectionKeyFrame(new DoubleCollection() { 10, 5 }, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.75))));
            dashAnimation.KeyFrames.Add(new LinearDoubleCollectionKeyFrame(new DoubleCollection() { 10, 5 }, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.5))));

            //Storyboard.SetTarget(animation, target);
            //Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            //storyboard.Children.Add(animation);

            //_storyboard.AddAnimation(dashAnimation, _animatedEllipse, Shape.StrokeDashArrayProperty);
            Storyboard.SetTarget(dashAnimation, _animatedEllipse);
            Storyboard.SetTargetProperty(dashAnimation, new PropertyPath(Shape.StrokeDashArrayProperty));
            _storyboard.Children.Add(dashAnimation);

            //_storyboard.AddAnimation(dashAnimation, _animatedEllipse, Shape.StrokeDashOffsetProperty);
            //_storyboard.AddAnimation(dashOffsetAnimation, _animatedEllipse, Shape.StrokeDashOffsetProperty);
        }

    }
}
