using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace TsdLib.Configuration
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
            : base("Invalid config file at: " + Environment.NewLine + string.Join(Environment.NewLine, configElements.Select(e => e.ToString())), inner) { }

        /// <summary>
        /// Initialize a new InvalidConfigFileException when no configuration data exists for the specified test system. 
        /// </summary>
        /// <param name="document">The <see cref="XDocument"/> that caused the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public InvalidConfigFileException(XDocument document, Exception inner = null)
            : base("Invalid config file at: " + Environment.NewLine + document, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the InvalidConfigFileException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected InvalidConfigFileException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an invalid file name.
    /// </summary>
    [Serializable]
    public class InvalidFileException : TsdLibException
    {
        /// <summary>
        /// Initialize a new InvalidFileException with the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file causing the exception</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public InvalidFileException(string fileName, Exception inner = null)
            : base("The filename " + fileName + " is invalid.", inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the InvalidFileException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected InvalidFileException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

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

    /// <summary>
    /// Exception due to an error writing to a shared configuration location.
    /// </summary>
    [Serializable]
    public class SharedConfigWriteFailedException : TsdLibException
    {
        public SharedConfigWriteFailedException(Exception inner = null)
            : base("Failed to write configuration to the shared location", inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the <see cref="SharedConfigWriteFailedException"/> class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected SharedConfigWriteFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an invalid configuration type.
    /// </summary>
    [Serializable]
    public class InvalidConfigTypeException : TsdLibException
    {
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