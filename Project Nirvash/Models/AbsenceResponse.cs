using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// AbsenceResponse class
    /// </summary>
    public class AbsenceResponse
    {
        [JsonProperty("events")]
        public List<Event> Events { get; set; }
    }
}
