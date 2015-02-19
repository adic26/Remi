using System;
using System.Runtime.Serialization;

namespace TsdLib.Configuration.Exceptions
{
    /// <summary>
    /// Exception due to an invalid file name.
    /// </summary>
    [Serializable]
    public class FileUploadException : TsdLibException
    {
        /// <summary>
        /// Initialize a new <see cref="FileUploadException"/> with the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file causing the exception</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public FileUploadException(string fileName, Exception inner = null)
            : base("The filename " + fileName + " is invalid.", inner)
        {
        }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the <see cref="FileUploadException"/> class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected FileUploadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}