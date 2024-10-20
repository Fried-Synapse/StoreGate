using System.Net;
using StoreGate.Common;
using StoreGate.Common.Commands;
using StoreGate.GitHub.Services;
using Version = StoreGate.GitHub.Models.Version;

namespace StoreGate.GitHub.Commands;

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

    public VersionCommand(VariableService variableService)
    {
        VariableService = variableService;
    }

    private VariableService VariableService { get; }


    [Option("v", "variable", "GitHub action variable name.", Default = Constants.GitHub.Action.DefaultVersionVariable)]
    private string Variable { get; set; } = Constants.GitHub.Action.DefaultVersionVariable;

    [Option("r", "read", "", FlagValue = ActionType.Read)]
    [Option("u", "update", "", FlagValue = ActionType.Update)]
    private ActionType Action { get; set; }

    [Option("M", "major", "", FlagValue = UpdateType.Major)]
    [Option("m", "minor", "", FlagValue = UpdateType.Minor)]
    [Option("p", "patch", "", FlagValue = UpdateType.Patch)]
    private UpdateType Update { get; set; }

    public override async Task RunAsync()
    {
        switch (Action)
        {
            case ActionType.Read:
                Console.WriteLine(await GetOrDefaultAsync(Variable));
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

                await VariableService.CreateOrUpdateAsync(Variable, version.ToString());
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