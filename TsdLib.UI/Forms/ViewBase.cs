using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TsdLib.Measurements;
using TsdLib.UI.Controls;

namespace TsdLib.UI.Forms
{
    /// <summary>
    /// Base UI form containing controls and code common to all TsdLib applications.
    /// Protected controls may be overridden in application implementations but private controls cannot be modified by derived UI classes.
    /// </summary>
    public partial class ViewBase : Form, IView
    {
        public ConfigControlBase ConfigControl { get { return multiConfigControl; } }

        public TestInfoDisplayControlBase TestInfoDisplayControl { get { return testInfoDataGridViewControl; } }

        public MeasurementDisplayControlBase MeasurementDisplayControl { get { return measurementDataGridViewControl; } }

        public TestSequenceControlBase TestSequenceControl { get { return testSequenceControl; } }

        public TestDetailsControlBase TestDetailsControl { get { return testDetailsControl; } }

        public TraceListenerControlBase TraceListenerControl { get { return traceListenerTextBoxControl; } }

        /// <summary>
        /// Initializes a new instance of the base UI form.
        /// </summary>
        protected ViewBase()
        {
            InitializeComponent();

            //TODO: use data binding to bind TestSequence.SelectedStationConfig = Config.SelectedStationConfig in the constructor - but have to expose Config.SelectedStationConfig as a BindingSource
            ConfigControl.ConfigSelectionChanged += Config_ConfigSelectionChanged;


            Load += (sender, args) => SetState(State.ReadyToTest);
        }

        void Config_ConfigSelectionChanged(object sender, IEnumerable<Configuration.IConfigItem> configItems)
        {
            ConfigControlBase control = sender as ConfigControlBase;
            if (control != null)
            {
                TestSequenceControl.SelectedStationConfig = control.SelectedStationConfig;
                TestSequenceControl.SelectedProductConfig = control.SelectedProductConfig;
                TestSequenceControl.SelectedTestConfig = control.SelectedTestConfig;
                TestSequenceControl.SelectedSequenceConfig = control.SelectedSequenceConfig;
            }
        }

        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        public virtual void SetState(State state)
        {
            foreach (TsdLibControl control in Controls)
                control.SetState(state);
        }

        /// <summary>
        /// Add a <see cref="MeasurementBase"/> to the DataGridView.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public virtual void AddMeasurement(MeasurementBase measurement)
        {
            measurementDataGridViewControl.AddMeasurement(measurement);
        }

        /// <summary>
        /// Add a general data object to the UI. Override to perform specific operations based on the type of the internal data value.
        /// </summary>
        /// <param name="data">Data to add.</param>
        public virtual void AddData(object data)
        {
            Trace.WriteLine("Received data: " + data);
        }

        private void ViewBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            TestSequenceControl.OnAbort(e);
        }
    }
}
