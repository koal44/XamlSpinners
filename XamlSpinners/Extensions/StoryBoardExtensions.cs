using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using XamlSpinners.Extensions;
using XamlSpinners.Utils;

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

            // Check if all animations have the same duration
            bool sameDuration = storyboard.Children
                .OfType<Timeline>()
                .Select(a => a.Duration)
                .Distinct()
                .Count() == 1;

            ReverseStoryboard(storyboard, containingObject);
            return;

            if (sameDuration)
            {
                ReverseStoryboardSimple(storyboard, containingObject);
            }
            else
            {
                ReverseStoryboardComplex(storyboard, containingObject);
            }

        }

        private static Exception InvalideFeOrFceDependencyObjectException(DependencyObject obj) => new InvalidOperationException($"The DependencyObject must be either a FrameworkElement or FrameworkContentElement, but was {obj.GetType().Name}.");

        private static TimeSpan? GetCurrentTime(this Storyboard storyboard, DependencyObject containingObject)
        {
            return (containingObject) switch
            {
                FrameworkElement fe => storyboard.GetCurrentTime(fe),
                FrameworkContentElement fce => storyboard.GetCurrentTime(fce),
                _ => throw InvalideFeOrFceDependencyObjectException(containingObject)
            };
        }

        private static void Begin(this Storyboard storyboard, DependencyObject containingObject, bool isControllable)
        {
            switch (containingObject)
            {
                case FrameworkElement fe: storyboard.Begin(fe, isControllable); break;
                case FrameworkContentElement fce: storyboard.Begin(fce, isControllable); break;
                default: throw InvalideFeOrFceDependencyObjectException(containingObject);
            }
        }

        private static void Seek(this Storyboard storyboard, DependencyObject containingObject, TimeSpan offset, TimeSeekOrigin origin)
        {
            switch (containingObject)
            {
                case FrameworkElement fe: storyboard.Seek(fe, offset, origin); break;
                case FrameworkContentElement fce: storyboard.Seek(fce, offset, origin); break;
                default: throw InvalideFeOrFceDependencyObjectException(containingObject);
            }
        }

        private static void Pause(this Storyboard storyboard, DependencyObject containingObject)
        {
            switch (containingObject)
            {
                case FrameworkElement fe: storyboard.Pause(fe); break;
                case FrameworkContentElement fce: storyboard.Pause(fce); break;
                default: throw InvalideFeOrFceDependencyObjectException(containingObject);
            }
        }

        private static void Resume(this Storyboard storyboard, DependencyObject containingObject)
        {
            switch (containingObject)
            {
                case FrameworkElement fe: storyboard.Resume(fe); break;
                case FrameworkContentElement fce: storyboard.Resume(fce); break;
                default: throw InvalideFeOrFceDependencyObjectException(containingObject);
            }
        }

        private static void Stop(this Storyboard storyboard, DependencyObject containingObject)
        {
            switch (containingObject)
            {
                case FrameworkElement fe: storyboard.Stop(fe); break;
                case FrameworkContentElement fce: storyboard.Stop(fce); break;
                default: throw InvalideFeOrFceDependencyObjectException(containingObject);
            }
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
                    case IKeyFrameAnimation keyFrameAnimation:
                        //keyFrameAnimation.KeyFrames.Reverse();
                        break;
                    default:
                        throw new Exception($"animation reversing is not supported");
                        // TODO: Add support for other animations:
                        // ColorAnimation, PointAnimation, RectAnimation, SizeAnimation, ThicknessAnimation,
                        // DoubleCollection, PathFigureCollection, PointCollection, Point3Collection, VectorCollection
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




        private static void ReverseStoryboardSimple(Storyboard storyboard, Spinner containingObject)
        {
            var duration = TimeSpan.Zero;

            if (storyboard.GetCurrentTime(containingObject) is not TimeSpan storyTime)
                throw new Exception($"storyTime was null");


            for (int i = 0; i < storyboard.Children.Count; i++)
            {
                Timeline? child = storyboard.Children[i];

                if (child is not AnimationTimeline animation)
                    throw new Exception($"child was not an AnimationTimeline");
                if (duration == TimeSpan.Zero)
                    duration = animation.Duration.TimeSpan;
                if (duration != TimeSpan.Zero && duration != animation.Duration.TimeSpan)
                    throw new Exception($"duration was not consistent");

                switch (animation)
                {
                    case DoubleAnimation doubleAnimation:
                        var reversedDoubleAnimation = doubleAnimation.Clone();
                        reversedDoubleAnimation.From = doubleAnimation.To;
                        reversedDoubleAnimation.To = doubleAnimation.From;

                        Storyboard.SetTarget(reversedDoubleAnimation, Storyboard.GetTarget(doubleAnimation));
                        Storyboard.SetTargetProperty(reversedDoubleAnimation, Storyboard.GetTargetProperty(doubleAnimation));
                        storyboard.Children[i] = reversedDoubleAnimation;
                        //reversedStoryboard.Children.Add(reversedDoubleAnimation);

                        break;
                    case IKeyFrameAnimation keyFrameAnimation:
                        //keyFrameAnimation.KeyFrames.Reverse();
                        break;
                    default:
                        throw new Exception($"animation reversing is not supported");
                        // TODO: Add support for other animations:
                        // ColorAnimation, PointAnimation, RectAnimation, SizeAnimation, ThicknessAnimation,
                        // DoubleCollection, PathFigureCollection, PointCollection, Point3Collection, VectorCollection
                }
            }

            if (duration == TimeSpan.Zero) return;

            var progress = TimeSpan.FromTicks(storyTime.Ticks % duration.Ticks);
            var reversedProgress = duration - progress;

            storyboard.Stop(containingObject);

            storyboard.Begin(containingObject, true);
            storyboard.Seek(containingObject, reversedProgress, TimeSeekOrigin.BeginTime);
        }

        private static void ReverseStoryboardComplex(Storyboard storyboard, Spinner containingObject)
        {
            if (storyboard.GetCurrentTime(containingObject) is not TimeSpan storyTime)
                throw new Exception($"storyTime was null");

            storyboard.Pause(containingObject);

            for (int i = 0; i < storyboard.Children.Count; i++)
            {
                Timeline? child = storyboard.Children[i];
                if (child is not AnimationTimeline animation)
                    throw new Exception($"child was not an AnimationTimeline");

                switch (animation)
                {
                    case DoubleAnimation doubleAnimation:
                        var reversedDoubleAnimation = doubleAnimation.Clone();
                        reversedDoubleAnimation.From = doubleAnimation.To;
                        reversedDoubleAnimation.To = doubleAnimation.From;

                        Storyboard.SetTarget(reversedDoubleAnimation, Storyboard.GetTarget(doubleAnimation));
                        Storyboard.SetTargetProperty(reversedDoubleAnimation, Storyboard.GetTargetProperty(doubleAnimation));


                        var duration = doubleAnimation.Duration.TimeSpan;
                        var progress = TimeSpan.FromTicks(storyTime.Ticks % duration.Ticks);
                        var reversedProgress = duration - progress;

                        var clock = reversedDoubleAnimation.CreateClock(true);
                        clock.Controller.Seek(reversedProgress, TimeSeekOrigin.BeginTime);
                        var controler = clock.Controller;
                        //clock.Controller.Begin();

                        storyboard.Children[i] = reversedDoubleAnimation;
                        //storyboard.ApplyAnimationClock(Storyboard.GetTargetProperty(doubleAnimation), clock, HandoffBehavior.SnapshotAndReplace);

                        break;
                    default:
                        throw new Exception($"animation reversing is not supported");
                }
            }

            storyboard.Resume(containingObject);
            //storyboard.Begin(containingObject, true);

            foreach (var item in storyboard.Children)
            {

            }


            //storyboard.Begin(containingObject, true);

            //storyboard.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    var storyClock = storyboard.GetStoryboardClock(containingObject);

            //    switch (storyClock)
            //    {
            //        case ClockGroup clockGroup:
            //            foreach (var child in clockGroup.Children)
            //            {
            //                if (child is not AnimationClock animationClock)
            //                    throw new Exception($"child was not an AnimationClock");

            //                duration = animationClock.Timeline.Duration.TimeSpan;
            //                var progress = TimeSpan.FromTicks(storyTime.Ticks % duration.Ticks);
            //                var reversedProgress = duration - progress;

            //                animationClock.InternalSeek(reversedProgress);
            //                //animationClock.Controller.Seek(reversedProgress, TimeSeekOrigin.BeginTime);
            //            }
            //            break;
            //        default:
            //            throw new Exception($"storyClock was not a ClockGroup");
            //    }
            //}), DispatcherPriority.ContextIdle);


            /*
             * 
            foreach (var child in storyboard.Children)
            {
                if (child is not AnimationTimeline animation)
                    throw new Exception($"child was not an AnimationTimeline");

                // Create a clock for this animation
                var clock = animation.CreateClock();

                duration = animation.Duration.TimeSpan;
                var progress = TimeSpan.FromTicks(storyTime.Ticks % duration.Ticks);
                var reversedProgress = duration - progress;

                // Seek the clock to the reversed progress
                clock.Controller.Seek(reversedProgress, TimeSeekOrigin.BeginTime);

                // Apply the clock to the property
                var target = Storyboard.GetTarget(animation);
                var targetName = Storyboard.GetTargetName(animation);
                var targetPropertyPath = Storyboard.GetTargetProperty(animation)
                    ?? throw new Exception($"property was null");

                if (targetName != null && target == null)
                {
                    target = containingObject switch
                    {
                        FrameworkElement fe => fe.FindName(targetName) as DependencyObject,
                        FrameworkContentElement fce => fce.FindName(targetName) as DependencyObject,
                        _ => throw InvalideFeOrFceDependencyObjectException(containingObject)
                    };
                }
                if (target == null) throw new Exception($"target was null");

                // need to get the property from the property path / starting target
                // likely the final target will not be the same as the starting target
                // unless the property path is just a property name
                // will have to use the targetPropertyPath.PathParameters
                // https://learn.microsoft.com/en-us/dotnet/api/system.windows.propertypath.path?view=windowsdesktop-8.0&redirectedfrom=MSDN#System_Windows_PropertyPath_Path

                //If using this PropertyPath for a complex path for a storyboard target, Path is a tokenized string format that describes the relationships of the various objects given in the PathParameters.

                //Each item in the array is specified in this format by the array index for the item enclosed in parentheses. For example, to specify the first item in the array, the string token is (0).

                //Relationships between items ("steps" in the path) are specified by a dot (.). The property forward of the dot is the first step in the path, the property after is the second step, and so on (you can specify steps beyond two). The last step in the chain always represents the property being animated.

                //Items within collection properties are accessed with an indexer syntax, with the index within square brackets ([ and ]). The indexer is additive to the token representing the property. For example, the following is a two-step path, with the token combination in the first step specifying the second item from within the collection of that property: (0)[1].(1) . You cannot use an indexer on the last property in the chain; you cannot animate the actual collection position, you must animate a property on that object.


                // for current situation, targetName is "OuterRing", target is DashLengthRatio
                // targetPropertyPath.Path is "(0).(1)" with PathParameters [0] = {RenderTransform} and [1] = {Angle}

                var foo = new PathParser();
                var bar = foo.Parse(targetPropertyPath.Path);


                //var property = target.GetType().GetProperty(targetPropertyPath.Path);
                var property = GetTargetProperty(clock, target, targetPropertyPath);

                storyboard.ApplyAnimationClock(property, clock, HandoffBehavior.SnapshotAndReplace);
            }

            */

        }

        private static DependencyProperty GetTargetProperty(Clock currentClock, DependencyObject parentObject, PropertyPath parentPropertyPath)
        {
            Timeline currentTimeline = currentClock.Timeline;

            DependencyObject? targetObject = parentObject;
            PropertyPath currentPropertyPath = parentPropertyPath;
            DependencyProperty targetProperty;

            // The TargetProperty trumps the TargetName property.
            var localTargetObject = (DependencyObject)currentTimeline.GetValue(Storyboard.TargetProperty);
            if (localTargetObject != null)
            {
                targetObject = localTargetObject;
            }

            var propertyPath = (PropertyPath)currentTimeline.GetValue(Storyboard.TargetPropertyProperty);
            if (propertyPath != null)
            {
                currentPropertyPath = propertyPath;
            }

            if (currentPropertyPath == null)
                throw new InvalidOperationException($"Storyboard_TargetPropertyRequired, {currentTimeline.GetType()}");

            using (currentPropertyPath.SetContext(targetObject))
            {
                if (currentPropertyPath.GetLength() < 1)
                    throw new InvalidOperationException("Storyboard_PropertyPathEmpty");

                if (currentPropertyPath.GetLastAccessor() is not DependencyProperty animatedProperty)
                    throw new InvalidOperationException($"{nameof(animatedProperty)} was null");

                targetProperty = animatedProperty;
            }

            return targetProperty;
        }

        private static void ClockTreeWalkRecursive(
            Storyboard storyboard,
            Clock currentClock,                /* No two calls will have the same currentClock     */
            DependencyObject containingObject, /* Remains the same through all the recursive calls */
            INameScope nameScope,              /* Remains the same through all the recursive calls */
            DependencyObject parentObject,
            string? parentObjectName,
            PropertyPath parentPropertyPath,
            HybridDictionary clockMappings,
            Int64 layer                        /* Remains the same through all the recursive calls */)
        {
            var handoffBehavior = HandoffBehavior.SnapshotAndReplace;
            Timeline currentTimeline = currentClock.Timeline;

            DependencyObject? targetObject = parentObject;
            string? currentObjectName = parentObjectName;
            PropertyPath currentPropertyPath = parentPropertyPath;

            // If we have target object/property information, use it instead of the parent's information.
            string nameString = (string)currentTimeline.GetValue(Storyboard.TargetNameProperty);
            if (nameString != null)
            {
                if (nameScope is Style)
                    throw new InvalidOperationException($"Storyboard_TargetNameNotAllowedInStyle, {nameString}");

                currentObjectName = nameString;
            }

            // The TargetProperty trumps the TargetName property.
            DependencyObject localTargetObject = (DependencyObject)currentTimeline.GetValue(Storyboard.TargetProperty);
            if (localTargetObject != null)
            {
                targetObject = localTargetObject;
                currentObjectName = null;
            }

            PropertyPath propertyPath = (PropertyPath)currentTimeline.GetValue(Storyboard.TargetPropertyProperty);
            if (propertyPath != null)
            {
                currentPropertyPath = propertyPath;
            }

            if (currentClock is AnimationClock animationClock)
            {
                if (targetObject == null)
                {
                    // Resolve the target object name. If no name specified, use the containing object.
                    if (currentObjectName != null)
                    {
                        DependencyObject mentor = WpfUtils.FindMentor(containingObject)
                            ?? throw new InvalidOperationException($"Mentor not found for {currentObjectName}");

                        targetObject = WpfUtils.ResolveTargetName(currentObjectName, nameScope, mentor);
                    }
                    else
                    {
                        targetObject = containingObject as FrameworkElement;
                        targetObject ??= containingObject as FrameworkContentElement;

                        if (targetObject == null)
                            throw new InvalidOperationException($"Storyboard_NoTarget, {currentTimeline.GetType()}");
                    }
                }

                // See if we have a property name to use.
                if (currentPropertyPath == null)
                    throw new InvalidOperationException($"Storyboard_TargetPropertyRequired, {currentTimeline.GetType()}");

                // A property name can be a straightforward property name (like "Angle")
                // but may be a more complex multi-step property path.  The two cases
                // are handled differently.
                using (currentPropertyPath.SetContext(targetObject))
                {
                    if (currentPropertyPath.GetLength() < 1)
                        throw new InvalidOperationException("Storyboard_PropertyPathEmpty");

                    if (currentPropertyPath.GetLength() == 1)
                    {
                        // We have a simple single-step property.
                        if (currentPropertyPath.GetAccessor(0) is not DependencyProperty targetProperty)
                            throw new InvalidOperationException($"Storyboard_PropertyPathMustPointToDependencyProperty, {currentPropertyPath.Path}");

                        var animatedTarget = new ObjectPropertyPair(targetObject, targetProperty);
                        UpdateMappings(clockMappings, animatedTarget, animationClock);
                    }
                    else
                    {
                        ProcessComplexPath(clockMappings, targetObject, currentPropertyPath, animationClock, handoffBehavior, layer);
                    }
                }
            }
            else if (currentClock is MediaClock mediaClock)
            {
                throw new NotImplementedException("Media clock not supported");
                //ApplyMediaClock(nameScope, containingObject, targetObject, currentObjectName, mediaClock);
            }
            else if (currentClock is ClockGroup clockGroup)
            {
                ClockCollection childrenClocks = clockGroup.Children;

                for (int i = 0; i < childrenClocks.Count; i++)
                {
                    ClockTreeWalkRecursive(
                        storyboard,
                        childrenClocks[i],
                        containingObject,
                        nameScope,
                        targetObject,
                        currentObjectName,
                        currentPropertyPath,
                        clockMappings,
                        layer);
                }
            }
            else
            {
                throw new InvalidOperationException($"Storyboard_InvalidClock, {currentClock.GetType()}");
            }
        }

        /// <summary>
        ///     Given an animation clock, add it to the data structure which tracks
        /// all the clocks along with their associated target object and property.
        /// </summary>
        private static void UpdateMappings(HybridDictionary clockMappings, ObjectPropertyPair mappingKey, AnimationClock animationClock)
        {
            var mappedObject = clockMappings[mappingKey];

            if (mappedObject == null)
            {
                // No clock currently in storage, put this clock in that slot.
                clockMappings[mappingKey] = animationClock;
            }
            else if (mappedObject is AnimationClock clock)
            {
                // One clock currently in storage, up-convert to list and replace in slot.
                clockMappings[mappingKey] = new List<AnimationClock>
                {
                    clock,
                    animationClock
                };
            }
            else if (mappedObject is List<AnimationClock> clockList)
            {
                clockList.Add(animationClock);
            }
            else
            {
                throw new InvalidOperationException($"Internal error - clockMappings table contains an unexpected object {mappedObject.GetType()}");
            }
        }

        private static readonly Lazy<MethodInfo> _getStoryboardClockMethodInfo = new(
        () => typeof(Storyboard)
            .GetMethod("GetStoryboardClock", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(DependencyObject) }, null)
            ?? throw new InvalidOperationException("Could not find GetStoryboardClock method in Storyboard"),
        isThreadSafe: true);

        //private static readonly Lazy<MethodInfo> _getStoryboardClockMethodInfo = new(
        //    () => typeof(Storyboard)
        //        .GetMethod("GetStoryboardClock", BindingFlags.NonPublic | BindingFlags.Instance)
        //        ?? throw new InvalidOperationException("Could not find GetStoryboardClock method in Storyboard"),
        //    isThreadSafe: true);

        public static Clock GetStoryboardClock(this Storyboard storyboard, DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "DependencyObject cannot be null.");

            return (Clock)(_getStoryboardClockMethodInfo.Value.Invoke(storyboard, new object[] { obj })
                ?? throw new InvalidOperationException("Unable to get the Storyboard clock."));
        }


        /// <summary>
        ///     For complex property paths, we need to dig our way down to the
        /// property and attach the animation clock there.  We will not be able to
        /// actually attach the clocks if the targetProperty points to a frozen
        /// Freezable.  More extensive handling will be required for that case.
        /// </summary>
        private static void ProcessComplexPath(HybridDictionary clockMappings, DependencyObject targetObject,
            PropertyPath path, AnimationClock animationClock, HandoffBehavior handoffBehavior, Int64 layer)
        {
            // For complex paths, the target object/property differs from the actual
            //  animated object/property.
            //
            // Example:
            //  TargetName="Rect1" TargetProperty="(Rectangle.LayoutTransform).(RotateTransform.Angle)"
            //
            // The target object is a Rectangle.
            // The target property is LayoutTransform.
            // The animated object is a RotateTransform
            // The animated property is Angle.

            // Currently unsolved problem: If the LayoutTransform is not a RotateTransform,
            //  we have no way of knowing.  We'll merrily set up to animate the Angle
            //  property as an attached property, not knowing that the value will be
            //  completely ignored.

            var targetProperty = path.GetAccessor(0) as DependencyProperty;

            // Two different ways to deal with property paths.  If the target is
            //  on a frozen Freezable, we'll have to make a clone of the value and
            //  attach the animation on the clone instead.
            // For all other objects, we attach the animation clock directly on the
            //  specified animating object and property.
            object targetPropertyValue = targetObject.GetValue(targetProperty);

            var animatedObject = path.GetLastItem() as DependencyObject;
            var animatedProperty = path.GetLastAccessor() as DependencyProperty;

            if (animatedObject == null || animatedProperty == null || targetProperty == null)
                throw new InvalidOperationException($"Storyboard_PropertyPathUnresolved, {path.Path}");

            bool isPropertyCloningRequired = targetPropertyValue is Freezable freezableValue && freezableValue.IsFrozen;

            if (isPropertyCloningRequired)
            {
                throw new NotImplementedException();

                //if (targetObject is not FrameworkElement or FrameworkContentElement)
                //    throw new InvalidOperationException($"Storyboard_ComplexPathNotSupported, {path.Path}");

                //if (targetPropertyValue is not Freezable)
                //    throw new InvalidOperationException($"Don't clone a non-freezable");

                //// To enable animations on frozen Freezable objects, complex
                ////  path processing is done on a clone of the value.
                //var clone = ((Freezable)targetPropertyValue).Clone();
                //SetComplexPathClone(targetObject, targetProperty, targetPropertyValue, clone);

                //// Promote the clone to the EffectiveValues cache
                //targetObject.InvalidateProperty(targetProperty);

                //if (targetObject.GetValue(targetProperty) != clone)
                //    throw new InvalidOperationException($"Storyboard_ImmutableTargetNotSupported, {path.Path}");

                //// Now that we have a clone, update the animatedObject and animatedProperty
                ////  with references to those on the clone.
                //using (path.SetContext(targetObject))
                //{
                //    animatedObject = path.GetLastItem() as DependencyObject;
                //    animatedProperty = path.GetLastAccessor() as DependencyProperty;
                //}

                //// And set up to listen to changes on this clone.
                //ChangeListener.ListenToChangesOnFreezable(
                //    targetObject, clone, targetProperty, (Freezable)targetPropertyValue);
            }

            // Apply animation clock on the animated object/animated property.
            var directApplyTarget = new ObjectPropertyPair(animatedObject, animatedProperty);
            UpdateMappings(clockMappings, directApplyTarget, animationClock);
        }

        //private static void SetComplexPathClone(DependencyObject o, DependencyProperty dp, object source, object clone)
        //{
        //    FrugalMap clonesMap = ComplexPathCloneField.GetValue(o);

        //    if (clone != DependencyProperty.UnsetValue)
        //    {
        //        clonesMap[dp.GlobalIndex] = new CloneCacheEntry(source, clone);
        //    }
        //    else
        //    {
        //        clonesMap[dp.GlobalIndex] = DependencyProperty.UnsetValue;
        //    }

        //    // FrugalMap is a struct - after a change it needs to be set back on the object.
        //    ComplexPathCloneField.SetValue(o, clonesMap);
        //}


    }

}
