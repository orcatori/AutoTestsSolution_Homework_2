namespace UiTests.Infrastructure
{
    public static class AppConfig
    {
        public static string UiBaseUrl => "http://localhost:5173";
        public static string LoginPath => "/login";
        public static string LoginUrl => UiBaseUrl.TrimEnd('/') + LoginPath;
        public static bool Headless => true;
    }
}