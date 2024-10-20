using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace StoreGate.Common.Commands;

public class CommandRunner
{
    private enum ArgType
    {
        ShortOption,
        LongOption,
        Value
    }

    public CommandRunner(AbstractCommand command, ILogger<CommandRunner> logger)
    {
        Logger = logger;
        Command = command;
        foreach (PropertyInfo property in Command.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            List<OptionAttribute> optionAttributes = property.GetCustomAttributes<OptionAttribute>().ToList();
            if (optionAttributes.Count == 0)
            {
                continue;
            }

            RequiredAttribute? requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null)
            {
                RequiredProperties.Add(property, (requiredAttribute, optionAttributes));
            }

            OptionBinders.AddRange(optionAttributes.Select(oa => new OptionBinder(Command, property, oa)));
        }
    }

    protected ILogger Logger { get; }

    private AbstractCommand Command { get; }
    private List<OptionBinder> OptionBinders { get; } = new();
    private Dictionary<PropertyInfo, (RequiredAttribute, IEnumerable<OptionAttribute>)> RequiredProperties { get; } = new();

    public async Task RunAsync(string[] args)
    {
        BindOptions(args[1..]);
        await Command.RunAsync();
    }

    private void BindOptions(string[] args)
    {
        string option = string.Empty;
        List<string> optionValues = new();
        foreach (string arg in args)
        {
            ArgType type = GetOption(arg, out string value);
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

        if (option != string.Empty)
        {
            BindOption(option, optionValues);
        }

        CheckRequiredOptions();
    }

    private void BindOption(string option, List<string> values)
    {
        OptionBinder? binder = OptionBinders.FirstOrDefault(binder => binder.IsOption(option));

        if (binder == null)
        {
            Logger.LogWarning($"Unmapped option [{option}]");
            return;
        }

        binder.Bind(values);
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

    private void CheckRequiredOptions()
    {
        foreach (KeyValuePair<PropertyInfo, (RequiredAttribute RequiredAttribute, IEnumerable<OptionAttribute> OptionsAttributes)>
                     kvp in RequiredProperties)
        {
            if (kvp.Key.GetValue(Command) != null)
            {
                continue;
            }

            Type type = Nullable.GetUnderlyingType(kvp.Key.PropertyType) ?? kvp.Key.PropertyType;
            string message = $"Missing required option [{kvp.Value.RequiredAttribute.ErrorMessage ?? kvp.Key.Name}].";
            if (type.IsEnum)
            {
                message = $"{message} Possible options: {string.Join(',', kvp.Value.OptionsAttributes.Select(b => b.LongOption))}.";
            }

            Logger.LogError(message);
        }
    }
}