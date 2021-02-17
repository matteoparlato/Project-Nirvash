using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Didactict class
    /// </summary>
    public class Didactict
    {
        [JsonProperty("teacherId")]
        public string TeacherID { get; set; }

        [JsonProperty("teacherName")]
        public string TeacherName { get; set; }

        [JsonProperty("teacherFirstName")]
        public string TeacherFirstName { get; set; }

        [JsonProperty("teacherLastName")]
        public string TeacherLastName { get; set; }

        [JsonProperty("folders")]
        public List<Folder> Folders { get; set; }

        public List<Content> Contents { get; set; } = new List<Content>();

        public string SubjectName { get; set; }
    }
}
