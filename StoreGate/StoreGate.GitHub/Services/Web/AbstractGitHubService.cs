using System.Net.Http.Headers;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services.Web;

public abstract class AbstractGitHubService<T> : AbstractWebService<T>
{
    protected AbstractGitHubService(GitHubData data)
    {
        Data = data;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.Token);
    }

    protected GitHubData Data { get; }
    protected override string ApiUri => $"https://api.github.com/repos/{Data.Owner}/{Data.Repo}";
}