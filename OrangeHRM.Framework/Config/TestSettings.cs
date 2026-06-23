using OrangeHRM.Framework.Driver;

namespace OrangeHRM.Framework.Config;

public sealed class TestSettings
{
    public string OrangeHrmBaseUrl { get; init; } = "https://opensource-demo.orangehrmlive.com/web/index.php";

    public string OrangeHrmUsername { get; init; } = "Admin";

    public string OrangeHrmPassword { get; init; } = "admin123";

    public string RestfulBookerBaseUrl { get; init; } = "https://restful-booker.herokuapp.com";

    public BrowserType Browser { get; init; } = BrowserType.Chrome;

    public bool Headless { get; init; }

    public int TimeoutSeconds { get; init; } = 20;

    public string ArtifactsDirectory { get; init; } = "TestResults";

    public static TestSettings FromEnvironment()
    {
        return new TestSettings
        {
            OrangeHrmBaseUrl = Read("ORANGEHRM_BASE_URL", "https://opensource-demo.orangehrmlive.com/web/index.php").TrimEnd('/'),
            OrangeHrmUsername = Read("ORANGEHRM_USERNAME", "Admin"),
            OrangeHrmPassword = Read("ORANGEHRM_PASSWORD", "admin123"),
            RestfulBookerBaseUrl = Read("BOOKER_BASE_URL", "https://restful-booker.herokuapp.com").TrimEnd('/'),
            Browser = ParseBrowser(Read("BROWSER", "Chrome")),
            Headless = ParseBool(Read("HEADLESS", Read("CI", "false"))),
            TimeoutSeconds = ParseInt(Read("TIMEOUT_SECONDS", "20"), 20),
            ArtifactsDirectory = Read("TEST_ARTIFACTS_DIR", "TestResults")
        };
    }

    private static string Read(string name, string fallback)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    private static BrowserType ParseBrowser(string value)
    {
        return Enum.TryParse<BrowserType>(value, ignoreCase: true, out var browser)
            ? browser
            : BrowserType.Chrome;
    }

    private static bool ParseBool(string value)
    {
        return bool.TryParse(value, out var result) && result;
    }

    private static int ParseInt(string value, int fallback)
    {
        return int.TryParse(value, out var result) && result > 0 ? result : fallback;
    }
}
