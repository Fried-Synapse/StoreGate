using Microsoft.Extensions.Logging;

namespace StoreGate;
    
public class GitHubLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new GitHubLogger();

    public void Dispose() { }
}

public class GitHubLogger : ILogger
{
    public bool IsEnabled(LogLevel logLevel)
        => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => default!;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        string prefix = string.Empty;

        switch (logLevel)
        {
            case LogLevel.Trace:
                break;
            case LogLevel.Debug:
                break;
            case LogLevel.Information:
                break;
            case LogLevel.Warning:
                prefix = "##[warning]";
                break;
            case LogLevel.Error:
                prefix = "##[error]";
                break;
            case LogLevel.Critical:
                prefix = "##[error]";
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
        }

        Console.WriteLine($"{prefix}{formatter(state, exception)}");
    }
}