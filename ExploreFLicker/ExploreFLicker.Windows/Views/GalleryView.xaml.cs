using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFlicker.Models;
using ExploreFlicker.ViewModels;
using Newtonsoft.Json;

namespace ExploreFlicker.Views
{

    public sealed partial class GalleryView : ExtendedPage
    {
        private readonly GalleryViewModel _galleryViewModel;
        public GalleryView()
        {
            this.InitializeComponent();
            _galleryViewModel = (GalleryViewModel)DataContext;
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void LoadState(object sender, LoadStateEventArgs e)
        {
            base.LoadState(sender, e);

            //NeedsRefresh will return true if page is New/Refresh or has been discarded from cache
            if (NeedsRefresh)
            {
                //Clear previous values if any as this page is cached.
                _galleryViewModel.Photos = null;
                _galleryViewModel.SelectedPhoto = null;

                var json = e.NavigationParameter as string;
                if (String.IsNullOrEmpty(json)) return;
                var parameters = JsonConvert.DeserializeObject<GalleryNavigationParameters>(json);
                //Set photos gallery and selected photo.
                _galleryViewModel.Photos = parameters.Photos;
                _galleryViewModel.SelectedPhoto = parameters.Photos[parameters.Index];

            }
        }
    }
}
