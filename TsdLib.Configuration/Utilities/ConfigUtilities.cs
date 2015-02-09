using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.Configuration.Utilities
{
    public class ConfigUtilities
    {
        public static string GetBaseTypeName(Type configType)
        {
            try
            {
                string configTypeName = configType.Name;

                if (configTypeName.EndsWith("Common"))
                    return configTypeName;

                return GetBaseTypeName(configType.BaseType);
            }
            catch (Exception ex)
            {
                throw new InvalidConfigTypeException(configType, ex);
            }

        }
    }
}
