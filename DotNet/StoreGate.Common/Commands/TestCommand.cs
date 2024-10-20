namespace StoreGate.Common.Commands;

#if DEBUG
[Command("test", "A test subject")]
public class TestCommand : AbstractCommand
{
    [Option("m", "message", "Message to display.")]
    private string Message { get; set; } = "";

    public override async Task RunAsync()
    {
        await Task.CompletedTask;
        Console.WriteLine($"Confirming message: {Message}");
    }
}
#endif