using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLib.UI.Controls
{
    /// <summary>
    /// Contains functionality to display tracing information in a text box.
    /// </summary>
    public partial class TraceListenerTextBoxControl : UserControl, ITraceListenerControl
    {
        private readonly TextBoxTraceListener _listener;
        public TraceListener Listener
        {
            get { return _listener; }
        }

        /// <summary>
        /// Initialize a new <see cref="TraceListenerTextBoxControl"/>
        /// </summary>
        public TraceListenerTextBoxControl()
        {
            InitializeComponent();
            _listener = new TextBoxTraceListener(textBox);
        }

        /// <summary>
        /// Clear the trace output from the trace listener's text box.
        /// </summary>
        public void Clear()
        {
            _listener.TextBox.Clear();
        }

        public virtual void SetState(State state)
        {
            if (state.HasFlag(State.TestStarting))
                Clear();
        }

        #region nested TraceListener implementation

        /// <summary>
        /// Subscribe a text box to monitor the Trace and Debug output messages.
        /// </summary>
        private class TextBoxTraceListener : TraceListener
        {
            public TextBoxBase TextBox { get; private set; }
            private readonly BlockingCollection<string> _queue = new BlockingCollection<string>();
            private readonly TaskScheduler _uiTaskScheduler;

            /// <summary>
            /// Initializes a new instance of the TextBoxTraceListener by subscribing a text box to monitor the Trace and Debug output messages.
            /// </summary>
            /// <param name="textBox">Text box to subscribe to trace messages.</param>
            public TextBoxTraceListener(TextBoxBase textBox)
            {
                TextBox = textBox;
                TextBox.HandleCreated += StartQueue;
                _uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }

            private void StartQueue(object sender, EventArgs e)
            {
                Task.Run(() =>
                {
                    while (!_queue.IsCompleted)
                    {
                        try
                        {
                            string data = _queue.Take();
                            Task.Factory.StartNew(
                                () => TextBox.AppendText(data),
                                CancellationToken.None,
                                TaskCreationOptions.None,
                                _uiTaskScheduler);
                        }
                        catch (InvalidOperationException) { }
                    }
                });
            }


            /// <summary>
            /// Write a message to the text box.
            /// </summary>
            /// <param name="message">Message to write.</param>
            public override void Write(string message)
            {
                if (TextBox.IsDisposed)
                    return;

                _queue.Add(message);
            }
            
            /// <summary>
            /// Write a message to the text box, terminated with a NewLine character.
            /// </summary>
            /// <param name="message">Message to write.</param>
            public override void WriteLine(string message)
            {
                Write(message + Environment.NewLine);
            }

            protected override void Dispose(bool disposing)
            {
                _queue.CompleteAdding();
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
