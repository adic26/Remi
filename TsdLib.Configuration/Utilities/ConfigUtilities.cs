using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TsdLib.Configuration.Utilities
{
    /// <summary>
    /// Contains static methods to retrieve additional information about config items and types.
    /// </summary>
    public static class ConfigUtilities
    {
        /// <summary>
        /// Gets the common base type name of the specified config type.
        /// </summary>
        /// <param name="configType">Type of the config item.</param>
        /// <returns>The name of the common base type.</returns>
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

        /// <summary>
        /// Gets the name of the IXXXConfig interface implemented by the specified config item type.
        /// </summary>
        /// <param name="configType">Type of the config item.</param>
        /// <returns>The name of the IXXXConfig interface.</returns>
        public static string GetConfigItemInterfaceName(Type configType)
        {
            try
            {
                Type configInterface = configType.FindInterfaces(MyInterfaceFilter, "I.*Config").FirstOrDefault();
                if (configInterface == null)
                    throw new InvalidConfigTypeException(configType);
                return configInterface.Name;
            }
            catch (Exception ex)
            {
                throw new InvalidConfigTypeException(configType, ex);
            }
        }

        private static bool MyInterfaceFilter(Type typeObj, Object criteriaObj)
        {
            return Regex.IsMatch(typeObj.ToString(), criteriaObj.ToString());
        }
    }
}
