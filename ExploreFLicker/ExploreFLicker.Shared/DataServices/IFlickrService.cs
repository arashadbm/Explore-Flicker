using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExploreFLicker.Models.Request;
using ExploreFLicker.Models.Response;
using FlickrExplorer.DataServices.Requests;

namespace ExploreFLicker.DataServices
{
    public interface IFlickrService
    {
        Task<ResponseWrapper<RecentPhotosResponse>> GetRecentPhotosAsync(GetRecentPhotosParameters parameters, CancellationToken? token = null);
    }
}
