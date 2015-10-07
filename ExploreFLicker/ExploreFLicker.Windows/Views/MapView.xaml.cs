
using Bing.Maps;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFlicker.Models.Response;
using ExploreFlicker.UserControls;
using ExploreFlicker.Viewmodels;
using ExploreFlicker.ViewModels;
using Newtonsoft.Json;


namespace ExploreFlicker.Views
{
    public sealed partial class MapView : ExtendedPage
    {
        private readonly MapViewModel _mapViewModel;

        public MapView()
        {
            this.InitializeComponent();
            _mapViewModel = (MapViewModel)DataContext;
        }

        protected override void LoadState(object sender, LoadStateEventArgs e)
        {
            base.LoadState(sender, e);
            string json = e.NavigationParameter as string;
            if (string.IsNullOrWhiteSpace(json)) return;
            var photo = JsonConvert.DeserializeObject<Photo>(json);

            _mapViewModel.Photo = photo;

            //add pushpin
            string title = string.IsNullOrWhiteSpace(photo.Title) ? ViewModelLocator.Resources.Photo : photo.Title;
            SimplePushbin bin=new SimplePushbin(title);
            MapLayer.SetPosition(bin, _mapViewModel.Location);

            Map.Children.Add(bin);
            
            //Zoom the map to the specified location  and zoom level 16.
            Map.SetView(_mapViewModel.Location, 16);
        }
    }
}
