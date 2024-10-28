using Environment = StoreGate.Common.Models.Environment;

namespace StoreGate.GitHub;

public class GitHubEnvironment : Environment
{
    public string Repo { get; } = GetEnvironmentVariable("GITHUB_REPOSITORY");
    public string Token { get; } = GetEnvironmentVariable("ACCESS_TOKEN");
}