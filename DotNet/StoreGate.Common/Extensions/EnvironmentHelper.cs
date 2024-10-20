namespace StoreGate.Common.Extensions;

public static class EnvironmentHelper
{
    public static string GetEnvironmentVariable(string variable)
        => Environment.GetEnvironmentVariable(variable) ??
           throw new KeyNotFoundException($"Missing environment variable: {variable}");

    public static void Exit(ExitReason reason)
        => Environment.Exit((int)reason);
}

public enum ExitReason
{
    Success = 0,
    FailedValidation = 1
}