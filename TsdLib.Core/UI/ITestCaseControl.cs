using System;
using System.Collections.Generic;
using TsdLib.Configuration;

namespace TsdLib.UI
{
    public interface ITestCaseControl
    {
        event EventHandler<string> TestCaseSelected;
        event EventHandler TestCaseSaved;

        void DisplayTestCases(IEnumerable<ITestCase> testCases);
    }
}