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

namespace gitfoot.ViewModels
{
    public class BaseViewModel
    {
        public BaseViewModel()
        { }
        public BaseViewModel(string id)
        {
            Id = id;
        }

        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
