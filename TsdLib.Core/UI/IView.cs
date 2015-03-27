using System;
using System.ComponentModel;
using TsdLib.Observer;

namespace TsdLib.UI
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {
        ITestCaseControl TestCaseControl { get; }

        IConfigControl ConfigControl { get; }
        
        ITestInfoDisplayControl TestInfoDisplayControl { get; }

        IMeasurementDisplayControl MeasurementDisplayControl { get; }

        ITestSequenceControl TestSequenceControl { get; }

        ITestDetailsControl TestDetailsControl { get; }

        /// <summary>
        /// Gets a TraceListener used to write trace and debug information to the user interface.
        /// </summary>
        ITraceListenerControl TraceListenerControl { get; }

        IProgressControl ProgressControl { get; }
        /// <summary>
        /// Event fired when the UI is about to close.
        /// </summary>
        event EventHandler<CancelEventArgs> UIClosing;

        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        void SetState(State state);

        /// <summary>
        /// Sets the text displayed in the title section of the UI.
        /// </summary>
        /// <param name="title">Text to display.</param>
        void SetTitle(string title);

        /// <summary>
        /// Add user-defined data to the UI display.
        /// </summary>
        /// <param name="data">Data object to add.</param>
        void AddData(DataContainer data);
    }
}
