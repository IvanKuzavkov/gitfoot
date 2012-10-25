using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace gitfoot
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _panoramaImage;

        public string PanoramaImage
        {
            get 
            { 
                return _panoramaImage; 
            }
            set 
            {
                if (_panoramaImage != value)
                {
                    _panoramaImage = value;
                    NotifyPropertyChanged("PanoramaImage");
                }
            }
        }
        

        public MainViewModel()
        {
            this.NewsItems = new ObservableCollection<ItemViewModel>();
            PanoramaImage = "http://octodex.github.com/images/stormtroopocat.jpg";
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> NewsItems { get; private set; }
        public ObservableCollection<ItemViewModel> RepositItems { get; private set; }
        public ObservableCollection<ItemViewModel> IssuesItems { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";

        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            this.IsDataLoaded = true;
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