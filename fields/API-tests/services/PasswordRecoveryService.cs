using System.Net;
using Newtonsoft.Json.Linq;

namespace fields.services
{
    public class PasswordRecoveryService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PasswordRecoveryService(HttpClient client, string baseUrl)
        {
            _client = client;
            _baseUrl = baseUrl;
        }

        public async Task<(HttpResponseMessage Response, JObject Json)> SendRecoveryRequestAsync(string email)
        {
            var endpoint = $"{_baseUrl}/api/v1/password-recovery/{Uri.EscapeDataString(email)}";
            var response = await _client.PostAsync(endpoint, null);
            
            JObject json;
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                json = string.IsNullOrWhiteSpace(content) 
                    ? new JObject() 
                    : JObject.Parse(content);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse response for email '{email}'", ex);
            }
            
            return (response, json);
        }
    }
}