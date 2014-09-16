using System;
using System.Runtime.Serialization;

namespace TsdLib.TestSequence
{
    /// <summary>
    /// Exception due to an invalid test sequence source code file.
    /// </summary>
    [Serializable]
    public class TestSequenceException : TsdLibException
    {
        /// <summary>
        /// Initialize a TestSequenceException for the specified source file name.
        /// </summary>
        /// <param name="sequenceFileName">Path to the test sequence source code file that caused the error.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the TestSequenceException.</param>
        public TestSequenceException(string sequenceFileName, Exception inner = null)
            : base(sequenceFileName + " does not contain valid test sequence source code.", inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the TestSequenceException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected TestSequenceException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }
}
