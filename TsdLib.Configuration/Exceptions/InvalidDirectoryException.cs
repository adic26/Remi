using System;
using System.Runtime.Serialization;

namespace TsdLib.Configuration.Exceptions
{
    /// <summary>
    /// Exception due to an invalid or missing directory.
    /// </summary>
    [Serializable]
    public class InvalidDirectoryException : TsdLibException
    {
        public InvalidDirectoryException(string directory, Exception inner = null)
            : base("The path " + directory + " is invalid. Please ensure the directory exists.", inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the InvalidFilePathException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected InvalidDirectoryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

}