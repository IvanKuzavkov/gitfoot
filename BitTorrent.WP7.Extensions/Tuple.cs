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
using System.ComponentModel;

namespace BitTorrent.WP7.Extensions
{
    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

    }
    public class Tuple
    {
        public static Tuple<T1, T2> From<T1, T2>(T1 t1, T2 t2)
        {
            return new Tuple<T1, T2>(t1, t2);
        }

        public static NotifiedTuple<T1, T2> FromN<T1, T2>(T1 t1, T2 t2)
        {
            return new NotifiedTuple<T1, T2>(t1, t2);
        }
    }

    public class NotifiedTuple<T1, T2> : INotifyPropertyChanged
    {
        private T1 _item1;
        public T1 Item1
        {
            get { return _item1; }
            set
            {
                _item1 = value;
                RaisePropertyChanged("Item1");
            }
        }

        private T2 _item2;
        public T2 Item2
        {
            get { return _item2; }
            set
            {

                _item2 = value;
                RaisePropertyChanged("Item2");

            }
        }


        public NotifiedTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        #region INotifyPropertyChanged Members
        protected void RaisePropertyChanged(string propertyName, bool InDispatcher)
        {
            if (InDispatcher)
                Deployment.Current.Dispatcher.BeginInvoke(() => this.RaisePropertyChanged(propertyName));
            else
                RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
