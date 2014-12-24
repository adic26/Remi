using System.Diagnostics;

namespace TsdLib.UI
{
    /// <summary>
    /// Defines properties required to add a <see cref="TraceListener"/> to the UI.
    /// </summary>
    public interface ITraceListenerControl : ITsdLibControl
    {
        /// <summary>
        /// Gets a trace listener.
        /// </summary>
        TraceListener Listener { get; }
    }
}