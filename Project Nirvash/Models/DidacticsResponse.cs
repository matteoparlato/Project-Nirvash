using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// DidacticsResponse class
    /// </summary>
    public class DidacticsResponse
    {
        [JsonProperty("didacticts")]
        public List<Didactict> Didactics { get; set; }
    }
}
