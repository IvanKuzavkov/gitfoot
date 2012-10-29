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
using gitfoot.Service;
using BitTorrent.WP7.Services;
using Microsoft.Phone.Shell;

namespace gitfoot.ViewModels
{
    public class AppViewModel
    {
        private GithubApiService GHService { get; set; }
        private INavigationService NavigationService { get; set; }
        public INotificationController NotificationController { get; set; }

        public AppViewModel(INavigationService navigateService, GithubApiService ghservice, INotificationController notController, ICacheManager cache)
        {
            NavigationService = navigateService;
            GHService = ghservice;
            NotificationController = notController;


            //Autologin can be implemented here
            NavigationService.Navigate(AppPages.LoginPage);
        }
    }
}
