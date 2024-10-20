using System.Dynamic;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services;

public class ReleaseService : AbstractService<Release>
{
    public ReleaseService(Config config) : base(config)
    {
    }

    protected override string BasePath => "releases";

    public async Task<Release?> CreateAsync(Release release)
        => await PostAsync(GetApiUri(), release);

    public async Task<string> UploadAsync(Release release, string name, string fileName)
    {
        dynamic query = new ExpandoObject();
        query.name = name;
        query.label = name;
        return await UploadAsync<string>(GetUploadUri($"{release.Id}/assets", query), fileName);
    }
}