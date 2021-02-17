using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Note class
    /// </summary>
    public class Note
    {
        [JsonProperty("evtId")]
        public int ID { get; set; }

        [JsonProperty("evtText")]
        public string Description { get; set; }

        [JsonProperty("evtDate")]
        public DateTime PublishDateTime { get; set; }

        private string _authorName;
        [JsonProperty("authorName")]
        public string AuthorName
        {
            get => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_authorName.ToLower());
            set => _authorName = value;
        }

        [JsonProperty("readStatus")]
        public bool ReadStatus { get; set; }
    }
}
