using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// NoticeboardResponse class
    /// </summary>
    public class NoticeboardResponse
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}
