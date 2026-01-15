using System.Net;
using Newtonsoft.Json.Linq;

namespace fields.@base
{
    public static class ApiHelper
    {
        public static void AssertStatusCode(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
        {
            Assert.That(
                response.StatusCode,
                Is.EqualTo(expectedStatusCode),
                $"Expected {(int)expectedStatusCode} {expectedStatusCode}, " +
                $"got {(int)response.StatusCode} {response.ReasonPhrase}"
            );
        }

        public static async Task<JObject> ParseJsonResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        public static FormUrlEncodedContent CreateFormData(params KeyValuePair<string, string>[] data)
        {
            return new FormUrlEncodedContent(data);
        }
    }
}