using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;

namespace ColorCraft
{
    public class FreezableCollection<T> : Freezable, IList<T>, IList, INotifyCollectionChanged
        where T : Freezable
    {
        #region Data 

        private List<T> _collection;
        private static FreezableCollection<T>? _instance;

        #endregion

        #region Constructors

        public FreezableCollection()
            : base()
        {
            _collection = new List<T>();
        }

        public FreezableCollection(int capacity)
            : base()
        {
            _collection = new List<T>(capacity);
        }

        public FreezableCollection(IEnumerable<T> collection)
            : base()
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            _collection = new List<T>(collection);

            // not sure if this is needed as the collection is being initialized and not modified
            //foreach (var item in _collection)
            //{
            //    OnFreezablePropertyChanged(null, item);
            //}
        }

        #endregion

        #region Static Methods 

        public static FreezableCollection<T> Empty
        {
            get
            {
                if (_instance == null)
                {
                    var emptyCollection = new FreezableCollection<T>
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

        public new FreezableCollection<T> Clone() => (FreezableCollection<T>)base.Clone();

        protected override Freezable CreateInstanceCore() => new FreezableCollection<T>();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            if (sourceFreezable is not FreezableCollection<T> sourceCollection)
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
            if (sourceFreezable is not FreezableCollection<T> sourceCollection)
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
            if (sourceFreezable is not FreezableCollection<T> sourceCollection)
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
            if (sourceFreezable is not FreezableCollection<T> sourceCollection)
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

        #region List<T> Methods

        public void Sort()
        {
            WritePreamble();
            _collection.Sort();
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            WritePostscript();
        }

        public void Sort(IComparer<T> comparer)
        {
            WritePreamble();
            _collection.Sort(comparer);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            WritePostscript();
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            WritePreamble();
            _collection.Sort(index, count, comparer);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            WritePostscript();
        }

        public void Sort(Comparison<T> comparison)
        {
            WritePreamble();
            _collection.Sort(comparison);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            WritePostscript();
        }

        public void Reverse()
        {
            WritePreamble();
            _collection.Reverse();
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            WritePostscript();
        }

        public void Reverse(int index, int count)
        {
            WritePreamble();
            _collection.Reverse(index, count);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
            WritePostscript();
        }

        public void TrimExcess()
        {
            WritePreamble();
            _collection.TrimExcess();
            WritePostscript();
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            WritePreamble();

            var index = _collection.Count;
            var oldCount = _collection.Count;
            var newCount = 0;

            foreach (var item in collection)
            {
                if (item == null) throw new ArgumentException("Collection contains null elements", nameof(collection));

                OnFreezablePropertyChanged(null, item);
                _collection.Add(item);
                newCount++;
            }

            if (newCount > 0)
            {
                OnCollectionChanged(new(NotifyCollectionChangedAction.Add, _collection.GetRange(oldCount, newCount), index));
            }

            WritePostscript();
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            WritePreamble();

            var oldCount = _collection.Count;
            var newCount = 0;

            foreach (var item in collection)
            {
                if (item == null) throw new ArgumentException("Collection contains null elements", nameof(collection));

                OnFreezablePropertyChanged(null, item);
                _collection.Insert(index + newCount, item);
                newCount++;
            }

            if (newCount > 0)
            {
                OnCollectionChanged(new(NotifyCollectionChangedAction.Add, _collection.GetRange(oldCount, newCount), index));
            }

            WritePostscript();
        }

        public void RemoveRange(int index, int count)
        {
            WritePreamble();

            var removedItems = _collection.GetRange(index, count);

            for (int i = 0; i < count; i++)
            {
                OnFreezablePropertyChanged(_collection[index + i], null);
            }

            _collection.RemoveRange(index, count);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, removedItems, index));

            WritePostscript();
        }

        public void RemoveAll(Predicate<T> match)
        {
            WritePreamble();

            var removedItems = _collection.FindAll(match);

            foreach (var item in removedItems)
            {
                OnFreezablePropertyChanged(item, null);
            }

            _collection.RemoveAll(match);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, removedItems));

            WritePostscript();
        }

        public void ForEach(Action<T> action)
        {
            WritePreamble();
            _collection.ForEach(action);
            WritePostscript();
        }

        public bool TrueForAll(Predicate<T> match)
        {
            ReadPreamble();
            return _collection.TrueForAll(match);
        }

        public int BinarySearch(T item)
        {
            ReadPreamble();
            return _collection.BinarySearch(item);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            ReadPreamble();
            return _collection.BinarySearch(item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            ReadPreamble();
            return _collection.BinarySearch(index, count, item, comparer);
        }

        public bool Exists(Predicate<T> match)
        {
            ReadPreamble();
            return _collection.Exists(match);
        }

        public T? Find(Predicate<T> match)
        {
            ReadPreamble();
            return _collection.Find(match);
        }

        public FreezableCollection<T> FindAll(Predicate<T> match)
        {
            ReadPreamble();
            return new(_collection.FindAll(match));
        }

        public int FindIndex(Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindIndex(match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindIndex(startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindIndex(startIndex, count, match);
        }

        public T? FindLast(Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindLast(match);
        }

        public int FindLastIndex(Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindLastIndex(match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindLastIndex(startIndex, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            ReadPreamble();
            return _collection.FindLastIndex(startIndex, count, match);
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
