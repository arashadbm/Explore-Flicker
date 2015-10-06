using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExploreFlicker.Common;
using ExploreFlicker.DataServices;
using ExploreFlicker.Helpers;
using ExploreFlicker.Models;
using ExploreFlicker.Models.Request;
using ExploreFlicker.Models.Response;
using ExploreFlicker.Views;
using ExploreFlickr.Strings;
using FlickrExplorer.DataServices.Interfaces;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFLicker.ViewModels
{
    public class SearchViewModel : BindableBase
    {
        #region Fields
        private const int PerPage = 40;
        private readonly IFlickrService _flickrService;
        private readonly INavigationService _navigationService;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly Resources _resources;
        private CancellationTokenSource _cts;
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
        /// This represensts the current page number in search results photos.
        /// </summary>
        private int CurrentPageOfSearch { get; set; }


        private readonly ObservableCollection<Photo> _searchCollection = new ObservableCollection<Photo>();
        public ObservableCollection<Photo> SearchCollection
        {
            get { return _searchCollection; }
        }

        #endregion

        #region Initialization

        public SearchViewModel(IFlickrService flickrService, Resources resources, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _flickrService = flickrService;
            _resources = resources;
            _navigationService = navigationService;
            _messageResolver = messageResolver;
            //Initialize Commands
            SearchPhotosCommand = new AsyncExtendedCommand<string>(SearchInitialPhotosAsync);
            PhotoClickedCommand = new ExtendedCommand<Photo>(PhotoClicked);
        }

        #endregion

        #region Commands
        /// <summary>
        /// Command for loading first page of search results.
        /// Search term is passed as an argument.
        /// </summary>
        public AsyncExtendedCommand<string> SearchPhotosCommand { get; set; }

        /// <summary>
        /// Command to be executed when user clicks on any photo.
        /// It will pass as an argument the clicked item.
        /// </summary>
        public ExtendedCommand<Photo> PhotoClickedCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method will load first page of search results.
        /// </summary>
        /// <returns></returns>
        private async Task SearchInitialPhotosAsync(string searchTerm)
        {
            //Cancel previous call if any
            ReAssignCancellationToken();
            var token = _cts.Token;

            //Clear previous results
            SearchCollection.Clear();

            //Clear previous error
            ErrorMessage = null;

            //Check if the search term is empty before making request
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                IsBusy = false;
                return;
            }
            try
            {
                //Show progress to user
                IsBusy = true;
                BusyMessage = _resources.Loading;

                var response = await SearchPhotosAsync(1, PerPage, searchTerm, token);
                if (token.IsCancellationRequested) return;
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result.Photos != null)
                {
                    UpdateSearchCollection(response);
                }
                else
                {
                    ErrorMessage = _messageResolver.ResultToMessage(response);
                }
            }
            catch (Exception)
            {
                if (!token.IsCancellationRequested)
                {
                    //Show Error
                    ErrorMessage = RequestMessage.GetClientErrorMessage();
                }
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    //Don't set is busy when token is canceled, as there is other ongoing operation
                    IsBusy = false;
                }
            }

        }

        /// <summary>
        /// Method for cancelling previous Api call if any and instantiating new CancellationTokenSource
        /// </summary>
        public void ReAssignCancellationToken()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
            //Create new cts for the new task
            _cts = new CancellationTokenSource();
        }

        /// <summary>
        /// This methold will be triggered when user scrolls near the end of list.
        /// It will add new search results to the end of list.
        /// </summary>
        /// <returns></returns>
        public async Task LoadMoreSearchResultsAsync(string searchTerm)
        {
            //Cancel previous call if any
            ReAssignCancellationToken();
            var token = _cts.Token;
            try
            {

                var response = await SearchPhotosAsync(CurrentPageOfSearch + 1, PerPage, searchTerm, token);
                if (token.IsCancellationRequested) return;
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    UpdateSearchCollection(response);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// This method is used for both loading initial photos and for loading more photos.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        private async Task<ResponseWrapper<SearchPhotosResponse>> SearchPhotosAsync(int page, int perPage, string searchTerm, CancellationToken token)
        {
            var extras = new List<string> { RecentPhotosExtras.Geo, RecentPhotosExtras.Description };
            var parameters = new SearchPhotoParameters()
            {
                Extras = extras,
                Page = page,
                PerPage = perPage,
                Text = searchTerm
            };
            var data = await _flickrService.SearchPhotosAsync(parameters);
            return data;
        }

        /// <summary>
        /// This method is responsible for adding new items to the collection
        /// and updating current page number.
        /// </summary>
        /// <param name="response"></param>
        private void UpdateSearchCollection(ResponseWrapper<SearchPhotosResponse> response)
        {
            CurrentPageOfSearch = response.Result.Photos.Page;
            var list = response.Result.Photos.List;
            foreach (var photo in list)
            {
                SearchCollection.Add(photo);
            }
        }

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
                Photos = SearchCollection.ToList()
            };
            _navigationService.NavigateByPage<GalleryView>(galleryParameters);
        }
        #endregion
    }
}
