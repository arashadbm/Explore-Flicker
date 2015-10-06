using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExploreFlicker.Models.Request;
using ExploreFlicker.Models.Response;
using ExploreFlicker.DataServices.QueryParameters;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFlicker.DataServices
{
    public class FlickrService : IFlickrService
    {
        #region Fields
        private const string BaseUrl = "https://api.flickr.com/services/rest/";
        private const string APiKey = "8cacc0510b4acf1e0434c70da9ec04a9";
        private const string Format = "json";
        private const int NoJsonCallBack = 1;
        private readonly Func<BaseRequest> _requestFactory;
        #endregion

        #region Constructor
        public FlickrService(Func<BaseRequest> requestFactory)
        {
            _requestFactory = requestFactory;
        }
        #endregion

        public async Task<ResponseWrapper<RecentPhotosResponse>> GetRecentPhotosAsync(GetRecentPhotosParameters parameters, CancellationToken? token = default(CancellationToken?))
        {
            var request = _requestFactory.Invoke();
            parameters.ApiKey = APiKey;
            parameters.Format = Format;
            parameters.Nojsoncallback = NoJsonCallBack;
            request.RequestUrl = (BaseUrl).AppendQueryString(parameters);
            return await request.GetAsync<RecentPhotosResponse>(token);
        }

        public async Task<ResponseWrapper<SearchPhotosResponse>> SearchPhotosAsync(SearchPhotoParameters parameters, CancellationToken? token = null)
        {
            var request = _requestFactory.Invoke();
            parameters.ApiKey = APiKey;
            parameters.Format = Format;
            parameters.Nojsoncallback = NoJsonCallBack;
            request.RequestUrl = (BaseUrl).AppendQueryString(parameters);
            return await request.GetAsync<SearchPhotosResponse>(token);
        }
    }
}
