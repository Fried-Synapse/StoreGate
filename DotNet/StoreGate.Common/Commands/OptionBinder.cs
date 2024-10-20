using System.Reflection;

namespace StoreGate.Common.Commands;

public class OptionBinder
{
    private static Dictionary<Type, Func<string, object?>> BindingRules { get; } = new()
    {
    };

    public static void AddRule<T>(Func<string, object?> rule)
    {
        if (BindingRules.ContainsKey(typeof(T)))
        {
            throw new ArgumentException("Rule already exists.");
        }

        BindingRules.Add(typeof(T), rule);
    }

    public OptionBinder(AbstractCommand command, PropertyInfo property, OptionAttribute optionAttribute)
    {
        Command = command;
        Property = property;
        OptionAttribute = optionAttribute;
    }

    private AbstractCommand Command { get; }
    private PropertyInfo Property { get; }
    private OptionAttribute OptionAttribute { get; }

    public bool IsOption(string option)
        => OptionAttribute.LongOption == option || OptionAttribute.ShortOption == option;

    //TODO if list bind each item
    public void Bind(List<string> values)
        => Property.SetValue(Command, GetValue(values));

    private object? GetValue(List<string> values)
        => Property.PropertyType switch
        {
            { IsEnum: true } => OptionAttribute.FlagValue,
            { } stringType when stringType == typeof(string) => values[0],
            _ => BindingRules.ContainsKey(Property.PropertyType)
                ? BindingRules[Property.PropertyType](values[0])
                : throw new ArgumentException($"Unknown datatype while trying to bind -{OptionAttribute.ShortOption}/--{OptionAttribute.LongOption}.")
        };
}