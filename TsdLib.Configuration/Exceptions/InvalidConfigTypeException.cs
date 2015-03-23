using System;
using System.Runtime.Serialization;

namespace TsdLib.Configuration.Exceptions
{
    /// <summary>
    /// Exception due to an invalid configuration type.
    /// </summary>
    [Serializable]
    public class InvalidConfigTypeException : TsdLibException
    {
        /// <summary>
        /// Initialize a new instance with the specified configuration type.
        /// </summary>
        /// <param name="configType">Type of configuration that caused the error.</param>
        /// <param name="message">OPTIONAL: Additional information describing the error.</param>
        /// <param name="inner">OPTIONAL: The exception that caused the InvalidConfigTypeException</param>
        public InvalidConfigTypeException(Type configType, string message = "", Exception inner = null)
            : base(string.Format("The configuration type: {0} is invalid.{1}{2}", configType.Name, Environment.NewLine, message), inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the <see cref="InvalidConfigTypeException"/> class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected InvalidConfigTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

}