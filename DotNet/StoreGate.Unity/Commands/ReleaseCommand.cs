using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;

namespace StoreGate.GitHub.Commands;

[Command("release-uas", "Releases using the current version")]
public class ReleaseCommand : AbstractCommand
{
    public ReleaseCommand(ILogger<ReleaseCommand> logger) : base(logger)
    {
    }

  
    public override async Task RunAsync()
    {
      
    }
}