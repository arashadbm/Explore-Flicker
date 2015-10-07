using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ExploreFlicker.Common;
using ExploreFlicker.Controls;
using ExploreFlicker.Models.Response;
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

        protected override async void LoadState(object sender, LoadStateEventArgs e)
        {
            base.LoadState(sender, e);
            string json= e.NavigationParameter as string;
            if (string.IsNullOrWhiteSpace(json)) return;
            var photo = JsonConvert.DeserializeObject<Photo>(json);
            if (photo == null) return;
            _mapViewModel.Photo = photo;

            //Zoom the map to the specified location with Bow animation and zoom level 14.
            await Map.TrySetViewAsync(_mapViewModel.Geopoint, 14, null, null, MapAnimationKind.Bow);
        }

        #region Clean Up Logic
        /// <summary>
        /// There is memory leak inside Winrt version of Map Control, It won't be garabage collected.
        /// Few Visits to the map page will crash the app. 
        /// Setting Navigation cache to required will waste memory for non core function page like this.
        /// As a workaround, we will remove manually children of map and then remove map from its parent.
        /// If you make memory analysis , you will find memory drops.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (Map != null)
            {
                try
                {
                    Map.Children.Clear();

                    //Remove map from parent control.
                    Grid parentGrid = VisualTreeHelper.GetParent(Map) as Grid;

                    if (parentGrid != null)
                        parentGrid.Children.Remove(Map);

                    //Force Garbage Collection
                    GC.Collect(2);
                    GC.WaitForPendingFinalizers();
                    GC.Collect(2);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion

    }
}
