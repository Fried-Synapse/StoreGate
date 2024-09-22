using Microsoft.Extensions.DependencyInjection;

namespace StoreGate.Common.Extensions;

public static class ServiceExtensions
{
    public static void AddAllTransient<T>(this IServiceCollection services)
    {
        Type interfaceType = typeof(T);

        // Find all types that implement the specified interface
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (Type type in types)
        {
            services.AddTransient(type);
        }
    }

}