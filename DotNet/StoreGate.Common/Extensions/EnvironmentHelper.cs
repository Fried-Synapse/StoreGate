namespace StoreGate.Common.Extensions;

public static class EnvironmentHelper
{
    public static string GetEnvironmentVariable(string variable)
        => Environment.GetEnvironmentVariable(variable) ??
           throw new KeyNotFoundException($"Missing environment variable: {variable}");
}