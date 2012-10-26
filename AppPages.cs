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

namespace gitfoot
{
    public enum AppPages
    {
        [FilePath("/MainPage.xaml")]
        MainPage,

        [FilePath("/LoginPage.xaml")]
        LoginPage,

/*        [FilePath("/Views/MainViewPage.xaml")]
        MainViewPage,

        [FilePath("/Views/SettingsPage.xaml")]
        SettingsPage,

        [FilePath("/Views/RssViewPage.xaml")]
        RssViewPage,

        [FilePath("/Views/TorrentViewPage.xaml")]
        TorrentViewPage*/
    }
}
