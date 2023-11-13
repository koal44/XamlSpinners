using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations
{
    public class PointCollectionAnimationUsingKeyFrames : PointCollectionAnimationBase, IKeyFrameAnimation, IAddChild
    {
        private PointCollectionKeyFrameCollection? _keyFrames;
        private ResolvedKeyFrameEntry[]? _sortedResolvedKeyFrames;
        private bool _areKeyTimesValid;

        public PointCollectionAnimationUsingKeyFrames()
            : base()
        {
            _areKeyTimesValid = true;
        }


        #region Freezable 

        public new PointCollectionAnimationUsingKeyFrames Clone() => (PointCollectionAnimationUsingKeyFrames)base.Clone();


        public new PointCollectionAnimationUsingKeyFrames CloneCurrentValue()
            => (PointCollectionAnimationUsingKeyFrames)base.CloneCurrentValue();


        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            canFreeze &= Freeze(_keyFrames, isChecking);

            if (canFreeze & !_areKeyTimesValid)
            {
                ResolveKeyTimes();
            }

            return canFreeze;
        }


        protected override void OnChanged()
        {
            _areKeyTimesValid = false;

            base.OnChanged();
        }


        protected override Freezable CreateInstanceCore() => new PointCollectionAnimationUsingKeyFrames();


        protected override void CloneCore(Freezable sourceFreezable)
        {
            var sourceAnimation = (PointCollectionAnimationUsingKeyFrames)sourceFreezable;
            base.CloneCore(sourceFreezable);

            CopyCommon(sourceAnimation, isCurrentValueClone: false);
        }


        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            var sourceAnimation = (PointCollectionAnimationUsingKeyFrames)sourceFreezable;
            base.CloneCurrentValueCore(sourceFreezable);

            CopyCommon(sourceAnimation, isCurrentValueClone: true);
        }


        protected override void GetAsFrozenCore(Freezable source)
        {
            var sourceAnimation = (PointCollectionAnimationUsingKeyFrames)source;
            base.GetAsFrozenCore(source);

            CopyCommon(sourceAnimation, isCurrentValueClone: false);
        }


        protected override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            var sourceAnimation = (PointCollectionAnimationUsingKeyFrames)source;
            base.GetCurrentValueAsFrozenCore(source);

            CopyCommon(sourceAnimation, isCurrentValueClone: true);
        }


        private void CopyCommon(PointCollectionAnimationUsingKeyFrames sourceAnimation, bool isCurrentValueClone)
        {
            _areKeyTimesValid = sourceAnimation._areKeyTimesValid;

            if (_areKeyTimesValid && sourceAnimation._sortedResolvedKeyFrames != null)
            {
                // _sortedResolvedKeyFrames is an array of ResolvedKeyFrameEntry so the notion of CurrentValueClone doesn't apply 
                _sortedResolvedKeyFrames = (ResolvedKeyFrameEntry[])sourceAnimation._sortedResolvedKeyFrames.Clone();
            }

            if (sourceAnimation._keyFrames != null)
            {
                if (isCurrentValueClone)
                {
                    _keyFrames = (PointCollectionKeyFrameCollection)sourceAnimation._keyFrames.CloneCurrentValue();
                }
                else
                {
                    _keyFrames = (PointCollectionKeyFrameCollection)sourceAnimation._keyFrames.Clone();
                }

                OnFreezablePropertyChanged(null, _keyFrames);
            }
        }

        #endregion Freezable

        #region IAddChild

        void IAddChild.AddChild(object child)
        {
            WritePreamble();

            if (child == null) throw new ArgumentNullException(nameof(child));
            AddChild(child);

            WritePostscript();
        }


        protected virtual void AddChild(object child)
        {
            if (child is not PointCollectionKeyFrame keyFrame) throw new Exception();
            KeyFrames.Add(keyFrame);
        }


        void IAddChild.AddText(string childText)
        {
            if (childText == null) throw new ArgumentNullException(nameof(childText));
            AddText(childText);
        }


        protected virtual void AddText(string childText)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region PointCollectionAnimationBase

        /// 

        /// Calculates the value this animation believes should be the current value for the property. 
        /// 

        /// 
        /// This value is the suggested origin value provided to the animation 
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain
        /// this value will be the snapshot value if one is available or the
        /// base property value if it is not; otherise this value will be the 
        /// value returned by the previous animation in the chain with an
        /// animationClock that is not Stopped. 
        ///  
        /// 
        /// This value is the suggested destination value provided to the animation 
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is
        /// in the first composition layer of animations on a property;
        /// otherwise this value will be the output value from the previous 
        /// composition layer of animations for the property.
        ///  
        ///  
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its 
        /// output value.
        /// 
        /// 
        /// The value this animation believes should be the current value for the property. 
        /// 
        protected sealed override PointCollection GetCurrentValueCore(PointCollection defaultOriginValue, PointCollection defaultDestinationValue, AnimationClock animationClock)
        {
            Debug.Assert(animationClock.CurrentState != ClockState.Stopped);

            if (_keyFrames == null)
            {
                return defaultDestinationValue;
            }

            // We resolved our KeyTimes when we froze, but also got notified 
            // of the frozen state and therefore invalidated ourselves.
            if (!_areKeyTimesValid)
            {
                ResolveKeyTimes();
            }

            if (_sortedResolvedKeyFrames == null)
            {
                return defaultDestinationValue;
            }

            var currentTime = animationClock.CurrentTime!.Value;
            int keyFrameCount = _sortedResolvedKeyFrames.Length;
            int maxKeyFrameIndex = keyFrameCount - 1;

            PointCollection currentIterationValue;

            Debug.Assert(maxKeyFrameIndex >= 0, "maxKeyFrameIndex is less than zero which means we don't actually have any key frames.");

            int currentResolvedKeyFrameIndex = 0;

            // Skip all the key frames with key times lower than the current time. 
            // currentResolvedKeyFrameIndex will be greater than maxKeyFrameIndex
            // if we are past the last key frame. 
            while (currentResolvedKeyFrameIndex < keyFrameCount
                   && currentTime > _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex]._resolvedKeyTime)
            {
                currentResolvedKeyFrameIndex++;
            }

            // If there are multiple key frames at the same key time, be sure to go to the last one. 
            while (currentResolvedKeyFrameIndex < maxKeyFrameIndex
                   && currentTime == _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex + 1]._resolvedKeyTime)
            {
                currentResolvedKeyFrameIndex++;
            }

            if (currentResolvedKeyFrameIndex == keyFrameCount)
            {
                // Past the last key frame. 
                currentIterationValue = GetResolvedKeyFrameValue(maxKeyFrameIndex);
            }
            else if (currentTime == _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex]._resolvedKeyTime)
            {
                // Exactly on a key frame. 
                currentIterationValue = GetResolvedKeyFrameValue(currentResolvedKeyFrameIndex);
            }
            else
            {
                // Between two key frames.
                double currentSegmentProgress;
                PointCollection fromValue;

                if (currentResolvedKeyFrameIndex == 0)
                {
                    // The current key frame is the first key frame so we have
                    // some special rules for determining the fromValue and an
                    // optimized method of calculating the currentSegmentProgress. 

                    // If we're additive we want the base value to be a zero value 
                    // so that if there isn't a key frame at time 0.0, we'll use 
                    // the zero value for the time 0.0 value and then add that
                    // later to the base value. 
                    if (IsAdditive)
                    {
                        // Get a zero value for the type.
                        fromValue = AnimatedTypeHelpers.GetZeroValuePointCollection(defaultOriginValue);
                    }
                    else
                    {
                        fromValue = defaultOriginValue;
                    }

                    // Current segment time divided by the segment duration.
                    // Note: the reason this works is that we know that we're in
                    // the first segment, so we can assume:
                    // 
                    // currentTime.TotalMilliseconds                                  = current segment time
                    // _sortedResolvedKeyFrames[0]._resolvedKeyTime.TotalMilliseconds = current segment duration 

                    currentSegmentProgress = currentTime.TotalMilliseconds
                                             / _sortedResolvedKeyFrames[0]._resolvedKeyTime.TotalMilliseconds;
                }
                else
                {
                    int previousResolvedKeyFrameIndex = currentResolvedKeyFrameIndex - 1;
                    var previousResolvedKeyTime = _sortedResolvedKeyFrames[previousResolvedKeyFrameIndex]._resolvedKeyTime;

                    fromValue = GetResolvedKeyFrameValue(previousResolvedKeyFrameIndex);

                    var segmentCurrentTime = currentTime - previousResolvedKeyTime;
                    var segmentDuration = _sortedResolvedKeyFrames[currentResolvedKeyFrameIndex]._resolvedKeyTime - previousResolvedKeyTime;

                    currentSegmentProgress = segmentCurrentTime.TotalMilliseconds
                                            / segmentDuration.TotalMilliseconds;
                }

                currentIterationValue = GetResolvedKeyFrame(currentResolvedKeyFrameIndex).InterpolateValue(fromValue, currentSegmentProgress);
            }



            // If we're cumulative, we need to multiply the final key frame
            // value by the current repeat count and add this to the return 
            // value.
            if (IsCumulative)
            {
                int currentRepeat = (int)(animationClock.CurrentIteration! - 1);

                if (currentRepeat > 0)
                {
                    currentIterationValue = AnimatedTypeHelpers.AddPointCollection(
                        currentIterationValue,
                        AnimatedTypeHelpers.ScalePointCollection(GetResolvedKeyFrameValue(maxKeyFrameIndex), currentRepeat));
                }
            }

            // If we're additive we need to add the base value to the return value. 
            if (IsAdditive)
            {
                return AnimatedTypeHelpers.AddPointCollection(defaultOriginValue, currentIterationValue);
            }


            return currentIterationValue;
        }

        /// 

        /// Provide a custom natural Duration when the Duration property is set to Automatic.
        /// 

        ///  
        /// The Clock whose natural duration is desired.
        ///  
        ///  
        /// If the last KeyFrame of this animation is a KeyTime, then this will
        /// be used as the NaturalDuration; otherwise it will be one second. 
        /// 
        protected override sealed Duration GetNaturalDurationCore(Clock clock)
        {
            return new Duration(LargestTimeSpanKeyTime);
        }

        #endregion

        #region IKeyFrameAnimation 

        IList IKeyFrameAnimation.KeyFrames
        {
            get => KeyFrames;
            set => KeyFrames = (PointCollectionKeyFrameCollection)value;
        }


        public PointCollectionKeyFrameCollection KeyFrames
        {
            get
            {
                ReadPreamble();

                // The reason we don't just set _keyFrames to the empty collection
                // in the first place is that null tells us that the user has not 
                // asked for the collection yet. The first time they ask for the
                // collection and we're unfrozen, policy dictates that we give
                // them a new unfrozen collection. All subsequent times they will
                // get whatever collection is present, whether frozen or unfrozen. 

                if (_keyFrames == null)
                {
                    if (IsFrozen)
                    {
                        _keyFrames = PointCollectionKeyFrameCollection.Empty;
                    }
                    else
                    {
                        WritePreamble();

                        _keyFrames = new PointCollectionKeyFrameCollection();

                        OnFreezablePropertyChanged(null, _keyFrames);

                        WritePostscript();
                    }
                }

                return _keyFrames;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                WritePreamble();

                if (value != _keyFrames)
                {
                    OnFreezablePropertyChanged(_keyFrames, value);
                    _keyFrames = value;

                    WritePostscript();
                }
            }
        }


        public bool ShouldSerializeKeyFrames()
        {
            ReadPreamble();

            return _keyFrames != null && _keyFrames.Count > 0;
        }

        #endregion

        #region Public Properties

        /// 

        /// If this property is set to true, this animation will add its value
        /// to the base value or the value of the previous animation in the
        /// composition chain.  Another way of saying this is that the units 
        /// specified in the animation are relative to the base value rather
        /// than absolute units. 
        /// 

        /// 
        /// In the case where the first key frame's resolved key time is not 
        /// 0.0 there is slightly different behavior between KeyFrameVectorAnimations
        /// with IsAdditive set and without.  Animations with the property set to false
        /// will behave as if there is a key frame at time 0.0 with the value of the
        /// base value.  Animations with the property set to true will behave as if 
        /// there is a key frame at time 0.0 with a zero value appropriate to the type
        /// of the animation.  These behaviors provide the results most commonly expected 
        /// and can be overridden by simply adding a key frame at time 0.0 with the preferred value. 
        /// 
        public bool IsAdditive
        {
            get => (bool)GetValue(IsAdditiveProperty);
            set => SetValue(IsAdditiveProperty, value);
        }

        /// 

        /// If this property is set to true, the value of this animation will 
        /// accumulate over repeat cycles.  For example, if this is a point
        /// animation and your key frames describe something approximating and 
        /// arc, setting this property to true will result in an animation that 
        /// would appear to bounce the point across the screen.
        /// 

        /// 
        /// This property works along with the IsAdditive property.  Setting
        /// this value to true has no effect unless IsAdditive is also set
        /// to true. 
        /// 
        public bool IsCumulative
        {
            get => (bool)GetValue(IsCumulativeProperty);
            set => SetValue(IsCumulativeProperty, value);
        }

        #endregion

        #region Private Methods 

        private struct KeyTimeBlock
        {
            public int BeginIndex;
            public int EndIndex;
        }

        private PointCollection GetResolvedKeyFrameValue(int resolvedKeyFrameIndex)
        {
            Debug.Assert(_areKeyTimesValid, "The key frames must be resolved and sorted before calling GetResolvedKeyFrameValue");

            return GetResolvedKeyFrame(resolvedKeyFrameIndex).Value;
        }

        private PointCollectionKeyFrame GetResolvedKeyFrame(int resolvedKeyFrameIndex)
        {
            Debug.Assert(_areKeyTimesValid, "The key frames must be resolved and sorted before calling GetResolvedKeyFrame");

            return _keyFrames![_sortedResolvedKeyFrames![resolvedKeyFrameIndex]._originalKeyFrameIndex];
        }

        /// 

        /// Returns the largest time span specified key time from all of the key frames. 
        /// If there are not time span key times a time span of one second is returned
        /// to match the default natural duration of the From/To/By animations.
        /// 

        private TimeSpan LargestTimeSpanKeyTime
        {
            get
            {
                bool hasTimeSpanKeyTime = false;
                var largestTimeSpanKeyTime = TimeSpan.Zero;

                if (_keyFrames != null)
                {
                    int keyFrameCount = _keyFrames.Count;

                    for (int index = 0; index < keyFrameCount; index++)
                    {
                        var keyTime = _keyFrames[index].KeyTime;

                        if (keyTime.Type == KeyTimeType.TimeSpan)
                        {
                            hasTimeSpanKeyTime = true;

                            if (keyTime.TimeSpan > largestTimeSpanKeyTime)
                            {
                                largestTimeSpanKeyTime = keyTime.TimeSpan;
                            }
                        }
                    }
                }

                if (hasTimeSpanKeyTime)
                {
                    return largestTimeSpanKeyTime;
                }
                else
                {
                    return TimeSpan.FromSeconds(1.0);
                }
            }
        }

        private void ResolveKeyTimes()
        {
            Debug.Assert(!_areKeyTimesValid, "KeyFrameVectorAnimaton.ResolveKeyTimes() shouldn't be called if the key times are already valid.");

            int keyFrameCount = 0;

            if (_keyFrames != null)
            {
                keyFrameCount = _keyFrames.Count;
            }

            if (keyFrameCount == 0)
            {
                _sortedResolvedKeyFrames = null;
                _areKeyTimesValid = true;
                return;
            }

            _sortedResolvedKeyFrames = new ResolvedKeyFrameEntry[keyFrameCount];

            int index = 0;

            // Initialize the _originalKeyFrameIndex.
            for (; index < keyFrameCount; index++)
            {
                _sortedResolvedKeyFrames[index]._originalKeyFrameIndex = index;
            }

            var duration = Duration;

            // calculationDuration represents the time span we will use to resolve 
            // percent key times. This is defined as the value in the following
            // precedence order: 
            //   1. The animation's duration, but only if it is a time span, not auto or forever.
            //   2. The largest time span specified key time of all the key frames.
            //   3. 1 second, to match the From/To/By animations.

            var calculationDuration = duration.HasTimeSpan
                ? duration.TimeSpan
                : LargestTimeSpanKeyTime;

            int maxKeyFrameIndex = keyFrameCount - 1;
            var unspecifiedBlocks = new List<KeyTimeBlock>();
            bool hasPacedKeyTimes = false;

            // 
            // Pass 1: Resolve Percent and Time key times.
            // 

            index = 0;
            while (index < keyFrameCount)
            {
                var keyTime = _keyFrames![index].KeyTime;

                switch (keyTime.Type)
                {
                    case KeyTimeType.Percent:

                        _sortedResolvedKeyFrames[index]._resolvedKeyTime = TimeSpan.FromMilliseconds(keyTime.Percent * calculationDuration.TotalMilliseconds);
                        index++;
                        break;

                    case KeyTimeType.TimeSpan:

                        _sortedResolvedKeyFrames[index]._resolvedKeyTime = keyTime.TimeSpan;

                        index++;
                        break;

                    case KeyTimeType.Paced:
                    case KeyTimeType.Uniform:

                        if (index == maxKeyFrameIndex)
                        {
                            // If the last key frame doesn't have a specific time 
                            // associated with it its resolved key time will be
                            // set to the calculationDuration, which is the 
                            // defined in the comments above where it is set.
                            // Reason: We only want extra time at the end of the
                            // key frames if the user specifically states that
                            // the last key frame ends before the animation ends. 

                            _sortedResolvedKeyFrames[index]._resolvedKeyTime = calculationDuration;
                            index++;
                        }
                        else if (index == 0
                                 && keyTime.Type == KeyTimeType.Paced)
                        {
                            // Note: It's important that this block come after
                            // the previous if block because of rule precendence. 

                            // If the first key frame in a multi-frame key frame 
                            // collection is paced, we set its resolved key time 
                            // to 0.0 for performance reasons.  If we didn't, the
                            // resolved key time list would be dependent on the 
                            // base value which can change every animation frame
                            // in many cases.

                            _sortedResolvedKeyFrames[index]._resolvedKeyTime = TimeSpan.Zero;
                            index++;
                        }
                        else
                        {
                            if (keyTime.Type == KeyTimeType.Paced)
                            {
                                hasPacedKeyTimes = true;
                            }

                            var block = new KeyTimeBlock
                            {
                                BeginIndex = index
                            };

                            // NOTE: We don't want to go all the way up to the
                            // last frame because if it is Uniform or Paced its 
                            // resolved key time will be set to the calculation
                            // duration using the logic above.
                            //
                            // This is why the logic is: 
                            //    ((++index) < maxKeyFrameIndex)
                            // instead of: 
                            //    ((++index) < keyFrameCount) 

                            while ((++index) < maxKeyFrameIndex)
                            {
                                var type = _keyFrames[index].KeyTime.Type;

                                if (type == KeyTimeType.Percent || type == KeyTimeType.TimeSpan)
                                {
                                    break;
                                }
                                else if (type == KeyTimeType.Paced)
                                {
                                    hasPacedKeyTimes = true;
                                }
                            }

                            Debug.Assert(index < keyFrameCount,
                                "The end index for a block of unspecified key frames is out of bounds.");

                            block.EndIndex = index;
                            unspecifiedBlocks.Add(block);
                        }

                        break;
                }
            }

            //
            // Pass 2: Resolve Uniform key times. 
            //

            for (int j = 0; j < unspecifiedBlocks.Count; j++)
            {
                var block = unspecifiedBlocks[j];
                var blockBeginTime = TimeSpan.Zero;

                if (block.BeginIndex > 0)
                {
                    blockBeginTime = _sortedResolvedKeyFrames[block.BeginIndex - 1]._resolvedKeyTime;
                }

                // The number of segments is equal to the number of key
                // frames we're working on plus 1.  Think about the case 
                // where we're working on a single key frame.  There's a 
                // segment before it and a segment after it.
                // 
                //  Time known         Uniform           Time known
                //  ^                  ^                 ^
                //  |                  |                 |
                //  |   (segment 1)    |   (segment 2)   | 

                int segmentCount = (block.EndIndex - block.BeginIndex) + 1;
                var uniformTimeStep = TimeSpan.FromTicks((_sortedResolvedKeyFrames[block.EndIndex]._resolvedKeyTime - blockBeginTime).Ticks / segmentCount);

                index = block.BeginIndex;
                var resolvedTime = blockBeginTime + uniformTimeStep;

                while (index < block.EndIndex)
                {
                    _sortedResolvedKeyFrames[index]._resolvedKeyTime = resolvedTime;

                    resolvedTime += uniformTimeStep;
                    index++;
                }
            }

            //
            // Pass 3: Resolve Paced key times. 
            //

            if (hasPacedKeyTimes)
            {
                ResolvePacedKeyTimes();
            }

            //
            // Sort resolved key frame entries. 
            //

            Array.Sort(_sortedResolvedKeyFrames);

            _areKeyTimesValid = true;
            return;
        }

        /// 

        /// This should only be called from ResolveKeyTimes and only at the
        /// appropriate time. 
        /// 

        private void ResolvePacedKeyTimes()
        {
            Debug.Assert(_keyFrames != null && _keyFrames.Count > 2,
                "Caller must guard against calling this method when there are insufficient keyframes.");

            // If the first key frame is paced its key time has already 
            // been resolved, so we start at index 1.

            int index = 1;
            int maxKeyFrameIndex = _sortedResolvedKeyFrames!.Length - 1;

            do
            {
                if (_keyFrames[index].KeyTime.Type == KeyTimeType.Paced)
                {
                    //
                    // We've found a paced key frame so this is the 
                    // beginning of a paced block. 
                    //

                    // The first paced key frame in this block.
                    int firstPacedBlockKeyFrameIndex = index;

                    // List of segment lengths for this paced block. 
                    var segmentLengths = new List<double>();

                    // The resolved key time for the key frame before this 
                    // block which we'll use as our starting point.
                    TimeSpan prePacedBlockKeyTime = _sortedResolvedKeyFrames[index - 1]._resolvedKeyTime;

                    // The total of the segment lengths of the paced key
                    // frames in this block.
                    double totalLength = 0.0;

                    // The key value of the previous key frame which will be 
                    // used to determine the segment length of this key frame. 
                    PointCollection prevKeyValue = _keyFrames[index - 1].Value;

                    do
                    {
                        PointCollection currentKeyValue = _keyFrames[index].Value;

                        // Determine the segment length for this key frame and
                        // add to the total length. 
                        totalLength += AnimatedTypeHelpers.GetSegmentLengthPointCollection(prevKeyValue, currentKeyValue);

                        // Temporarily store the distance into the total length 
                        // that this key frame represents in the resolved
                        // key times array to be converted to a resolved key
                        // time outside of this loop.
                        segmentLengths.Add(totalLength);

                        // Prepare for the next iteration. 
                        prevKeyValue = currentKeyValue;
                        index++;
                    }
                    while (index < maxKeyFrameIndex
                            && _keyFrames[index].KeyTime.Type == KeyTimeType.Paced);

                    // index is currently set to the index of the key frame 
                    // after the last paced key frame.  This will always
                    // be a valid index because we limit ourselves with 
                    // maxKeyFrameIndex. 

                    // We need to add the distance between the last paced key 
                    // frame and the next key frame to get the total distance
                    // inside the key frame block.
                    totalLength += AnimatedTypeHelpers.GetSegmentLengthPointCollection(prevKeyValue, _keyFrames[index].Value);

                    // Calculate the time available in the resolved key time space.
                    var pacedBlockDuration = _sortedResolvedKeyFrames[index]._resolvedKeyTime - prePacedBlockKeyTime;

                    // Convert lengths in segmentLengths list to resolved
                    // key times for the paced key frames in this block. 
                    for (int i = 0, currentKeyFrameIndex = firstPacedBlockKeyFrameIndex; i < segmentLengths.Count; i++, currentKeyFrameIndex++)
                    {
                        // The resolved key time for each key frame is:
                        // 
                        // The key time of the key frame before this paced block
                        // + ((the percentage of the way through the total length) 
                        //    * the resolved key time space available for the block) 
                        _sortedResolvedKeyFrames[currentKeyFrameIndex]._resolvedKeyTime = prePacedBlockKeyTime + TimeSpan.FromMilliseconds(
                            (segmentLengths[i] / totalLength) * pacedBlockDuration.TotalMilliseconds);
                    }
                }
                else
                {
                    index++;
                }
            }
            while (index < maxKeyFrameIndex);
        }

        #endregion
    }
}
