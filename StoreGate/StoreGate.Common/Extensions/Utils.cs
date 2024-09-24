namespace StoreGate.Common.Extensions;

public static class Utils
{
    public static IEnumerable<Type> FindAllTypes<T>()
    {
        Type interfaceType = typeof(T);
        var x = AppDomain.CurrentDomain.GetAssemblies();
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    }
}