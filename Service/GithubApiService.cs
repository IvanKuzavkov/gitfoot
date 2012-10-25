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

namespace gitfoot.Service
{
    public class GithubApiService
    {
        protected string ServiceApiUrl { get; set; }

        protected string User { get; set; }
        protected string Password { get; set; }

        public GithubApiService(string serviceUrl = "https://api.github.com")
        {
            ServiceApiUrl = serviceUrl;
        }


        private HttpWebRequest GetRequest(string endpoint)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", ServiceApiUrl, endpoint));

            if (request == null)
                return null;

            return request;
        }


        public IObservable<AuthorizationResponse> CreateAuthorization()
        {
            string restRequest = string.Format("{0}/{1}", ServiceApiUrl, "authorizations");

            AuthorizationRequest req = new AuthorizationRequest();
            req.scopes = new List<string>();
            req.scopes.Add("repo");

            return this.Post<AuthorizationRequest, AuthorizationResponse>(restRequest, req);
        }

        public void UseCredentials(string user, string pwd)
        {
            User = user;
            Password = pwd;
        }

        public IObservable<T> Get<T>(string endpoint)
        {
            return GetRequest(endpoint).GetJson<T>();
        }

        public IObservable<Unit> Post<T>(string endpoint, T obj)
        {
            var request = GetRequest(endpoint);
            request.Credentials = new NetworkCredential(User, Password);

            return request.PostJson(obj);
        }

        public IObservable<V> Post<T, V>(string endpoint, T obj)
        {

            //if (obj is System.Windows.Media.Imaging.BitmapImage)
            //    return GetRequest(endpoint)
            //            .PostImage<V>(obj as System.Windows.Media.Imaging.BitmapImage);
            //else
            //    return GetRequest(endpoint).PostJson<T, V>(obj);


            return GetRequest(endpoint).PostJson<T, V>(obj);
        }
        public IObservable<Unit> Delete(string endpoint)
        {
            return GetRequest(endpoint).DeleteJson();
        }

        public IObservable<TResult> Put<TSource, TResult>(string endpoint, TSource obj)
        {
            return GetRequest(endpoint).PutJson<TSource, TResult>(obj);
        }

 
    }
}
