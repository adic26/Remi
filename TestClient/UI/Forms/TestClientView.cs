﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using TsdLib;
using TsdLib.Measurements;
using TsdLib.TestSystem;
using TsdLib.TestSystem.Observer;
using TsdLib.UI;

namespace TestClient.UI.Forms
{
    /// <summary>
    /// Base UI form containing controls and code common to all TsdLib applications.
    /// Protected controls may be overridden in application implementations but private controls cannot be modified by derived UI classes.
    /// </summary>
    public partial class TestClientView : Form, IView
    {
        public virtual ITestCaseControl TestCaseControl { get { return testCasesMenuItem; } }

        public virtual IConfigControl ConfigControl { get { return multiConfigControl; } }

        public virtual ITestInfoDisplayControl TestInfoDisplayControl { get { return testInfoDataGridViewControl; } }

        public virtual IMeasurementDisplayControl MeasurementDisplayControl { get { return measurementDataGridViewControl; } }

        public virtual ITestSequenceControl TestSequenceControl { get { return testSequenceControl; } }

        public virtual ITestDetailsControl TestDetailsControl { get { return testDetailsControl; } }

        public virtual ITraceListenerControl TraceListenerControl { get { return traceListenerTextBoxControl; } }

        public virtual IProgressControl ProgressControl { get { return progressControl; } }

        private SynchronizationContext context;
        /// <summary>
        /// Initializes a new instance of the UI form.
        /// </summary>
        public TestClientView()
        {
            InitializeComponent();
            context = SynchronizationContext.Current;
        }

        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        public virtual void SetState(State state)
        {
            foreach (object control in Controls)
            {
                ITsdLibControl tsdCtrl = control as ITsdLibControl;
                if (tsdCtrl != null)
                    tsdCtrl.SetState(state);
            }
        }

        /// <summary>
        /// Add an <see cref="IMeasurement"/> to the DataGridView.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        public virtual void AddMeasurement(IMeasurement measurement)
        {
            measurementDataGridViewControl.AddMeasurement(measurement);
        }

        /// <summary>
        /// Add a general data object to the UI. Override to perform specific operations based on the type of the internal data value.
        /// </summary>
        /// <param name="data">Data to add.</param>
        public void AddData(object data)
        {
            DataContainer transientData = data as DataContainer;
            if (transientData != null)
                context.Post(s => addData((dynamic)transientData.Data), null);
            else
                context.Post(s => addData((dynamic)data), null);
        }

        private void addData(object data)
        {
            Trace.WriteLine(string.Format("Unsupported data type received: {0}", data.GetType().Name));
            Trace.WriteLine(string.Format("String representation: {0}", data));
        }

        private void addData(Tuple<int, string> data)
        {
            MessageBox.Show("Int = " + data.Item1 + " String = " + data.Item2);

        }

        private void addData(int data)
        {
            MessageBox.Show("Int = " + data);
        }

        private void ViewBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHandler<CancelEventArgs> invoker = UIClosing;
            if (invoker != null)
                invoker(this, e);
        }

        /// <summary>
        /// Sets the text displayed in the title section of the UI.
        /// </summary>
        /// <param name="title">Text to display.</param>
        public void SetTitle(string title)
        {
            Text = title;
        }

        /// <summary>
        /// Event fired when the UI is about to close.
        /// </summary>
        public event EventHandler<CancelEventArgs> UIClosing;
    }
}
