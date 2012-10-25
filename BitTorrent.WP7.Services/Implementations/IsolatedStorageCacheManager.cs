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
using System.IO.IsolatedStorage;

namespace BitTorrent.WP7.Services
{
    public class IsolatedStorageCacheManager:ICacheManager
    {
        IsolatedStorageSettings Settings;
        private IsolatedStorageCacheManager()
        {
            Settings = IsolatedStorageSettings.ApplicationSettings;
        }

        private static ICacheManager _instance;
        public static ICacheManager Instance
        {
            get { return _instance ?? (_instance = new IsolatedStorageCacheManager()); }
        }

        #region ICacheManager Members

        public void Add(string key, object value)
        {
            if (Contains(key))
                Settings.Remove(key);
            Settings.Add(key, value);
            Settings.Save();
        }

        public bool Contains(string key)
        {
            return Settings.Contains(key);
        }

        public T Get<T>(string key)
        {
            T result;
            if (Settings.TryGetValue(key, out result))
                return result;

            return default(T);
        }

        public T Get<T>(string key, Func<T> initFunction, double? timeToLive)
        {
            if (!Contains(key))
            {
                var value = initFunction();
                Add(key, value);
                return value;
            }
            else
                return Get<T>(key);
        }

        public bool Remove(string key)
        {
            var res = Settings.Remove(key);
            if (res == true)
            {
                Settings.Save();
                return true;
            }
            return false;
        }

        public object this[string key]
        {
            get
            {
                return Settings[key];
            }
            set
            {
                Settings[key] = value;
                Settings.Save();
            }
        }

        #endregion
    }
}
