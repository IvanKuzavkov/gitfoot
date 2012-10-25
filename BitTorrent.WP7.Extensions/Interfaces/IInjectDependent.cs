using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitTorrent.WP7.Extensions
{
    public interface IInjectDependent<T> where T:class
    {
        void Inject(T instance);
        void Erase(T instance);
    }
}
