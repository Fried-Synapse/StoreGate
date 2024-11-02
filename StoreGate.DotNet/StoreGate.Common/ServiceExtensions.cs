using Microsoft.Extensions.DependencyInjection;

namespace StoreGate.Common;

public static class ServiceExtensions
{
    public static void AddAllTransient<T>(this IServiceCollection services)
    {
        foreach (Type type in Utils.FindAllTypes<T>())
        {
            services.AddTransient(type);
        }
    }
}