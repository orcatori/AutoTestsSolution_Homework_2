using fields.@base;
using fields.request.login;
using Newtonsoft.Json.Linq;

namespace fields.helpers
{
    public static class LoginApiHelper
    {
        public static FormUrlEncodedContent CreateLoginFormData(LoginRequest payload)
        {
            return ApiHelper.CreateFormData(
                new KeyValuePair<string, string>("grant_type", payload.GrantType),
                new KeyValuePair<string, string>("username", payload.Username),
                new KeyValuePair<string, string>("password", payload.Password)
            );
        }

        public static void ValidateTokenResponse(JObject json)
        {
            Assert.IsTrue(
                json.ContainsKey("access_token"),
                "Response must contain access_token"
            );
            
            Assert.That(
                json["token_type"]?.ToString().ToLower(),
                Is.EqualTo("bearer"),
                "token_type must be 'bearer'"
            );
        }
    }
}