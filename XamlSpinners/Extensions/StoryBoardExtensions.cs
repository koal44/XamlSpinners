using System;
using System.Linq;
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

        public static void AddAnimation(this Storyboard storyboard, AnimationTimeline animation, DependencyObject target, string targetProperty, string targetName, FrameworkElement container)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));
            if (animation == null) throw new ArgumentNullException(nameof(animation));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (string.IsNullOrEmpty(targetProperty)) throw new ArgumentNullException(nameof(targetProperty));
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

        public static void Reverse(this Storyboard storyboard, Spinner containingObject)
        {
            if (storyboard is null) return;
            if (storyboard.Children.Count == 0) return;

            ReverseStoryboard(storyboard, containingObject);
        }

        private static void ReverseStoryboard(Storyboard storyboard, Spinner containingObject)
        {
            if (storyboard.GetCurrentTime(containingObject) is not TimeSpan storyTime)
                throw new Exception($"storyTime was null");

            for (int i = 0; i < storyboard.Children.Count; i++)
            {
                Timeline? child = storyboard.Children[i];

                if (child is not AnimationTimeline animation)
                    throw new Exception($"child was not an AnimationTimeline");

                switch (animation)
                {
                    case DoubleAnimation doubleAnimation:
                        ReverseDoubleAnimation(doubleAnimation, storyTime);
                        break;
                    default:
                        throw new Exception($"animation reversing is not supported");
                        // TODO: Add support for other animations
                }
            }

            storyboard.Stop(containingObject);
            storyboard.Begin(containingObject, true);
        }

        public static void ReverseDoubleAnimation(DoubleAnimation ani, TimeSpan storyboardTime)
        {
            if (ani.To is null || ani.From is null || ani.RepeatBehavior != RepeatBehavior.Forever)
                throw new NotImplementedException("Only cyclic double animations with From and To values are supported");

            var From = ani.From.Value;
            var To = ani.To.Value;
            ani.From = To;
            ani.To = From;
            ani.BeginTime ??= TimeSpan.Zero;

            var duration = ani.Duration.TimeSpan;
            var totalDuration = ani.AutoReverse ? duration + duration : duration;

            TimeSpan progressTime = TimeSpan.FromTicks((storyboardTime - ani.BeginTime.Value).Ticks % totalDuration.Ticks);

            TimeSpan reversedProgressTime = ani.AutoReverse
                ? (progressTime <= duration
                    ? duration - progressTime
                    : totalDuration - (progressTime - duration))
                : duration - progressTime;

            bool hasAnimationStarted = storyboardTime >= ani.BeginTime.Value;

            ani.BeginTime = hasAnimationStarted
                ? -reversedProgressTime
                : ani.BeginTime.Value - storyboardTime;
        }

    }
}
