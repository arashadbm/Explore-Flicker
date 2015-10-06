using Newtonsoft.Json;

namespace ExploreFlicker.Models.Response
{
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
}