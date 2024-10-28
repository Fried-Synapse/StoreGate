using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.Unity.Services;

namespace StoreGate.Unity.Commands;

[Command("unityCreatePackage", "Activates Unity's licence")]
public class CreatePackageCommand : AbstractUnityCommand
{
    public CreatePackageCommand(
        UnityService unityService,
        UnityEnvironment environment,
        ILogger<ActivateCommand> logger)
        : base(unityService, environment, logger)
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

    public override async Task RunAsync()
    {
        await TryActivate();
        await UnityService.CreatePackageAsync(ProjectPath, AssetsPaths!, PackageName!);
    }
}