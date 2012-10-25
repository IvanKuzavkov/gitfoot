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
using System.Reflection;

namespace BitTorrent.WP7.Services
{
    public static class AttributeFacade
    {
        public static string AppPagePath<T>(T enumObj)
        {
            FieldInfo fieldInfo = typeof(T).GetField(enumObj.ToString());
            FilePathAttribute[] attributes = (FilePathAttribute[])fieldInfo.GetCustomAttributes(typeof(FilePathAttribute), false);

            if (attributes.Length == 0 )
                throw new ArgumentException("Field should be mark with FilePathAttribute");

            return attributes[0].Path;
        }

        public static string GetFilePath<T>(T enumObj)
        {
            FieldInfo fieldInfo = typeof(T).GetField(enumObj.ToString());
            FilePathAttribute[] attributes = (FilePathAttribute[])fieldInfo.GetCustomAttributes(typeof(FilePathAttribute), false);

            if (attributes.Length == 0)
                return null;

            return attributes[0].Path;
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class FilePathAttribute : Attribute
    {
        readonly string positionalString;

        // This is a positional argument
        public FilePathAttribute(string path)
        {
            this.positionalString = path;
        }

        public string Path
        {
            get { return positionalString; }
        }
    }
}
