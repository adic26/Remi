using System;

namespace TsdLib.TestSystem.TestSequence
{
    public interface ITestSequence
    {
        void Abort();
        void Abort(Exception ex);
    }
}