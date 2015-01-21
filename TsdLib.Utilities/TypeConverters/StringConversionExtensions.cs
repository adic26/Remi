using System.ComponentModel;

namespace TsdLib.Utilities.TypeConverters
{
    public static class StringConversionExtensions
    {
        public static string GetDescription(this object obj)
        {
            string name = obj.ToString();
            object[] attrs = obj.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attrs.Length > 0) ? ((DescriptionAttribute)attrs[0]).Description : name;
        }
    }
}
