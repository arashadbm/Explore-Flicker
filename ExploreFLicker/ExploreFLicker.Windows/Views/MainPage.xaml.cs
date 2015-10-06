using ExploreFlicker.Common;
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
using ExploreFlicker.Controls;
using ExploreFlicker.ViewModels;
using ExploreFLicker.ViewModels;


namespace ExploreFlicker.Views
{

    public sealed partial class MainPage : ExtendedPage
    {
        #region Fields
        private readonly MainViewModel _mainViewModel;
        #endregion

        #region initialization
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            //Set view models fields
            _mainViewModel = (MainViewModel)DataContext;
        }
        #endregion

        #region  Methods

        #region  OnNavigate & Reset Cache
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Clear cache, if page is coming back from Gallery View.
            if (e.NavigationMode == NavigationMode.Back)
                ResetPageCache();
        }

        /// <summary>
        /// This resets the frame cache, it will clear all pages cache except those with Required cache mode.
        /// </summary>
        private void ResetPageCache()
        {
            var cacheSize = Frame.CacheSize;
            Frame.CacheSize = 0;
            Frame.CacheSize = cacheSize;
        }
        #endregion

        #region Load data events
        protected override void LoadState(object sender, LoadStateEventArgs e)
        {
            base.LoadState(sender, e);
            if (NeedsRefresh)
            {
                _mainViewModel.LoadInitialPhotosCommand.Execute(null);
            }
        }

        /// <summary>
        /// Triggered by reaching the end of list (or near the end based on configuration)
        /// Indicates the need for loading more Recent photos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PhotosCollections_OnLoadMoreRequested(object sender, HorizontalGridView.LoadMoreEventArgs e)
        {
            await _mainViewModel.LoadMorePhotosAsync();
            e.IsLoadingMore = false;
        }



        #endregion

        #endregion
    }
}
