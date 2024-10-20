using Microsoft.Extensions.Logging;

namespace StoreGate.Common.Commands;

#if DEBUG
[Command("test", "A test subject")]
public class TestCommand : AbstractCommand
{
    public TestCommand(ILogger<TestCommand> logger) : base(logger)
    {
    }
    
    [Option("m", "message", "Message to display.")]
    private string Message { get; set; } = "";

    public override async Task RunAsync()
    {
        await Task.CompletedTask;
        Logger.LogInformation($"Confirming message: {Message}");
    }
}
#endif