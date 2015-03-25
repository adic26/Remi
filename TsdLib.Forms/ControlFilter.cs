using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.Forms
{
    public class ControlFilter
    {
        private readonly Dictionary<Control, OperatingMode> _dictionary;
        private readonly ITestDetails _testDetails;

        public ControlFilter(Dictionary<Control, OperatingMode> dictionary, ITestDetails testDetails)
        {
            _dictionary = dictionary;
            _testDetails = testDetails;
            _testDetails.TestSystemIdentityChanged += testDetails_TestSystemIdentityChanged;
        }

        void testDetails_TestSystemIdentityChanged(object sender, string e)
        {
            foreach (var kvp in _dictionary)
            {
                Control ctrl = kvp.Key;
                OperatingMode mode = kvp.Value;

                ctrl.Enabled = mode >= _testDetails.TestSystemMode;
            }
        }

        public void Update(OperatingMode operatingMode)
        {
            foreach (var kvp in _dictionary)
            {
                Control ctrl = kvp.Key;
                OperatingMode mode = kvp.Value;

                ctrl.Enabled = mode < operatingMode;
            }
        }
    }
}
