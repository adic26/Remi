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

        public ICancellationManager TestSequence { get; private set;}

        /// <summary>
        /// Initialize a new 
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> that will be used to handle UI events.</param>
        /// <param name="testSequenceCancellationManager">Reference to the test sequence cancellation manager.</param>
        public ControllerProxy(IView view, ICancellationManager testSequenceCancellationManager)
        {
            ViewProxy = view;
            TestSequence = testSequenceCancellationManager;
        }

        /// <summary>
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase.InfoEventProxy"/>. Calls <see cref="TsdLib.UI.ITestInfoDisplayControl.AddTestInfo"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase"/> where the information was captured.</param>
        /// <param name="testInfo">The <see cref="ITestInfo"/> that was captured.</param>
        public void InfoAdded(object sender, ITestInfo testInfo)
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
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase.MeasurementEventProxy"/>. Calls <see cref="TsdLib.UI.IMeasurementDisplayControl.AddMeasurement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase"/> where the measurement was captured.</param>
        /// <param name="measurementBase">The <see cref="IMeasurement"/> that was captured.</param>
        public void MeasurementAdded(object sender, IMeasurement measurementBase)
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
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase.MeasurementEventProxy"/>. Calls <see cref="TsdLib.UI.IMeasurementDisplayControl.AddMeasurement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase"/> where the measurement was captured.</param>
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
        /// Default handler for the <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase.DataEventProxy"/>. Calls <see cref="IView.AddData"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TsdLib.TestSystem.TestSequence.TestSequenceBase"/> where the measurement was captured.</param>
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
