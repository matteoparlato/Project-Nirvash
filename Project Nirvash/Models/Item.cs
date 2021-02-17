using Newtonsoft.Json;
using Project_Nirvash.Helpers;
using System;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Item class
    /// </summary>
    public class Item : Observable
    {
        [JsonProperty("pubId")]
        public int ID { get; set; }

        [JsonProperty("pubDT")]
        public DateTime PublishDateTime { get; set; }

        private bool _readStatus;
        [JsonProperty("readStatus")]
        public bool ReadStatus
        {
            get => _readStatus;
            set
            {
                Set(ref _readStatus, value);
                ReadStatusIcon = _readStatus.ToString();
            }
        }

        [JsonProperty("evtCode")]
        public string EventCode { get; set; }

        [JsonProperty("cntId")]
        public int ContentID { get; set; }

        [JsonProperty("cntValidFrom")]
        public DateTime ContentValidFrom { get; set; }

        [JsonProperty("cntValidTo")]
        public DateTime ContentValidTo { get; set; }

        [JsonProperty("cntValidInRange")]
        public bool ContentValidInRange { get; set; }

        [JsonProperty("cntStatus")]
        public string ContentStatus { get; set; }

        [JsonProperty("cntTitle")]
        public string ContentTitle { get; set; }

        [JsonProperty("cntCategory")]
        public string ContentCategory { get; set; }

        [JsonProperty("cntHasChanged")]
        public bool ContentHasChanged { get; set; }

        [JsonProperty("cntHasAttach")]
        public bool ContentHasAttachments { get; set; }

        [JsonProperty("needJoin")]
        public bool NeedJoin { get; set; }

        [JsonProperty("needReply")]
        public bool NeedReply { get; set; }

        [JsonProperty("needFile")]
        public bool NeedFile { get; set; }

        [JsonProperty("evento_id")]
        public string EventID { get; set; }

        [JsonProperty("dinsert_allegato")]
        public DateTime LastPublishingDateTime { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        private string _readStatusIcon;
        public string ReadStatusIcon
        {
            get => _readStatusIcon;
            set => Set(ref _readStatusIcon, value.GetLocalized());
        }
    }
}
