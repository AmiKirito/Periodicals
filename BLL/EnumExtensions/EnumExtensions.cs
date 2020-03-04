using System;

namespace BLL.EnumExtensions
{
    public class EnumExtensions
    {
        public static T ParseEnumValue<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
