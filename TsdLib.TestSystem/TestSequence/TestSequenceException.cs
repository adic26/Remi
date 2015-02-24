using System;
using System.Runtime.Serialization;
using TsdLib.Configuration.Common;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Exception due to an invalid test sequence source code file.
    /// </summary>
    [Serializable]
    public class TestSequenceException : TsdLibException
    {
        /// <summary>
        /// Initialize a TestSequenceException for the specified test sequence.
        /// </summary>
        /// <param name="testSequence">Name of the test sequence that caused the error.</param>
        /// <param name="message">A message describing the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the TestSequenceException.</param>
        public TestSequenceException(SequenceConfigCommon testSequence, string message, Exception inner = null)
            : base(string.Format("Error on {0}. {1}.", testSequence.GetType().Name, message), inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the TestSequenceException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected TestSequenceException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
