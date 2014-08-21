namespace TsdLib
{
    /// <summary>
    /// Describes various states that the system can be in.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// A test is currently running. The only allowable input should be to abort.
        /// </summary>
        TestInProgress,
        /// <summary>
        /// The system is ready to configure or start a test. 
        /// </summary>
        ReadyToTest
    }
}