using System;

namespace TsdLib.TestSystem
{
    public interface ITestSequence
    {
        void Abort();
        void Abort(Exception ex);
    }
}