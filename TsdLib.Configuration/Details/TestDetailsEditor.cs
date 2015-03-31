using System;
using System.Windows.Forms;
using TsdLib.Forms;

namespace TsdLib.Configuration.Details
{
    public class TestDetailsEditor : ITestDetailsEditor
    {
        public EventHandler TestSystemIdentityChanged;

        public bool Edit(ITestDetails testDetails, bool detailsFromDatabase)
        {
            string testSystemName = testDetails.TestSystemName;
            Version testSystemVersion = testDetails.TestSystemVersion;
            OperatingMode testSystemMode = testDetails.TestSystemMode;

            if (UpdateTestDetails(testDetails, detailsFromDatabase))
            {
                if (testSystemName != testDetails.TestSystemName || testSystemVersion != testDetails.TestSystemVersion || testSystemMode != testDetails.TestSystemMode)
                {
                    EventHandler handler = TestSystemIdentityChanged;
                    if (handler != null)
                        handler(this, EventArgs.Empty);
                    return true;
                }
                return false;
            }
            return false;
        }

        protected virtual bool UpdateTestDetails(ITestDetails testDetails, bool detailsFromDatabase)
        {
            using (PropertyGridEditor editor = new PropertyGridEditor(testDetails))
                return editor.ShowDialog() == DialogResult.OK;
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
