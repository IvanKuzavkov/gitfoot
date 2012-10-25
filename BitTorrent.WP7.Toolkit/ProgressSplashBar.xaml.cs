using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BitTorrent.WP7.Toolkit
{
    public partial class ProgressSplashBar : UserControl
    {
        public ProgressSplashBar()
        {
            InitializeComponent();
			this.progressBar.IsIndeterminate = true;

            this.DataContext = SplashProgressBar.Instance;
        }
    }
}
