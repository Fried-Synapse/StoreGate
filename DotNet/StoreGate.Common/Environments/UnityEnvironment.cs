namespace StoreGate.Common.Environments;

public static class UnityEnvironment
{
    public static string Username { get; } = Environment.GetEnvironmentVariable("UNITY_USERNAME");
    public static string Password { get; } = Environment.GetEnvironmentVariable("UNITY_PASSWORD");
    public static string Licence { get; } = Environment.GetEnvironmentVariable("UNITY_LICENCE");
    public static string Serial { get; } = Environment.GetEnvironmentVariable("UNITY_SERIAL");
}