namespace fields.@base
{
    public static class EndpointUrls
    {
        public static string BaseUrl => "http://127.0.0.1:8000";
        
        public static string LoginAccessToken => $"{BaseUrl}/api/v1/login/access-token";
    }
}