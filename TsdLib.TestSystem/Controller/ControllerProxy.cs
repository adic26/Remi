using System;
using TsdLib.Measurements;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Encapsulates the <see cref="Controller"/> event handlers, allowing them to be executed in a remote AppDomain.
    /// These event handlers will be executed on the UI thread.
    /// </summary>
    //[Serializable]
    internal class ControllerProxy : MarshalByRefObject
    {
        /// <summary>
        /// Gets a reference to the View object, representing the user interface.
        /// </summary>
        public IView ViewProxy { get; private set; }

        public ICancellable TestSequence { get; private set;}

        /// <summary>
        /// Initialize a new 
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> that will be used to handle UI events.</param>
        /// <param name="testSequence">Reference to the test sequence.</param>
        public ControllerProxy(IView view, ICancellable testSequence)
        {
            ViewProxy = view;
            TestSequence = testSequence;
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}.InfoEventProxy"/>. Calls <see cref="TsdLib.UI.ITestInfoDisplayControl.AddTestInfo"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the information was captured.</param>
        /// <param name="testInfo">The <see cref="TestInfo"/> that was captured.</param>
        public void InfoAdded(object sender, TestInfo testInfo)
        {
            try
            {
                if (ViewProxy.TestInfoDisplayControl != null)
                    ViewProxy.TestInfoDisplayControl.AddTestInfo(testInfo);
            }
            catch (Exception ex)
            {
                TestSequence.Abort(ex);
            }
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}.MeasurementEventProxy"/>. Calls <see cref="TsdLib.UI.IMeasurementDisplayControl.AddMeasurement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the measurement was captured.</param>
        /// <param name="measurementBase">The <see cref="MeasurementBase"/> that was captured.</param>
        public void MeasurementAdded(object sender, MeasurementBase measurementBase)
        {
            try
            {
                if (ViewProxy.MeasurementDisplayControl != null)
                    ViewProxy.MeasurementDisplayControl.AddMeasurement(measurementBase);
            }
            catch (Exception ex)
            {
                TestSequence.Abort(ex);
            }
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}.MeasurementEventProxy"/>. Calls <see cref="TsdLib.UI.IMeasurementDisplayControl.AddMeasurement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the measurement was captured.</param>
        /// <param name="progress">The progress represented as a percentage.</param>
        public void ProgressUpdated(object sender, Tuple<int, int> progress)
        {
            try
            {
                if (ViewProxy.ProgressControl != null)
                    ViewProxy.ProgressControl.UpdateProgress(progress.Item1, progress.Item2);
            }
            catch (Exception ex)
            {
                TestSequence.Abort(ex);
            }
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}.DataEventProxy"/>. Calls <see cref="IView.AddData"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the measurement was captured.</param>
        /// <param name="data">The data that was captured.</param>
        public void DataAdded(object sender, object data)
        {
            ViewProxy.AddData(data);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
