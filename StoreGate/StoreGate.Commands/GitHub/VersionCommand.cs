using StoreGate.Commands.Common;
using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services;

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

    public VersionCommand(VersionService versionService)
    {
        VersionService = versionService;
    }

    private VersionService VersionService { get; }

    private const string VariableDefaultValue = "Version";

    [Option("v", "variable", "GitHub action variable name.", Default = VariableDefaultValue)]
    private string Variable { get; set; } = VariableDefaultValue;

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
                Console.WriteLine(await VersionService.GetOrDefaultAsync(Variable));
                break;
            case ActionType.Update:
                StoreGateVersion version = await VersionService.GetOrDefaultAsync(Variable);
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

                await VersionService.Set(Variable, version);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Action));
        }
    }
}