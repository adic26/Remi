using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsdLib.View
{
    public partial class ViewBase : Form, IView //TODO: make abstract
    {
        private readonly TextBoxTraceListener _textBoxTraceListener;

        public ViewBase()
        {
            InitializeComponent();
            _textBoxTraceListener = new TextBoxTraceListener(textBox_Status);
            Trace.Listeners.Add(_textBoxTraceListener);
            Disposed += (o, e) => Trace.Listeners.Remove(_textBoxTraceListener);
        }

        public void Launch()
        {
            ShowDialog();
        }

        public void AddMeasurement(Measurement measurement)
        {
            //measurementDataGridView1.Rows.Add(measurement.MeasuredVal, measurement.LowerLim, measurement.UpperLim, measurement.Units, measurement.Result);
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
