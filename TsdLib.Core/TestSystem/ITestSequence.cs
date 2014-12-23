using System;

namespace TsdLib.TestSystem
{
    public interface ITestSequence : IDisposable
    {
        void Abort(Exception error = null); 
    }
}