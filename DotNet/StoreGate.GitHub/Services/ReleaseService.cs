using System.Dynamic;
using Microsoft.Extensions.Logging;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services;

public class ReleaseService : AbstractService<Release>
{
    public ReleaseService(Config config, ILogger<ReleaseService> logger) : base(config, logger)
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