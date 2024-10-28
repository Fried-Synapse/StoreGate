using Microsoft.Extensions.Logging;
using StoreGate.Common.Extensions;

namespace StoreGate.Common.Services;

public abstract class AbstractCommandService : AbstractService
{
    protected AbstractCommandService(ILogger logger) : base(logger)
    {
    }

    protected abstract string CommandFileName { get; }

    protected virtual ProcessRunner GetRunner()
        => new(Logger, CommandFileName);
}