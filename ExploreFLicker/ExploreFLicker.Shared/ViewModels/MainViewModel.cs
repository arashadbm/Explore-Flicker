using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExploreFlicker.Common;
using ExploreFlicker.DataServices;
using ExploreFlicker.Helpers;
using ExploreFlicker.Models.Request;
using ExploreFlicker.Models.Response;
using ExploreFlicker.Views;
using ExploreFlickr.Strings;
using ExploreFlicker.Models;
using FlickrExplorer.DataServices.Interfaces;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFlicker.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Fields
        private const int PerPage = 40;
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
        /// This represensts the current page number in Recent photos.
        /// </summary>
        private int CurrentPageOfRecent { get; set; }

        private readonly ObservableCollection<Photo> _photosCollection = new ObservableCollection<Photo>();
        public ObservableCollection<Photo> PhotosCollection
        {
            get { return _photosCollection; }
        }

        #endregion

        #region Initialization

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
        /// <summary>
        /// Command for loading first page of photos.
        /// </summary>
        public AsyncExtendedCommand LoadInitialPhotosCommand { get; set; }

        /// <summary>
        /// Command to be executed when user clicks on any photo.
        /// It will pass as an argument the clicked item.
        /// </summary>
        public ExtendedCommand<Photo> PhotoClickedCommand { get; set; }

        #endregion

        #region Methods

        #region Load Recent photos methods

        /// <summary>
        /// This method will load first page of photos.
        /// </summary>
        /// <returns></returns>
        private async Task LoadInitialPhotosAsync()
        {
            //Disable command until executing is finished
            LoadInitialPhotosCommand.CanExecute = false;

            //Show progress to user
            IsBusy = true;
            BusyMessage = _resources.Loading;

            //Clear previous error
            ErrorMessage = null;
            try
            {
                var response = await LoadPhotosAsync(1, PerPage);
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result.Photos != null)
                {
                    UpdateCollection(response);
                }
                else
                {
                    ErrorMessage = _messageResolver.ResultToMessage(response);
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

        /// <summary>
        /// This methold will be triggered when user scrolls near the end of list.
        /// It will add new results to the end of list.
        /// </summary>
        /// <returns></returns>
        public async Task LoadMorePhotosAsync()
        {
            if (IsBusy) return;
            try
            {
                LoadInitialPhotosCommand.CanExecute = false;
                var response = await LoadPhotosAsync(CurrentPageOfRecent + 1, PerPage);
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    UpdateCollection(response);
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

        /// <summary>
        /// This method is used for both loading initial photos and for loading more photos.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is responsible for adding new items to the collection
        /// and updating current page number.
        /// </summary>
        /// <param name="response"></param>
        private void UpdateCollection(ResponseWrapper<RecentPhotosResponse> response)
        {
            CurrentPageOfRecent = response.Result.Photos.Page;
            var list = response.Result.Photos.List;
            foreach (var photo in list)
            {
                PhotosCollection.Add(photo);
            }
        }

        #endregion

        /// <summary>
        /// Method to be executed when user clicks on any photo.
        /// Argument is the clicked photo.
        /// </summary>
        private void PhotoClicked(Photo photo)
        {
            if (photo == null) return;
            var galleryParameters = new GalleryNavigationParameters()
            {
                SelectedPhoto = photo,
                Photos = PhotosCollection.ToList()
            };
            _navigationService.NavigateByPage<GalleryView>(galleryParameters);
        }

        #endregion
    }
}
