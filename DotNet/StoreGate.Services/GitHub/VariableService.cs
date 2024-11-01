using System.Net;
using Microsoft.Extensions.Logging;
using StoreGate.Models.Common;
using StoreGate.Models.GitHub;

namespace StoreGate.Services.GitHub;

public class VariableService : AbstractService<Variable>
{
    public VariableService(ILogger<VariableService> logger) : base(logger)
    {
    }

    protected override string BasePath => "actions/variables";

    public async Task<string?> GetAsync(string name)
    {
        return (await base.GetAsync(GetApiUri(name)))?.Value;
    }

    public async Task CreateOrUpdateAsync(string name, string value)
    {
        Variable variable = new()
        {
            Name = name,
            Value = value
        };
        try
        {
            await GetAsync(name);
            await PatchAsync(GetApiUri(name), variable);
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == HttpStatusCode.NotFound)
        {
            await PostAsync(GetApiUri(), variable);
        }
    }
}