using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// LessonsResponse class
    /// </summary>
    public class LessonsResponse
    {
        [JsonProperty("lessons")]
        public List<Lesson> Lessons { get; set; }
    }
}
