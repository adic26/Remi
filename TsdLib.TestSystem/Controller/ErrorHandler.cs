using System;
using System.Diagnostics;

namespace TsdLib.TestSystem.Controller
{
    public class ErrorHandler : IErrorHandler
    {
        public bool TryHandleError(Exception ex, string source)
        {
            Trace.WriteLine(ex);
#if DEBUG
            return false;
#else
            System.Threading.Tasks.Task.Run(() =>
            {
                bool helpLinkPresent = ex.HelpLink != null;
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(string.Join(Environment.NewLine,
                        "Error: " + ex.GetType().Name,
                        "Message: " + ex.Message,
                        helpLinkPresent ? "Would you like to view help for this error?" : ""),
                    "Error occurred in " + source,
                    helpLinkPresent ? System.Windows.Forms.MessageBoxButtons.YesNo : System.Windows.Forms.MessageBoxButtons.OK);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Trace.WriteLine(task.Exception);
            });
            return true;
#endif
        }
    }
}
