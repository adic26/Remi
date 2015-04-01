using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using TsdLib.TestSystem;

//TODO: remove all controls - they'll be implemented in derived classes
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
            AddData((dynamic)dataContainer.Data);
        }

        private void AddData(object data)
        {
            Trace.WriteLine(string.Format("Unsupported data type received: {0}", data.GetType().Name));
            Trace.WriteLine(string.Format("String representation: {0}", data));
        }

        private void ViewBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnUIClosing(e);
        }

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
