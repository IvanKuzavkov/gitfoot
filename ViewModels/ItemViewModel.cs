using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using gitfoot.ViewModels;
using gitfoot.Models;

namespace gitfoot.ViewModels
{
    public class ItemViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ItemViewModel()
        {
        }

        public ItemViewModel(BaseModel model)
            : base(model.id.ToString())
        {
            Name = model.name;
        }

        public ItemViewModel(Repository repo)
            : base(repo.full_name)
        {
            Name = repo.name;
        }

        public ItemViewModel(Issue issue)
            : base(issue.body)
        {
            Name = issue.title;
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _image;
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    NotifyPropertyChanged("Image");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}