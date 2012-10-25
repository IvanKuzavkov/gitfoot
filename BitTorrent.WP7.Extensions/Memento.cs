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

namespace BitTorrent.WP7.Extensions
{
    public class Memento<T>
    {
        T _state;
        public virtual void SaveMemento(T value)
        {
            _state = value;
        }
        public virtual T LoadMemento()
        {
            return _state;
        }
    }

    public interface IMemento
    {
        void Load();
        void Save();
        void Reset();
    }

    public interface IRollBack
    {
        bool CanRollBack { get; }
        bool RollBack();
    }
}
