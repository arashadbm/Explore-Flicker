using System;
using System.Collections.Generic;
using System.Text;
using ExploreFlicker.DataServices.QueryParameters;

namespace ExploreFlicker.Models.Request
{
    public class FlickerParameters
    {
        /// <summary>
        /// API Method
        /// </summary>
        [QueryParameter("method")]
        public virtual string Method { get; set; }

        /// <summary>
        /// Your application ApiKey
        /// </summary>
        [QueryParameter("api_key")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Format of response, ex: json
        /// </summary>
        [QueryParameter("format")]
        public string Format { get; set; }

        /// <summary>
        /// Default value is 1 for Json, as per flickr api sample
        /// </summary>
        [QueryParameter("nojsoncallback")]
        public int Nojsoncallback { get; set; }

    }
}
