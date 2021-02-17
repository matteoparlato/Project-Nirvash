using Newtonsoft.Json;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Attachment class
    /// </summary>
    public class Attachment
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("attachNum")]
        public int AttachmentNo { get; set; }
    }
}