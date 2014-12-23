using System;
using System.Diagnostics;

namespace TsdLib.UI.Controls
{
    public partial class TraceListenerControlBase : TsdLibLabelledControl
    {
        private readonly DefaultTraceListener _defaultListener = new DefaultTraceListener();
        public virtual TraceListener Listener { get { return _defaultListener; } }

        public TraceListenerControlBase()
        {
            InitializeComponent();
            Text = "Status";
            HandleCreated += (sender, e) => Trace.Listeners.Add(Listener);
        }
    }
}
