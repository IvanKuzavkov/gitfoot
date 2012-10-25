using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitTorrent.WP7.Extensions
{
    public interface IProgressController<T>
    {
        void Start(T item);
        void Pause(T item);
        void Resume(T item);
        void Stop(T item);
        void Cancel(T item);
    }
}
