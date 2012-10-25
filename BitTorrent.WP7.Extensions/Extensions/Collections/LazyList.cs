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

namespace BitTorrent.WP7.Extensions
{
    public class LazyList : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public LazyList()
        { }



        public void Insert(int newIndex, object value)
        {
            //int[] sortedKeys = cache.Keys.ToArray();
            //Array.Sort(sortedKeys);

            //for (int i = sortedKeys.Length - 1; i >= 0; i--)
            //{
            //  int key = sortedKeys[i];
            //  if (key < newIndex)
            //    break;

            //  cache[key + 1] = cache[key];
            //  cache[key].Index++;
            //}

            //cache[newIndex] = (SampleLazyDataItem)value;
            //var handler = CollectionChanged;
            //if (handler != null)
            //  handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, newIndex));
        }

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
