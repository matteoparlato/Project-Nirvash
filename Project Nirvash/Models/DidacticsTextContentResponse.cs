using Newtonsoft.Json;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// DidacticsTextContentResponse class
    /// </summary>
    public class DidacticsTextContentResponse
    {
        [JsonProperty("item")]
        public ContentTextItem TextItem { get; set; }
    }
}
