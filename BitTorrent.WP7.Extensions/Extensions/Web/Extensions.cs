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
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading;
using System.Linq;

namespace BitTorrent.WP7.Extensions
{
    public static class Extensions
    {
        #region HttpWebRequest
        public static IObservable<T> GetJson<T>(this HttpWebRequest request)
        {
            request.Method = "GET";
            request.Accept = "application/json";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                        {
                            using (var responseStream = response.GetResponseStream())
                            {
                                
                                return JsonConvert.DeserializeObject<T>(new StreamReader(responseStream).ReadToEnd());
                            }
                        });
        }

        public static IObservable<WebResponse> GetJsonResponse(this HttpWebRequest request)
        {
            request.Method = "GET";
           request.Accept = "application/json";

            return Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)();
        }

        public static IObservable<Unit> PostJson<T>(this HttpWebRequest request, T obj)
        {
            request.Method = "POST";
            request.ContentType = "application/json";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                        {
                            using (requestStream)
                            {
                                var serializer = new DataContractJsonSerializer(typeof(T));
                                serializer.WriteObject(requestStream, obj);
                                requestStream.Close();
                            }

                            return
                                Observable.FromAsyncPattern<WebResponse>(
                                    request.BeginGetResponse,
                                    request.EndGetResponse)();
                        },
                        (requestStream, webResponse) => new Unit());
        }

        public static IObservable<V> PostJson<T, V>(this HttpWebRequest request, T obj)
        {
            request.Method = "POST";
            request.ContentType = "application/json";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                        {
                            using (requestStream)
                            {
                                var serializer =  new DataContractJsonSerializer(typeof(T));
                                serializer.WriteObject(requestStream, obj);
                                requestStream.Close();
                            }

                            return
                                Observable.FromAsyncPattern<WebResponse>(
                                    request.BeginGetResponse,
                                    request.EndGetResponse)()
                                    .Select(
                                        response =>
                                        {
                                            using (var responseStream = response.GetResponseStream())
                                            {
                                                return JsonConvert.DeserializeObject<V>(new StreamReader(responseStream).ReadToEnd());
                                            }
                                        });
                        });
        }

        public static IObservable<V> PutJson<T, V>(this HttpWebRequest request, T obj)
        {
            request.Method = "PUT";
            request.ContentType = "application/json";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                        {
                            using (requestStream)
                            {
                                var serializer = new DataContractJsonSerializer(typeof(T));
                                serializer.WriteObject(requestStream, obj);
                                requestStream.Close();
                            }

                            return
                                Observable.FromAsyncPattern<WebResponse>(
                                    request.BeginGetResponse,
                                    request.EndGetResponse)()
                                    .Select(
                                        response =>
                                        {
                                            using (var responseStream = response.GetResponseStream())
                                            {
                                                return JsonConvert.DeserializeObject<V>(new StreamReader(responseStream).ReadToEnd());
                                            }
                                        });
                        });
        }

        static string boundary = "----uTorrentRemote";

        //public static IObservable<V> PostImage<V>(this HttpWebRequest request, BitmapImage obj)
        //{
        //    request.ContentType = "multipart/form-data; boundary=" + boundary;
        //    request.Method = "POST";

        //    return
        //        Observable
        //            .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
        //            .SelectMany(
        //                requestStream =>
        //                {
        //                    using (requestStream)
        //                    {

        //                        StringBuilder postData = new StringBuilder();
        //                        postData.Append("--" + boundary + "\r\n");
        //                        postData.Append("Content-Disposition: form-data; name=\"Image\"; filename=\"UserAvatar.jpeg\"\r\n");
        //                        postData.Append("Content-Type: image/jpeg\r\n\r\n");

        //                        ManualResetEvent resetEvent = new ManualResetEvent(false);
        //                        byte[] byteArrayData = new byte[0];
        //                        obj.Dispatcher.BeginInvoke(() =>
        //                        {
        //                            byteArrayData = obj.ConvertToBytes();
        //                            resetEvent.Set();
        //                        }
        //                        );

        //                        resetEvent.WaitOne(10000);

        //                        StringBuilder postData2 = new StringBuilder();
        //                        postData2.Append("\r\n--" + boundary + "\r\n");
        //                        postData2.Append("Content-Disposition: form-data; name=\"sizes\"\r\n\r\n16,24,32,48,128,256,512");
        //                        postData2.Append("\r\n--" + boundary + "\r\n");

        //                        var combineArray = 
        //                            Encoding.UTF8.GetBytes(postData.ToString())
        //                                .Concat(byteArrayData)
        //                                .Concat(Encoding.UTF8.GetBytes(postData2.ToString()))
        //                            .ToArray();

        //                        requestStream.Write(combineArray, 0, combineArray.Length);


        //                        requestStream.Close();
        //                    }

        //                    return Observable.FromAsyncPattern<WebResponse>(
        //                        request.BeginGetResponse,
        //                        request.EndGetResponse)()
        //                        .Select(
        //                            response =>
        //                            {
        //                                using (var responseStream = response.GetResponseStream())
        //                                {
        //                                    return JsonConvert.DeserializeObject<V>(new StreamReader(responseStream).ReadToEnd());
        //                                }
        //                            });
        //                });

        //}
        public static IObservable<Unit> DeleteJson(this HttpWebRequest request)
        {
            request.Method = "DELETE";

            return
                  Observable.FromAsyncPattern<WebResponse>(
                                    request.BeginGetResponse,
                                    request.EndGetResponse)()
                                    .Select(r =>
                                        {
                                            if (r is HttpWebResponse)
                                                if ((r as HttpWebResponse).StatusCode == HttpStatusCode.OK)
                                                    return new Unit();
                                            throw new WebException("Status code is " + (r as HttpWebResponse).StatusCode.ToString());
                                        });
        }


        //public static byte[] ConvertToBytes(this BitmapImage bitmapImage)
        //{
        //    byte[] data = null;

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        WriteableBitmap wBitmap = new WriteableBitmap(bitmapImage);
        //        wBitmap.SaveJpeg(stream, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, 100);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        data = stream.GetBuffer();
        //    }

        //    return data;
        //}

        #endregion
    }
}
