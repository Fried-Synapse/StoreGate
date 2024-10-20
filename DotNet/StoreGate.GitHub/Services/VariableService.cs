using System.Net;
using Microsoft.Extensions.Logging;
using StoreGate.Common;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services;

public class VariableService : AbstractService<Variable>
{
    public VariableService(Config config, ILogger<VariableService> logger) : base(config, logger)
    {
    }

    protected override string BasePath => "actions/variables";

    public async Task<string?> GetAsync(string name)
    {
        return (await base.GetAsync(GetApiUri(name)))?.Value;
    }

    public async Task CreateOrUpdateAsync(string name, string value)
    {
        Logger.LogInformation(Config.Token?[^5..]);
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