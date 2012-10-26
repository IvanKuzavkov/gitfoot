﻿using System;
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
using gitfoot.Service;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using gitfoot.Models;
using Microsoft.Phone.Reactive;

namespace gitfoot.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private GithubApiService GHService { get; set; }
        private INavigationService NavigationService { get; set; }
        public INotificationController NotificationController { get; set; }

        private bool _isLogin;

        public bool IsLogin
        {
            get { return _isLogin; }
            set
            {
                if (_isLogin != value)
                {
                    _isLogin = value;
                    NotifyPropertyChanged("IsLogin");
                }
            }
        }
        

        private string user;

        public string User
        {
            get { return user; }
            set 
            {
                if (user != value)
                {
                    user = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string pass;

        public string Password
        {
            get { return pass; }
            set
            {
                if (pass != value)
                {
                    pass = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }
        

        public LoginViewModel(INavigationService navigateService, GithubApiService ghservice, INotificationController notController)
        {
            IsLogin = false;
            NavigationService = navigateService;
            GHService = ghservice;
            NotificationController = notController;

            _loginCommand = new DelegateCommand(() =>
            {
                NotificationController.SplashScreen.IsVisible = true;
                GHService.UseCredentials(User, Password);
            });
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

        private DelegateCommand _loginCommand;
        public DelegateCommand Login { get { return _loginCommand; } }
    }
}
