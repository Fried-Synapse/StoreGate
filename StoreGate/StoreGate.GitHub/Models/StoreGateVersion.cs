namespace StoreGate.GitHub.Models;

using System;

public record StoreGateVersion(int Major = 0, int Minor = 0, int Patch = 0) : IComparable<StoreGateVersion>
{
    public int Major { get; set; } = Major;
    public int Minor { get; set; } = Minor;
    public int Patch { get; set; } = Patch;

    public override string ToString()
        => $"{Major}.{Minor}.{Patch}";

    public override int GetHashCode()
        => HashCode.Combine(Major, Minor, Patch);

    public int CompareTo(StoreGateVersion? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (Major != other.Major)
        {
            return Major.CompareTo(other.Major);
        }

        if (Minor != other.Minor)
        {
            return Minor.CompareTo(other.Minor);
        }

        return Patch.CompareTo(other.Patch);
    }

    #region Static

    public static StoreGateVersion Parse(string? versionString)
    {
        if (versionString == null)
        {
            throw new FormatException("Version string must be in the format Major.Minor.Patch");
        }

        string[] parts = versionString.Split('.');
        if (parts.Length > 3)
        {
            throw new FormatException("Version string must be in the format Major.Minor.Patch");
        }

        if (!int.TryParse(parts[0], out int major))
        {
            throw new FormatException("Invalid version number format.");
        }

        int minor = 0;

        if (parts.Length > 1 && !int.TryParse(parts[1], out minor))
        {
            throw new FormatException("Invalid minor version format.");
        }

        int patch = 0;
        if (parts.Length > 2 && !int.TryParse(parts[2], out patch))
        {
            throw new FormatException("Invalid patch version format.");
        }

        return new StoreGateVersion(major, minor, patch);
    }

    public static bool TryParse(string? versionString, out StoreGateVersion version)
    {
        version = new StoreGateVersion();
        try
        {
            version = Parse(versionString);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    #endregion
}