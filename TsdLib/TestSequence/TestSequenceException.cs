using System;

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
        public TestSequenceException(string sequenceFileName)
            : base(sequenceFileName + " does not contain valid test sequence source code.") { }
    }
}
