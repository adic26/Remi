using System.Collections.Generic;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.Forms
{
    /// <summary>
    /// Provides a mechanism to enable or disable controls based on the permission levels associated with <see cref="OperatingMode"/>
    /// </summary>
    public class ControlFilter
    {
        private readonly Dictionary<Control, OperatingMode> _dictionary;
        private readonly ITestDetails _testDetails;

        public ControlFilter(Dictionary<Control, OperatingMode> dictionary)
        {
            _dictionary = dictionary;
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
