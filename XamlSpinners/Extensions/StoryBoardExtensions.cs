using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XamlSpinners
{
    public static class StoryboardExtensions
    {
        /// <summary>
        /// Adds an animation to the specified storyboard and associates it with a target and its property.
        /// This method registers the target with a name, which is one way to ensure that the target is findable by the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to which the animation will be added.</param>
        /// <param name="animation">The animation timeline to be added to the storyboard.</param>
        /// <param name="target">The object to which the animation will be applied.</param>
        /// <param name="targetProperty">The dependency property that the animation targets.</param>
        /// <param name="targetName">The target will be registered with this name</param>
        /// <param name="container">The naming container (typically 'this' in the calling context).</param>
        public static void AddAnimation(this Storyboard storyboard, AnimationTimeline animation, DependencyObject target, DependencyProperty targetProperty, string targetName, FrameworkElement container)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));
            if (string.IsNullOrEmpty(targetName)) throw new ArgumentNullException(nameof(targetName));
            if (container == null) throw new ArgumentNullException(nameof(container));

            var existingTarget = container.FindName(targetName);
            if (existingTarget != null && !ReferenceEquals(existingTarget, target))
            {
                throw new ArgumentException($"A different target with the name '{targetName}' already exists in '{container.GetType().Name}:{container.Name}'.");
            }

            if (existingTarget == null)
            {
                container.RegisterName(targetName, target);
            }

            container.RegisterName(targetName, target);
            Storyboard.SetTargetName(animation, targetName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(targetProperty));
            storyboard.Children.Add(animation);
        }


        public static void AddAnimation(this Storyboard storyboard, AnimationTimeline animation, DependencyObject targetProperty, DependencyProperty property)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));

            Storyboard.SetTarget(animation, targetProperty);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            storyboard.Children.Add(animation);
        }

        public static void AddAnimation(this Storyboard storyboard, AnimationTimeline animation, DependencyObject targetProperty, string property)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));

            Storyboard.SetTarget(animation, targetProperty);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            storyboard.Children.Add(animation);
        }
    }

}
