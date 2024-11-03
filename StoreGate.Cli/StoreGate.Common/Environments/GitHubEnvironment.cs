namespace StoreGate.Common.Environments;

public static class GitHubEnvironment
{
    public static string Repo { get; } = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
    public static string Token { get; } = Environment.GetEnvironmentVariable("ACCESS_TOKEN");
}