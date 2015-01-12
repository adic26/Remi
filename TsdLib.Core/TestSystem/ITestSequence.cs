using System;

namespace TsdLib.TestSystem
{
    public interface ITestSequence : IDisposable
    {
        void Abort(Exception error = null);
        bool CancelledByUser { get; }
        Exception Error { get; }

        /// <summary>
        /// Gets the number of steps in the current operation, used for reporting progress.
        /// </summary>
        int NumberOfSteps { get; set; }
    }
}