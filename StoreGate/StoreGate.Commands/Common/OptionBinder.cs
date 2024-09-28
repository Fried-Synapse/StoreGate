using System.Reflection;

namespace StoreGate.Commands.Common;

public class OptionBinder
{
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

    public void Bind(List<string> values)
    {
        switch (Property.PropertyType)
        {
            case { IsEnum: true }:
                BindEnum();
                break;
            case { } stringType when stringType == typeof(string):
                BindString(values[0]);
                break;
            default:
                throw new ArgumentException("No binding implemented");
        }
    }

    private void BindEnum()
    {
        Property.SetValue(Command, OptionAttribute.FlagValue);
    }

    private void BindString(string value)
    {
        Property.SetValue(Command, value);
    }
}