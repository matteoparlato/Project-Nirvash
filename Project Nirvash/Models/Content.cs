using Newtonsoft.Json;
using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Content class
    /// </summary>
    public class Content
    {
        public const string FILE = "file";
        public const string LINK = "link";
        public const string TEXT = "text";

        [JsonProperty("contentId")]
        public int ID { get; set; }

        [JsonProperty("contentName")]
        public string Name { get; set; }

        [JsonProperty("objectId")]
        public int ObjectID { get; set; }

        [JsonProperty("objectType")]
        public string ObjectType { get; set; }

        [JsonProperty("shareDT")]
        public DateTime LastPublishingDateTime { get; set; }

        public string ObjectTypeIcon
        {
            get => ObjectType.GetLocalized();
        }

        public string Folder { get; set; }

        /// <summary>
        /// Method which searches the passed keyword across the DidacticsItems collection.
        /// </summary>
        /// <param name="keyword">The keyword to search inside the collection</param>
        /// <returns>The list containing Content items which name contains the passed keyword</returns>
        public static List<Content> SearchContentByName(string keyword)
        {
            return Singleton<ClientExtensions>.Instance.AccountDetails.DidacticsItems.SelectMany(didactict => didactict.Contents).Where(content => content.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
