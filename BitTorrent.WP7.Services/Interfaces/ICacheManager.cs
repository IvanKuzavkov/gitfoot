using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitTorrent.WP7.Services
{
    public interface ICacheManager
    {
        void Add(string key, object value);

        bool Contains(string key);

        T Get<T>(string key);
        T Get<T>(string key, Func<T> initFunction, double? timeToLive);

        bool Remove(string key);

        object this[string key]
        {
            get;
            set;
        }
    }

    public interface ICacheManager<T> : ICacheManager
    {
        void Add(string key, T value);

        T Get(string key);
        T Get(string key, Func<T> initFunction, double? timeToLive);

        new T this[string key]
        {
            get;
            set;
        }
    }
}
