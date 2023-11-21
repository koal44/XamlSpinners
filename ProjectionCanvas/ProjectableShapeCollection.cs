using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

namespace ProjectionCanvas
{
    public partial class ProjectableShapeCollection : Animatable, IList, IList<ProjectableShape>
    {
        #region Fields

        private static ProjectableShapeCollection? s_empty;

        internal List<ProjectableShape> _collection;
        internal uint _version = 0;

        #endregion Fields

        #region IList<T>

        public void Add(ProjectableShape value)
        {
            AddHelper(value);
        }

        public void Clear()
        {
            WritePreamble();

            _collection.Clear();
            ++_version;

            WritePostscript();
        }

        public bool Contains(ProjectableShape value)
        {
            ReadPreamble();
            return _collection.Contains(value);
        }

        public int IndexOf(ProjectableShape value)
        {
            ReadPreamble();
            return _collection.IndexOf(value);
        }

        public void Insert(int index, ProjectableShape value)
        {
            if (value == null) throw new ArgumentException(null, nameof(value));
            WritePreamble();

            OnFreezablePropertyChanged(oldValue: null, newValue: value); 
            _collection.Insert(index, value);
            ++_version;

            WritePostscript();
        }

        public bool Remove(ProjectableShape value)
        {
            WritePreamble();

            int index = IndexOf(value);

            if (index >= 0)
            {
                var oldValue = _collection[index];
                OnFreezablePropertyChanged(oldValue, null);
                _collection.RemoveAt(index);
                ++_version;

                WritePostscript();

                return true;
            }

            return false;
        }


        public void RemoveAt(int index)
        {
            RemoveAtWithoutFiringPublicEvents(index);
            // RemoveAtWithoutFiringPublicEvents incremented the version
            WritePostscript();
        }

        internal void RemoveAtWithoutFiringPublicEvents(int index)
        {
            WritePreamble();

            var oldValue = _collection[index];
            OnFreezablePropertyChanged(oldValue, null);
            _collection.RemoveAt(index);

            ++_version;

            // No WritePostScript to avoid firing the Changed event.
        }

        public ProjectableShape this[int index]
        {
            get
            {
                ReadPreamble();
                return _collection[index];
            }
            set
            {
                if (value == null) throw new ArgumentException(null, nameof(value));

                WritePreamble();

                if (!Object.ReferenceEquals(_collection[index], value))
                {
                    var oldValue = _collection[index];
                    OnFreezablePropertyChanged(oldValue, value);
                    _collection[index] = value;
                }

                ++_version;
                WritePostscript();
            }
        }

        #endregion

        #region ICollection<T>

        public int Count
        {
            get
            {
                ReadPreamble();
                return _collection.Count;
            }
        }

        public void CopyTo(ProjectableShape[] array, int index)
        {
            ReadPreamble();

            if (array == null) throw new ArgumentNullException(nameof(array));

            if (index < 0 || (index + _collection.Count) > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _collection.CopyTo(array, index);
        }

        bool ICollection<ProjectableShape>.IsReadOnly
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        #endregion

        #region IEnumerable<T>


        public Enumerator GetEnumerator()
        {
            ReadPreamble();
            return new Enumerator(this);
        }

        IEnumerator<ProjectableShape> IEnumerable<ProjectableShape>.GetEnumerator() => GetEnumerator();

        #endregion

        #region IList

        bool IList.IsReadOnly => ((ICollection<ProjectableShape>)this).IsReadOnly;

        bool IList.IsFixedSize
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
                // Forwards to typed implementation
                this[index] = Cast(value);
            }
        }

        int IList.Add(object? value)
        {
            if (value == null)
                throw new ArgumentException(null, nameof(value));
            return AddHelper(Cast(value));
        }

        bool IList.Contains(object? value)
        {
            if (value is not ProjectableShape typedValue)
                return false;
            return Contains(typedValue);
        }

        int IList.IndexOf(object? value)
        {
            if (value is not ProjectableShape typedValue)
                return -1;
            return IndexOf(typedValue);
        }

        void IList.Insert(int index, object? value)
        {
            if (value == null)
                throw new ArgumentException(null, nameof(value));
            Insert(index, Cast(value));
        }

        void IList.Remove(object? value)
        {
            if (value is not ProjectableShape typedValue) 
                throw new ArgumentException(null, nameof(value));
            Remove(typedValue);
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            ReadPreamble();

            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (index < 0 || (index + _collection.Count) > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (array.Rank != 1)
                throw new ArgumentException("Rank must be 1", nameof(array));

            int count = _collection.Count;
            for (int i = 0; i < count; i++)
            {
                array.SetValue(_collection[i], index + i);
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ReadPreamble();
                return IsFrozen || Dispatcher != null;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                ReadPreamble();
                return this;
            }
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Helpers Methods

        internal static ProjectableShapeCollection Empty
        {
            get
            {
                if (s_empty == null)
                {
                    var collection = new ProjectableShapeCollection();
                    collection.Freeze();
                    s_empty = collection;
                }

                return s_empty;
            }
        }

        internal ProjectableShape Internal_GetItem(int i) => _collection[i];

        private static ProjectableShape Cast(object? value)
        {
            if (value == null)
                throw new ArgumentException(null, nameof(value));

            if (value is not ProjectableShape typedValue)
                throw new ArgumentException(null, nameof(value));

            return typedValue;
        }

        private int AddHelper(ProjectableShape value)
        {
            int index = AddWithoutFiringPublicEvents(value);
            // AddAtWithoutFiringPublicEvents incremented the version
            WritePostscript();

            return index;
        }

        internal int AddWithoutFiringPublicEvents(ProjectableShape? value)
        {
            if (value == null) 
                throw new ArgumentException(null, nameof(value));

            WritePreamble();

            var newValue = value;
            OnFreezablePropertyChanged(oldValue: null, newValue);
            _collection.Add(newValue);
            int index = _collection.Count - 1;

            ++_version;
            // No WritePostScript to avoid firing the Changed event.
            return index;
        }

        #endregion Private Helpers

        #region Freezable

        public new ProjectableShapeCollection Clone()
            => (ProjectableShapeCollection)base.Clone();

        public new ProjectableShapeCollection CloneCurrentValue()
            => (ProjectableShapeCollection)base.CloneCurrentValue();

        protected override Freezable CreateInstanceCore() => new ProjectableShapeCollection();

        protected override void CloneCore(Freezable source)
        {
            if (source is not ProjectableShapeCollection typedSource)
            {
                throw new ArgumentException(null, nameof(source));
            }

            base.CloneCore(source);
            int count = typedSource._collection.Count;

            _collection = new List<ProjectableShape>(count);

            for (int i = 0; i < count; i++)
            {
                ProjectableShape newValue = (ProjectableShape)typedSource._collection[i].Clone();
                OnFreezablePropertyChanged(oldValue: null, newValue);
                _collection.Add(newValue);
            }
        }

        protected override void CloneCurrentValueCore(Freezable source)
        {
            if (source is not ProjectableShapeCollection typedSource)
            {
                throw new ArgumentException(null, nameof(source));
            }

            base.CloneCurrentValueCore(source);

            int count = typedSource._collection.Count;

            _collection = new List<ProjectableShape>(count);

            for (int i = 0; i < count; i++)
            {
                var newValue = (ProjectableShape)typedSource._collection[i].CloneCurrentValue();
                OnFreezablePropertyChanged(oldValue: null, newValue);
                _collection.Add(newValue);
            }

        }

        protected override void GetAsFrozenCore(Freezable source)
        {
            if (source is not ProjectableShapeCollection typedSource)
            {
                throw new ArgumentException(null, nameof(source));
            }

            base.GetAsFrozenCore(source);

            int count = typedSource._collection.Count;

            _collection = new List<ProjectableShape>(count);

            for (int i = 0; i < count; i++)
            {
                ProjectableShape newValue = (ProjectableShape)typedSource._collection[i].GetAsFrozen();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
            }

        }

        protected override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            if (source is not ProjectableShapeCollection typedSource)
            {
                throw new ArgumentException(null, nameof(source));
            }

            base.GetCurrentValueAsFrozenCore(source);

            int count = typedSource._collection.Count;

            _collection = new List<ProjectableShape>(count);

            for (int i = 0; i < count; i++)
            {
                ProjectableShape newValue = (ProjectableShape)typedSource._collection[i].GetCurrentValueAsFrozen();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
            }

        }

        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            int count = _collection.Count;
            for (int i = 0; i < count && canFreeze; i++)
            {
                canFreeze &= Freezable.Freeze(_collection[i], isChecking);
            }

            return canFreeze;
        }

        #endregion ProtectedMethods

        #region Enumerator

        public struct Enumerator : IEnumerator, IEnumerator<ProjectableShape>
        {
            internal Enumerator(ProjectableShapeCollection? list)
            {
                Debug.Assert(list != null, "list may not be null.");

                _list = list;
                _version = list._version;
                _index = -1;
                _current = default;
            }

            readonly void IDisposable.Dispose()
            {

            }

            public bool MoveNext()
            {
                _list.ReadPreamble();

                if (_version == _list._version)
                {
                    if (_index > -2 && _index < _list._collection.Count - 1)
                    {
                        _current = _list._collection[++_index];
                        return true;
                    }
                    else
                    {
                        _index = -2; // -2 indicates "past the end"
                        return false;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Collection changed during enumeration.");
                }
            }

            public void Reset()
            {
                _list.ReadPreamble();

                if (_version == _list._version)
                {
                    _index = -1;
                }
                else
                {
                    throw new InvalidOperationException("Collection changed during enumeration.");
                }
            }

            readonly object? IEnumerator.Current => Current;

            public readonly ProjectableShape Current
            {
                get
                {
                    if (_index > -1)
                    {
                        return _current!;
                    }
                    else if (_index == -1)
                    {
                        throw new InvalidOperationException("Enumeration not started.");
                    }
                    else
                    {
                        Debug.Assert(_index == -2, "expected -2, got " + _index + "\n");
                        throw new InvalidOperationException("Enumeration ended.");
                    }
                }
            }

            private ProjectableShape? _current;
            private readonly ProjectableShapeCollection _list;
            private readonly uint _version;
            private int _index;
        }

        #endregion

        #region Constructors

        public ProjectableShapeCollection()
        {
            _collection = new List<ProjectableShape>();
        }

        public ProjectableShapeCollection(int capacity)
        {
            _collection = new List<ProjectableShape>(capacity);
        }

        public ProjectableShapeCollection(IEnumerable<ProjectableShape> collection)
        {
            WritePreamble();

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (collection is ICollection<ProjectableShape> icollectionOfT)
            {
                _collection = new List<ProjectableShape>(icollectionOfT);
                foreach (ProjectableShape item in collection)
                {
                    if (item == null)
                        throw new NoNullAllowedException(nameof(item));
                    OnFreezablePropertyChanged(oldValue: null, item);
                }
            }
            else
            {
                _collection = new List<ProjectableShape>();

                foreach (ProjectableShape item in collection)
                {
                    if (item == null)
                        throw new NoNullAllowedException(nameof(item));
                    var newValue = item;
                    OnFreezablePropertyChanged(oldValue: null, newValue);
                    _collection.Add(newValue);
                }
            }

            WritePostscript();
        }

        #endregion Constructors

    }
}