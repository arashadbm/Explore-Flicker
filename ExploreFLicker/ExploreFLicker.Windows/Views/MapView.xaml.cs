
using Bing.Maps;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFlicker.Models.Response;
using ExploreFlicker.Viewmodels;
using ExploreFlicker.ViewModels;


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
            var photo = e.NavigationParameter as Photo;
            if (photo == null) return;
            _mapViewModel.Photo = photo;

            //add pushpin
            Pushpin bin = new Pushpin()
            {
                Text = string.IsNullOrWhiteSpace(photo.Title) ? ViewModelLocator.Resources.Photo : photo.Title
            };
            MapLayer.SetPosition(bin, _mapViewModel.Location);
            Map.Children.Add(bin);
            //Zoom the map to the specified location  and zoom level 1.
            Map.SetView(_mapViewModel.Location, 16);
        }
    }
}
