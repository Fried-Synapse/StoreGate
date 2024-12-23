namespace StoreGate.Common.Environments;

public static class Environment
{
    public static string GetEnvironmentVariable(string variable)
        => System.Environment.GetEnvironmentVariable(variable) ?? string.Empty;
    public static void Exit(ExitReason reason)
        => System.Environment.Exit((int)reason);
}

public enum ExitReason
{
    Success = 0,
    FailedValidation = 1
}