using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.Extensions.Logging;
using StoreGate.Commands.Common;
using StoreGate.Models.Common;
using StoreGate.Services.GitHub;
using Version = StoreGate.Models.Common.Version;

namespace StoreGate.Commands.GitHub;

[Command("version", "Manipulates the stored version on github")]
public class VersionCommand : AbstractCommand
{
    private enum ActionType
    {
        None,
        Read,
        Update
    }

    private enum UpdateType
    {
        None,
        Patch,
        Minor,
        Major
    }

    public VersionCommand(VariableService variableService, ILogger<VersionCommand> logger) : base(logger)
    {
        VariableService = variableService;
    }

    private VariableService VariableService { get; }


    [Option("v", "variable", "GitHub action variable name.", Default = Constants.Action.DefaultVersionVariable)]
    private string Variable { get; set; } = Constants.Action.DefaultVersionVariable;

    [Required(ErrorMessage = "Action Type")]
    [Option("r", "read", "", FlagValue = ActionType.Read)]
    [Option("u", "update", "", FlagValue = ActionType.Update)]
    private ActionType? Action { get; set; }

    [Required(ErrorMessage = "Update Type")]
    [Option("M", "major", "", FlagValue = UpdateType.Major)]
    [Option("m", "minor", "", FlagValue = UpdateType.Minor)]
    [Option("p", "patch", "", FlagValue = UpdateType.Patch)]
    private UpdateType? Update { get; set; }

    public override async Task RunAsync()
    {
        switch (Action)
        {
            case ActionType.Read:
                Logger.LogInformation(await GetOrDefaultAsync(Variable));
                break;
            case ActionType.Update:
                Version version = await GetOrDefaultAsync(Variable);
                switch (Update)
                {
                    case UpdateType.Patch:
                        version.Patch++;
                        break;
                    case UpdateType.Minor:
                        version.Minor++;
                        break;
                    case UpdateType.Major:
                        version.Major++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Update));
                }

                await VariableService.CreateOrUpdateAsync(Variable, version);
                Logger.LogInformation(version);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Action));
        }
    }
    
    private async Task<Version> GetOrDefaultAsync(string variable)
    {
        string? response;
        try
        {
            response = await VariableService.GetAsync(variable);
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == HttpStatusCode.NotFound)
        {
            return new Version();
        }

        if (!Version.TryParse(response, out Version version))
        {
            throw new Exception($"Wrong version found: \"{response}\".");
        }

        return version;
    }
}