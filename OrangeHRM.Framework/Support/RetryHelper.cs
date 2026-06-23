namespace OrangeHRM.Framework.Support;

/// <summary>
/// Provides retry logic for operations that may transiently fail,
/// such as interacting with elements behind loading spinners or animations.
/// </summary>
public static class RetryHelper
{
    /// <summary>
    /// Executes <paramref name="action"/> up to <paramref name="maxAttempts"/> times,
    /// sleeping <paramref name="delay"/> between failures. Rethrows on final failure.
    /// </summary>
    public static void Execute(
        Action action,
        int maxAttempts = 3,
        TimeSpan? delay = null,
        string? description = null)
    {
        Execute<bool>(() => { action(); return true; }, maxAttempts, delay, description);
    }

    /// <summary>
    /// Executes <paramref name="func"/> up to <paramref name="maxAttempts"/> times,
    /// sleeping <paramref name="delay"/> between failures. Rethrows on final failure.
    /// </summary>
    public static T Execute<T>(
        Func<T> func,
        int maxAttempts = 3,
        TimeSpan? delay = null,
        string? description = null)
    {
        if (maxAttempts < 1) throw new ArgumentOutOfRangeException(nameof(maxAttempts), "Must be at least 1.");

        var retryDelay = delay ?? TimeSpan.FromMilliseconds(500);
        Exception? lastException = null;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return func();
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                lastException = ex;
                var label = description is not null ? $" [{description}]" : string.Empty;
                TestLogger.Warn($"Attempt {attempt}/{maxAttempts}{label} failed: {ex.GetType().Name} — retrying in {retryDelay.TotalMilliseconds}ms");
                Thread.Sleep(retryDelay);
            }
        }

        // Final attempt — let exception propagate naturally.
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            var label = description is not null ? $" [{description}]" : string.Empty;
            TestLogger.Error($"All {maxAttempts} attempts{label} failed. Last error: {ex.Message}");
            throw;
        }
    }
}
