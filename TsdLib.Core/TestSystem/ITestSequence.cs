using System;

namespace TsdLib.TestSystem
{
    public interface ITestSequence : IDisposable
    {
        void Abort(Exception error = null);
        bool CancelledByUser { get; }
        Exception Error { get; }
    }
}