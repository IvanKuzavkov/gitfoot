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
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Phone.Reactive;
using BitTorrent.WP7.TorrentRemote.App.Services;

namespace gitfoot.Service
{
    public class OctocatsService
    {
        private string OctocatsLink { get; set; }

        private string OctocatsPattern { get; set; }

        private string _currentOctocat;
        public string CurrentOctocat 
        {
            get { return _currentOctocat; }

            private set
            {
                if (_currentOctocat != value)
                {
                    _currentOctocat = value;
//                    var t = App.Current.Resources["ViewModelLocator"];
//                    (t as ViewModelLocator).MainViewModel.ImageUri = new Uri(_currentOctocat);
                }
            }
        }

        public OctocatsService(string link = "http://feeds.feedburner.com/Octocats", string pattern = "http://octodex.github.com/images/")
        {
            OctocatsLink = link;
            OctocatsPattern = pattern;

            CurrentOctocat = "http://octodex.github.com/images/stormtroopocat.jpg";

            GetOctocats();
        }


        public void GetOctocats()
        {
            HttpWebRequest request = WebRequest.CreateHttp(OctocatsLink);

            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                .Select(r =>
                    {
                        using (StreamReader reader = new StreamReader(r.GetResponseStream()))
                        {
                            List<Uri> links = fetchOctocatsLinks(reader.ReadToEnd());
                            int randInd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)).Next(links.Count);
                            return links[randInd];
                        }
                    })
                    .Subscribe(s =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    CurrentOctocat = s.ToString();

                                    var t = App.Current.Resources["ViewModelLocator"];
                                    (t as ViewModelLocator).MainViewModel.ImageUri = s;
                                });
                        });
        }

        private List<Uri> fetchOctocatsLinks(string html)
        {
            List<Uri> links = new List<Uri>();
            string regexImg = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection mathesImg = Regex.Matches(html, regexImg, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            foreach (Match match in mathesImg)
            {
                string href = match.Groups[1].Value;
                if (looksLikeOctocatLink(href))
                {
                    links.Add(new Uri(href));
                }
            }

            return links;
        }

        private bool looksLikeOctocatLink(string link)
        {
            return link.StartsWith(OctocatsPattern, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
