using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;

namespace XamlSpinners
{
    public class SurfaceElementCollection<T> : Freezable, IList<T>, IList, INotifyCollectionChanged
        where T : SurfaceElement
    {
        #region Data 

        private List<T> _collection;
        private static SurfaceElementCollection<T>? _instance;

        #endregion

        #region Constructors

        public SurfaceElementCollection()
            : base()
        {
            _collection = new List<T>();
        }

        #endregion

        #region Static Methods 

        public static SurfaceElementCollection<T> Empty
        {
            get
            {
                if (_instance == null)
                {
                    var emptyCollection = new SurfaceElementCollection<T>
                    {
                        _collection = new List<T>(0)
                    };
                    emptyCollection.Freeze();

                    _instance = emptyCollection;
                }

                return _instance;
            }
        }

        #endregion

        #region Freezable 

        public new SurfaceElementCollection<T> Clone() => (SurfaceElementCollection<T>)base.Clone();

        protected override Freezable CreateInstanceCore() => new SurfaceElementCollection<T>();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection<T> sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.CloneCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                var element = (T)sourceCollection._collection[i].Clone();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection<T> sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.CloneCurrentValueCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                var element = (T)sourceCollection._collection[i].CloneCurrentValue();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection<T> sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.GetAsFrozenCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                var element = (T)sourceCollection._collection[i].GetAsFrozen();
                _collection.Add(element);
                OnFreezablePropertyChanged(null, element);
            }
        }


        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not SurfaceElementCollection<T> sourceCollection)
                throw new ArgumentNullException(nameof(sourceFreezable));

            base.GetCurrentValueAsFrozenCore(sourceFreezable);
            int count = sourceCollection._collection.Count;

            _collection = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                var element = (T)sourceCollection._collection[i].GetCurrentValueAsFrozen();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            ReadPreamble();
            return _collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
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

        public void CopyTo(Array array, int index)
        {
            ReadPreamble();
            ((ICollection)_collection).CopyTo(array, index);
        }

        #endregion

        #region ICollection<T>

        public bool IsReadOnly
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        public void Add(T element)
        {
            AddInternal(element);
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

        public bool Contains(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            ReadPreamble();
            return _collection.Contains(element);
        }

        public void CopyTo(T[] array, int index)
        {
            ReadPreamble();
            _collection.CopyTo(array, index);
        }

        public bool Remove(T element)
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

        #endregion

        #region IList<T>

        public int IndexOf(T element)
        {
            ReadPreamble();
            return _collection.IndexOf(element);
        }

        public void Insert(int index, T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            WritePreamble();

            OnFreezablePropertyChanged(null, element);
            _collection.Insert(index, element);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Add, element, index));

            WritePostscript();
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

        public T this[int index]
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

        #region IList

        public bool IsFixedSize
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        object? IList.this[int index]
        {
            get => this[index];
            set
            {
                if (value is not T typedValue)
                    throw new ArgumentException($"Argument is not of type {typeof(T)}", nameof(value));
                this[index] = typedValue;
            }
        }

        int IList.Add(object? value)
        {
            if (value is not T typedValue)
                throw new ArgumentException($"Argument is not of type {typeof(T)}", nameof(value));
            return AddInternal(typedValue);
        }

        bool IList.Contains(object? value)
        {
            if (value is not T typedValue)
                throw new ArgumentException($"Argument is not of type {typeof(T)}", nameof(value));
            return Contains(typedValue);
        }

        int IList.IndexOf(object? value)
        {
            if (value is not T typedValue)
                throw new ArgumentException($"Argument is not of type {typeof(T)}", nameof(value));
            return IndexOf(typedValue);
        }

        void IList.Insert(int index, object? value)
        {
            if (value is not T typedValue)
                throw new ArgumentException($"Argument is not of type {typeof(T)}", nameof(value));
            Insert(index, typedValue);
        }

        void IList.Remove(object? value)
        {
            if (value is not T typedValue)
                throw new ArgumentException($"Argument is not of type {typeof(T)}", nameof(value));
            Remove(typedValue);
        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Private Methods

        private int AddInternal(T element)
        {
            WritePreamble();

            OnFreezablePropertyChanged(null, element);
            _collection.Add(element);
            var index = _collection.Count - 1;
            OnCollectionChanged(new(NotifyCollectionChangedAction.Add, element, index));

            WritePostscript();

            return index;
        }

        #endregion
    }
}