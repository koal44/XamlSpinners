using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace System.Windows.Media.Animation2
{
    /// 

    /// This collection is used in conjunction with a KeyFrameVectorAnimation
    /// to animate a Vector property value along a set of key frames. 
    /// 

    public class VectorKeyFrameCollection2 : Freezable, IList
    {
        #region Data 

        private List<VectorKeyFrame2> _keyFrames;
        private static VectorKeyFrameCollection2 s_emptyCollection;

        #endregion

        #region Constructors

        /// 

        /// Creates a new VectorKeyFrameCollection.
        /// 

        public VectorKeyFrameCollection2()
            : base()
        {
            _keyFrames = new List<VectorKeyFrame2>(2);
        }

        #endregion

        #region Static Methods 

        /// 

        /// An empty VectorKeyFrameCollection. 
        /// 

        public static VectorKeyFrameCollection2 Empty
        {
            get
            {
                if (s_emptyCollection == null)
                {
                    VectorKeyFrameCollection2 emptyCollection = new VectorKeyFrameCollection2();

                    emptyCollection._keyFrames = new List<VectorKeyFrame2>(0);
                    emptyCollection.Freeze();

                    s_emptyCollection = emptyCollection;
                }

                return s_emptyCollection;
            }
        }

        #endregion

        #region Freezable 

        /// 

        /// Creates a freezable copy of this VectorKeyFrameCollection. 
        /// 

        /// The copy 
        public new VectorKeyFrameCollection2 Clone()
        {
            return (VectorKeyFrameCollection2)base.Clone();
        }

        /// 

        /// Implementation of Freezable.CreateInstanceCore. 
        /// 

        /// The new Freezable. 
        protected override Freezable CreateInstanceCore()
        {
            return new VectorKeyFrameCollection2();
        }

        /// 

        /// Implementation of Freezable.CloneCore. 
        /// 

        protected override void CloneCore(Freezable sourceFreezable)
        {
            VectorKeyFrameCollection2 sourceCollection = (VectorKeyFrameCollection2)sourceFreezable;
            base.CloneCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<VectorKeyFrame2>(count);

            for (int i = 0; i < count; i++)
            {
                VectorKeyFrame2 keyFrame = (VectorKeyFrame2)sourceCollection._keyFrames[i].Clone();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        /// 

        /// Implementation of Freezable.CloneCurrentValueCore.
        /// 

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            VectorKeyFrameCollection2 sourceCollection = (VectorKeyFrameCollection2)sourceFreezable;
            base.CloneCurrentValueCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<VectorKeyFrame2>(count);

            for (int i = 0; i < count; i++)
            {
                VectorKeyFrame2 keyFrame = (VectorKeyFrame2)sourceCollection._keyFrames[i].CloneCurrentValue();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        /// 

        /// Implementation of Freezable.GetAsFrozenCore. 
        /// 

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            VectorKeyFrameCollection2 sourceCollection = (VectorKeyFrameCollection2)sourceFreezable;
            base.GetAsFrozenCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<VectorKeyFrame2>(count);

            for (int i = 0; i < count; i++)
            {
                VectorKeyFrame2 keyFrame = (VectorKeyFrame2)sourceCollection._keyFrames[i].GetAsFrozen();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        /// 

        /// Implementation of Freezable.GetCurrentValueAsFrozenCore. 
        /// 

        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            VectorKeyFrameCollection2 sourceCollection = (VectorKeyFrameCollection2)sourceFreezable;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);

            int count = sourceCollection._keyFrames.Count;

            _keyFrames = new List<VectorKeyFrame2>(count);

            for (int i = 0; i < count; i++)
            {
                VectorKeyFrame2 keyFrame = (VectorKeyFrame2)sourceCollection._keyFrames[i].GetCurrentValueAsFrozen();
                _keyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }

        /// 

        /// 
        /// 

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

        /// 

        /// Returns an enumerator of the VectorKeyFrames in the collection.
        /// 

        public IEnumerator GetEnumerator()
        {
            ReadPreamble();

            return _keyFrames.GetEnumerator();
        }

        #endregion

        #region ICollection

        /// 

        /// Returns the number of VectorKeyFrames in the collection. 
        /// 

        public int Count
        {
            get
            {
                ReadPreamble();

                return _keyFrames.Count;
            }
        }

        /// 

        /// See ICollection.IsSynchronized.
        /// 

        public bool IsSynchronized
        {
            get
            {
                ReadPreamble();

                return (IsFrozen || Dispatcher != null);
            }
        }

        /// 

        /// See ICollection.SyncRoot.
        /// 

        public object SyncRoot
        {
            get
            {
                ReadPreamble();

                return ((ICollection)_keyFrames).SyncRoot;
            }
        }

        /// 

        /// Copies all of the VectorKeyFrames in the collection to an 
        /// array.
        /// 

        void ICollection.CopyTo(Array array, int index)
        {
            ReadPreamble();

            ((ICollection)_keyFrames).CopyTo(array, index);
        }

        /// 

        /// Copies all of the VectorKeyFrames in the collection to an
        /// array of VectorKeyFrames.
        /// 

        public void CopyTo(VectorKeyFrame2[] array, int index)
        {
            ReadPreamble();

            _keyFrames.CopyTo(array, index);
        }

        #endregion

        #region IList

        /// 

        /// Adds a VectorKeyFrame to the collection.
        /// 

        int IList.Add(object keyFrame)
        {
            return Add((VectorKeyFrame2)keyFrame);
        }

        /// 

        /// Adds a VectorKeyFrame to the collection. 
        /// 

        public int Add(VectorKeyFrame2 keyFrame)
        {
            if (keyFrame == null)
            {
                throw new ArgumentNullException("keyFrame");
            }

            WritePreamble();

            OnFreezablePropertyChanged(null, keyFrame);
            _keyFrames.Add(keyFrame);

            WritePostscript();

            return _keyFrames.Count - 1;
        }

        /// 

        /// Removes all VectorKeyFrames from the collection. 
        /// 

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

        /// 

        /// Returns true of the collection contains the given VectorKeyFrame.
        /// 

        bool IList.Contains(object keyFrame)
        {
            return Contains((VectorKeyFrame2)keyFrame);
        }

        /// 

        /// Returns true of the collection contains the given VectorKeyFrame.
        /// 

        public bool Contains(VectorKeyFrame2 keyFrame)
        {
            ReadPreamble();

            return _keyFrames.Contains(keyFrame);
        }

        /// 

        /// Returns the index of a given VectorKeyFrame in the collection.
        /// 

        int IList.IndexOf(object keyFrame)
        {
            return IndexOf((VectorKeyFrame2)keyFrame);
        }

        /// 

        /// Returns the index of a given VectorKeyFrame in the collection.
        /// 

        public int IndexOf(VectorKeyFrame2 keyFrame)
        {
            ReadPreamble();

            return _keyFrames.IndexOf(keyFrame);
        }

        /// 

        /// Inserts a VectorKeyFrame into a specific location in the collection.
        /// 

        void IList.Insert(int index, object keyFrame)
        {
            Insert(index, (VectorKeyFrame2)keyFrame);
        }

        /// 

        /// Inserts a VectorKeyFrame into a specific location in the collection.
        /// 

        public void Insert(int index, VectorKeyFrame2 keyFrame)
        {
            if (keyFrame == null)
            {
                throw new ArgumentNullException("keyFrame");
            }

            WritePreamble();

            OnFreezablePropertyChanged(null, keyFrame);
            _keyFrames.Insert(index, keyFrame);

            WritePostscript();
        }

        /// 

        /// Returns true if the collection is frozen.
        /// 

        public bool IsFixedSize
        {
            get
            {
                ReadPreamble();

                return IsFrozen;
            }
        }

        /// 

        /// Returns true if the collection is frozen.
        /// 

        public bool IsReadOnly
        {
            get
            {
                ReadPreamble();

                return IsFrozen;
            }
        }

        /// 

        /// Removes a VectorKeyFrame from the collection. 
        /// 

        void IList.Remove(object keyFrame)
        {
            Remove((VectorKeyFrame2)keyFrame);
        }

        /// 

        /// Removes a VectorKeyFrame from the collection.
        /// 

        public void Remove(VectorKeyFrame2 keyFrame)
        {
            WritePreamble();

            if (_keyFrames.Contains(keyFrame))
            {
                OnFreezablePropertyChanged(keyFrame, null);
                _keyFrames.Remove(keyFrame);

                WritePostscript();
            }
        }

        /// 

        /// Removes the VectorKeyFrame at the specified index from the collection. 
        /// 

        public void RemoveAt(int index)
        {
            WritePreamble();

            OnFreezablePropertyChanged(_keyFrames[index], null);
            _keyFrames.RemoveAt(index);

            WritePostscript();
        }

        /// 

        /// Gets or sets the VectorKeyFrame at a given index.
        /// 

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (VectorKeyFrame2)value;
            }
        }

        /// 

        /// Gets or sets the VectorKeyFrame at a given index. 
        /// 

        public VectorKeyFrame2 this[int index]
        {
            get
            {
                ReadPreamble();

                return _keyFrames[index];
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(String.Format(CultureInfo.InvariantCulture, "VectorKeyFrameCollection[{0}]", index));
                }

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
    }
} 
