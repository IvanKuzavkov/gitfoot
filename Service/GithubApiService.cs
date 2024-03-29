﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using gitfoot.Models;
using gitfoot.ViewModels;
using Microsoft.Phone.Reactive;
using RestSharp;
using BitTorrent.WP7.TorrentRemote.App.Services;


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
            var request = new RestRequest("user/issues", Method.GET);

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("filter", "all");

            restClient.ExecuteAsync<List<Issue>>(request, response =>
                {
                    response.Data.ForEach(issue => issues.Add(new ItemViewModel(issue)));
                });
        }

        static public void GetOrgs(User user)
        {
            var request = new RestRequest("user/orgs", Method.GET);

            restClient.ExecuteAsync<List<Organization>>(request, response =>
                {
                    user.Organizations = response.Data;

                    response.Data.ForEach(org =>
                        {
                            GetReposForOrganization(org);
                        });
                });
        }

        public static void GetReposForOrganization(Organization org)
        {
            var request = new RestRequest(string.Format("orgs/{0}/repos", org.login), Method.GET);

            restClient.ExecuteAsync<List<Repository>>(request, response =>
                {
                    var t = App.Current.Resources["ViewModelLocator"];
                    var repos = (t as ViewModelLocator).MainViewModel.RepositItems;

                    response.Data.ForEach(repo =>
                        {
                            repos.Add(new ItemViewModel(repo));
                            GetIssuesForRepo(repo);
                        });

                   
                });
        }

        public static void GetIssuesForRepo(Repository repo)
        {
            var request = new RestRequest(string.Format("repos/{0}/{1}/issues", repo.owner.login, repo.name), Method.GET);

            restClient.ExecuteAsync<List<Issue>>(request, response =>
                {
                    var t = App.Current.Resources["ViewModelLocator"];
                    var issues = (t as ViewModelLocator).MainViewModel.IssuesItems;

                    response.Data.ForEach(issue => issues.Add(new ItemViewModel(issue)));
                });
        }

    }
}
