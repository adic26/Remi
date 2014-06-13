using System;

namespace TsdLib
{
    [Obsolete]
    public interface IConnect : IDisposable
    {
        string Address { get; }
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
    }
}