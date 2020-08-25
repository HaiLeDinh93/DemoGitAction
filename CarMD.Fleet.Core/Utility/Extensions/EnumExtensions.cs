using System;
using System.ComponentModel;
using System.Linq;

namespace CarMD.Fleet.Core.Utility.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                  as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
       where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}