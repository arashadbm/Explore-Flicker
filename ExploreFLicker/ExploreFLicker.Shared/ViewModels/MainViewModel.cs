﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ExploreFlicker.Common;
using ExploreFlicker.Helpers;
using ExploreFlickr.Strings;
using ExploreFLicker.DataServices;
using ExploreFLicker.Models.Request;
using ExploreFLicker.Models.Response;
using FlickrExplorer.DataServices.Interfaces;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFLicker.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Fields
        private const int PerPage = 20;
        private readonly IFlickrService _flickrService;
        private readonly INavigationService _navigationService;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly Resources _resources;
       
        #endregion

        #region Properties

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        private RequestMessage _errorMessage;
        public RequestMessage ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        /// <summary>
        /// This represensts the current page number in Flicker Explore.
        /// </summary>
        private int CurrentPage { get; set; }

        /// <summary>
        /// This value represent MaxPage which can be retrieved using @PerPage amount.
        /// </summary>
        private int MaxPage { get; set; }

        private int _totalImages;
        /// <summary>
        /// This value represents total images available to be retrieved from Flickr Explore.
        /// </summary>
        public int TotalImages
        {
            get { return _totalImages; }
            set { SetProperty(ref _totalImages, value); }
        }


        private readonly ObservableCollection<Photo> _photosCollection = new ObservableCollection<Photo>();
        public ObservableCollection<Photo> PhotosCollection
        {
            get { return _photosCollection; }
        }

        #endregion

        #region MainViewModel

        public MainViewModel(IFlickrService flickrService, Resources resources, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _flickrService = flickrService;
            _resources = resources;
            _navigationService = navigationService;
            _messageResolver = messageResolver;
            //Initialize Commands
            LoadInitialPhotosCommand = new AsyncExtendedCommand(LoadInitialPhotosAsync);
            PhotoClickedCommand = new ExtendedCommand<Photo>(PhotoClicked);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand LoadInitialPhotosCommand { get; set; }
        public ExtendedCommand<Photo> PhotoClickedCommand { get; set; }
        #endregion

        #region Methods

        private async Task LoadInitialPhotosAsync()
        {
            LoadInitialPhotosCommand.CanExecute = false;
            IsBusy = true;
            BusyMessage = _resources.Loading;
            try
            {
                var data = await LoadPhotosAsync(1, PerPage);
                if (data.ResponseStatus == ResponseStatus.SuccessWithResult && data.Result.Photos != null)
                {
                    var list = data.Result.Photos.List;
                    PhotosCollection.Clear();
                    foreach (var photo in list)
                    {
                        PhotosCollection.Add(photo);
                    }
                }
                else
                {
                    ErrorMessage = _messageResolver.ResultToMessage(data);
                }
            }
            catch (Exception)
            {
                //Show Error
                ErrorMessage = RequestMessage.GetClientErrorMessage();
            }
            finally
            {
                LoadInitialPhotosCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        private async Task LoadMorePhotosAsync()
        {
            if (IsBusy) return;
            try
            {
                LoadInitialPhotosCommand.CanExecute = false;
                var response = await LoadPhotosAsync(1, PerPage);
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    var list = response.Result.Photos.List;
                    PhotosCollection.Clear();
                    foreach (var photo in list)
                    {
                        PhotosCollection.Add(photo);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                LoadInitialPhotosCommand.CanExecute = true;
            }
        }

        private async Task<ResponseWrapper<RecentPhotosResponse>> LoadPhotosAsync(int page, int perPage)
        {
            var extras = new List<string> { RecentPhotosExtras.Geo, RecentPhotosExtras.Description };
            var parameters = new GetRecentPhotosParameters()
            {
                Extras = extras,
                Page = page,
                PerPage = perPage
            };
            var data = await _flickrService.GetRecentPhotosAsync(parameters);
            return data;
        }

        private void PhotoClicked(Photo photo)
        {

        }

        #endregion
    }
}
