using fields.@base;
using NUnit.Framework;

namespace fields.tests.passwordrecovery
{
    [TestFixture]
    [Category("Security")]
    public class PasswordRecoverySecurityTests : BaseApiTest
    {
        [Test]
        [Description("Should handle SQL injection attempts appropriately")]
        public async Task PasswordRecovery_SqlInjectionAttempt_ReturnsAppropriateResponse()
        {
            var injectionAttempts = new[]
            {
                "test' OR '1'='1'--",
                "test@example.com'; DROP TABLE users;--",
                "test@example.com' UNION SELECT * FROM users--"
            };

            foreach (var attempt in injectionAttempts)
            {
                var (response, _) = await PasswordRecoveryService.SendRecoveryRequestAsync(attempt);
                
                Assert.That((int)response.StatusCode, Is.LessThan(500),
                    $"SQL injection attempt should not cause server error: {attempt}");
                
                SimpleLogger.Log.Info($"SQL injection attempt '{attempt}' returned {(int)response.StatusCode}");
            }
        }

        [Test]
        [Description("Should handle path traversal attempts")]
        public async Task PasswordRecovery_PathTraversal_ReturnsAppropriateResponse()
        {
            var traversalAttempts = new[]
            {
                "../../../etc/passwd",
                "..\\..\\windows\\system32",
                "%2e%2e%2f%2e%2e%2fetc%2fpasswd"
            };

            foreach (var attempt in traversalAttempts)
            {
                var (response, _) = await PasswordRecoveryService.SendRecoveryRequestAsync(attempt);
                
                Assert.That((int)response.StatusCode, Is.LessThan(500),
                    $"Path traversal attempt should not cause server error: {attempt}");
                
                SimpleLogger.Log.Info($"Path traversal attempt '{attempt}' returned {(int)response.StatusCode}");
            }
        }

        [Test]
        [Description("Should handle XSS attempts")]
        public async Task PasswordRecovery_XssAttempt_ReturnsAppropriateResponse()
        {
            var xssAttempts = new[]
            {
                "<script>alert('xss')</script>@example.com",
                "\"><script>alert(1)</script>@test.com",
                "test@example.com<img src=x onerror=alert(1)>"
            };

            foreach (var attempt in xssAttempts)
            {
                var (response, _) = await PasswordRecoveryService.SendRecoveryRequestAsync(attempt);
                
                Assert.That((int)response.StatusCode, Is.LessThan(500),
                    $"XSS attempt should not cause server error: {attempt}");
                
                SimpleLogger.Log.Info($"XSS attempt '{attempt}' returned {(int)response.StatusCode}");
            }
        }

        [Test]
        [Description("Response times should be reasonable")]
        [Timeout(3000)]
        public async Task PasswordRecovery_ResponseTime_IsReasonable()
        {
            var testEmail = Faker.Internet.Email(provider: $"test-{Guid.NewGuid():N}.com");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var (response, _) = await PasswordRecoveryService.SendRecoveryRequestAsync(testEmail);
            stopwatch.Stop();
            
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(2000),
                $"Response time should be under 2 seconds, was {stopwatch.ElapsedMilliseconds}ms");
            
            SimpleLogger.Log.Info($"Response time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}