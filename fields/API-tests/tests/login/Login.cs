using System.Net;
using fields.@base;
using fields.data;
using fields.helpers;
using fields.request.login;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace fields.tests.login
{
    [TestFixture]
    public class LoginApiTests : BaseTest
    {
        private string LoginEndpoint => AppConfig.LoginEndpoint;

        [Test]
        [Description("Valid user returns access token and token_type bearer")]
        public async Task Login_ValidUser_ReturnsToken()
        {
            var payload = new LoginRequest(
                grantType: "password",
                username: TestData.ValidUserEmail,
                password: TestData.ValidUserPassword
            );

            var formData = LoginApiHelper.CreateLoginFormData(payload);
            
            SimpleLogger.Log.Info($"POST {LoginEndpoint} with username={payload.Username}");
            
            var response = await Client.PostAsync(LoginEndpoint, formData);
            
            ApiHelper.AssertStatusCode(response, HttpStatusCode.OK);
            
            var json = await ApiHelper.ParseJsonResponse(response);
            LoginApiHelper.ValidateTokenResponse(json);
        }

        [Test]
        [TestCaseSource(typeof(TestData), nameof(TestData.InvalidLoginCases))]
        public async Task Login_InvalidUser_ReturnsError(LoginRequest payload)
        {
            var formData = LoginApiHelper.CreateLoginFormData(payload);
            
            SimpleLogger.Log.Info($"POST {LoginEndpoint} with username={payload.Username}");
            
            var response = await Client.PostAsync(LoginEndpoint, formData);
            
            if (payload.GrantType != "password")
            {
                // 422 
                ApiHelper.AssertStatusCode(response, HttpStatusCode.UnprocessableEntity);
                SimpleLogger.Log.Info($"[WARNING] Grant type '{payload.GrantType}' is not supported");
            }
            else
            {
                // 400/401 
                Assert.That(
                    response.StatusCode, 
                    Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.Unauthorized),
                    $"Expected 400 BadRequest or 401 Unauthorized for invalid credentials: username='{payload.Username}'"
                );
                
                var json = await ApiHelper.ParseJsonResponse(response);
                Assert.IsTrue(
                    json.ContainsKey("detail"),
                    "Error response should contain detail message"
                );
                
                SimpleLogger.Log.Info($"[WARNING] Login failed for user '{payload.Username}': {json["detail"]}");
            }
        }
    }
}