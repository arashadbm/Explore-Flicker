using System;
using System.Collections.Generic;
using ExploreFlicker.Common;
using ExploreFlicker.Helpers;
using ExploreFlicker.Models.Response;
using ExploreFlicker.Views;

namespace ExploreFlicker.ViewModels
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
        /// <summary>
        /// List of photos from the source of Selected photos, which were passed from previous page.
        /// </summary>
        public List<Photo> Photos
        {
            get { return _photos; }
            set { SetProperty(ref _photos, value); }
        }

        private Photo _selectedPhoto;
        /// <summary>
        /// Used to keep track of selected photo in view.
        /// </summary>
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
        /// <summary>
        /// True value means there is available longtiude and latitude for this photo.
        /// </summary>
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

        /// <summary>
        /// Command for showing map for the selected photo, if possible.
        /// </summary>
        public ExtendedCommand<Photo> ShowMapCommand { get; set; }
        #endregion

        #region Methods
        private void ShowMap(Photo photo)
        {
            if (photo == null) return;
            //Navigation to Map
            _navigationService.NavigateByPage<MapView>(photo);
        }

        #endregion
    }
}
