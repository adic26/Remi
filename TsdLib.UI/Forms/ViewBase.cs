using System;
using System.ComponentModel;
using System.Windows.Forms;
using TsdLib.TestSystem;

namespace TsdLib.UI.Forms
{
    /// <summary>
    /// Base UI form containing controls and code common to all TsdLib applications.
    /// Protected controls may be overridden in application implementations but private controls cannot be modified by derived UI classes.
    /// </summary>
    public partial class ViewBase : Form, IView
    {
        public virtual ITestCaseControl TestCaseControl { get { return null; } }

        public virtual IConfigControl ConfigControl { get { return null; } }

        public virtual ITestInfoDisplayControl TestInfoDisplayControl { get { return null; } }

        public virtual IMeasurementDisplayControl MeasurementDisplayControl { get { return null; } }

        public virtual ITestSequenceControl TestSequenceControl { get { return null; } }

        public virtual ITestDetailsControl TestDetailsControl { get { return null; } }

        public virtual ITraceListenerControl TraceListenerControl { get { return null; } }

        public virtual IProgressControl ProgressControl { get { return null; } }

        public virtual IDataVisualizerContainerControl  DataVisualizer { get { return null; } }

        /// <summary>
        /// Initializes a new instance of the base UI form.
        /// </summary>
        protected ViewBase()
        {
            InitializeComponent();

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
        /// Add a general data object to the UI. Override to perform specific operations based on the type of the internal data value.
        /// </summary>
        /// <param name="dataContainer">Data to add.</param>
        public virtual void AddArbitraryData(DataContainer dataContainer)
        {
            DataVisualizer.Add((dynamic)dataContainer.Data);
            AddData((dynamic)dataContainer.Data);
        }

        /// <summary>
        /// Fallback method for the dynamic method overload resolution.
        /// </summary>
        /// <param name="data">Data that did not match an overloaded method parameter.</param>
        public void AddData(object data)
        {

        }

        private void ViewBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnUIClosing(e);
        }

        /// <summary>
        /// Fire the UIClosing event.
        /// </summary>
        /// <param name="e">Mechanism allowing cancellation.</param>
        protected virtual void OnUIClosing(FormClosingEventArgs e)
        {
            EventHandler<CancelEventArgs> invoker = UIClosing;
            if (invoker != null)
                invoker(this, e);
        }

        /// <summary>
        /// Sets the text displayed in the title section of the UI.
        /// </summary>
        /// <param name="title">Text to display.</param>
        public virtual void SetTitle(string title)
        {
            Text = title;
        }

        /// <summary>
        /// Event fired when the UI is about to close.
        /// </summary>
        public event EventHandler<CancelEventArgs> UIClosing;
    }
}
