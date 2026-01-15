namespace fields.@base
{
    public sealed class SimpleLogger
    {
        private static readonly Lazy<SimpleLogger> lazy = new Lazy<SimpleLogger>(() => new SimpleLogger());
        private SimpleLogger() { }

        public static SimpleLogger Log => lazy.Value;

        public void Info(string message) => Console.WriteLine($"[INFO] {DateTime.Now:O} - {message}");
        public void Error(string message) => Console.WriteLine($"[ERROR] {DateTime.Now:O} - {message}");
    }
}