using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsdLib.View
{
    public abstract partial class ViewBase : Form, IView //TODO: make abstract
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

        public event EventHandler Configure;
        private void button_Settings_Click(object sender, EventArgs e)
        {
            if (Configure != null)
                Configure(this, new EventArgs());
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
