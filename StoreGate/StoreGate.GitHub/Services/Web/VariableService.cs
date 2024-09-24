using System.Net;
using StoreGate.Common;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services.Web;

public class VariableService : AbstractGitHubService<GitHutVariable>
{
    public VariableService(GitHubData data) : base(data)
    {
    }

    protected override string BasePath => "actions/variables";

    public async Task<string?> GetAsync(string name)
    {
        return (await base.GetAsync(GetUri(name)))?.Value;
    }

    public async Task CreateOrUpdateAsync(string name, string value)
    {
        GitHutVariable variable = new()
        {
            Name = name,
            Value = value
        };
        try
        {
            await GetAsync(name);
            await PatchAsync(GetUri(name), variable);
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == HttpStatusCode.NotFound)
        {
            await PostAsync(GetUri(), variable);
        }
    }
}