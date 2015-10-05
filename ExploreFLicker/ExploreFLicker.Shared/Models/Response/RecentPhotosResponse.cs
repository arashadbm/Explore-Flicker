using System;
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

    public class Photos
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pages")]
        public int Pages { get; set; }
        [JsonProperty("perpage")]
        public int Perpage { get; set; }
        [JsonProperty("total")]
        public string Total { get; set; }
        [JsonProperty("photo")]
        public Photo[] List { get; set; }
    }

    public class Photo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("farm")]
        public int Farm { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ispublic")]
        public int Ispublic { get; set; }

        [JsonProperty("isfriend")]
        public int Isfriend { get; set; }

        [JsonProperty("isfamily")]
        public int Isfamily { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public object Longitude { get; set; }

        [JsonProperty("accuracy")]
        public int Accuracy { get; set; }

        [JsonProperty("context")]
        public int Context { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("woeid")]
        public string Woeid { get; set; }

        [JsonProperty("geo_is_family")]
        public int GeoIsFamily { get; set; }

        [JsonProperty("geo_is_friend")]
        public int GeoIsFriend { get; set; }

        [JsonProperty("geo_is_contact")]
        public int GeoIsContact { get; set; }

        [JsonProperty("geo_is_public")]
        public int GeoIsPublic { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        //Constructed Properties which isn't returned by backend

        public string ThumbnailPath
        {
            get { return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_q.jpg", Farm, Server, Id, Secret); }
        }

        public string MediumPath
        {
            get { return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_z.jpg", Farm, Server, Id, Secret); }
        }
    }

    public class Description
    {
        [JsonProperty("_content")]
        public string Content { get; set; }
    }

}
