using System;
using System.Threading;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Contains functionality to abort test sequences and manage test sequence errors.
    /// </summary>
    public class TestSequenceCancellationManager : MarshalByRefObject, ICancellationManager
    {
        private readonly CancellationTokenSource _userCancellationTokenSource;
        private readonly CancellationTokenSource _errorCancellationTokenSource;
        private readonly CancellationTokenSource _linkedCancellationTokenSource;

        /// <summary>
        /// Initialize a new <see cref="TestSequenceCancellationManager"/>.
        /// </summary>
        public TestSequenceCancellationManager()
        {
            _userCancellationTokenSource = new CancellationTokenSource();
            _errorCancellationTokenSource = new CancellationTokenSource();
            _linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_userCancellationTokenSource.Token, _errorCancellationTokenSource.Token);
        }

        /// <summary>
        /// Returns true if the test sequence was cancelled by the user. False if it was cancelled due to internal error.
        /// </summary>
        public bool CancelledByUser
        {
            get { return _userCancellationTokenSource.Token.IsCancellationRequested; }
        }
        /// <summary>
        /// Gets or sets the internal error responsible for test sequence cancellation.
        /// </summary>
        public Exception Error { get; set; }
        /// <summary>
        /// Abort the test sequence due to user cancellation or error.
        /// </summary>
        /// <param name="error">If cancelling due to error, pass the responsible exception. If cancelling to to user, pass null.</param>
        public void Abort(Exception error = null)
        {
            if (error == null)
                _userCancellationTokenSource.Cancel();
            else
            {
                Error = error;
                _errorCancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Gets a <see cref="CancellationToken"/> that can be checked to determine if cancellation has been requested.
        /// </summary>
        public CancellationToken Token
        {
            get { return _linkedCancellationTokenSource.Token; }
        }

        /// <summary>
        /// Prevents remoting exceptions due to lease expiration in a secondary AppDomain
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
