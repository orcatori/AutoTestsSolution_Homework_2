using System.Text;
using Bogus;
using fields.request;
using fields.request.user_name;
using Newtonsoft.Json;

namespace fields.@base
{
    public static class TestUserManager
    {
        private static HttpClient Client => ApiClientSingleton.Instance;

        // Генерация случайного пользователя
        public static UserCreateRequest GenerateRandomUser()
        {
            var faker = new Faker();
            var email = faker.Internet.Email();
            var fullName = faker.Name.FullName();
            var password = faker.Internet.Password(10, true, null, "@1A"); // сложный пароль
            return new UserCreateRequest(email, password, fullName);
        }

        // Создать пользователя на сервере
        public static async Task<UserCreateRequest> CreateUserAsync(UserCreateRequest user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(AppConfig.UsersEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                SimpleLogger.Log.Error($"Failed to create user {user.Email}. StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}");
                return null;
            }

            SimpleLogger.Log.Info($"User {user.Email} created successfully.");
            return user;
        }
    }
}