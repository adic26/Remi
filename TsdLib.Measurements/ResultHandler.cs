using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Configuration;

namespace TsdLib.Measurements
{
    public class ResultHandler : IResultHandler
    {
        private readonly List<Task> _loggingTasks;
        private readonly ITestDetails _testDetails;

        public ResultHandler(ITestDetails testDetails)
        {
            _loggingTasks = new List<Task>();
            LoggingTasks = new ReadOnlyCollection<Task>(_loggingTasks);
            _testDetails = testDetails;
        }

        /// <summary>
        /// Gets a list of active tasks responsible for logging test results.
        /// </summary>
        protected ReadOnlyCollection<Task> LoggingTasks { get; private set; }

        public void SaveResults(ITestInfo[] testInfo, IMeasurement[] measurements, DateTime start, DateTime end, bool publish)
        {
            ITestSummary testSummary = new TestSummary(CalculateOverallTestResult(measurements).ToString(), start, end);

            ITestResults testResults = new TestResultCollection(_testDetails, measurements, testSummary, testInfo);

            _loggingTasks.Add(Task.Run(() =>
            {
                ITestResults localTestResults = testResults; //Create a local reference in case they are overwritten by the next sequence before the logging is complete.
                if (Thread.CurrentThread.Name != null)
                    Thread.CurrentThread.Name = "Result Handler Thread";
                Thread.CurrentThread.IsBackground = false;
                SaveResults(localTestResults);
                if (publish)
                    PublishResults(localTestResults);
            }).ContinueWith(t =>
            {
                _loggingTasks.Remove(t);
                if (t.IsFaulted && t.Exception != null)
                    Trace.WriteLine("Failed to log test results!" + Environment.NewLine + string.Join(Environment.NewLine, t.Exception.Flatten().InnerExceptions));
            })
            );
        }

        protected virtual MeasurementResult CalculateOverallTestResult(IMeasurement[] measurements)
        {
            if (!measurements.Any() || measurements.All(m => m.Result == MeasurementResult.Undefined))
                return MeasurementResult.Undefined;

            return measurements.All(m => m.Result == MeasurementResult.Pass) ? MeasurementResult.Pass : MeasurementResult.Fail;
        }

        /// <summary>
        /// Saves the specified <see cref="ITestResults"/> as xml and csv to the TsdLib.SpecialFolders location.
        /// </summary>
        /// <param name="results">The <see cref="ITestResults"/> that was captured by the test sequence.</param>
        protected virtual void SaveResults(ITestResults results)
        {
            results.SaveXml();
            string csvResultsFile = results.SaveCsv();

            Trace.WriteLine("CSV results saved to " + csvResultsFile);
        }

        /// <summary>
        /// Override to published the specified <see cref="ITestResults"/> to a database or user-defined location.
        /// </summary>
        /// <param name="results">The <see cref="ITestResults"/> that was captured by the test sequence.</param>
        protected virtual void PublishResults(ITestResults results)
        {

        }
    }
}
