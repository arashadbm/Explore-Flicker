using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ExploreFlicker.Common;
using ExploreFlicker.DataServices;
using ExploreFlicker.Helpers;
using ExploreFlicker.Models.Request;
using ExploreFlicker.Models.Response;
using ExploreFlickr.Strings;
using FlickrExplorer.DataServices.Interfaces;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFLicker.ViewModels
{
    /// <summary>
    /// This viewmodel will be used to show galley of photos despite their original source, i.e Recent Photos, Search, etc...
    /// </summary>
    public class GalleryViewModel : BindableBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        #endregion

        #region Properties
        private List<Photo> _photos;
        public List<Photo> Photos
        {
            get { return _photos; }
            set { SetProperty(ref _photos, value); }
        }

        private Photo _selectedPhoto;
        public Photo SelectedPhoto
        {
            get { return _selectedPhoto; }
            set
            {
                if (SetProperty(ref _selectedPhoto, value))
                {
                    if (_selectedPhoto == null)
                    {
                        IsMapAvailable = false;
                        return;
                    }
                    IsMapAvailable = _selectedPhoto.Latitude != 0 && _selectedPhoto.Longitude != 0;
                }

            }
        }

        private bool _isMapAvailable;
        public bool IsMapAvailable
        {
            get { return _isMapAvailable; }
            set { SetProperty(ref _isMapAvailable, value); }
        }


        #endregion

        #region MainViewModel

        public GalleryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            //Initialize Commands
            ShowMapCommand = new ExtendedCommand<Photo>(ShowMap);
        }

        #endregion

        #region Commands
        public ExtendedCommand<Photo> ShowMapCommand { get; set; }
        #endregion

        #region Methods
        private void ShowMap(Photo photo)
        {
            //Navigation to Map
        }

        #endregion
    }
}
