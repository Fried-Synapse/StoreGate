using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using StoreGate.Commands.Common;
using StoreGate.Services.Unity;

namespace StoreGate.Commands.Unity;

[Command("unityCreatePackage", "Activates Unity's licence")]
public class CreatePackageCommand : AbstractLicencedUnityCommand
{
    public CreatePackageCommand(
        UnityService unityService,
        ILogger<CreatePackageCommand> logger)
        : base(unityService, logger)
    {
    }

    [Option("p", "projectPath", "Unity project path")]
    private string ProjectPath { get; set; } = ".";

    [Required(ErrorMessage = nameof(AssetsPaths))]
    [Option("a", "assetsPaths", "Paths of all assets. Separated with ';'.")]
    private string? AssetsPaths { get; set; }

    [Required(ErrorMessage = nameof(PackageName))]
    [Option("n", "packageName", "Name of the package to release.")]
    private string? PackageName { get; set; }

    protected override async Task RunLicencedAsync()
    {
        await UnityService.CreatePackageAsync(ProjectPath, AssetsPaths!, PackageName!);
    }
}