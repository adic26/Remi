using System;

namespace TsdLib.UI
{
    /// <summary>
    /// Defines methods and events required to control test details on the UI.
    /// </summary>
    public interface ITestDetailsControl : ITsdLibControl
    {
        /// <summary>
        /// Event fired when requesting to edit the test details.
        /// </summary>
        event EventHandler<bool> EditTestDetails;
    }
}