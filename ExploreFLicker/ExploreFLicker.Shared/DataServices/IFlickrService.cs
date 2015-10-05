using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExploreFlicker.Models.Request;
using ExploreFlicker.Models.Response;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFlicker.DataServices
{
    public interface IFlickrService
    {
        Task<ResponseWrapper<RecentPhotosResponse>> GetRecentPhotosAsync(GetRecentPhotosParameters parameters, CancellationToken? token = null);
    }
}
