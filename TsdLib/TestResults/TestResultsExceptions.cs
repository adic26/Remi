using System;
using System.Xml.Linq;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Exception caused by attempting to access a MeasurementParameter that does not exist in the MeasurementParameterCollection.
    /// </summary>
    public class MeasurementParameterException : TsdLibException
    {
        /// <summary>
        /// Initialize a MeasurementParameterException for the specified parameter name.
        /// </summary>
        /// <param name="name">Name of the parameter that does not exisit.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the MeasurementParameterException.</param>
        public MeasurementParameterException(string name, Exception inner = null)
            : base("A MeasurementParameter named " + name + " does not exist.", inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the MeasurementParameterException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected MeasurementParameterException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }

    /// <summary>
    /// Exception caused by an error serializing or deserializing test results, measurements or headers.
    /// </summary>
    public class SerializationException : TsdLibException
    {
        /// <summary>
        /// Initialize a new SerializationException with a specified message.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the SerializationException.</param>
        public SerializationException(string message, Exception inner = null)
            : base(message, inner) { }

        /// <summary>
        /// Initialize a new SerializationException resulting from a missing XML element.
        /// </summary>
        /// <param name="outerElement">The <see cref="T:System.Xml.Linq.XElement"/> that should contain the inner element.</param>
        /// <param name="innerElementName">Name of the inner element that was not found.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the SerializationException.</param>
        public SerializationException(XElement outerElement, string innerElementName, Exception inner = null)
            : base("The XML element " + outerElement.Name + " does not contain a valid " + innerElementName + " element.", inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the SerializationException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected SerializationException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }
}
