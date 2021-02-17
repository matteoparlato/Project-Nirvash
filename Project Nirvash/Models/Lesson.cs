using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Lesson class
    /// </summary>
    public class Lesson
    {
        [JsonProperty("evtId")]
        public int ID { get; set; }

        [JsonProperty("evtDate")]
        public DateTime LessonDate { get; set; }

        [JsonProperty("evtCode")]
        public string Code { get; set; }

        [JsonProperty("evtHPos")]
        public int Position { get; set; }

        [JsonProperty("evtDuration")]
        public int Duration { get; set; }

        [JsonProperty("classDesc")]
        public string ClassDescription { get; set; }

        private string _authorName;
        [JsonProperty("authorName")]
        public string AuthorName
        {
            get => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_authorName.ToLower());
            set => _authorName = value;
        }

        [JsonProperty("subjectId")]
        public int SubjectID { get; set; }

        [JsonProperty("subjectCode")]
        public string SubjectCode { get; set; }

        [JsonProperty("subjectDesc")]
        public string SubjectDescription { get; set; }

        [JsonProperty("lessonType")]
        public string LessonType { get; set; }

        [JsonProperty("lessonArg")]
        public string Description { get; set; }
    }
}
