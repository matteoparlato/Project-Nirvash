using Newtonsoft.Json;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// ContentLinkItem class
    /// </summary>
    public class ContentLinkItem
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
