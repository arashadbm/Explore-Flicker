using System;
using System.Collections.Generic;
using System.Text;
using ExploreFlicker.Models.Response;
using Newtonsoft.Json;

namespace ExploreFlicker.Models.Response
{
    public class SearchPhotosResponse
    {
        [JsonProperty("photos")]
        public Photos Photos { get; set; }
        [JsonProperty("stat")]
        public string Stat { get; set; }
    }
}
