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
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Prism.ViewModel;
using BitTorrent.WP7.Services;

namespace BitTorrent.WP7.Toolkit
{

    public class SplashProgressBar : SplashController<ProgressSplashBar>
    {
        private SplashProgressBar()
        { }
    }

    public class SplashScreen : SplashController<ProgressSplashScreen>
    {
        private SplashScreen()
        { }
    }

}
