using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services;
using Version = StoreGate.GitHub.Models.Version;

namespace StoreGate.GitHub.Commands;

[Command("release", "Releases using the current version")]
public class ReleaseCommand : AbstractCommand
{
    public ReleaseCommand(ReleaseService releaseService, ILogger<ReleaseCommand> logger) : base(logger)
    {
        ReleaseService = releaseService;
    }

    private ReleaseService ReleaseService { get; }

    [Required(ErrorMessage = "Version")]
    [Option("v", "version", "Version for the release.", Default = Constants.GitHub.Action.DefaultVersionVariable)]
    private Version? Version { get; set; }

    [Required(ErrorMessage = "Name")]
    [Option("n", "name", "The name of the file to appear on GitHub.")]
    private string? Name { get; set; }

    [Required(ErrorMessage = "File Path")]
    [Option("f", "file", "File path.")]
    private string? FileName { get; set; }

    public override async Task RunAsync()
    {
        Release? created = await ReleaseService.CreateAsync(new Release(Version!));
        if (created == null)
        {
            throw new Exception("Could not create release");
        }

        Console.WriteLine(await ReleaseService.UploadAsync(created, Name!, FileName!));

        await Task.CompletedTask;
    }
}