using System;
using TsdLib.Measurements;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Encapsulates the <see cref="Controller"/> event handlers, allowing them to be executed in a remote AppDomain.
    /// These event handlers will be executed on the UI thread.
    /// </summary>
    internal class ControllerProxy : MarshalByRefObject
    {
        /// <summary>
        /// Gets a reference to the View object, representing the user interface.
        /// </summary>
        public IView ViewProxy { get; private set; }

        /// <summary>
        /// Initialize a new 
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> that will be used to handle UI events.</param>
        public ControllerProxy(IView view)
        {
            ViewProxy = view;
        }

        /// <summary>
        /// Default handler for the <see cref="TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.InfoEventProxy"/>. Calls <see cref="IView.AddInformation"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the information was captured.</param>
        /// <param name="testInfo">The <see cref="TestInfo"/> that was captured.</param>
        public void InfoAdded(object sender, TestInfo testInfo)
        {
            ViewProxy.AddInformation(testInfo);
        }

        /// <summary>
        /// Default handler for the <see cref="TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.MeasurementEventProxy"/>. Calls <see cref="IView.AddMeasurement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}"/> where the measurement was captured.</param>
        /// <param name="measurementBase">The <see cref="MeasurementBase"/> that was captured.</param>
        public void MeasurementAdded(object sender, MeasurementBase measurementBase)
        {
            ViewProxy.AddMeasurement(measurementBase);
        }

        /// <summary>
        /// Default handler for the <see cref="TestSequence.TestSequenceBase{TStationConfig, TProductConfig, TTestConfig}.DataEventProxy"/>. Calls <see cref="IView.AddData"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TestSequence.TestSequenceBase{TStationConfig,TProductConfig,TTestConfig}"/> where the measurement was captured.</param>
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
