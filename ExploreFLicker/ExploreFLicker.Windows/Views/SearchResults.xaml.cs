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

    public sealed partial class SearchResults : ExtendedPage
    {
        #region Fields
        private readonly SearchViewModel _searchViewModel;
        #endregion

        #region initialization
        public SearchResults()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            //Set view models fields
            _searchViewModel = (SearchViewModel)DataContext;
        }
        #endregion

        #region  Methods


        #region Load data events
        protected override void LoadState(object sender, LoadStateEventArgs e)
        {
            base.LoadState(sender, e);
            if (NeedsRefresh)
            {
                string searchTerm = e.NavigationParameter as string;
                if (String.IsNullOrWhiteSpace(searchTerm))
                {
                    SearchTextBox.Text = "";
                    return;
                }
                //Set Search TextBox
                SearchTextBox.Text = searchTerm;
                //load
                _searchViewModel.SearchPhotosCommand.Execute(searchTerm);
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
            await _searchViewModel.LoadMoreSearchResultsAsync(SearchTextBox.Text);
            e.IsLoadingMore = false;
        }



        #endregion

        #endregion
    }
}
