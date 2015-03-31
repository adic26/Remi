using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLib.TestSystem.Controller
{
    public class ErrorHandler : IErrorHandler
    {
        public void HandleError(Exception ex, string source)
        {
            Trace.WriteLine(ex);
            Task.Run(() =>
            {
                bool helpLinkPresent = ex.HelpLink != null;
                DialogResult result = MessageBox.Show(string.Join(Environment.NewLine,
                        "Error: " + ex.GetType().Name,
                        "Message: " + ex.Message,
                        helpLinkPresent ? "Would you like to view help for this error?" : ""),
                    "Error occurred in " + source,
                    helpLinkPresent ? MessageBoxButtons.YesNo : MessageBoxButtons.OK);

                if (result == DialogResult.Yes)
                    Process.Start(ex.HelpLink);
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Trace.WriteLine(task.Exception);
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}
