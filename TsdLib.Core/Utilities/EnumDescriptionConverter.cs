using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace TsdLib.Utilities
{
    public class EnumDescriptionConverter : EnumConverter
    {
        public EnumDescriptionConverter(Type t)
            : base(t) { }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                throw new ArgumentException("Can only convert to string.", "destinationType");

            if (value == null)
                throw new ArgumentNullException("value");

            Type baseType = value.GetType().BaseType;
            if (baseType == null || !(baseType == typeof(Enum)))
                throw new ArgumentException("Can only convert an instance of enum.", "value");

            return value.GetDescription();
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                Enum standard = Enum.GetValues(EnumType)
                    .Cast<Enum>()
                    .First(en => en
                        .GetType()
                        .GetField(en.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .Cast<DescriptionAttribute>()
                        .First()
                        .Description == value.ToString()
                    );

                return standard;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
