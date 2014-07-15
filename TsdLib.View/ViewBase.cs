using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLib.View
{
    public partial class ViewBase : Form //TODO: make abstract
    {
        private readonly TextBoxTraceListener _textBoxTraceListener;

        public ViewBase()
        {
            InitializeComponent();
            _textBoxTraceListener = new TextBoxTraceListener(textBox_Status);
            Trace.Listeners.Add(_textBoxTraceListener);

            Disposed += (o, e) => Trace.Listeners.Remove(_textBoxTraceListener);
        }

        public void AddMeasurement(Measurement measurement)
        {
            dataGridView1.Rows.Add(measurement.MeasuredVal, measurement.LowerLim, measurement.UpperLim,
                measurement.Units, measurement.Result);
        }
    }
}
