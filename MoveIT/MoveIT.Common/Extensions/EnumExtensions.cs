using System.ComponentModel;

namespace MoveIT.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToSnakeCaseString(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
