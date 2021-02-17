using Newtonsoft.Json;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// UserCredentials class
    /// </summary>
    public class UserCredentials
    {
        [JsonProperty("uid")]
        public string Username { get; set; }

        [JsonProperty("pass")]
        public string Password { get; set; }

        /// <summary>
        /// Constructor which initializes a UserCredentials object with passed information.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        public UserCredentials(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
