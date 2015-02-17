using System;

namespace TsdLib
{
    /// <summary>
    /// Describes various states that the system can be in.
    /// </summary>
    [Flags]
    public enum State
    {
        /// <summary>
        /// A test is currently running.
        /// </summary>
        TestStarting = 1,
        /// <summary>
        /// The system is ready to configure or start a test. 
        /// </summary>
        ReadyToTest = 2
    }
}