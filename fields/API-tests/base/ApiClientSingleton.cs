namespace fields.@base
{
    public sealed class ApiClientSingleton
    {
        private static readonly Lazy<HttpClient> lazy = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            return client;
        });

        private ApiClientSingleton() { }

        public static HttpClient Instance => lazy.Value;
    }
}