using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using ExploreFlicker.Common;
using ExploreFlicker.Models.Response;

namespace ExploreFlicker.ViewModels
{
    public class MapViewModel : BindableBase
    {
        #region properties
        private Photo _photo;
        public Photo Photo
        {
            get { return _photo; }
            set
            {
                if (SetProperty(ref _photo, value))
                {
                    OnPropertyChanged(@"Geopoint");
                }
            }
        }

        public Geopoint Geopoint
        {
            get
            {
                if (Photo == null) return new Geopoint(new BasicGeoposition());

                var geopoint = new Geopoint(new BasicGeoposition()
                {
                    Longitude = Photo.Longitude,
                    Latitude = Photo.Latitude
                });
                return geopoint;
            }
        }

        public Point Anchor
        {
            get { return new Point(0.5, 1); }
        }
        #endregion
    }
}
