using System;
using System.Runtime.Serialization;

namespace TsdLib.Configuration.Exceptions
{
    /// <summary>
    /// Exception due to an invalid configuration type.
    /// </summary>
    [Serializable]
    public class MissingConfigTypeException : TsdLibException
    {
        public MissingConfigTypeException(string configType, string message = "", Exception inner = null)
            : base(string.Format("The configuration type: {0} could not be located.{1}{2}", configType, Environment.NewLine, message), inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the <see cref="MissingConfigTypeException"/> class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected MissingConfigTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

}