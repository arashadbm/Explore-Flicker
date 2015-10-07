using Newtonsoft.Json;

namespace ExploreFlicker.Models.Response
{
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
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

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


#if WINDOWS_PHONE_APP
            //q	large square 150x150
              public string ThumbnailPath
        {
            get { return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_q.jpg", Farm, Server, Id, Secret); }
}
            //z	medium 640, 640 on longest side
         public string MediumPath
    {
        get { return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_z.jpg", Farm, Server, Id, Secret); }
    }
#else
        //Larger resolution for windows version
        // n small, 320 on longest side
        public string ThumbnailPath
        {
            get { return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_n.jpg", Farm, Server, Id, Secret); }
        }
        //b	large, 1024 on longest side*
        public string MediumPath
        {
            get { return string.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}_b.jpg", Farm, Server, Id, Secret); }
        }
#endif


    }
}