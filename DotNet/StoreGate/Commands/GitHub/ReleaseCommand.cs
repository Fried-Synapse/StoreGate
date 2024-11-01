using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using StoreGate.Commands.Common;
using StoreGate.Models.GitHub;
using StoreGate.Services.GitHub;
using Version = StoreGate.Models.Common.Version;

namespace StoreGate.Commands.GitHub;

[Command("release-gh", "Releases using the current version")]
public class ReleaseCommand : AbstractCommand
{
    public ReleaseCommand(ReleaseService releaseService, ILogger<ReleaseCommand> logger) : base(logger)
    {
        ReleaseService = releaseService;
    }

    private ReleaseService ReleaseService { get; }

    [Required(ErrorMessage = nameof(Version))]
    [Option("v", "version", "Version for the release.", Default = Constants.Action.DefaultVersionVariable)]
    private Version? Version { get; set; }

    [Required(ErrorMessage = nameof(Name))]
    [Option("n", "name", "The name of the file to appear on GitHub.")]
    private string? Name { get; set; }

    [Required(ErrorMessage = nameof(FileName))]
    [Option("f", "file", "File path.")]
    private string? FileName { get; set; }

    public override async Task RunAsync()
    {
        Release? created = await ReleaseService.CreateAsync(new Release(Version!));
        if (created == null)
        {
            throw new Exception("Could not create release");
        }

        await ReleaseService.UploadAsync(created, Name!, FileName!);

        await Task.CompletedTask;
    }
}