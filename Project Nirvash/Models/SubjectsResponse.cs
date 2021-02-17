using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// SubjectsResponse class
    /// </summary>
    public class SubjectsResponse
    {
        [JsonProperty("subjects")]
        public List<Subject> Subjects { get; set; }
    }
}
