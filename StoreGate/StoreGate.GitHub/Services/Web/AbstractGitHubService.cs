using System.Net.Http.Headers;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services.Web;

public abstract class AbstractGitHubService<T> : AbstractWebService<T>
{
    protected AbstractGitHubService(GitHubConfig config)
    {
        Config = config;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
    }

    protected GitHubConfig Config { get; }
    protected override string ApiUri => $"https://api.github.com/repos/{Config.Owner}/{Config.Repo}";
}