using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using TsdLib.Measurements;

namespace TsdLib.UI.Forms
{
    /// <summary>
    /// Base UI form containing controls and code common to all TsdLib applications.
    /// Protected controls may be overridden in application implementations but private controls cannot be modified by derived UI classes.
    /// </summary>
    public partial class ViewBase : Form, IView
    {
        public event EventHandler<CancelEventArgs> UIClosing;

        public virtual IConfigControl ConfigControl { get { return multiConfigControl; } }

        public virtual ITestInfoDisplayControl TestInfoDisplayControl { get { return testInfoDataGridViewControl; } }

        public virtual IMeasurementDisplayControl MeasurementDisplayControl { get { return measurementDataGridViewControl; } }

        public virtual ITestSequenceControl TestSequenceControl { get { return testSequenceControl; } }

        public virtual ITestDetailsControl TestDetailsControl { get { return testDetailsControl; } }

        public virtual ITraceListenerControl TraceListenerControl { get { return traceListenerTextBoxControl; } }

        /// <summary>
        /// Initializes a new instance of the base UI form.
        /// </summary>
        protected ViewBase()
        {
            InitializeComponent();
            Load += ViewBase_Load;
        }

        void ViewBase_Load(object sender, EventArgs e)
        {
            SetState(State.ReadyToTest);
            ConfigControl.ConfigSelectionChanged += Config_ConfigSelectionChanged;
            Config_ConfigSelectionChanged(ConfigControl, EventArgs.Empty);
        }

        void Config_ConfigSelectionChanged(object sender, EventArgs e)
        {
            IConfigControl control = sender as IConfigControl;
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
            foreach (ITsdLibControl control in Controls)
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
            EventHandler<CancelEventArgs> invoker = UIClosing;
            if (invoker != null)
                invoker(this, e);
        }

        /// <summary>
        /// Gets or sets the text displayed in the form title.
        /// </summary>
        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }
    }
}
