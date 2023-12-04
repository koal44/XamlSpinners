using FluentAssertions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XamlSpinners.Tests
{
    public class PathParserTests
    {
        private readonly AnimatedButtonsUserControl _animatedButtonsUserControl;

        public PathParserTests()
        {
            _animatedButtonsUserControl = new AnimatedButtonsUserControl();
            //_animatedButtonsUserControl.Show(); // Necessary to properly initialize visual elements
        }

        [Fact]
        public void TestIndexedPropertyPath()
        {
            var target = _animatedButtonsUserControl.IndexedPropertyButton;
            var path = new PropertyPath("RenderTransform.Children[1].Angle");

            var (finalTarget, finalProperty) = new PathParser().ResolveFinalAnimationTargetAndProperty(target!, path);

            // Assert
            var transformGroup = target!.RenderTransform as TransformGroup;
            transformGroup.Should().NotBeNull();
            transformGroup!.Children.Should().HaveCount(2);
            var rotateTransform = transformGroup.Children[1] as System.Windows.Media.RotateTransform;
            rotateTransform.Should().NotBeNull();

            finalTarget.Should().Be(rotateTransform);
            finalProperty.Should().Be(System.Windows.Media.RotateTransform.AngleProperty);
        }

        [Fact]
        public void TestSinglePropertyPath()
        {
            var target = _animatedButtonsUserControl.SinglePropertyButton;
            var path = new PropertyPath(UIElement.OpacityProperty);

            var (finalTarget, finalProperty) = new PathParser().ResolveFinalAnimationTargetAndProperty(target!, path);

            // Assert
            finalTarget.Should().Be(target);
            finalProperty.Should().Be(UIElement.OpacityProperty);
        }

        [Fact]
        public void TestIndirectPropertyPath()
        {
            var target = _animatedButtonsUserControl.IndirectPropertyButton;
            var path = new PropertyPath("Background.Color");

            var (finalTarget, finalProperty) = new PathParser().ResolveFinalAnimationTargetAndProperty(target!, path);

            // Assert
            var brush = target!.Background as SolidColorBrush;
            brush.Should().NotBeNull();
            finalTarget.Should().Be(brush);
            finalProperty.Should().Be(SolidColorBrush.ColorProperty);
        }

        [Fact]
        public void TestAttachedPropertyPath()
        {
            var target = _animatedButtonsUserControl.AttachedPropertyButton;
            var path = new PropertyPath(Canvas.LeftProperty);

            var (finalTarget, finalProperty) = new PathParser().ResolveFinalAnimationTargetAndProperty(target!, path);

            // Assert
            finalTarget.Should().Be(target);
            finalProperty.Should().Be(Canvas.LeftProperty);
        }
    }

    public class AnimatedButtonsUserControl : UserControl
    {
        private readonly Storyboard _storyboard;

        public Button? IndexedPropertyButton { get; set; }
        public Button? SinglePropertyButton { get; set; }
        public Button? IndirectPropertyButton { get; set; }
        public Button? AttachedPropertyButton { get; set; }

        public AnimatedButtonsUserControl()
        {
            //InitializeComponent();
            _storyboard = new Storyboard();

            CreateIndexedPropertyButton();
            CreateSinglePropertyButton();
            CreateIndirectPropertyButton();
            CreateAttachedPropertyButton();

            SetupUI();

            //_storyboard.Begin(this);
        }

        private void SetupUI()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            stackPanel.Children.Add(IndexedPropertyButton);
            stackPanel.Children.Add(SinglePropertyButton);
            stackPanel.Children.Add(IndirectPropertyButton);

            var attachedPropertyCanvas = new Canvas();
            attachedPropertyCanvas.Children.Add(AttachedPropertyButton);
            attachedPropertyCanvas.Width = 200;
            attachedPropertyCanvas.Height = 200;
            stackPanel.Children.Add(attachedPropertyCanvas);

            this.Content = stackPanel;
        }

        private void CreateIndexedPropertyButton()
        {
            IndexedPropertyButton = new Button
            {
                Width = 100,
                Height = 100,
                Content = "Indexed Prop"
            };

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(1, 1));

            var rotateTransform = new System.Windows.Media.RotateTransform(0);
            transformGroup.Children.Add(rotateTransform);

            IndexedPropertyButton.RenderTransform = transformGroup;

            var name = "IndexedPropertyButton";
            RegisterName(name, IndexedPropertyButton);

            var rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(5),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTargetName(rotateAnimation, name);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("RenderTransform.Children[1].Angle"));
            _storyboard.Children.Add(rotateAnimation);

        }

        private void CreateSinglePropertyButton()
        {
            SinglePropertyButton = new Button
            {
                Width = 100,
                Height = 100,
                Content = "Single Prop",
                Margin = new Thickness(10)
            };

            var name = "SinglePropertyButton";
            RegisterName(name, SinglePropertyButton);

            var opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTargetName(opacityAnimation, name);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
            _storyboard.Children.Add(opacityAnimation);
        }

        private void CreateIndirectPropertyButton()
        {
            IndirectPropertyButton = new Button
            {
                Width = 100,
                Height = 100,
                Content = "Indirect Prop",
                Margin = new Thickness(10)
            };

            var name = "IndirectPropertyButton";
            RegisterName(name, IndirectPropertyButton);

            var colorAnimation = new ColorAnimation
            {
                From = Colors.Red,
                To = Colors.Blue,
                Duration = TimeSpan.FromSeconds(3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            var brush = new SolidColorBrush(Colors.Red);
            IndirectPropertyButton.Background = brush;

            Storyboard.SetTargetName(colorAnimation, name);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Background.Color"));
            _storyboard.Children.Add(colorAnimation);
        }

        private void CreateAttachedPropertyButton()
        {
            AttachedPropertyButton = new Button
            {
                Width = 100,
                Height = 100,
                Content = "Attached Prop",
                Margin = new Thickness(10)
            };

            var name = "AttachedPropertyButton";
            RegisterName(name, AttachedPropertyButton);

            var moveAnimation = new DoubleAnimation
            {
                From = 0,
                To = 100,
                Duration = TimeSpan.FromSeconds(4),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            Canvas.SetLeft(AttachedPropertyButton, 0);

            Storyboard.SetTargetName(moveAnimation, name);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath(Canvas.LeftProperty));
            _storyboard.Children.Add(moveAnimation);
        }

    }
}
