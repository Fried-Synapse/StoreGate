using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using StoreGate.Common;
using StoreGate.Services.Abstract;

namespace StoreGate.Services.Unity;

public class UnityService : AbstractCommandService
{
    public UnityService(ILogger<UnityService> logger) : base(logger)
    {
    }

    protected override string CommandFileName =>
#if DEBUG
        "/Applications/Unity/Hub/Editor/2022.3.39f1/Unity.app/Contents/MacOS/Unity";
#else
        "unity-editor";
#endif

    protected override ProcessRunner GetRunner()
        => base.GetRunner()
            .AddOption("-batchmode")
            .AddOption("-quit")
            .AddOption("-logFile -");

    public async Task ActivateLicenceAsync(string username, string password, string serial)
        => await GetRunner()
            .AddOption("-username", username)
            .AddOption("-password", password)
            .AddOption("-serial", serial)
            .RunAsync();

    public async Task ReturnLicenceAsync()
        => await GetRunner()
            .AddOption("-returnlicense")
            .RunAsync();

    public async Task CreatePackageAsync(string projectPath, string assetsPath, string packageName)
        => await GetRunner()
            .AddOption("-projectPath", projectPath)
            .ExecuteMethod("CreatePackage", assetsPath, $"{packageName}.unitypackage")
            .RunAsync();

    public string GetSerial(string licence)
    {
        XDocument document = XDocument.Parse(licence);

        string? developerDataValue = document.Root?
            .Descendants("DeveloperData")
            .Attributes("Value")
            .FirstOrDefault()?
            .Value;

        if (string.IsNullOrEmpty(developerDataValue) || developerDataValue.Length < 4)
        {
            return string.Empty;
        }

        return Encoding.UTF8.GetString(Convert.FromBase64String(developerDataValue)).Substring(4);
    }
}