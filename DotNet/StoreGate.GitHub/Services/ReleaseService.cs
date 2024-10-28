using System.Dynamic;
using Microsoft.Extensions.Logging;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services;

public class ReleaseService : AbstractService<Release>
{
    public ReleaseService(GitHubEnvironment environment, ILogger<ReleaseService> logger) : base(environment, logger)
    {
    }

    protected override string BasePath => "releases";

    public async Task<Release?> CreateAsync(Release release)
        => await PostAsync(GetApiUri(), release);

    public async Task UploadAsync(Release release, string name, string fileName)
    {
        dynamic query = new ExpandoObject();
        query.name = name;
        query.label = name;
        await UploadAsync(GetUploadUri($"{release.Id}/assets", query), fileName);
    }
}