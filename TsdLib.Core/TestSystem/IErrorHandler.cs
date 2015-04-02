using System;

namespace TsdLib.TestSystem
{
    public interface IErrorHandler
    {
        bool TryHandleError(Exception error, string source);
    }
}