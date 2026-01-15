using Newtonsoft.Json;

namespace fields.request.user_name
{
    public class UserCreateRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonProperty("is_superuser")]
        public bool IsSuperUser { get; set; } = false;

        public UserCreateRequest(string email, string password, string fullName, bool isSuperUser = false)
        {
            Email = email;
            Password = password;
            FullName = fullName;
            IsSuperUser = isSuperUser;
        }
    }
}