using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Folder class
    /// </summary>
    public class Folder
    {
        [JsonProperty("folderId")]
        public int ID { get; set; }

        [JsonProperty("folderName")]
        public string Name { get; set; }

        [JsonProperty("lastShareDT")]
        public DateTime LastPublishingDateTime { get; set; }

        [JsonProperty("contents")]
        public List<Content> Contents { get; set; }
    }
}