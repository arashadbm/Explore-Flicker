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

        #region  OnNavigate & Reset Cache
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ResetPageCache();
        }

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

        private async void PhotosCollections_OnLoadMoreRequested(object sender, VerticalGridView.LoadMoreEventArgs e)
        {
            await _mainViewModel.LoadMorePhotosAsync();
            e.IsLoadingMore = false;
        }

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

        private void WrapGrid_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (!(e.NewSize.Width > 0) ) return;
            VariableSizedWrapGrid itemsWrapGrid = sender as VariableSizedWrapGrid;
            if (itemsWrapGrid == null) return;
            var width = (e.NewSize.Width);
            itemsWrapGrid.ItemWidth = width / 2;
            itemsWrapGrid.ItemHeight = itemsWrapGrid.ItemWidth;
        }

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

        private void GoToVisualState()
        {
            VisualStateManager.GoToState(this, VisibleStateName, true);
            SearchTextBox.Focus(FocusState.Programmatic);
        }

        private void GoToHiddenState()
        {
            VisualStateManager.GoToState(this, HiddenStateName, false);
            _searchViewModel.IsBusy = false;
            _searchViewModel.ReAssignCancellationToken();
            SearchTextBox.Text = "";
            _searchViewModel.SearchCollection.Clear();
            DummyButton.Focus(FocusState.Programmatic);
        }
    }
}
