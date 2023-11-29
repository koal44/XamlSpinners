using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public static class StoryboardExtensions
    {
        public static void AddAnimation(this Storyboard storyboard, AnimationTimeline animation, DependencyObject target, DependencyProperty property)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (target == null) throw new ArgumentNullException(nameof(target));

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            storyboard.Children.Add(animation);
        }

        public static void AddAnimation(this Storyboard storyboard, AnimationTimeline animation, DependencyObject target, string property)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (target == null) throw new ArgumentNullException(nameof(target));

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            storyboard.Children.Add(animation);
        }
    }

}
