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
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace BitTorrent.WP7.Extensions
{
    public static class Helper
    {
        #region Images

        public static class Images
        {
            const string ImageServerUrl = "http://howler.bittorrent.com.s3.amazonaws.com";

            const string ImageSize48x48 = "_48x48.png";
            const string ImageSize128x128 = "_128x128.png";
            const string ImageSize256x256 = "_256x256.png";
            public enum ImageSize
            {
                png_48x48,
                png_128x128,
                png_256x256
            }
            public static string GetImagePath(string iconId, ImageSize size)
            {
                if (string.IsNullOrEmpty(iconId))
                    return null;

                switch (size)
                {
                    case ImageSize.png_48x48:
                        return string.Format(@"{0}/{1}{2}", ImageServerUrl, iconId, ImageSize48x48);
                    case ImageSize.png_128x128:
                        return string.Format(@"{0}/{1}{2}", ImageServerUrl, iconId, ImageSize128x128);
                    case ImageSize.png_256x256:
                        return string.Format(@"{0}/{1}{2}", ImageServerUrl, iconId, ImageSize256x256);
                    default:
                        return null;
                }

                return null;
            }
        }
        #endregion
    }
}
