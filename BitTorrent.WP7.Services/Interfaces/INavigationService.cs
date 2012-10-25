using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitTorrent.WP7.Extensions;

namespace BitTorrent.WP7.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        Uri CurrentSource { get; }
        bool Navigate(Uri source);
        bool Navigate<T>(T page);
        bool Navigate<T>(T page, params Tuple<string,string>[] navigationParams);
        void GoBack();
    }
}
