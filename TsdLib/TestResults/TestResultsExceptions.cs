using System;
using System.Runtime.Serialization;

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
}
