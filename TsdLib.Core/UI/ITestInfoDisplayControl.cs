using TsdLib.Measurements;

namespace TsdLib.UI
{
    /// <summary>
    /// Defines methods required to display test information on the UI.
    /// </summary>
    public interface ITestInfoDisplayControl : ITsdLibControl
    {
        /// <summary>
        /// Add test information to the UI display.
        /// </summary>
        /// <param name="testInfo">Test information to add.</param>
        void AddTestInfo(ITestInfo testInfo);

        /// <summary>
        /// Clear the test information from the UI.
        /// </summary>
        void ClearTestInfo();
    }
}