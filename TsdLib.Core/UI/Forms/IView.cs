using TsdLib.UI.Controls;

namespace TsdLib.UI.Forms
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets a TraceListener used to write trace and debug information to the user interface.
        /// </summary>
        TraceListenerControlBase TraceListenerControl { get; }

        ConfigControlBase ConfigControl { get; }

        TestInfoDisplayControlBase TestInfoDisplayControl { get; }

        MeasurementDisplayControlBase MeasurementDisplayControl { get; }

        TestSequenceControlBase TestSequenceControl { get; }

        TestDetailsControlBase TestDetailsControl { get; }

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
        /// Gets or sets the text displayed in the title section of the UI.
        /// </summary>
        string Text { get; set; }
    }


}
