﻿using System;
using System.ComponentModel;

namespace TsdLib.UI
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {
        event EventHandler<CancelEventArgs> UIClosing;

        /// <summary>
        /// Gets a TraceListener used to write trace and debug information to the user interface.
        /// </summary>
        ITraceListenerControl TraceListenerControl { get; }

        IConfigControl ConfigControl { get; }

        ITestInfoDisplayControl TestInfoDisplayControl { get; }

        IMeasurementDisplayControl MeasurementDisplayControl { get; }

        IProgressControl ProgressControl { get; }

        ITestSequenceControl TestSequenceControl { get; }

        ITestDetailsControl TestDetailsControl { get; }

        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        void SetState(State state);

        /// <summary>
        /// Add user-defined data to the UI display.
        /// </summary>
        /// <param name="data">Data object to add.</param>
        void AddData(object data);

        /// <summary>
        /// Sets the text displayed in the title section of the UI.
        /// </summary>
        /// <param name="title">Text to display.</param>
        void SetTitle(string title);
    }


}