using Bogus;
using fields.request;
using fields.request.login;

namespace fields.data
{
    public static class TestData
    {
        public const string ValidUserEmail = "testuser@example.com";
        public const string ValidUserPassword = "testpassword";
        
        public const string AnotherValidEmail = "testuse2r@example.com";
        public const string AnotherValidPassword = "testuser2";
        
        // Невалидные данные для логина
        public static IEnumerable<TestCaseData> InvalidLoginCases()
        {
            var faker = new Faker();

            // Случайный неправильный email
            yield return new TestCaseData(
                new LoginRequest("password", faker.Internet.Email(), ValidUserPassword)
            ).SetName("RandomEmail_ShouldReturn_BadRequest");

            // Случайный неправильный пароль
            yield return new TestCaseData(
                new LoginRequest("password", ValidUserEmail, faker.Internet.Password())
            ).SetName("RandomPassword_ShouldReturn_BadRequest");

            // Пустой email
            yield return new TestCaseData(
                new LoginRequest("password", "", ValidUserPassword)
            ).SetName("EmptyEmail_ShouldReturn_BadRequest");

            // Пустой пароль
            yield return new TestCaseData(
                new LoginRequest("password", ValidUserEmail, "")
            ).SetName("EmptyPassword_ShouldReturn_BadRequest");

            // Неверный grant_type
            yield return new TestCaseData(
                new LoginRequest("wrongtype", ValidUserEmail, ValidUserPassword)
            ).SetName("WrongGrantType_ShouldReturn_422");
        }
        
        // Невалидные email для тестирования
        public static IEnumerable<string> InvalidEmails => new[]
        {
            "invalid-email",           // без @
            "invalid@",                // без домена
            "@example.com",            // без имени пользователя
            "test@.com",               // без домена
            "test@example.",           // без TLD
            "test@example..com",       // двойная точка
            "test@example@com",        // два @
            "test@example com",        // пробел
            "",                        // пустая строка
            "   ",                     // пробелы
            "test@example.c",          // слишком короткий TLD
            "a@b.c",                   // слишком короткие части
            "test@exa mple.com",       // пробел в домене
            "test@-example.com",       // дефис в начале домена
            "test@example-.com",       // дефис в конце домена
        };
        
        // Email с максимально допустимой длиной
        public static string VeryLongEmail => 
            $"{new string('a', 64)}@{new string('b', 186)}.com";
    }
}