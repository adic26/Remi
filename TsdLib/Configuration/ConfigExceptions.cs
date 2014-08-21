using System;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Exception due to an error saving or loading configuration data.
    /// </summary>
    [Serializable]
    public class ConfigException : TsdLibException
    {
        /// <summary>
        /// Initialize a new ConfigException for the specified configuration type and instance name. 
        /// </summary>
        /// <param name="type">Type of configuration object.</param>
        /// <param name="name">Name of configuration instance.</param>
        public ConfigException(Type type, string name)
            : base("An instance of " + type.Name + " named " + name + "could not be found") { }
    }
}