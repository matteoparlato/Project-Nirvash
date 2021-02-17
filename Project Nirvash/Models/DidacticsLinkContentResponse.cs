using Newtonsoft.Json;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// DidacticsLinkContentResponse class
    /// </summary>
    public class DidacticsLinkContentResponse
    {
        [JsonProperty("item")]
        public ContentLinkItem LinkItem { get; set; }
    }
}
