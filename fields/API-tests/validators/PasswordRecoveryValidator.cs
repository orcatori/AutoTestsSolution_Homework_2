using System.Net;
using Newtonsoft.Json.Linq;

namespace fields.validators
{
    public static class PasswordRecoveryValidator
    {
        public static void ValidateExistingUserResponse(HttpResponseMessage response, JObject json)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new AssertionException(
                    $"Expected 200 OK for existing user, got {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            var message = json["message"]?.ToString();
            if (message != "Password recovery email sent")
            {
                throw new AssertionException(
                    $"Expected message 'Password recovery email sent', got '{message}'");
            }
        }

        public static void ValidateNonExistingUserResponse(HttpResponseMessage response, JObject json)
        {
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new AssertionException(
                    $"Expected 404 Not Found for non-existing user, got {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            
            if (response.Content.Headers.ContentLength > 0)
            {
                var message = json["message"]?.ToString();
                var expectedMessage = "The user with this email does not exist in the system.";
                
                if (!string.IsNullOrEmpty(message) && message != expectedMessage)
                {
                    throw new AssertionException(
                        $"Expected message '{expectedMessage}', got '{message}'");
                }
            }
        }

        public static void ValidateSecurityResponse(HttpResponseMessage response, string attemptType)
        {
            if ((int)response.StatusCode >= 500)
            {
                throw new AssertionException(
                    $"{attemptType} should not cause server error (500), got {(int)response.StatusCode}");
            }
        }
    }
}