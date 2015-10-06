using Newtonsoft.Json;

namespace ExploreFlicker.Models.Response
{
    public class Description
    {
        [JsonProperty("_content")]
        public string Content { get; set; }
    }
}