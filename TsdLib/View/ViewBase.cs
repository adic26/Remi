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

        #region Config ComboBox Data Bindings

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
                    panel_Controls.Enabled = true;
                    break;
                case State.TestInProgress:
                    textBox_Status.Clear();
                    dataGridView_Information.Rows.Clear();
                    measurementDataGridView.Rows.Clear();
                    button_ExecuteTestSequence.Enabled = false;
                    button_AbortTestSequence.Enabled = true;
                    panel_Controls.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// Add a <see cref="TsdLib.TestResults.TestInfo"/> to the DataGridView.
        /// </summary>
        /// <param name="info">TestInfo object to add.</param>
        public virtual void AddInformation(TestInfo info)
        {
            dataGridView_Information.Rows.Add(info.Name, info.Value);
        }

        /// <summary>
        /// Add a <see cref="TsdLib.TestResults.MeasurementBase"/> to the DataGridView.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public virtual void AddMeasurement(MeasurementBase measurement)
        {
            measurementDataGridView.AddMeasurement(measurement);
        }

        /// <summary>
        /// Add a general data object to the UI. Override to perform specific operations based on the type of the internal data value.
        /// </summary>
        /// <param name="data">Data to add.</param>
        public virtual void AddData(Data data)
        {
            Trace.WriteLine("Received data: " + data);
        }

        #endregion

        #region UI Event Handlers

        /// <summary>
        /// Event fired when requesting to modify the test system configuration.
        /// </summary>
        public event EventHandler ViewEditConfiguration;
        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            if (ViewEditConfiguration != null)
                ViewEditConfiguration(this, new EventArgs());
        }

        /// <summary>
        /// Event fired when requesting to edit the test details.
        /// </summary>
        public event EventHandler<bool> EditTestDetails;
        private void button_EditTestDetails_Click(object sender, EventArgs e)
        {
            bool fromDb = checkBox_GetDetailsFromDatabase.Checked;
            checkBox_GetDetailsFromDatabase.Checked = false;
            if (EditTestDetails != null)
                EditTestDetails(this, fromDb);
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
