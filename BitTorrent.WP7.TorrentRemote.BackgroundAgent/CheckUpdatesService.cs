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
//using BitTorrent.WP7.TorrentRemote.Core.Services.Raptor;
using BitTorrent.WP7.Services;
using System.Collections.Generic;
using System.Linq;
using BitTorrent.WP7.TorrentRemote.Services;
using Newtonsoft.Json;
using BitTorrent.WP7.TorrentRemote.Services.Raptor;
using BitTorrent.WP7.Extensions;


using BitTorrent.WP7.TorrentRemote.Core.Models;
namespace BitTorrent.WP7.TorrentRemote.Core.Services
{
    public class CheckUpdatesService: IDisposable
    {
        IDisposable DisposableItem;
        IAppSettingsStore AppSettings { get; set; }
        IWebServiceClient Service { get; set; }
        ICipherManager CipherManager { get; set; }
        int UpdateCounter;
        public CheckUpdatesService(IAppSettingsStore appSettings, IWebServiceClient service, ICipherManager cipherManager)
        {
            AppSettings = appSettings;
            Service = service;
            CipherManager = cipherManager;
        }

        System.Threading.ManualResetEvent _waitHandle;
        public IEnumerable<string> Updates()
        {

            var result = new List<string>();
            if (AppSettings.CurrentUser == null
                || AppSettings.CurrentUser.FavoriteTorrents == null
                || !AppSettings.CurrentUser.FavoriteTorrents.Any(t => !t.IsDownloaded))
                return result;

            UpdateCounter = AppSettings.UpdateCounter;
            _waitHandle = new System.Threading.ManualResetEvent(false);

            if (AppSettings.CurrentUser != null
                    && !string.IsNullOrEmpty(AppSettings.CurrentUser.AESKey)
                    && !string.IsNullOrEmpty(AppSettings.CurrentUser.Cookies)
                    && !string.IsNullOrEmpty(AppSettings.CurrentUser.ClientUrl))
            {
                CipherManager.Init(AppSettings.CurrentUser.AESKey);
                Service.ServiceUrl = AppSettings.CurrentUser.ClientUrl;
                Service.Login(ParseCookies(AppSettings.CurrentUser.ClientUrl, AppSettings.CurrentUser.Cookies));

                DisposableItem =
                    Service.TryConnect()
                        .Catch((Exception ex)=>ForceLogin(ex))
                        .SelectMany(r =>
                            {
                                if (r == true)
                                {
                                    return GetList();
                                }
                                else
                                    return Observable.Return<string>(null);
                            })
                            .Subscribe(
                            r =>
                            {
                                if (r == null)
                                    return;

                                var notDownloadedTorrents = AppSettings.CurrentUser.FavoriteTorrents.Where(i => !i.IsDownloaded);

                                var torrents = GetTorrents(r);

                                var upd = torrents
                                            .Where(t => t != null && t.Progress >= 1000
                                                            && notDownloadedTorrents.Any(i => i.Id == t.Id))
                                            .Select(t => new { torrent = t, storrent = notDownloadedTorrents.FirstOrDefault(i => i.Id == t.Id) })
                                            .ToList();

                                UpdateCounter += upd.Count();
                                result.AddRange(upd.Where(i => i.storrent.IsNotified).Select(i => i.torrent.Name));

                                upd.ForEach(i => i.storrent.IsDownloaded = true);
                                AppSettings.UpdateCounter = UpdateCounter;
                                AppSettings.CurrentUser.Save();
                                //  _waitHandle.Set();
                            },
                    () => _waitHandle.Set());


            _waitHandle.WaitOne(25000);
            }
            return result;
        }
        public int GetUpdateCount() { return UpdateCounter; }

        private IObservable<string> GetList()
        {
            return Service.Post<string, string>(
               RaptorService.BuildEndpoint(Tuple.From("list", "1"), Tuple.From("getmsg", "1"))
               , null);
        }


        private IObservable<bool> ForceLogin(Exception ex)
        {

            //    var observer = Observer.Create<TaskCompleteSummary<string>>(
            //(r) =>
            //{
            //    switch (r.Result)
            //    {
            //        case TaskResult.Info:
            //            if (r.Message != null)
            //                NotificationController.Message = r.Message;
            //            break;
            //        case TaskResult.Success:

            //            break;
            //        case TaskResult.Error:
            //            NotificationController.SplashScreen.IsVisible = false;
            //            ErrorHandling(r);
            //            break;
            //        case TaskResult.Complete:
            //            IsLogin = UTService.IsLogin;
            //            SaveCurrentUser(r.ResultObject);
            //            NavigationService.Navigate(AppPages.MainViewPage);
            //            NotificationController.SplashScreen.IsVisible = false;
            //            this.Dispose();

            //            break;
            //        default:
            //            break;
            //    }
            //});

            //    DisposableItems.Add(UTService.LoginNotification.Subscribe(observer));
            //    DisposableItems.Add(UTService.LoginNotification.Connect());

            try
            {
                Service.ServiceUrl = null;
                return Service.LoginNotification

                                .Catch(Observable.Return(new TaskCompleteSummary<string>(TaskResult.Error, "Connection exception. Service unavailable")))
                                .Where(r=>r.Result == TaskResult.Complete || r.Result == TaskResult.Error)
                                .SelectMany(r =>
                                {
                                    if (r.Result == TaskResult.Complete)
                                    {
                             //           SaveCurrentUser(r.ResultObject);
                                        return Observable.Return(true);
                                    }
                                    else
                                        return Observable.Return(false);
                                });
            }
            finally
            {
                Service.LoginNotification.Connect();
                Service.Login(AppSettings.CurrentUser.UserName, AppSettings.CurrentUser.Password);
            }

        }

        private void SaveCurrentUser(string p)
        {
            var currentUser = AppSettings.CurrentUser;

            try
            {
                currentUser.ClientUrl = Service.ServiceUrl;
                currentUser.AESKey = CipherManager.Get(AESCipherManager.AESKey).ToString();
                currentUser.Cookies = Service.Cookies.GetCookieHeader(new Uri(Service.ServiceUrl));
            }
            catch (Exception ex)
            {
            }
            AppSettings.CurrentUser = currentUser;
        }

        private IEnumerable<Torrent> GetTorrents(string s)
        {
            if (string.IsNullOrEmpty(s))
                return Enumerable.Empty<Torrent>(); 

            ListResponse list = null;
            try
            {
                list = new ListResponse(JsonConvert.DeserializeObject<JsonListResponse>(s));
            }
            catch (Exception ex)
            {
            }

            if (list == null)
                return Enumerable.Empty<Torrent>();

            return list.Torrents;
        }


        private CookieContainer ParseCookies(string uriString, string cookies)
        {
            var container = new CookieContainer();
            var uri = new Uri(uriString);
            foreach (var cookie in cookies.Split(';'))
                container.SetCookies(uri, cookie);
            return container;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (DisposableItem != null)
                DisposableItem.Dispose();
        }

        #endregion
    }
}
