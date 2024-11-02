using Microsoft.Extensions.Logging;
using StoreGate.Common;

namespace StoreGate.Services.Abstract;

public abstract class AbstractCommandService : AbstractService
{
    protected AbstractCommandService(ILogger logger) : base(logger)
    {
    }

    protected abstract string CommandFileName { get; }

    protected virtual ProcessRunner GetRunner()
        => new(Logger, CommandFileName);
}