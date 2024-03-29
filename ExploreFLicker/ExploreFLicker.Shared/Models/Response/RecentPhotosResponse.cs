﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExploreFlicker.Models.Response
{

    public class RecentPhotosResponse
    {
        [JsonProperty("photos")]
        public Photos Photos { get; set; }
        [JsonProperty("stat")]
        public string Stat { get; set; }
    }
}
