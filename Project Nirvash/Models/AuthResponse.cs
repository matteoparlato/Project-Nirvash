using Newtonsoft.Json;
using Project_Nirvash.Helpers;
using System;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// AuthResponse class
    /// </summary>
    public class AuthResponse
    {
        [JsonProperty("ident")]
        public string Identity { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("release")]
        public DateTime Release { get; set; }

        [JsonProperty("expire")]
        public DateTime Expiration { get; set; }

        public string UserID { get => Identity.RemoveAllButNumbers(); }
    }
}
