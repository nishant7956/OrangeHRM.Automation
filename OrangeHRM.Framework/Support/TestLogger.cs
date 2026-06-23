namespace OrangeHRM.Framework.Support;

/// <summary>
/// Structured test logger that writes timestamped, levelled messages to stdout.
/// Messages surface in TRX output and Allure step logs during test runs.
/// </summary>
public static class TestLogger
{
    public static void Info(string message) => Write("INFO", message);
    public static void Warn(string message) => Write("WARN", message);
    public static void Error(string message) => Write("ERROR", message);
    public static void Step(string message) => Write("STEP", message);

    private static void Write(string level, string message)
    {
        var timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff");
        Console.WriteLine($"[{timestamp}] [{level,-5}] {message}");
    }
}
