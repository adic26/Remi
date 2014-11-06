using System;

namespace TsdLib.InstrumentLibrary
{
    /// <summary>
    /// Exception due to an error generating source code and/or assembly.
    /// </summary>
    [Serializable]
    public class CodeGeneratorException : Exception
    {
        /// <summary>
        /// Initialize a new CodeGeneratorException with the specified message.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CodeGeneratorException.</param>
        public CodeGeneratorException(string message, Exception inner = null)
            : base(message, inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the CodeGeneratorException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected CodeGeneratorException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }
}