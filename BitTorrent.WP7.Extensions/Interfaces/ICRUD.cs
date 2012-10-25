using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BitTorrent.WP7.Extensions
{
    public interface ICRUD<T>
    {
        void Create(T obj);
        void Delete(T obj);
        void Update(T obj);

        T ReadSync(string key);
        IObservable<T> Read(string key);
    }

}
