using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// AgendaResponse class
    /// </summary>
    public class AgendaResponse
    {
        [JsonProperty("agenda")]
        public List<Agendum> Appointments { get; set; }
    }
}
