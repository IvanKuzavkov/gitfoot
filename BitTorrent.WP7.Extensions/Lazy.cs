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
    public class Lazy<T>
    {
        private T value;
        private Func<T> loader;

        public Lazy(T value)
        {
            this.value = value;
        }

        public Lazy(Func<T> loader)
        {
            this.loader = loader;
        }

        public T Value
        {
            get
            {
                if (loader != null)
                {
                    value = loader();
                    loader = null;
                }
                return value;
            }
        }

        public static implicit operator T(Lazy<T> lazy)
        {
            return lazy.Value;
        }

        public static implicit operator Lazy<T>(T value)
        {
            return new Lazy<T>(value);
        }
    }

    public static class Lazy
    {
        public static Lazy<T> From<T>(Func<T> loader)
        {
            return new Lazy<T>(loader);
        }
        public static Lazy<T> From<T>(T value)
        {
            return new Lazy<T>(value);
        }
    }
}
