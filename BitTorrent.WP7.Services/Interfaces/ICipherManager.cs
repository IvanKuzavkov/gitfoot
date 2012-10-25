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

namespace BitTorrent.WP7.Services
{
    public interface ICipherManager
    {
        void Init(params object[] args);
        bool IsInited { get; set; }
        object Get(string key);
        byte[] Encrypt(byte[] input, params object[] args);
        byte[] Decrypt(byte[] input, params object[] args);
    }
}
