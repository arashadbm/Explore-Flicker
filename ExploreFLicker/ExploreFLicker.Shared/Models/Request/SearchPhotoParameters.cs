using System;
using System.Collections.Generic;
using System.Text;
using ExploreFlicker.DataServices.QueryParameters;
using ExploreFlicker.Models.Request;

namespace ExploreFlicker.Models.Request
{
   public class SearchPhotoParameters:FlickerParameters
    {
        public override sealed string Method
        {
            get { return "flickr.photos.search"; }
            set { throw new NotSupportedException(); }
        }

        [QueryArray("extras", ArrayFormat = ArrayFormat.SingleKey)]
        public List<string> Extras { get; set; }

        [QueryParameter("per_page")]
        public int PerPage { get; set; }

        [QueryParameter("text")]
        public string Text { get; set; }

        /// <summary>
        /// Minimum value is 1
        /// </summary>
        [QueryParameter("page")]
        public int Page { get; set; }
    }
}
