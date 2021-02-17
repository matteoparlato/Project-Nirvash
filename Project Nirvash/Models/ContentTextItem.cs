using Newtonsoft.Json;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// ContentTextItem class
    /// </summary>
    public class ContentTextItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
