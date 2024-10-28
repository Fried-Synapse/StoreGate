using Environment = StoreGate.Common.Models.Environment;

namespace StoreGate.Unity;

public class UnityEnvironment : Environment
{
    public string Username { get; } = GetEnvironmentVariable("UNITY_USERNAME");
    public string Password { get; } = GetEnvironmentVariable("UNITY_PASSWORD");
    public string Licence { get; } = GetEnvironmentVariable("UNITY_LICENCE");
    public string Serial { get; } = GetEnvironmentVariable("UNITY_SERIAL");
}