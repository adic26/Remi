using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TsdLib.UI
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {

        /// <summary>
        /// Event fired when the UI is about to close.
        /// </summary>
        event EventHandler<CancelEventArgs> UIClosing;

        /// <summary>
        /// Gets a TraceListener used to write trace and debug information to the user interface.
        /// </summary>
        ITraceListenerControl TraceListenerControl { get; }

        IMeasurementDisplayControl MeasurementDisplayControl { get; }



        /// <summary>
        /// Add user-defined data to the UI display.
        /// </summary>
        /// <param name="data">Data object to add.</param>
        void AddData(object data);


        ITestDetailsControl TestDetailsControl { get; }



        IProgressControl ProgressControl { get; }


        ITestInfoDisplayControl TestInfoDisplayControl { get; }

        /// <summary>
        /// Sets the text displayed in the title section of the UI.
        /// </summary>
        /// <param name="title">Text to display.</param>
        void SetTitle(string title);


        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        void SetState(State state);
    }

    /// <summary>
    /// Extends <see cref="IView"/> by adding type parameters for configuration types.
    /// </summary>
    public interface IView<TStationConfig, TProductConfig, TTestConfig> : IView
        where TStationConfig : IStationConfig
        where TProductConfig : IProductConfig
        where TTestConfig : ITestConfig
    {
        IConfigControl<TStationConfig, TProductConfig, TTestConfig> ConfigControl { get; }

        ITestSequenceControl<TStationConfig, TProductConfig, TTestConfig> TestSequenceControl { get; }
    }


}
