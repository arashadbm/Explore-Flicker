using System;
using System.Collections.Generic;
using System.Text;
using ExploreFlicker.DataServices.QueryParameters;

namespace ExploreFlicker.Models.Request
{
    public class GetRecentPhotosParameters : FlickerParameters
    {
        public override sealed string Method
        {
            get { return "flickr.photos.getRecent"; }
            set { throw new NotSupportedException(); }
        }

        [QueryArray("extras", ArrayFormat = ArrayFormat.SingleKey)]
        public List<string> Extras { get; set; }

        [QueryParameter("per_page")]
        public int PerPage { get; set; }

        /// <summary>
        /// Minimum value is 1
        /// </summary>
        [QueryParameter("page")]
        public int Page { get; set; }


    }

    public class RecentPhotosExtras
    {
        public const string Geo = "geo";
        public const string Description = "description";
    }
}
