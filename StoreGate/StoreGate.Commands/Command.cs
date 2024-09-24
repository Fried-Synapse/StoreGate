using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services;

namespace StoreGate.Commands;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class OptionAttribute : Attribute
{
    public string ShortOption { get; set; }
    public string LongOption { get; set; }
    public string Description { get; set; }
    public string Default { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public CommandAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}

public interface ICommand
{
}

public abstract class AbstractCommand : ICommand
{
    public abstract Task RunAsync();
}

[Command("version")]
public class VersionCommand : AbstractCommand
{
    private enum ActionType
    {
        Read,
        Update
    }

    private enum UpdateType
    {
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

    [Option(ShortOption = "v", LongOption = "variable", Description = "GitHub action variable name.", Default = VariableDefaultValue)]
    private string Variable { get; set; } = VariableDefaultValue;

    [Option(ShortOption = "r", LongOption = "read")]
    [Option(ShortOption = "u", LongOption = "update")]
    private ActionType? Action { get; set; }

    [Option(ShortOption = "M", LongOption = "major")]
    [Option(ShortOption = "m", LongOption = "minor")]
    [Option(ShortOption = "p", LongOption = "patch")]
    private UpdateType? Update { get; set; }

    public override async Task RunAsync()
    {
        Console.WriteLine("yeeey. Next step bind values");
        return;
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