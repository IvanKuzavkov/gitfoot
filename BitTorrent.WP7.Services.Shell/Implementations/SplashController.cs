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
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Controls.Primitives;

namespace BitTorrent.WP7.Services
{

    public class SplashController<T> : NotificationObject, ISplashController
        where T : UIElement, new()
    {
        private volatile Popup popup = null;
        private volatile bool isShown = false;

        protected SplashController()
        { }

        private static ISplashController _instance;
        public static ISplashController Instance
        {
            get { return _instance ?? (_instance = new SplashController<T>()); }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    if (value == true)
                        Show();
                    else
                        Hide();

                    _isVisible = value;

                    Deployment.Current.Dispatcher.BeginInvoke(
                            () => RaisePropertyChanged("IsVisible"));
                }
            }
        }

        private void Show()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (isShown == false)
                {
                    popup = new Popup();
                    popup.Child = new T();
                    popup.IsOpen = true;
                    isShown = true;
                }
            });
        }

        private void Hide()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (popup != null)
                {
                    popup.IsOpen = false;
                }
                isShown = false;
            });
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () => RaisePropertyChanged("Message"));
                }
            }
        }
    }

}
