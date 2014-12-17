using System;
using System.Collections.Generic;
using TsdLib.Configuration;

namespace TsdLib.Measurements
{
    public static class MeasurementsFactory
    {
        public static ITestResults CreateTestResults(ITestDetails testDetails, IEnumerable<MeasurementBase> measurements, string finalResult, DateTime dateStarted, DateTime dateCompleted, IEnumerable<TestInfo> info = null)
        {
            return new TestResultCollection(testDetails, measurements, new TestSummary(finalResult, dateStarted, dateCompleted), info);
        }
    }
}
