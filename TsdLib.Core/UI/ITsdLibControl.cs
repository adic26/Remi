namespace TsdLib.UI
{
    /// <summary>
    /// Common interface that all UI controls should implement.
    /// </summary>
    public interface ITsdLibControl
    {
        /// <summary>
        /// Set the behaviour of the control based on the current state of the test system.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        void SetState(State state);
    }
}