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
            (pamora.Background as ImageBrush).Opacity = 0.2;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            ViewModel.onImageChanged += new EventHandler(ViewModel_onImageChanged);
        }

        void ViewModel_onImageChanged(object sender, EventArgs e)
        {
            (pamora.Background as ImageBrush).ImageSource = (DataContext as MainViewModel).PanoramaImage;
            (pamora.Background as ImageBrush).Opacity = 0.2;
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
            string strItemIndex;
            if (NavigationContext.QueryString.TryGetValue("goto", out strItemIndex))
            {
                pamora.DefaultItem = pamora.Items[Convert.ToInt32(strItemIndex)];
                if (e.NavigationMode == NavigationMode.New && e.IsNavigationInitiator)
                {
                    while (NavigationService.RemoveBackEntry() != null)
                    { }
                }
            }
            else
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

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex != -1)
            {
                string s = String.Format("/MainPage.xaml?goto={0}", (sender as ListBox).SelectedIndex + 1);
                NavigationService.Navigate(new Uri(s, UriKind.Relative));
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        {
            ViewModel.Logout();
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }
    }
}