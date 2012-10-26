using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using gitfoot.ViewModels;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace gitfoot
{
    public partial class MainPage : PhoneApplicationPage
    {
        MainViewModel _viewModel;
        private MainViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = (MainViewModel)this.DataContext); }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Set the data context of the listbox control to the sample data
            PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(Current_Activated);

            (pamora.Background as ImageBrush).ImageSource = (DataContext as MainViewModel).PanoramaImage;
            (pamora.Background as ImageBrush).Opacity = 0.4;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            (pamora.Background as ImageBrush).ImageSource = (DataContext as MainViewModel).PanoramaImage;
            (pamora.Background as ImageBrush).Opacity = 0.4;
        }
        // Load data for the ViewModel Items

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (!viewModel.IsDataLoaded)
            {
                viewModel.LoadData();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.NotificationController.SplashScreen.IsVisible = false;
            if (e.NavigationMode == NavigationMode.New && e.IsNavigationInitiator)
            {
                while (NavigationService.RemoveBackEntry() != null)
                { }
            }

            ViewModel.Init();
        }
    }
}