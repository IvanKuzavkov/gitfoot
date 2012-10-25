using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BitTorrent.WP7.Extensions
{
    public class ExtendedObservableCollection<TInput,TOutput> : IList<TOutput>, ICollection<TOutput>, IEnumerable<TOutput>, IList, ICollection, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private ObservableCollection<TOutput> _collection;
        private ObservableCollection<TInput> _inputCollection;

        private Predicate<TOutput> _filter;
        private Converter<TInput, TOutput> _converter;

        private event NotifyCollectionChangedEventHandler _collectionchanged;
        private event PropertyChangedEventHandler _propertychanged;
        
        public ExtendedObservableCollection(ObservableCollection<TOutput> collection)
        {
            SubscribeOnDispatcher = true;

            _converter = null;
            _filter = null;
            _collection = collection;
            _collection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            ((INotifyPropertyChanged)_collection).PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        public ExtendedObservableCollection(ObservableCollection<TInput> collection, Converter<TInput, TOutput> converter)
        {
            SubscribeOnDispatcher = true;
            _converter = converter;
            _filter = null;

            _inputCollection = collection;
            _inputCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(_inputCollection_CollectionChanged);
         //   ((INotifyPropertyChanged)_inputCollection).PropertyChanged += new PropertyChangedEventHandler(ExtendedObservableCollection_PropertyChanged);
            _collection = new ObservableCollection<TOutput>(collection.Select(i => _converter(i)));

            _collection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            ((INotifyPropertyChanged)_collection).PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);

        }



        void ExtendedObservableCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _inputCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems == null)
                            break;
                        _collection.Insert(e.NewStartingIndex, _converter((TInput)e.NewItems[0]));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        _collection.RemoveAt(e.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        if (e.NewItems == null)
                            break;
                        _collection[e.NewStartingIndex] = _converter((TInput)e.NewItems[0]);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        _collection = new ObservableCollection<TOutput>(_inputCollection.Select(i => _converter(i)));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
#warning ErrorHandling
            }
        }
        public bool SubscribeOnDispatcher { get; set; }

        public static ExtendedObservableCollection<TInput,TOutput> Empty
        {
            get { return new ExtendedObservableCollection<TInput,TOutput>(new ObservableCollection<TOutput>()); }
        }
        public void Add(TOutput item)
        {
            if (_filter != null && _filter(item) == false)
                throw new InvalidOperationException();
            _collection.Add(item);
        }

        public int Add(object value)
        {
            Add((TOutput)value);
            return IndexOf(value);
        }

        public void Clear()
        {
            if (_filter == null)
                _collection.Clear();
            else
                for (int i = 0; i < _collection.Count; )
                {
                    TOutput item = _collection[i];
                    if (_filter(item))
                        _collection.RemoveAt(i);
                    else
                        i++;
                }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collectionchanged += value; }
            remove { _collectionchanged -= value; }
        }

        public bool Contains(TOutput item)
        {
            if (_filter != null && _filter(item) == false)
                return false;
            return _collection.Contains(item);
        }

        public bool Contains(object value)
        {
            if (_filter != null && _filter((TOutput)value) == false)
                return false;
            return ((IList)_collection).Contains(value);
        }

        public void CopyTo(TOutput[] array, int arrayIndex)
        {
            if (_filter == null)
                _collection.CopyTo(array, arrayIndex);
            else
            {
                for (int i = 0; i < _collection.Count; i++)
                {
                    TOutput item = _collection[i];
                    if (_filter(item))
                        array[arrayIndex++] = item;
                }
            }
        }

        public void CopyTo(Array array, int index)
        {
            if (_filter == null)
                ((IList)_collection).CopyTo(array, index);
            else
            {
                for (int i = 0; i < _collection.Count; i++)
                {
                    TOutput item = _collection[i];
                    if (_filter(item))
                        array.SetValue(item, index++);
                }
            }
        }

        public int Count
        {
            get
            {
                if (_filter == null)
                    return _collection.Count;
                else
                {
                    int count = 0;
                    for (int i = 0; i < _collection.Count; i++)
                    {
                        TOutput item = _collection[i];
                        if (_filter(item))
                            count++;
                    }
                    return count;
                }
            }
        }

        public Predicate<TOutput> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }


        public IEnumerator<TOutput> GetEnumerator()
        {
            return new FilteredEnumerator(this, _collection.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FilteredEnumerator(this, _collection.GetEnumerator());
        }

        public int IndexOf(TOutput item)
        {
            if (_filter == null)
                return _collection.IndexOf(item);
            else
            {
                int count = 0;
                for (int i = 0; i < _collection.Count; i++)
                {
                    TOutput indexitem = _collection[i];
                    if (_filter(indexitem))
                    {
                        if (indexitem.Equals(item))
                            return count;
                        else
                            count++;
                    }
                }
                return -1;
            }
        }

        public int IndexOf(object value)
        {
            if (value is TOutput)
                return IndexOf((TOutput)value);
            else
                return -1;
        }

        public void Insert(int index, TOutput item)
        {
            if (_filter != null && _filter(item) == false)
                throw new InvalidOperationException();

            if (_filter == null || index == 0)
                _collection.Insert(index, item);
            else
            {
                for (int i = 0; i < _collection.Count; i++)
                {
                    TOutput indexitem = _collection[i];
                    if (_filter(indexitem))
                    {
                        index--;
                        if (index == 0)
                        {
                            _collection.Insert(i + 1, item);
                            return;
                        }
                    }
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public void Insert(int index, object value)
        {
            Insert(index, (TOutput)value);
        }

        public bool IsFixedSize
        {
            get { return ((IList)_collection).IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return ((IList)_collection).IsReadOnly; }
        }

        public bool IsSynchronized
        {
            get { return ((IList)_collection).IsSynchronized; }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertychanged += value; }
            remove { _propertychanged -= value; }
        }

        public bool Remove(TOutput item)
        {
            if (_filter != null && _filter(item) == false)
                return false;

            return _collection.Remove(item);
        }

        public void Remove(object value)
        {
            if (!(value is TOutput) || (_filter != null && _filter((TOutput)value) == false))
                return;

            ((IList)_collection).Remove(value);
        }

        public void RemoveAt(int index)
        {
            if (_filter == null)
                _collection.RemoveAt(index);
            else
            {
                for (int i = 0; i < _collection.Count; i++)
                {
                    TOutput indexitem = _collection[i];
                    if (_filter(indexitem))
                    {
                        if (index == 0)
                        {
                            _collection.RemoveAt(i);
                            return;
                        }
                        else
                            index--;
                    }
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public object SyncRoot
        {
            get { return ((IList)_collection).SyncRoot; }
        }

        public TOutput this[int index]
        {
            get
            {
                if (_filter == null)
                    return _collection[index];
                else
                {
                    for (int i = 0; i < _collection.Count; i++)
                    {
                        TOutput indexitem = _collection[i];
                        if (_filter(indexitem))
                        {
                            if (index == 0)
                            {
                                return indexitem;
                            }
                            else
                                index--;
                        }
                    }
                    throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                if (_filter == null)
                    _collection[index] = value;
                else if (_filter(value) == false)
                    throw new InvalidOperationException();
                else
                {
                    for (int i = 0; i < _collection.Count; i++)
                    {
                        TOutput indexitem = _collection[i];
                        if (_filter(indexitem))
                        {
                            if (index == 0)
                            {
                                _collection[i] = value;
                            }
                            else
                                index--;
                        }
                    }
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                return (object)this[index];
            }
            set
            {
                this[index] = (TOutput)value;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_collectionchanged != null)
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    RaiseCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    return;
                }
                // Check the NewItems
                List<TOutput> newlist = new List<TOutput>();
                if (e.NewItems != null)
                    foreach (TOutput item in e.NewItems)
                        if (_filter == null || _filter(item) == true)
                            newlist.Add(item);

                // Check the OldItems
                List<TOutput> oldlist = new List<TOutput>();
                if (e.OldItems != null)
                    foreach (TOutput item in e.OldItems)
                        if (_filter == null || _filter(item) == true)
                            oldlist.Add(item);

                // Create the Add/Remove/Replace lists
                List<TOutput> addlist = new List<TOutput>();
                List<TOutput> removelist = new List<TOutput>();
                List<TOutput> replacelist = new List<TOutput>();

                //send corrent event
                foreach (TOutput item in newlist)
                    if (oldlist.Contains(item))
                        RaiseCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, this[IndexOf(item)], IndexOf(item)));//todo check
                    else
                        RaiseCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, IndexOf(item))); //todo check
                foreach (TOutput item in oldlist)
                    if (newlist.Contains(item))
                        continue;
                    else
                        RaiseCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, IndexOf(item)));//todo check

            }
        }

        void RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (SubscribeOnDispatcher)
                Deployment.Current.Dispatcher.BeginInvoke(() => { try { _collectionchanged(sender, args); } catch { } });
            else
                _collectionchanged(sender, args);
        }
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertychanged != null)
                _propertychanged(this, e);
        }

        private class FilteredEnumerator : IEnumerator<TOutput>, IEnumerator
        {
            private ExtendedObservableCollection<TInput, TOutput> _filteredcollection;
            private IEnumerator<TOutput> _enumerator;

            public FilteredEnumerator(ExtendedObservableCollection<TInput, TOutput> filteredcollection, IEnumerator<TOutput> enumerator)
            {
                _filteredcollection = filteredcollection;
                _enumerator = enumerator;
            }

            public TOutput Current
            {
                get
                {
                    if (_filteredcollection.Filter == null)
                        return _enumerator.Current;
                    else if (_filteredcollection.Filter(_enumerator.Current) == false)
                        throw new InvalidOperationException();
                    else
                        return _enumerator.Current;
                }
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                while (true)
                {
                    if (_enumerator.MoveNext() == false)
                        return false;
                    if (_filteredcollection.Filter == null
                        || _filteredcollection.Filter(_enumerator.Current) == true)
                        return true;
                }
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}

