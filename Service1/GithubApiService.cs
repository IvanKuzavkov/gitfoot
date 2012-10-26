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
using BitTorrent.WP7.Extensions;
using gitfoot.Models;
using System.Collections.Generic;
using RestSharp;
using System.Collections.ObjectModel;
using gitfoot.ViewModels;

namespace gitfoot.Service
{
    public class GithubApiService
    {
        protected string ServiceApiUrl { get; set; }

        protected string User { get; set; }
        protected string Password { get; set; }

        protected RestClient restClient;

        public GithubApiService(string serviceUrl = "https://api.github.com")
        {
            ServiceApiUrl = serviceUrl;

            restClient = new RestClient(ServiceApiUrl);
        }

        public void UseCredentials(string username, string password)
        {
            User = username;
            Password = password;

            restClient.Authenticator = new HttpBasicAuthenticator(User, Password);
        }


        public void GetUser(IObservable<User> user)
        {
            var request = new RestRequest("user", Method.GET);

            restClient.ExecuteAsync<User>(request, (response) =>
                {
                    user = Observable.Return<User>(response.Data);
                });

        }

        public void GetUserRepos(ObservableCollection<ItemViewModel> repos)
        {
            var request = new RestRequest("user/repos", Method.GET);

            restClient.ExecuteAsync<List<Repository>>(request, response =>
                {
                    response.Data.ForEach(repo => repos.Add(new ItemViewModel(repo)));
 //                   repos = new ObservableCollection<Repository>(response.Data);
                });
        }
    }
}
