using System.Diagnostics;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to add a <see cref="TraceListener"/> to the UI.
    /// </summary>
    public partial class TraceListenerControlBase : TsdLibLabelledControl, ITraceListenerControl
    {
        private readonly DefaultTraceListener _defaultListener = new DefaultTraceListener();
        /// <summary>
        /// Override with a trace listener implementation.
        /// </summary>
        public virtual TraceListener Listener { get { return _defaultListener; } }



        /// <summary>
        /// Initialize a the control and add the <see cref="Listener"/> the to <see cref="Trace.Listeners"/> collection.
        /// </summary>
        public TraceListenerControlBase()
        {
            InitializeComponent();
            Text = "Status";
            HandleCreated += (sender, e) => Trace.Listeners.Add(Listener);
        }

        public override void SetState(State state)
        {
            if (state.HasFlag(State.TestInProgress))
                Clear();
        }

        /// <summary>
        /// Clear the trace output from the UI.
        /// </summary>
        public virtual void Clear()
        {
            
        }
    }
}
