using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace TsdLib.Configuration.Exceptions
{
    /// <summary>
    /// Exception due to an error saving or loading configuration data.
    /// </summary>
    [Serializable]
    public class InvalidConfigFileException : TsdLibException
    {
        /// <summary>
        /// Initialize a new InvalidConfigFileException when no configuration data exists for the specified test system. 
        /// </summary>
        /// <param name="configElements">Zero or more <see cref="XElement"/> that caused the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public InvalidConfigFileException(Exception inner = null, params XElement[] configElements)
            : base("Invalid config file at: " + Environment.NewLine + string.Join(Environment.NewLine, configElements.Select(e => e.ToString())), inner)
        {
        }

        /// <summary>
        /// Initialize a new InvalidConfigFileException when no configuration data exists for the specified test system. 
        /// </summary>
        /// <param name="document">The <see cref="XDocument"/> that caused the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public InvalidConfigFileException(XDocument document, Exception inner = null)
            : base("Invalid config file at: " + Environment.NewLine + document, inner)
        {
        }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the InvalidConfigFileException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected InvalidConfigFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}