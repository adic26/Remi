using System;
using System.Diagnostics;
using System.IO;
using TsdLib.TestResults;
using TsdLib.View;

namespace TsdLib.Controller
{
    /// <summary>
    /// Encapsulates the <see cref="Controller"/> event handlers, allowing them to be executed in a secondary AppDomain.
    /// </summary>
    public class SequenceEventHandlersBase : MarshalByRefObject
    {
        /// <summary>
        /// Gets a reference to the View object, representing the user interface.
        /// </summary>
        protected IView ViewProxy { get; private set; }

        /// <summary>
        /// Initialize a new 
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> to direct UI events to.</param>
        public SequenceEventHandlersBase(IView view)
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
        /// <param name="eventArgs">The <see cref="TestResultCollection"/> that was captured, along with the information required to upload the results to a database.</param>
        protected internal virtual void TestComplete(object sender, TestCompleteEventArgs eventArgs)
        {
            DirectoryInfo resultsDirectory = SpecialFolders.GetResultsFolder(eventArgs.TestResults.Details.TestSystemName);

            string xmlResultsFile = eventArgs.TestResults.Save(resultsDirectory);
            string csvResultsFile = eventArgs.TestResults.SaveCsv(resultsDirectory);

            Trace.WriteLine("Test sequence completed.");
            Trace.WriteLine("XML results saved to " + xmlResultsFile);
            Trace.WriteLine("CSV results saved to " + csvResultsFile);
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.TestCancelledEventProxy"/>. Sets the UI state to <see cref="State.ReadyToTest"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the test was performed.</param>
        /// <param name="error">True if the test sequence was cancelled due to an error.</param>
        protected internal void TestCancelled(object sender, bool error)
        {
            ViewProxy.SetState(State.ReadyToTest);
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
