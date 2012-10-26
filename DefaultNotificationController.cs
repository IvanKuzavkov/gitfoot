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
using BitTorrent.WP7.Services;
using System.ComponentModel;
using BitTorrent.WP7.Toolkit;

namespace gitfoot
{
    public class DefaultNotificationController : INotifyPropertyChanged, INotificationController
    {
        public DefaultNotificationController()
        {
            _progressBar = SplashProgressBar.Instance;
            _screen = BitTorrent.WP7.Toolkit.SplashScreen.Instance;
        }

        private ISplashController _progressBar;
        private ISplashController _screen;

        private static INotificationController _instance;
        public static INotificationController Instance
        {
            get { return _instance ?? (_instance = new DefaultNotificationController()); }
        }

        #region IProgressBarController Members

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _screen.Message = value;
                    _progressBar.Message = value;
                    _message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        public ISplashController ProgressBar
        {
            get { return _progressBar; }
        }

        public ISplashController SplashScreen
        {
            get { return _screen; }
        }

        #endregion

        public void ShowMessage(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(message));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
