using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace TsdLib.Forms
{
    public class MultiLineStringConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            IEnumerable<string> v = value as IEnumerable<string>;
            if (v != null && destinationType == typeof(string))
                return string.Join("; ", v);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
