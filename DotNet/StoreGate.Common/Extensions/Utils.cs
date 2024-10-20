namespace StoreGate.Common.Extensions;

public static class Utils
{
    public static IEnumerable<Type> FindAllTypes<T>()
    {
        Type interfaceType = typeof(T);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    }
}