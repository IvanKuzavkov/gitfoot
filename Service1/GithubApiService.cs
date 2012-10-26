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

        static protected RestClient restClient;

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

                    GithubApiService.GetOrgs(response.Data);
                });

        }

        public void GetUserRepos(ObservableCollection<ItemViewModel> repos)
        {
            var request = new RestRequest("user/repos", Method.GET);

            restClient.ExecuteAsync<List<Repository>>(request, response =>
                {
                    response.Data.ForEach(repo => repos.Add(new ItemViewModel(repo)));
                });
        }

        public void GetIssues(ObservableCollection<ItemViewModel> issues)
        {
            var request = new RestRequest("issues", Method.GET);

            restClient.ExecuteAsync<List<Issue>>(request, response =>
                {
//                    string s = response.Content;
                    response.Data.ForEach(issue => issues.Add(new ItemViewModel(issue)));
                });
        }

        static public void GetOrgs(User user)
        {
            var request = new RestRequest("user/orgs", Method.GET);

            restClient.ExecuteAsync<List<Organization>>(request, response =>
                {
                    user.Organizations = response.Data;
                });
        }

        public void GetReposForOrganization(Organization org, ObservableCollection<ItemViewModel> repos)
        {
            var request = new RestRequest(string.Format("orgs/{0}/repos", org.login), Method.GET);

            restClient.ExecuteAsync<List<Repository>>(request, response =>
                {
                    response.Data.ForEach(repo => repos.Add(new ItemViewModel(repo)));
                });
        }

        public void GetIssuesForRepos(IEnumerable<Repository> repos)
        {

        }
    }
}
