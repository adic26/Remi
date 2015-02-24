using System;
using System.Threading;

namespace TsdLib.TestSystem
{
    /// <summary>
    /// Contains operations related to cancellation.
    /// </summary>
    public interface ICancellationManager
    {   
        /// <summary>
        /// Abort the test sequence due to user cancellation or error.
        /// </summary>
        /// <param name="error">If cancelling due to error, pass the responsible exception. If cancelling to to user, pass null.</param>
        void Abort(Exception error = null);
        /// <summary>
        /// Returns true if the test sequence was cancelled by the user. False if it was cancelled due to internal error.
        /// </summary>
        bool CancelledByUser { get; }
        /// <summary>
        /// Gets the internal error responsible for test sequence cancellation.
        /// </summary>
        Exception Error { get; set; }

        CancellationToken Token { get; }
    }
}