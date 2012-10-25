using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitTorrent.WP7.Extensions
{
    public interface IRepository
    {
        IEnumerable<T> Get<T>();
        IEnumerable<T> Get<T>(string filter);
        IObservable<IEnumerable<T>> GetAsync<T>();
        IObservable<IEnumerable<T>> GetAsync<T>(string filter);
        void Refresh();
    }
}
