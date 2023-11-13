using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations.Demo
{
    public partial class AnimatedSegmentsOnOneTwoMorph : UserControl
    {
        private readonly Storyboard _storyboard;

        private readonly string _dataFrom = "m 27,39 h -20.0 v -3.8 h 7.7 v -12.4 v -6.2 v -3.1 v -3.1 h -3.8 h -1.9 h -1.0 h -1.0 v -3.4 c 1.0 0.0 2.2 -0.1 3.3 -0.2 c 1.2 -0.2 2.1 -0.4 2.7 -0.8 c 0.8 -0.4 1.4 -0.9 1.8 -1.6 c 0.4 -0.6 0.7 -1.5 0.8 -2.6 h 3.8 v 8.3 v 4.2 v 4.2 v 4.2 v 4.2 v 4.2 v 4.2 h 7.5 z";

        private readonly string _dataTo = "m 27,39 h -25.0 v -5.2 c 1.7 -1.5 3.5 -3.0 5.2 -4.5 c 1.8 -1.5 3.4 -3.0 4.9 -4.4 c 3.2 -3.1 5.3 -5.5 6.5 -7.3 c 1.2 -1.8 1.8 -3.8 1.8 -5.9 c 0.0 -1.9 -0.6 -3.4 -1.9 -4.5 c -1.3 -1.1 -3.0 -1.6 -5.3 -1.6 c -1.5 0.0 -3.1 0.3 -4.9 0.8 c -1.8 0.5 -3.5 1.3 -5.1 2.4 h -0.2 v -5.2 c 0.6 -0.3 1.3 -0.6 2.1 -0.8 c 0.8 -0.3 1.7 -0.5 2.6 -0.8 c 1.0 -0.2 2.0 -0.4 2.9 -0.5 c 1.0 -0.1 1.9 -0.2 2.8 -0.2 c 3.8 0.0 6.8 0.9 8.9 2.8 c 2.1 1.8 3.2 4.3 3.2 7.4 c 0.0 1.4 -0.2 2.7 -0.5 3.9 c -0.3 1.2 -0.9 2.4 -1.6 3.4 c -0.6 1.0 -1.4 2.0 -2.3 3.0 c -0.9 1.0 -1.9 2.1 -3.2 3.3 c -1.8 1.7 -3.6 3.4 -5.5 5.1 c -1.9 1.6 -3.6 3.1 -5.3 4.5 h 19.9 z";

        public AnimatedSegmentsOnOneTwoMorph()
        {
            _storyboard = new Storyboard();
            DataContext = this;
            Loaded += UserControl4_Loaded;

            InitializeComponent();
        }

        private async void UserControl4_Loaded(object sender, RoutedEventArgs e)
        {
            // Create PathFigureCollection for 'From' and 'To'
            var from = Utils.ParsePathData(_dataFrom);
            var to = Utils.ParsePathData(_dataTo);

            var pathFigureCollectionAnimation = new PathFigureCollectionAnimation
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(2)),
                From = from,
                To = to
            };

            // var animatedGeometry = new PathGeometry() { Figures = from };
            var animatedGeometry = new PathGeometry() { };
            AnimatedPath.Data = animatedGeometry;






            //var expectedBezierSegments = new PathSegmentCollection()
            //{
            //    new BezierSegment(new Point(0, 10), new Point(0, 20), new Point(0, 30), true),
            //    new BezierSegment(new Point(10, 30), new Point(20, 30), new Point(30, 30), true),
            //    new BezierSegment(new Point(30, 0), new Point(15, 0), new Point(0,0), true)
            //};

            //var expectedPathFigure = new PathFigure(new Point(0, 0), expectedBezierSegments, true);
            //var expectedPathFigureCollection = new PathFigureCollection() { expectedPathFigure };

            //var expectedPathGeometry = new PathGeometry(expectedPathFigureCollection);
            //AnimatedPath.Data = expectedPathGeometry;
            //AnimatedPath.Fill = Brushes.Transparent;
            //AnimatedPath.Stroke = Brushes.Black;
            //AnimatedPath.StrokeThickness = 3;







            // Create and configure the Storyboard
            //Storyboard.SetTarget(pathFigureCollectionAnimation, animatedGeometry);
            //Storyboard.SetTargetProperty(pathFigureCollectionAnimation, new PropertyPath(PathGeometry.FiguresProperty));
            //_storyboard.Children.Add(pathFigureCollectionAnimation);




            Storyboard.SetTarget(pathFigureCollectionAnimation, AnimatedPath);
            Storyboard.SetTargetProperty(pathFigureCollectionAnimation, new PropertyPath("Data.Figures"));
            _storyboard.Children.Add(pathFigureCollectionAnimation);

            _storyboard.Begin();

            await Task.Delay(1000);
            Debug.WriteLine($"Original From:\n {Utils.PrintPathFigureCollection(from)}");
            Debug.WriteLine($"AnimatedPath.Data.Figures:\n {Utils.PrintPathFigureCollection(animatedGeometry.Figures)}");
        }

    }
}
