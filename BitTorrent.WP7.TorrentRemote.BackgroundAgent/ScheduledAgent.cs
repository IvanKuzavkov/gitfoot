using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System;
using System.Linq;
using BitTorrent.WP7.TorrentRemote.Services.Falcon;
using BitTorrent.WP7.TorrentRemote.Services.Raptor;
using BitTorrent.WP7.TorrentRemote.Services;
using BitTorrent.WP7.Services;
using BitTorrent.WP7.TorrentRemote.Core.Services;

namespace BitTorrent.WP7.TorrentRemote.BackgroundAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        public const string ServiceLoginUrl = "https://remote.utorrent.com";

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            try
            {
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    return;

                IAppSettingsStore settings = new AppSettingsStore(IsolatedStorageCacheManager.Instance);
                IFalconService fService = new FalconWebServiceClient(ServiceLoginUrl);
                using (var updatesService = new CheckUpdatesService(settings, new RaptorService(fService, AESCipherManager.Instance), AESCipherManager.Instance))
                {
                    var updates = updatesService.Updates();


                    UpdateTile(updatesService.GetUpdateCount());

                    if (updates.Count() == 0)
                    {
#if DEBUG
                        ShellToast toast2 = new ShellToast();
                        toast2.Title = "DEBUG: ";
                        toast2.Content = "No updates";
                        toast2.Show();
#endif
                    }
                    else
                    {
                        string message =
                            updates.Count() == 1
                            ? string.Format("Torrent {0} downloaded", updates.First())
                            : string.Format("{0} favorite torrents were downloaded", updates.Count());

                        ShellToast toast = new ShellToast();
                        toast.Title = "Torrents: ";
                        toast.Content = message;
                        toast.Show();
                    }
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                ShellToast toast = new ShellToast();
                toast.Title = "Error:";
                toast.Content = ex.Message;
                toast.Show();
#endif
            }
            finally
            {

                
#if DEBUG_AGENT
                ScheduledActionService.LaunchForTest(
                    task.Name, TimeSpan.FromSeconds(30));
#endif

                NotifyComplete();
            }
        }

        private void UpdateTile(int count)
        {
            ShellTile apptile = ShellTile.ActiveTiles.First();
            StandardTileData appTileData = new StandardTileData();

            appTileData.Count = count;

            if (count == 0)
            {
                appTileData.BackTitle = null;
                appTileData.BackContent = null;
            }
            else
            {
                appTileData.BackTitle = "µTorrent Remote";
                string message =
                        count == 1
                        ? string.Format("{0} torrent was downloaded", count)
                        : string.Format("{0} torrents were downloaded", count);

                appTileData.BackContent = message;
            }

            apptile.Update(appTileData); 
        }
    }
}