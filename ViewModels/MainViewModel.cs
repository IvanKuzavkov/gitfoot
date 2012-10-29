using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using BitTorrent.WP7.Extensions;
using System.IO;
using Microsoft.Phone.Reactive;
using BitTorrent.WP7.Services;
//using Microsoft.Practices.Prism.ViewModel;
using gitfoot.Service;
using gitfoot.Models;

namespace gitfoot.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        INavigationService _navigationService;
        GithubApiService GHService { get; set; }
        OctocatsService OctocatService { get; set; }
        public INotificationController NotificationController { get; set; }
        public event EventHandler onImageChanged;

        private Uri imageUri;
        public Uri ImageUri
        {
            get { return imageUri; }
            set
            {
                if (imageUri == value)
                    return;
                imageUri = value;
                bitmapImage = null;
                if (onImageChanged != null)
                    onImageChanged(this, null);
            }
        }

        string DefaultImage = "http://octodex.github.com/images/stormtroopocat.jpg";
        WeakReference<BitmapImage> bitmapImage;

        public ImageSource PanoramaImage
        {
            get
            {
                if (bitmapImage != null)
                {
                    if (bitmapImage.IsAlive)
                        return bitmapImage.Target;
                }

                if (imageUri != null)
                    ThreadPool.QueueUserWorkItem(DownloadImage, imageUri);

                return new BitmapImage(new Uri(DefaultImage, UriKind.Absolute));
            }
            set
            {
                string s = "12";
            }
        }

        void DownloadImage(object state)
        {
            HttpWebRequest request = WebRequest.CreateHttp(state as Uri);

            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(r =>
                    {
                        // Insert random sleep so that all images don't fire at once, impacting performance
                        Thread.Sleep(new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)).Next(1000));

                        using (Stream stream = r.GetResponseStream())
                        {
                            return CopyStream(stream, (int)r.ContentLength);
                        }
                    })
                    .Subscribe(s =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            BitmapImage bm = new BitmapImage();
                            bm.SetSource(s);

                            if (bitmapImage == null)
                                bitmapImage = new WeakReference<BitmapImage>(bm);
                            else
                                bitmapImage.Target = bm;
                            if (onImageChanged != null)
                                onImageChanged(this, null);
                        });
                    });
        }

        const int MAX_COPY_CHUNK_SIZE = 5000;
        static Stream CopyStream(Stream stream, int length)
        {
            Stream copy = new MemoryStream(length);

            int chunkSize = Math.Min(length, MAX_COPY_CHUNK_SIZE);
            byte[] buffer = new byte[chunkSize];
            int amountRead = 0;

            do
            {
                amountRead = stream.Read(buffer, 0, chunkSize);
                copy.Write(buffer, 0, amountRead);
            } while (amountRead == chunkSize);

            copy.Seek(0, SeekOrigin.Begin);
            return copy;
        }

        public MainViewModel()
        {
            this.NewsItems = new ObservableCollection<ItemViewModel>();
        }

        public MainViewModel(INavigationService navigationService, GithubApiService ghSvc, INotificationController notController, OctocatsService octocatSvc)
        {
            NotificationController = notController;
            GHService = ghSvc;
            OctocatService = octocatSvc;
 
            this.NewsItems = new ObservableCollection<ItemViewModel>();

//            User = Observable.Return(new User());

//            GHService.GetUser(User);

/*            User.Subscribe(user =>
                {
                    user.Organizations.ForEach(org => GHService.GetReposForOrganization(org, RepositItems));
                });

            RepositItems = new ObservableCollection<ItemViewModel>();
            GHService.GetUserRepos(RepositItems);

            IssuesItems = new ObservableCollection<ItemViewModel>();
            GHService.GetIssues(IssuesItems);

            _navigationService = navigationService;*/
        }

        public void Init()
        {
            RepositItems = new ObservableCollection<ItemViewModel>();
            GHService.GetUserRepos(RepositItems);

            IssuesItems = new ObservableCollection<ItemViewModel>();
            GHService.GetIssues(IssuesItems);
        }

        private IObservable<User> _user;
        public IObservable<User> User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        /// 
        private ObservableCollection<ItemViewModel> _newsItems;

        public ObservableCollection<ItemViewModel> NewsItems
        {
            get { return _newsItems; }
            set { _newsItems = value; }
        }
        
        public ObservableCollection<ItemViewModel> RepositItems { get; private set; }
        public ObservableCollection<ItemViewModel> IssuesItems { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            //Replace image url if needed
 //           ImageUri = new Uri(DefaultImage, UriKind.Absolute);
            ImageUri = new Uri(OctocatService.CurrentOctocat);
            this.IsDataLoaded = true;
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
    }
}