using Newtonsoft.Json;
using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Helpers;
using System;
using System.Globalization;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Agendum class
    /// </summary>
    public class Agendum
    {
        [JsonProperty("evtId")]
        public int ID { get; set; }

        [JsonProperty("evtCode")]
        public string Code { get; set; }

        [JsonProperty("evtDatetimeBegin")]
        public DateTime BeginDateTime { get; set; }

        [JsonProperty("evtDatetimeEnd")]
        public DateTime EndDateTime { get; set; }

        [JsonProperty("isFullDay")]
        public bool FullDay { get; set; }

        [JsonProperty("notes")]
        public string Description { get; set; }

        private string _authorName;
        [JsonProperty("authorName")]
        public string AuthorName
        {
            get => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_authorName.ToLower());
            set => _authorName = value;
        }

        [JsonProperty("classDesc")]
        public string ClassDescription { get; set; }

        [JsonProperty("subjectId")]
        public int? SubjectID { get; set; }

        private string subjectDescription;
        [JsonProperty("subjectDesc")]
        public string SubjectDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(subjectDescription))
                {
                    Subject subject = Subject.GetSubjectFromTeacher(Singleton<ClientExtensions>.Instance.AccountDetails.Subjects, new Teacher("", AuthorName));
                    return subject == null ? "EmptySubject".GetLocalized() : subject.Description;
                }
                return subjectDescription;
            }
            set => subjectDescription = value;
        }
    }
}
