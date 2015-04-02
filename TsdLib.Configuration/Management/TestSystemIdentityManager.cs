using System;
using TsdLib.Configuration.Details;

namespace TsdLib.Configuration.Management
{
    public class TestSystemIdentityManager : ITestSystemIdentityManager
    {
        public event EventHandler TestSystemIdentityChanged;

        private ITestDetails _testDetails;

        private string _testSystemName;
        private Version _testSystemVersion;
        private OperatingMode _testSystemMode;

        public void InitializeTestDetails(ITestDetails testDetails)
        {
            _testDetails = testDetails;
            _testSystemName = testDetails.TestSystemName;
            _testSystemVersion = testDetails.TestSystemVersion;
            _testSystemMode = testDetails.TestSystemMode;
        }

        public bool FireIfModified()
        {
            if (_testSystemName != _testDetails.TestSystemName || _testSystemVersion != _testDetails.TestSystemVersion || _testSystemMode != _testDetails.TestSystemMode)
            {
                EventHandler handler = TestSystemIdentityChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
