using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;

using AppStudio.Services;

namespace AppStudio
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            MainViewModels = new MainViewModels();
        }

        public MainViewModels MainViewModels { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Main page must be always the top entry
            NavigationService.RemoveBackEntry();

            MainViewModels.SetViewType(ViewTypes.List);

            DataContext = MainViewModels;
            MainViewModels.UpdateAppBar();
            await MainViewModels.LoadData(NetworkInterface.GetIsNetworkAvailable());

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SpeechServices.Stop();
            base.OnNavigatedFrom(e);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var panorama = sender as Panorama;
            if (panorama != null)
            {
                var item = panorama.SelectedItem as PanoramaItem;
                if (item != null)
                {
                    MainViewModels.SelectedItem = item.Content as ViewModelBase;
                }
            }
            SpeechServices.Stop();
        }
    }
}
