using Microsoft.Extensions.Logging;

namespace StoreGate.Common.Services;

public interface IService
{
}

public abstract class AbstractService : IService
{
    protected AbstractService(ILogger logger)
    {
        Logger = logger;
    }

    protected ILogger Logger { get; }
}