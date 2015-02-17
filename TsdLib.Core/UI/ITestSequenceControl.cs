using System;

namespace TsdLib.UI
{
    public interface ITestSequenceControl : ITsdLibControl
    {
        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        event EventHandler ExecuteTestSequence;

        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        event EventHandler AbortTestSequence;

        bool PublishResults { get; }
    }
}