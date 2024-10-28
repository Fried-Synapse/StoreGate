using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services;
using Version = StoreGate.Common.Models.Version;

namespace StoreGate.GitHub.Commands;

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