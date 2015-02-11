using System;
using System.Drawing;
using System.Windows.Forms;

namespace TsdLib.UI.Utilities
{
    /// <summary>
    /// Contains various utility methods related to how information or user interface elements are displayed.
    /// </summary>
    public static class Monitor
    {
        /// <summary>
        /// Show a <see cref="System.Windows.Forms.Form"/> on the specified monitor.
        /// </summary>
        /// <param name="monitorNumber">Number of the monitor on which to show the <see cref="System.Windows.Forms.Form"/>.</param>
        /// <param name="form"></param>
        public static void ShowOnMonitor(int monitorNumber, Form form)
        {
            Screen[] sc = Screen.AllScreens;

            if (monitorNumber > sc.Length - 1)
                monitorNumber = 0;

            Rectangle programsize = form.Bounds;
            Int32 left = (sc[monitorNumber].WorkingArea.Width - programsize.Width) / 2;
            Int32 top = (sc[monitorNumber].WorkingArea.Height - programsize.Height) / 2;

            form.Left = sc[monitorNumber].Bounds.Left + left;
            form.Top = sc[monitorNumber].Bounds.Top + top;
            form.StartPosition = FormStartPosition.Manual;
            form.Show();
        }
    }
}