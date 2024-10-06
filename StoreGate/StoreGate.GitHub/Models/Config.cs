namespace StoreGate.GitHub.Models;

public record Config
{
    public string? Repo { get; set; }
    public string? Token { get; set; }
}