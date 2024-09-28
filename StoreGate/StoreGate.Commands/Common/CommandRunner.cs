using System.Reflection;

namespace StoreGate.Commands.Common;

public class CommandRunner
{
    private enum ArgType
    {
        ShortOption,
        LongOption,
        Value
    }

    public CommandRunner(AbstractCommand command)
    {
        Command = command;
        foreach (PropertyInfo prop in Command.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            OptionProperties.AddRange(prop.GetCustomAttributes<OptionAttribute>()
                .Select(oa => new OptionBinder(Command, prop, oa)));
        }
    }

    private AbstractCommand Command { get; }
    private List<OptionBinder> OptionProperties { get; } = new();

    public async Task RunAsync(string[] args)
    {
        BindOptions(args);
        await Command.RunAsync();
    }

    private void BindOptions(string[] args)
    {
        int i = 0;
        string option = string.Empty;
        List<string> optionValues = new();
        while (i < args.Length - 1)
        {
            i++;
            ArgType type = GetOption(args[i], out string value);
            switch (type)
            {
                case ArgType.ShortOption:
                case ArgType.LongOption:
                    if (option == string.Empty)
                    {
                        option = value;
                        continue;
                    }

                    BindOption(option, optionValues);
                    option = value;
                    optionValues.Clear();
                    break;
                case ArgType.Value:
                    optionValues.Add(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        BindOption(option, optionValues);
    }

    private void BindOption(string option, List<string> values)
    {
        OptionBinder? optionProperty = OptionProperties.FirstOrDefault(binder => binder.IsOption(option));

        if (optionProperty == null)
        {
            Console.WriteLine($"Warning: unmapped option [{option}]");
            return;
        }

        optionProperty.Bind(values);
    }


    private ArgType GetOption(string option, out string value)
    {
        value = string.Empty;
        if (option[0] != '-')
        {
            value = option;
            return ArgType.Value;
        }

        if (option[1] != '-')
        {
            value = option[1..];
            return ArgType.ShortOption;
        }

        value = option[2..];
        return ArgType.LongOption;
    }
}