using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;

namespace XamlSpinners
{
    public class SurfaceElementCollection2 : Freezable, IList, IList<SurfaceElement>, INotifyCollectionChanged 
    {
        #region Data 

        private List<SurfaceElement> _collection;
        private static SurfaceElementCollection2? _instance;


        #endregion

        #region Constructors

        public SurfaceElementCollection2()
            : base()
        {
            _collection = new List<SurfaceElement>();
        }

        #endregion

        #region Static Methods 

        public static SurfaceElementCollection2 Empty
        {
            get
            {
                if (_instance == null)
                {
                    var emptyCollection = new SurfaceElementCollection2
                    {
                        _collection = new List<SurfaceElement>(0)
                    };
                    emptyCollection.Freeze();

                    _instance = emptyCollection;
                }

                return _instance;
            }
        }

        #endregion

        #region Freezable 

        public new SurfaceElementCollection2 Clone() => (SurfaceElementCollection2)base.Clone();

        protected override Freezable CreateInstanceCore() => new SurfaceElementCollection2();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection2 sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.CloneCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<SurfaceElement>(count);
            for (int i = 0; i < count; i++)
            {
                var element = sourceCollection._collection[i].Clone();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection2 sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.CloneCurrentValueCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<SurfaceElement>(count);
            for (int i = 0; i < count; i++)
            {
                var element = sourceCollection._collection[i].CloneCurrentValue();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection2 sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.GetAsFrozenCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<SurfaceElement>(count);
            for (int i = 0; i < count; i++)
            {
                var element = (SurfaceElement)sourceCollection._collection[i].GetAsFrozen();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }


        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection2 sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.GetCurrentValueAsFrozenCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<SurfaceElement>(count);
            for (int i = 0; i < count; i++)
            {
                var element = (SurfaceElement)sourceCollection._collection[i].GetCurrentValueAsFrozen();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }

        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            for (int i = 0; i < _collection.Count && canFreeze; i++)
            {
                canFreeze &= Freezable.Freeze(_collection[i], isChecking);
            }

            return canFreeze;
        }

        #endregion

        #region IEnumerable 

        public IEnumerator GetEnumerator()
        {
            ReadPreamble();
            return _collection.GetEnumerator();
        }

        #endregion

        #region ICollection

        public int Count
        {
            get
            {
                ReadPreamble();
                return _collection.Count;
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
                return ((ICollection)_collection).SyncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ReadPreamble();
            ((ICollection)_collection).CopyTo(array, index);
        }

        public void CopyTo(SurfaceElement[] array, int index)
        {
            ReadPreamble();
            _collection.CopyTo(array, index);
        }

        #endregion

        #region IList

        int IList.Add(object? element)
        {
            return Add((SurfaceElement?)element);
        }

        public int Add(SurfaceElement? element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            WritePreamble();

            OnFreezablePropertyChanged(null, element);
            _collection.Add(element);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Add, element, _collection.Count - 1));

            WritePostscript();

            return _collection.Count - 1;
        }

        public void Clear()
        {
            WritePreamble();

            if (_collection.Count > 0)
            {
                for (int i = 0; i < _collection.Count; i++)
                {
                    OnFreezablePropertyChanged(_collection[i], null);
                }

                _collection.Clear();

                OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));

                WritePostscript();
            }
        }

        bool IList.Contains(object? element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            return Contains((SurfaceElement)element);
        }

        public bool Contains(SurfaceElement? element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            ReadPreamble();
            return _collection.Contains(element);
        }

        int IList.IndexOf(object? element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            return IndexOf((SurfaceElement)element);
        }

        public int IndexOf(SurfaceElement element)
        {
            ReadPreamble();
            return _collection.IndexOf(element);
        }

        void IList.Insert(int index, object? element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            Insert(index, (SurfaceElement)element);
        }

        public void Insert(int index, SurfaceElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            WritePreamble();

            OnFreezablePropertyChanged(null, element);
            _collection.Insert(index, element);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Add, element, index));

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

        void IList.Remove(object? element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            _ = Remove((SurfaceElement)element);
        }

        public bool Remove(SurfaceElement element)
        {
            WritePreamble();

            int index = _collection.IndexOf(element);
            if (index < 0) return false; 

            OnFreezablePropertyChanged(element, null);
            _collection.RemoveAt(index);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, element, index));

            WritePostscript();

            return true;
        }


        public void RemoveAt(int index)
        {
            WritePreamble();

            var element = _collection[index];

            OnFreezablePropertyChanged(_collection[index], null);
            _collection.RemoveAt(index);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, element, index));

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
                this[index] = (SurfaceElement)value;
            }
        }

        public SurfaceElement this[int index]
        {
            get
            {
                ReadPreamble();
                return _collection[index];
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                WritePreamble();

                if (value != _collection[index])
                {
                    var oldValue = _collection[index];

                    OnFreezablePropertyChanged(_collection[index], value);
                    _collection[index] = value;
                    OnCollectionChanged(new(NotifyCollectionChangedAction.Replace, value, oldValue, index));

                    Debug.Assert(_collection[index] != null);

                    WritePostscript();
                }
            }
        }

        #endregion

        #region IList<T>

        void ICollection<SurfaceElement>.Add(SurfaceElement element)
        {
            Add(element);
        }

        bool ICollection<SurfaceElement>.Remove(SurfaceElement element)
        {
            return Remove(element);
        }

        IEnumerator<SurfaceElement> IEnumerable<SurfaceElement>.GetEnumerator()
        {
            ReadPreamble();
            return _collection.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion
    }
}