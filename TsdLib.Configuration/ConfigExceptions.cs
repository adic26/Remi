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
    /// Exception due to an error saving or loading configuration data.
    /// </summary>
    [Serializable]
    public class ConfigDoesNotExistException : TsdLibException
    {
        /// <summary>
        /// Initialize a new ConfigDoesNotExistException when a configuration instance with the specified name does not exist for the specified configuration type. 
        /// </summary>
        /// <param name="type">Type of configuration object.</param>
        /// <param name="name">Name of configuration instance.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        [Obsolete]
        public ConfigDoesNotExistException(Type type, string name, Exception inner = null)
            : base("An instance of " + type.Name + " named " + name + "could not be found", inner) { }

        /// <summary>
        /// Initialize a new ConfigDoesNotExistException when a configuration instance with the specified name does not exist for the specified configuration type. 
        /// </summary>
        /// <param name="typeName">Type of configuration object.</param>
        /// <param name="name">Name of configuration instance.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        [Obsolete]
        public ConfigDoesNotExistException(string typeName, string name, Exception inner = null)
            : base("An instance of " + typeName + " named " + name + "could not be found", inner) { }

        /// <summary>
        /// Initialize a new ConfigDoesNotExistException when no configuration data exists for the specified test system. 
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">Operating mode of the test system.</param>
        /// <param name="dataDescription">Description of the data.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public ConfigDoesNotExistException(string testSystemName, string testSystemVersion, string testSystemMode, string dataDescription, Exception inner = null)
            : base(dataDescription + " does not exist for " + testSystemName + " v." + testSystemVersion + " mode:" + testSystemMode, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the ConfigDoesNotExistException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected ConfigDoesNotExistException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to a ConfigGroup containing zero config instances. A default instance should always be generated by the ConfigManager.
    /// </summary>
    [Serializable]
    public class EmptyConfigGroupException : TsdLibException
    {
        /// <summary>
        /// Initialize a new EmptyConfigGroupException with the specified <see cref="IConfigManager"/> configuration group.
        /// </summary>
        /// <param name="configManager">The empty <see cref="IConfigManager"/>.</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public EmptyConfigGroupException(IConfigManager configManager, Exception inner = null)
            : base("No config instances could be located for " + configManager, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the EmptyConfigGroupException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected EmptyConfigGroupException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an invalid file path when uploading a file to the database.
    /// </summary>
    [Serializable]
    public class InvalidFilePathException : TsdLibException
    {
        /// <summary>
        /// Initialize a new InvalidFileException with the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file causing the exception</param>
        /// <param name="inner">OPTIONAL: The Exception that caused the ConfigDoesNotExistException.</param>
        public InvalidFilePathException(string fileName, Exception inner = null)
            : base("The filename " + fileName + " is invalid. Please ensure it is an absolute file path.", inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the InvalidFileException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected InvalidFilePathException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}