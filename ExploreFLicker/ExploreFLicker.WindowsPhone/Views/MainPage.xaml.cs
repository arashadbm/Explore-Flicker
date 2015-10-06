using System;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFlicker.ViewModels;
using ExploreFLicker.ViewModels;

namespace ExploreFlicker.Views
{

    public sealed partial class MainPage : ExtendedPage
    {
        #region Fields
        //Search State names
        private const string HiddenStateName = "HiddenState";
        private const string VisibleStateName = "VisibleState";

        //View Models
        private readonly MainViewModel _mainViewModel;
        private readonly SearchViewModel _searchViewModel;
        #endregion

        #region initialization
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            //Set view models fields
            _mainViewModel = (MainViewModel)DataContext;
            _searchViewModel = (SearchViewModel)SearchGrid.DataContext;

            //Hide Search, without animation
            VisualStateManager.GoToState(this, HiddenStateName, false);
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
        private async void PhotosCollections_OnLoadMoreRequested(object sender, VerticalGridView.LoadMoreEventArgs e)
        {
            await _mainViewModel.LoadMorePhotosAsync();
            e.IsLoadingMore = false;
        }

        /// <summary>
        /// Triggered by reaching the end of list (or near the end based on configuration)
        /// Indicates the need for loading more search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchCollections_OnLoadMoreRequested(object sender, VerticalGridView.LoadMoreEventArgs e)
        {
            var term = SearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(term))
            {
                e.IsLoadingMore = false;
                return;
            }
            await _searchViewModel.LoadMoreSearchResultsAsync(term);
            e.IsLoadingMore = false;
        }

        #endregion

        protected override void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //Check if the current state is searching
            var currentState = VisualStateManager.GetVisualStateGroups(LayoutGrid)[0].CurrentState;
            if (currentState != null && currentState.Name == VisibleStateName)
            {
                GoToHiddenState();
                e.Handled = true;
            }
            base.HardwareButtons_BackPressed(sender, e);
        }

        /// <summary>
        /// This event listener will be responsible for updating the GridViewItem width and size.
        /// It will make the gridview adaptive to any size changes wheter orientation or resolution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WrapGrid_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (!(e.NewSize.Width > 0)) return;
            ItemsWrapGrid itemsWrapGrid = sender as ItemsWrapGrid;
            if (itemsWrapGrid == null) return;
            var width = (e.NewSize.Width);
            itemsWrapGrid.ItemWidth = width / 2;
            itemsWrapGrid.ItemHeight = itemsWrapGrid.ItemWidth;
        }

       
        /// <summary>
        /// Search bar button logic for switching between Recent photos and Search view.
        /// Pressing will show Search view, pressing again shows back the Recent photos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchClicked(object sender, RoutedEventArgs e)
        {
            var currentState = VisualStateManager.GetVisualStateGroups(LayoutGrid)[0].CurrentState;
            if (currentState != null && currentState.Name == VisibleStateName)
            {
                GoToHiddenState();
            }
            else
            {
                GoToVisualState();
            }
        }

        /// <summary>
        /// Shows Search view.
        /// </summary>
        private void GoToVisualState()
        {
            VisualStateManager.GoToState(this, VisibleStateName, true);
            //show soft keyBoard, better user experience, Fewer clicks to start search
            SearchTextBox.Focus(FocusState.Programmatic);
        }

        /// <summary>
        /// Responsible for hiding search view and resetting all search data.
        /// </summary>
        private void GoToHiddenState()
        {
            VisualStateManager.GoToState(this, HiddenStateName, false);
            //cancel progress if any
            _searchViewModel.IsBusy = false;
            //cancel request if any
            _searchViewModel.ReAssignCancellationToken();
            //clear search term
            SearchTextBox.Text = "";
            //clear search results
            _searchViewModel.SearchCollection.Clear();
            //Hide keyboard by focusing on the dummy button.
            DummyButton.Focus(FocusState.Programmatic);
        }
        #endregion
    }
}
