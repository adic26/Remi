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
        /// <param name="inner">OPTIONAL: The Exception that caused the CompilerException.</param>
        public ConfigException(Type type, string name, Exception inner = null)
            : base("An instance of " + type.Name + " named " + name + "could not be found", inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the ConfigException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected ConfigException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }
}