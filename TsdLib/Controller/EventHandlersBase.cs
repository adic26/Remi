using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TsdLib.TestResults;
using TsdLib.View;

namespace TsdLib.Controller
{
    /// <summary>
    /// Encapsulates the <see cref="Controller"/> event handlers, allowing them to be executed in a secondary AppDomain.
    /// </summary>
    public class EventHandlersBase : MarshalByRefObject
    {
        protected readonly IView ViewProxy;

        /// <summary>
        /// Initialize a new 
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> to direct UI events to.</param>
        public EventHandlersBase(IView view)
        {
            ViewProxy = view;
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.InfoEventProxy"/>. Calls <see cref="IView.AddInformation"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the information was captured.</param>
        /// <param name="testInfo">The <see cref="TestInfo"/> that was captured.</param>
        protected internal virtual void InfoAdded(object sender, TestInfo testInfo)
        {
            ViewProxy.AddInformation(testInfo);
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.MeasurementEventProxy"/>. Calls <see cref="IView.AddMeasurement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the measurement was captured.</param>
        /// <param name="measurementBase">The <see cref="MeasurementBase"/> that was captured.</param>
        protected internal virtual void MeasurementAdded(object sender, MeasurementBase measurementBase)
        {
            ViewProxy.AddMeasurement(measurementBase);
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.DataEventProxy"/>. Calls <see cref="IView.AddData"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the measurement was captured.</param>
        /// <param name="data">The <see cref="Data"/> that was captured.</param>
        protected internal virtual void DataAdded(object sender, Data data)
        {
            ViewProxy.AddData(data);
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.TestCompleteEventProxy"/>. Saves the test results as xml and csv to the TsdLib.SpecialFolders location.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the test was performed.</param>
        /// <param name="testResults">The <see cref="TestResultCollection"/> that was captured.</param>
        protected internal virtual void TestComplete(object sender, TestResultCollection testResults)
        {
            DirectoryInfo resultsDirectory = SpecialFolders.GetResultsFolder(testResults.Details.TestSystemName);

            string xmlResultsFile = testResults.Save(resultsDirectory);
            string csvResultsFile = testResults.SaveCsv(resultsDirectory);

            Trace.WriteLine("Test sequence completed.");
            Trace.WriteLine("XML results saved to " + xmlResultsFile);
            Trace.WriteLine("CSV results saved to " + csvResultsFile);
        }

        /// <summary>
        /// Returns null to ensure that the remote object's lifetime is as long as the hosting AppDomain.
        /// </summary>
        /// <returns>Null, which corresponds to an unlimited lease time.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
