using System;
using System.Runtime.Serialization;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Exception thrown when attempting to run a test sequence with a missing configuration object.
    /// </summary>
    [Serializable]
    public class NoConfigSelectedException : TsdLibException
    {
        /// <summary>
        /// Initialize a new <see cref="NoConfigSelectedException"/>.
        /// </summary>
        /// <param name="configType">The type of configuration that is missing.</param>
        /// <param name="inner">Exception that caused the error.</param>
        public NoConfigSelectedException(string configType, Exception inner = null)
            : base("No " + configType + " selected", inner) { }


        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the NoConfigSelectedException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        
        protected NoConfigSelectedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
