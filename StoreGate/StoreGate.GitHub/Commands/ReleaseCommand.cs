using StoreGate.Common.Commands;
using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services;
using Version = StoreGate.GitHub.Models.Version;

namespace StoreGate.GitHub.Commands;

[Command("release", "Releases using the current version")]
public class ReleaseCommand : AbstractCommand
{
    public ReleaseCommand(
        ReleaseService releaseService)
    {
        ReleaseService = releaseService;
    }

    private ReleaseService ReleaseService { get; }

    [Option("v", "variable", "GitHub action variable name.", Default = Constants.GitHub.Action.DefaultVersionVariable)]
    private Version Version { get; set; } = new();

    [Option("n", "name", "The name of the file to appear on GitHub.")]
    private string Name { get; set; } = "";

    [Option("f", "file", "File path.")]
    private string FileName { get; set; } = "";

    public override async Task RunAsync()
    {
        Release? created = await ReleaseService.CreateAsync(new Release(Version));
        if (created == null)
        {
            throw new Exception("Could not create release");
        }

        Console.WriteLine(await ReleaseService.UploadAsync(created, Name, FileName));

        await Task.CompletedTask;
    }
}