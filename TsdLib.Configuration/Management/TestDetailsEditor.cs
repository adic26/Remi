using System.Windows.Forms;
using TsdLib.Configuration.Details;
using TsdLib.Forms;

namespace TsdLib.Configuration.Management
{
    public class TestDetailsEditor : ITestDetailsEditor
    {
        public void Edit(ITestDetails testDetails, bool detailsFromDatabase)
        {
            IdentityManager.InitializeTestDetails(testDetails);
            UpdateTestDetails(testDetails, detailsFromDatabase);
            IdentityManager.FireIfModified();
        }

        protected virtual bool UpdateTestDetails(ITestDetails testDetails, bool detailsFromDatabase)
        {
            using (PropertyGridEditor editor = new PropertyGridEditor(testDetails))
                return editor.ShowDialog() == DialogResult.OK;
        }

        private ITestSystemIdentityManager _identityManager;
        public ITestSystemIdentityManager IdentityManager
        {
            get { return _identityManager ?? (_identityManager = CreateIdentityManager()); }
        }

        protected virtual ITestSystemIdentityManager CreateIdentityManager()
        {
            return new TestSystemIdentityManager();
        }
    }
}
