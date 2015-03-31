using System;

namespace TsdLib.TestSystem
{
    public interface IErrorHandler : IDisposable
    {
        void HandleError(Exception error, string source);
    }
}