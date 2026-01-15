using fields.@base;
using fields.data;
using fields.validators;
using NUnit.Framework;

namespace fields.tests.passwordrecovery
{
    [TestFixture]
    public class PasswordRecoveryTests : BaseApiTest
    {
        [Test]
        [Description("Existing user should return 200 OK with success message")]
        public async Task PasswordRecovery_ExistingUser_ReturnsSuccess()
        {
            var existingEmail = TestData.ValidUserEmail;
            var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(existingEmail);
            
            PasswordRecoveryValidator.ValidateExistingUserResponse(response, json);
            SimpleLogger.Log.Info($"Password recovery initiated for '{existingEmail}'");
        }

        [Test]
        [Description("Non-existing user should return 404 Not Found")]
        public async Task PasswordRecovery_NonExistingUser_ReturnsNotFound()
        {
            var nonExistingEmail = Faker.Internet.Email(provider: $"example-{Guid.NewGuid():N}.com");
            var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(nonExistingEmail);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound),
                $"Non-existing user should return 404 Not Found, got {(int)response.StatusCode}");
            SimpleLogger.Log.Info($"Non-existing user '{nonExistingEmail}' correctly rejected with 404");
        }

        [Test]
        [Description("Invalid email format should return 404 Not Found")]
        public async Task PasswordRecovery_InvalidEmailFormat_ReturnsNotFound()
        {
            var invalidEmails = new[]
            {
                Faker.Random.AlphaNumeric(10),
                "test@",
                "@domain.com", 
                "test@domain",
                "test@domain.c",
                "   ",
                string.Empty
            };

            foreach (var invalidEmail in invalidEmails)
            {
                var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(invalidEmail);
                
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound),
                    $"Invalid email '{invalidEmail}' should return 404 Not Found");
                
                SimpleLogger.Log.Info($"Invalid email '{invalidEmail}' returned 404 (as expected)");
            }
        }

        [Test]
        [Description("Email with special characters should return appropriate response")]
        public async Task PasswordRecovery_EmailWithSpecialChars_ReturnsAppropriateResponse()
        {
            var specialEmail = Faker.Internet.Email(provider: "sub-domain.co.uk");
            var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(specialEmail);
            
            Assert.That(
                response.StatusCode,
                Is.EqualTo(System.Net.HttpStatusCode.OK).Or.EqualTo(System.Net.HttpStatusCode.NotFound),
                $"Should return either 200 or 404, got {(int)response.StatusCode}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                PasswordRecoveryValidator.ValidateExistingUserResponse(response, json);
            }
            
            SimpleLogger.Log.Info($"Email with special chars returned {(int)response.StatusCode}");
        }

        [Test]
        [Description("Very long email should be handled without server error")]
        public async Task PasswordRecovery_VeryLongEmail_ReturnsWithoutServerError()
        {
            var longLocalPart = Faker.Random.String(64, char.MinValue, char.MaxValue);
            var longDomain = Faker.Random.String(186, char.MinValue, char.MaxValue);
            var longEmail = $"{longLocalPart}@{longDomain}.com";
            
            var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(longEmail);
            
            Assert.That((int)response.StatusCode, Is.LessThan(500),
                $"Should not return 5xx error for long email");
            SimpleLogger.Log.Info($"Very long email returned {(int)response.StatusCode}");
        }

        [Test]
        [Description("Test consistent behavior for non-existing users")]
        public async Task PasswordRecovery_NonExistingUsers_ConsistentBehavior()
        {
            var testCases = new[]
            {
                new { Email = Faker.Internet.Email(provider: $"test-{Guid.NewGuid():N}.com"), Description = "Random valid email" },
                new { Email = "definitely-not-exist@example.com", Description = "Hardcoded non-existent" },
                new { Email = Faker.Random.AlphaNumeric(10), Description = "Random string" },
                new { Email = "   ", Description = "Whitespace" }
            };

            foreach (var testCase in testCases)
            {
                var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(testCase.Email);

                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound),
                    $"{testCase.Description} ('{testCase.Email}') should return 404");
                
                SimpleLogger.Log.Info($"{testCase.Description}: returned 404");
            }
        }

        [Test]
        [Description("Test response structure for existing user")]
        public async Task PasswordRecovery_ExistingUser_ResponseStructure()
        {
            var existingEmail = TestData.ValidUserEmail;
            var (response, json) = await PasswordRecoveryService.SendRecoveryRequestAsync(existingEmail);
            
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            Assert.That(json, Is.Not.Null, "Response should contain JSON");
            Assert.That(json["message"], Is.Not.Null, "Response should contain 'message' field");
            Assert.That(json["message"]?.ToString(), Is.EqualTo("Password recovery email sent"));
            
            SimpleLogger.Log.Info($"Existing user response structure is correct");
        }
    }
}