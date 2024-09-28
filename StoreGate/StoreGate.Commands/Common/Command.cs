namespace StoreGate.Commands.Common;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class OptionAttribute : Attribute
{
    public OptionAttribute(string shortOption, string longOption, string description)
    {
        ShortOption = shortOption;
        LongOption = longOption;
        Description = description;
    }

    public string ShortOption { get; }
    public string LongOption { get; }
    public string Description { get; }
    public object? Default { get; init; }
    public object? FlagValue { get; init; }
}

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public CommandAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get;  }
    public string Description { get;  }
}

public interface ICommand
{
}

public abstract class AbstractCommand : ICommand
{
    public abstract Task RunAsync();
}