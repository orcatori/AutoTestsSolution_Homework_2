using Newtonsoft.Json;

namespace fields.request.login
{
    public class LoginRequest
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginRequest(string grantType, string username, string password)
        {
            GrantType = grantType;
            Username = username;
            Password = password;
        }
    }
}