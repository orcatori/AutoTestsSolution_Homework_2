using System.Net;
using fields.@base;
using fields.request.passwordrecovery;
using Newtonsoft.Json.Linq;

namespace fields.helpers
{
    public static class PasswordRecoveryHelper
    {
        public static async Task<HttpResponseMessage> SendRecoveryRequest(
            HttpClient client, 
            string baseUrl,
            string email)
        {
            var endpoint = $"{baseUrl}/api/v1/password-recovery/{Uri.EscapeDataString(email)}";
            SimpleLogger.Log.Info($"POST {endpoint}");
            
            return await client.PostAsync(endpoint, null);
        }

        public static void ValidateSuccessResponse(JObject json)
        {
            Assert.IsTrue(
                json.ContainsKey("message"),
                "Response must contain 'message' field"
            );
            
            Assert.That(
                json["message"]?.ToString(),
                Is.Not.Null.And.Not.Empty,
                "Message field should not be empty"
            );
        }

        public static void ValidateValidationErrorResponse(JObject json)
        {
            Assert.IsTrue(
                json.ContainsKey("detail"),
                "Validation error response must contain 'detail' field"
            );
            
            var detail = json["detail"];
            Assert.IsTrue(
                detail?.Type == JTokenType.Array && detail.HasValues,
                "'detail' field should be a non-empty array"
            );
        }

        public static void LogValidationErrors(JObject json)
        {
            if (json.ContainsKey("detail") && json["detail"] is JArray detailArray)
            {
                foreach (var error in detailArray)
                {
                    if (error["msg"] != null)
                    {
                        SimpleLogger.Log.Info($"[VALIDATION ERROR] {error["msg"]}");
                    }
                }
            }
        }
    }
}