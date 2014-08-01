using System;
using System.Diagnostics;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.View
{
    public partial class ViewBase<TStationConfig, TProductConfig> : Form, IView
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
    {
        private readonly TextBoxTraceListener _textBoxTraceListener;

        protected ViewBase()
        {
            InitializeComponent();
            _textBoxTraceListener = new TextBoxTraceListener(textBox_Status);
            Trace.Listeners.Add(_textBoxTraceListener);

            comboBox_StationConfig.DataSource = Config<TStationConfig>.Manager.GetConfigGroup().GetList();
            comboBox_ProductConfig.DataSource = Config<TProductConfig>.Manager.GetConfigGroup().GetList();

        }
        
        public void Launch()
        {
            Text = Application.ProductName + " v." + Application.ProductVersion;



            ShowDialog();

        }

        public virtual void SetState(State state)
        {
            switch (state)
            {
                case State.ReadyToTest:
                    button_ExecuteTestSequence.Enabled = true;
                    button_AbortTestSequence.Enabled = false;
                    break;
                case State.TestInProgress:
                    button_ExecuteTestSequence.Enabled = false;
                    button_AbortTestSequence.Enabled = true;
                    break;
            }
        }

        public void AddMeasurement(Measurement measurement)
        {
            measurementDataGridView.AddMeasurement(measurement);
        }


        public event EventHandler EditStationConfig;
        private void button_StationConfig_Click(object sender, EventArgs e)
        {
            if (EditStationConfig != null)
                EditStationConfig(this, new EventArgs());
        }

        public event EventHandler EditProductConfig;
        private void button_ProductConfig_Click(object sender, EventArgs e)
        {
            if (EditProductConfig != null)
                EditProductConfig(this, new EventArgs());
        }

        public event EventHandler EditTestConfig;
        private void button_TestConfig_Click(object sender, EventArgs e)
        {
            if (EditTestConfig != null)
                EditTestConfig(this, new EventArgs());
        }

        public event EventHandler<TestSequenceEventArgs> ExecuteTestSequence;
        private void button_ExecuteTestSequence_Click(object sender, EventArgs e)
        {
            if (ExecuteTestSequence != null)
                ExecuteTestSequence(this,
                    new TestSequenceEventArgs(comboBox_StationConfig.SelectedText, comboBox_ProductConfig.SelectedText));
        }

        public event EventHandler AbortTestSequence;
        private void button_AbortTestSequence_Click(object sender, EventArgs e)
        {
            if (AbortTestSequence != null)
                AbortTestSequence(this, new EventArgs());
        }
    }
}
