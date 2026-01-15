namespace fields.@base
{
    public static class AppConfig
    {
        public static string BaseUrl => "http://127.0.0.1:8000";
        public static string LoginEndpoint => $"{BaseUrl}/api/v1/login/access-token";
        public static string UsersEndpoint => $"{BaseUrl}/api/v1/users/";
        
        public static string PasswordRecoveryEndpoint(string email) => $"/api/v1/password-recovery/{email}";
    }
}