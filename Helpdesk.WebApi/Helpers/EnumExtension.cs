using System.ComponentModel;
using System.Reflection;

namespace Helpdesk.WebApi.Helpers;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType()
            .GetField(value.ToString());

        if (field is null)
        {
            return "Unknown";
        }

        var descriptionAttr = field.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttr != null
            ? descriptionAttr.Description
            : value.ToString();
    }
}