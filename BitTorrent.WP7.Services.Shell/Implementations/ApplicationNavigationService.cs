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
using Microsoft.Phone.Controls;
using BitTorrent.WP7.Extensions;
using System.Linq;

namespace BitTorrent.WP7.Services
{
    public class ApplicationNavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame frame;
        
        public ApplicationNavigationService(PhoneApplicationFrame frame)
        {
            this.frame = frame;
        }

        public bool CanGoBack
        {
            get { return this.frame.CanGoBack; }
        }

        public Uri CurrentSource
        {
            get { return this.frame.CurrentSource; }
        }

        public bool Navigate(Uri source)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => this.frame.Navigate(source));
            return true;
        }

        public void GoBack()
        {
            this.frame.GoBack();
        }

        public bool Navigate<T>(T page)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return Navigate(new Uri(AttributeFacade.AppPagePath(page), UriKind.Relative));
        }

        public bool Navigate<T>(T page, params Extensions.Tuple<string, string>[] navigationParams)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var endpoint = AttributeFacade.AppPagePath(page);
            var uri = string.Format("{0}{1}"
                , endpoint
                , (navigationParams == null || navigationParams.Count() == 0) ? ""
                        : navigationParams
                        .Aggregate<Tuple<string, string>, string>(
                                    endpoint.Contains("?") ? "&" : "?"
                                    , (s, t) => s + t.TupleToArg() + "&")
                        .TrimEnd('&'));
            return Navigate(new Uri(uri, UriKind.Relative));
        }

    }
}
