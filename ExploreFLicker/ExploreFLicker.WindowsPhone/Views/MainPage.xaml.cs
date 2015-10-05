using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFLicker.ViewModels;

namespace ExploreFLicker.Views
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

        protected override void LoadState(object sender, LoadStateEventArgs e)
        {
            base.LoadState(sender, e);
            if (NeedsRefresh)
            {
                _mainViewModel.LoadInitialPhotosCommand.Execute(null);
            }
        }
    }
}
