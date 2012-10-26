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
using Funq;
using BitTorrent.WP7.Services;
using gitfoot.ViewModels;

namespace BitTorrent.WP7.TorrentRemote.App.Services
{

    public class ServiceContainer : IDisposable
    {
        public ServiceContainer()
        {
            Container = new Container();

            Init();
        }

        public Container Container { get; private set; }

        private void Init()
        {
            Container.Register<INavigationService>(_ => new ApplicationNavigationService(((gitfoot.App)Application.Current).RootFrame));


 //           Container.Register<ICacheManager>(IsolatedStorageCacheManager.Instance);


            Container.Register<MainViewModel>(c => new MainViewModel(
                                           c.Resolve<INavigationService>()))
                                           .ReusedWithin(ReuseScope.Container);

        }

        #region Dispose
        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Container.Dispose();
            }

            this.disposed = true;
        }
        #endregion
    }
}

