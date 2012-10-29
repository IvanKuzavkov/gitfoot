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
using Microsoft.Phone.Controls;
using gitfoot.ViewModels;
using System.Windows.Navigation;

namespace gitfoot
{
    public partial class LoginPage : PhoneApplicationPage
    {
        LoginViewModel _viewModel;
        private LoginViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = (LoginViewModel)this.DataContext); }
        }

        public LoginPage()
        {
            InitializeComponent();
            UserName.KeyUp += new KeyEventHandler(Keyboard_KeyUp);
            Pwd.KeyUp += new KeyEventHandler(Keyboard_KeyUp);

            ViewModel.NotificationController.SplashScreen.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SplashScreen_PropertyChanged);
        }

        void SplashScreen_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible" && !ViewModel.IsLogin)
                ApplicationBar.IsVisible = !ViewModel.NotificationController.SplashScreen.IsVisible;

        }

        void Keyboard_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == UserName)
                    Pwd.Focus();
                else
                    LoginBtn_Click(this, null);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationBar.IsVisible = true;
            if (e.NavigationMode == NavigationMode.New && e.IsNavigationInitiator)
            {
                while (NavigationService.RemoveBackEntry() != null)
                { }
            }
            ViewModel.Init();
        }

        private void LoginBtn_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName.Text))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("User name is empty"));
                return;
            }
            this.Focus();

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Internet is not available. Check connection and try again.");
                return;
            }

            if (ViewModel.Login.CanExecute())
                ViewModel.Login.Execute();

            ApplicationBar.IsVisible = false;
        }
    }
}