using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Animations
{
    public class PathFigureCollectionKeyFrameCollection : Freezable, IList, IList<PathFigureCollectionKeyFrame>
    {
        #region Data 

        private List<PathFigureCollectionKeyFrame> _keyFrames;
        private static PathFigureCollectionKeyFrameCollection? _instance;

        #endregion

        #region Constructors

        public PathFigureCollectionKeyFrameCollection()
            : base()
        {
            _keyFrames = new List<PathFigureCollectionKeyFrame>();
        }

        #endregion

        #region Static Methods 

        public static PathFigureCollectionKeyFrameCollection Empty
        {
            get
            {
                if (_instance == null)
                {
                    var emptyCollection = new PathFigureCollectionKeyFrameCollection
                    {
                        _keyFrames = new List<PathFigureCollectionKeyFrame>(0)
                    };
                    emptyCollection.Freeze();

                    _instance = emptyCollection;
                }

                return _instance;
            }
        }

        #endregion

        #region Freezable 

        public new PathFigureCollectionKeyFrameCollection Clone() => (PathFigureCollectionKeyFrameCollection)base.Clone();

        protected override Freezable CreateInstanceCore() => new PathFigureCollectionKeyFrameCollection();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            var sourceCollection = (PathFigureCollectionKeyFrameCollection)sourceFreezable;
            base.CloneCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<PathFigureCollectionKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                var keyFrame = (PathFigureCollectionKeyFrame)sourceCollection._keyFrames[i].Clone();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            var sourceCollection = (PathFigureCollectionKeyFrameCollection)sourceFreezable;
            base.CloneCurrentValueCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<PathFigureCollectionKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                var keyFrame = (PathFigureCollectionKeyFrame)sourceCollection._keyFrames[i].CloneCurrentValue();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            var sourceCollection = (PathFigureCollectionKeyFrameCollection)sourceFreezable;
            base.GetAsFrozenCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<PathFigureCollectionKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                var keyFrame = (PathFigureCollectionKeyFrame)sourceCollection._keyFrames[i].GetAsFrozen();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            var sourceCollection = (PathFigureCollectionKeyFrameCollection)sourceFreezable;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<PathFigureCollectionKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                var keyFrame = (PathFigureCollectionKeyFrame)sourceCollection._keyFrames[i].GetCurrentValueAsFrozen();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }

        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            for (int i = 0; i < _keyFrames.Count && canFreeze; i++)
            {
                canFreeze &= Freezable.Freeze(_keyFrames[i], isChecking);
            }

            return canFreeze;
        }

        #endregion

        #region IEnumerable 

        public IEnumerator GetEnumerator()
        {
            ReadPreamble();
            return _keyFrames.GetEnumerator();
        }

        #endregion

        #region ICollection

        public int Count
        {
            get
            {
                ReadPreamble();
                return _keyFrames.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                ReadPreamble();
                return (IsFrozen || Dispatcher != null);
            }
        }

        public object SyncRoot
        {
            get
            {
                ReadPreamble();
                return ((ICollection)_keyFrames).SyncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ReadPreamble();
            ((ICollection)_keyFrames).CopyTo(array, index);
        }

        public void CopyTo(PathFigureCollectionKeyFrame[] array, int index)
        {
            ReadPreamble();
            _keyFrames.CopyTo(array, index);
        }

        #endregion

        #region IList

        int IList.Add(object? keyFrame)
        {
            return Add((PathFigureCollectionKeyFrame?)keyFrame);
        }

        public int Add(PathFigureCollectionKeyFrame? keyFrame)
        {
            if (keyFrame == null)
            {
                throw new ArgumentNullException(nameof(keyFrame));
            }

            WritePreamble();

            OnFreezablePropertyChanged(null, keyFrame);
            _keyFrames.Add(keyFrame);

            WritePostscript();

            return _keyFrames.Count - 1;
        }

        public void Clear()
        {
            WritePreamble();

            if (_keyFrames.Count > 0)
            {
                for (int i = 0; i < _keyFrames.Count; i++)
                {
                    OnFreezablePropertyChanged(_keyFrames[i], null);
                }

                _keyFrames.Clear();

                WritePostscript();
            }
        }

        bool IList.Contains(object? keyFrame)
        {
            if (keyFrame == null) throw new ArgumentNullException(nameof(keyFrame));
            return Contains((PathFigureCollectionKeyFrame)keyFrame);
        }

        public bool Contains(PathFigureCollectionKeyFrame? keyFrame)
        {
            if (keyFrame == null) throw new ArgumentNullException(nameof(keyFrame));
            ReadPreamble();
            return _keyFrames.Contains(keyFrame);
        }

        int IList.IndexOf(object? keyFrame)
        {
            if (keyFrame == null) throw new ArgumentNullException(nameof(keyFrame));
            return IndexOf((PathFigureCollectionKeyFrame)keyFrame);
        }

        public int IndexOf(PathFigureCollectionKeyFrame keyFrame)
        {
            ReadPreamble();
            return _keyFrames.IndexOf(keyFrame);
        }

        void IList.Insert(int index, object? keyFrame)
        {
            if (keyFrame == null) throw new ArgumentNullException(nameof(keyFrame));
            Insert(index, (PathFigureCollectionKeyFrame)keyFrame);
        }

        public void Insert(int index, PathFigureCollectionKeyFrame keyFrame)
        {
            if (keyFrame == null) throw new ArgumentNullException(nameof(keyFrame));

            WritePreamble();

            OnFreezablePropertyChanged(null, keyFrame);
            _keyFrames.Insert(index, keyFrame);

            WritePostscript();
        }

        public bool IsFixedSize
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        void IList.Remove(object? keyFrame)
        {
            if (keyFrame == null) throw new ArgumentNullException(nameof(keyFrame));
            _ = Remove((PathFigureCollectionKeyFrame)keyFrame);
        }

        public bool Remove(PathFigureCollectionKeyFrame keyFrame)
        {
            WritePreamble();

            bool removed = _keyFrames.Remove(keyFrame);

            if (removed)
            {
                OnFreezablePropertyChanged(keyFrame, null);
                WritePostscript();
            }

            return removed;
        }


        public void RemoveAt(int index)
        {
            WritePreamble();

            OnFreezablePropertyChanged(_keyFrames[index], null);
            _keyFrames.RemoveAt(index);

            WritePostscript();
        }

        object? IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                this[index] = (PathFigureCollectionKeyFrame)value;
            }
        }

        public PathFigureCollectionKeyFrame this[int index]
        {
            get
            {
                ReadPreamble();
                return _keyFrames[index];
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                WritePreamble();

                if (value != _keyFrames[index])
                {
                    OnFreezablePropertyChanged(_keyFrames[index], value);
                    _keyFrames[index] = value;

                    Debug.Assert(_keyFrames[index] != null);

                    WritePostscript();
                }
            }
        }

        #endregion

        #region IList<T>

        void ICollection<PathFigureCollectionKeyFrame>.Add(PathFigureCollectionKeyFrame keyFrame)
        {
            Add(keyFrame);
        }

        bool ICollection<PathFigureCollectionKeyFrame>.Remove(PathFigureCollectionKeyFrame keyFrame)
        {
            return Remove(keyFrame);
        }

        IEnumerator<PathFigureCollectionKeyFrame> IEnumerable<PathFigureCollectionKeyFrame>.GetEnumerator()
        {
            ReadPreamble();
            return _keyFrames.GetEnumerator();
        }

        #endregion
    }
} 
