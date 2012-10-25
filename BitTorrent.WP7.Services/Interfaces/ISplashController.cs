using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;
using System.ComponentModel;

namespace BitTorrent.WP7.Services
{
    public interface ISplashController : INotifyPropertyChanged
    {
        bool IsVisible { get; set; }
        string Message { get; set; }
    }

}
