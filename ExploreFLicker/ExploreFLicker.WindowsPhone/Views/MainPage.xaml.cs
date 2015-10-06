using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFlicker.ViewModels;

namespace ExploreFlicker.Views
{

    public sealed partial class MainPage : ExtendedPage
    {
        private readonly MainViewModel _mainViewModel;
        public MainPage()
        {
            this.InitializeComponent();
            _mainViewModel = (MainViewModel)DataContext;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ResetPageCache();
        }

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

        private void WrapGrid_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (!(e.NewSize.Width > 0) || !(e.NewSize.Width > e.PreviousSize.Width)) return;
            VariableSizedWrapGrid itemsWrapGrid = sender as VariableSizedWrapGrid;
            if (itemsWrapGrid == null) return;
            var width = (e.NewSize.Width);
            itemsWrapGrid.ItemWidth = width / 2;
            itemsWrapGrid.ItemHeight = itemsWrapGrid.ItemWidth;
        }

        private void ResetPageCache()
        {
            var cacheSize = Frame.CacheSize;
            Frame.CacheSize = 0;
            Frame.CacheSize = cacheSize;
        }
    }
}
