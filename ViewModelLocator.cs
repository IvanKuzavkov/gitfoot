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
using gitfoot.ViewModels;
using BitTorrent.WP7.TorrentRemote.App.Services;

namespace BitTorrent.WP7.TorrentRemote.App.Services
{
    public class ViewModelLocator : IDisposable
    {
        ServiceContainer _container;
        bool disposed = false;

        public ViewModelLocator()
        {
            this._container = new ServiceContainer();
        }

        public void ReInit()
        {
            this._container.Dispose();
            this._container = new ServiceContainer();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return _container.Container.Resolve<MainViewModel>();
            }
        }
        #region IDisposable Members

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
                this._container.Dispose();
            }

            this.disposed = true;
        }

        #endregion
    }
}
