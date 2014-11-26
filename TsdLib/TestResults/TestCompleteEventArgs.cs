using System;
using TsdLib.TestSequence;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Provides test results and publishing information to handlers of the <see cref="TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}.TestCompleteEventProxy"/> event
    /// </summary>
    public class TestCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="TestResultCollection"/> containing the test results.
        /// </summary>
        public TestResultCollection TestResults { get; private set; }
        /// <summary>
        /// True if test results should be published to a database.
        /// </summary>
        public bool PublishResults { get; private set; }

        /// <summary>
        /// Initialize a new testCompleteEventArgs.
        /// </summary>
        /// <param name="testResults">The <see cref="TestResultCollection"/> containing the test results.</param>
        /// <param name="publishResults">True if test results should be published to a database.</param>
        public TestCompleteEventArgs(TestResultCollection testResults, bool publishResults)
        {
            TestResults = testResults;
            PublishResults = publishResults;
        }
    }
}