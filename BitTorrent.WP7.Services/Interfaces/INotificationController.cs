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
using System.ComponentModel;

namespace BitTorrent.WP7.Services
{
    public interface INotificationController : INotifyPropertyChanged
    {
        ISplashController ProgressBar { get; }
        ISplashController SplashScreen { get; }
        void ShowMessage(string message);
        string Message { get; set; }
    }
}
