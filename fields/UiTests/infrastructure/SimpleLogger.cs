namespace UiTests.Infrastructure
{
    public static class SimpleLogger
    {
        public static void Info(string msg) => Console.WriteLine($"[INFO] {DateTime.UtcNow:O} - {msg}");
        public static void Error(string msg) => Console.WriteLine($"[ERROR] {DateTime.UtcNow:O} - {msg}");
    }
}