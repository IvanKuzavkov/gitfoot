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

namespace BitTorrent.WP7.Extensions
{
    public static class Common
    {
        public static string TupleToArg(this Tuple<string, string> tuple)
        {
            if (tuple == null)
                return null;
            return string.Format("{0}={1}", tuple.Item1, tuple.Item2);
        }
    }
}
