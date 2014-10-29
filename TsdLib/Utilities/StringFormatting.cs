using System.Reflection;
using System.Text;

namespace TsdLib.Utilities
{
    /// <summary>
    /// Contains static methods used for formatting strings.
    /// </summary>
    public static class StringFormatting
    {
        /// <summary>
        /// Converts an object into a comma-separated value string by reflecting its properties.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="rowSeparator">OPTIONAL: Alternate string used to delimit rows.</param>
        /// <param name="columnSeparator">OPTIONAL: Alternate string used to delimit columns.</param>
        /// <returns>A CSV string representing the properties of the specified object.</returns>
        public static string ToCsv(this object obj, string rowSeparator = "\r\n", string columnSeparator = ",")
        {
            //TODO: Add space in split camel casing
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                sb.Append(propertyInfo.Name + columnSeparator + obj.GetType().GetProperty(propertyInfo.Name).GetValue(obj, null) + rowSeparator);

            return sb.ToString();
        }
    }

}