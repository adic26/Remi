using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using TsdLib.TestResults;

namespace TsdLib.View
{
    /// <summary>
    /// Base UI form containing controls and code common to all TsdLib applications.
    /// Protected controls may be overridden in application implementations but private controls cannot be modified by derived UI classes.
    /// </summary>
    public partial class ViewBase : Form, IView
    {
        /// <summary>
        /// Gets the TraceListener used for writing trace and debug information to the status text box.
        /// </summary>
        public TraceListener Listener { get; private set; }

        #region Config Data Bindings

        /// <summary>
        /// Sets the list of available Station Config instances.
        /// </summary>
        public IList StationConfigList { set { comboBox_StationConfig.DataSource = value; } }
        /// <summary>
        /// Sets the list of available Product Config instances.
        /// </summary>
        public IList ProductConfigList { set { comboBox_ProductConfig.DataSource = value; } }
        /// <summary>
        /// Sets the list of available Test Config instances.
        /// </summary>
        public IList TestConfigList { set { comboBox_TestConfig.DataSource = value; } }
        /// <summary>
        /// Sets the list of available Sequence Config instances.
        /// </summary>
        public IList SequenceConfigList { set { comboBox_SequenceConfig.DataSource = value; } }

        #endregion

        /// <summary>
        /// Initializes a new instance of the base UI form.
        /// </summary>
        protected ViewBase()
        {
            InitializeComponent();

            Listener = new TextBoxTraceListener(textBox_Status);
            Trace.Listeners.Add(Listener);

            Load += (sender, args) => SetState(State.ReadyToTest);
        }

        #region Display Methods

        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        public virtual void SetState(State state)
        {
            switch (state)
            {
                case State.ReadyToTest:
                    button_ExecuteTestSequence.Enabled = true;
                    button_AbortTestSequence.Enabled = false;
                    break;
                case State.TestInProgress:
                    measurementDataGridView.Rows.Clear();
                    button_ExecuteTestSequence.Enabled = false;
                    button_AbortTestSequence.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Add a new measurement to the data grid view.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public void AddMeasurement(Measurement measurement)
        {
            measurementDataGridView.AddMeasurement(measurement);
        }

        #endregion

        #region UI Event Handlers

        /// <summary>
        /// Event fired when requesting to modify the Station Config.
        /// </summary>
        public event EventHandler EditStationConfig;
        private void button_StationConfig_Click(object sender, EventArgs e)
        {
            if (EditStationConfig != null)
                EditStationConfig(this, new EventArgs());
        }

        /// <summary>
        /// Event fired when requesting to modify the Product Config.
        /// </summary>
        public event EventHandler EditProductConfig;
        private void button_ProductConfig_Click(object sender, EventArgs e)
        {
            if (EditProductConfig != null)
                EditProductConfig(this, new EventArgs());
        }

        /// <summary>
        /// Event fired when requesting to modify the Test Config.
        /// </summary>
        public event EventHandler EditTestConfig;
        private void button_TestConfig_Click(object sender, EventArgs e)
        {
            if (EditTestConfig != null)
                EditTestConfig(this, new EventArgs());
        }

        /// <summary>
        /// Event fired when requesting to modify the Sequence Config.
        /// </summary>
        public event EventHandler EditSequenceConfig;
        private void button_SequenceConfig_Click(object sender, EventArgs e)
        {
            if (EditSequenceConfig != null)
                EditSequenceConfig(this, new EventArgs());
        }
        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        public event EventHandler<TestSequenceEventArgs> ExecuteTestSequence;
        private void button_ExecuteTestSequence_Click(object sender, EventArgs e)
        {
            if (ExecuteTestSequence != null)
                ExecuteTestSequence(this,
                    new TestSequenceEventArgs(comboBox_StationConfig.SelectedItem, comboBox_ProductConfig.SelectedItem, comboBox_TestConfig.SelectedItem, comboBox_SequenceConfig.SelectedItem));
        }

        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        public event EventHandler AbortTestSequence;
        private void button_AbortTestSequence_Click(object sender, EventArgs e)
        {
            if (AbortTestSequence != null)
                AbortTestSequence(this, new EventArgs());
        }

        #endregion
    }
}
