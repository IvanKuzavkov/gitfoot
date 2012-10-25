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
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Shell;

namespace BitTorrent.WP7.Services
{
    public abstract class BaseApplicationService : IDisposable
    {
        private CompositeDisposable _disposableItems;
        protected CompositeDisposable DisposableItems { get { return _disposableItems ?? (_disposableItems = new CompositeDisposable()); } }
        private bool disposed;

        protected BaseApplicationService()
        {
            PhoneApplicationService.Current.Deactivated += this.OnDeactivated;
            PhoneApplicationService.Current.Activated += this.OnActivated;
        }
        public virtual void IsBeingDeactivated()
        {
            DisposableItems.Clear();
        }

        public virtual void IsBeingActivated()
        { 
        }

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
                DisposableItems.Dispose();

                PhoneApplicationService.Current.Deactivated -= this.OnDeactivated;
                PhoneApplicationService.Current.Activated -= this.OnActivated;
            }

            this.disposed = true;
        }

        private void OnDeactivated(object s, DeactivatedEventArgs e)
        {
            this.IsBeingDeactivated();
        }

        private void OnActivated(object s, ActivatedEventArgs e)
        {
            this.IsBeingActivated();
        }
    }
}
