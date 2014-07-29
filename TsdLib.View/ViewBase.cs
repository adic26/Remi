using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsdLib.View
{
    public partial class ViewBase : Form, IView
    {
        private readonly TextBoxTraceListener _textBoxTraceListener;

        protected ViewBase()
        {
            InitializeComponent();
            Text = Application.ProductName + " v." + Application.ProductVersion;
            _textBoxTraceListener = new TextBoxTraceListener(textBox_Status);
            Trace.Listeners.Add(_textBoxTraceListener);
            Disposed += (o, e) => Trace.Listeners.Remove(_textBoxTraceListener);
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public void Launch()
        {
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
            measurementDataGridView1.AddMeasurement(measurement);
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

        public event EventHandler ExecuteTestSequence;
        private void button_ExecuteTestSequence_Click(object sender, EventArgs e)
        {
            if (ExecuteTestSequence != null)
                ExecuteTestSequence(this, new EventArgs());
        }

        public event EventHandler AbortTestSequence;
        private void button_AbortTestSequence_Click(object sender, EventArgs e)
        {
            if (AbortTestSequence != null)
                AbortTestSequence(this, new EventArgs());
        }
    }
}
