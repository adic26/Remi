using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class TraceListenerTextBoxControl : TraceListenerControlBase
    {
        private readonly TextBoxTraceListener _listener;
        public override TraceListener Listener
        {
            get { return _listener; }
        }

        public TraceListenerTextBoxControl()
        {
            InitializeComponent();
            _listener = new TextBoxTraceListener(textBox);
        }

        /// <summary>
        /// Clear the trace output from the trace listener's text box.
        /// </summary>
        public override void Clear()
        {
            _listener.TextBox.Clear();
        }

        #region nested TraceListener implementation

        /// <summary>
        /// Subscribe a text box to monitor the Trace and Debug output messages.
        /// </summary>
        private class TextBoxTraceListener : TraceListener
        {
            public readonly TextBoxBase TextBox;
            readonly Action<string> _textBoxAppend;

            /// <summary>
            /// Returns null to ensure that the remote object's lifetime is as long as the hosting AppDomain.
            /// </summary>
            /// <returns>Null, which corresponds to an unlimited lease time.</returns>
            public override object InitializeLifetimeService()
            {
                return null;
            }

            private readonly StringBuilder _buffer;

            /// <summary>
            /// Initializes a new instance of the TextBoxTraceListener by subscribing a text box to monitor the Trace and Debug output messages.
            /// </summary>
            /// <param name="textBox">Text box to subscribe to trace messages.</param>
            public TextBoxTraceListener(TextBoxBase textBox)
            {
                TextBox = textBox;
                _textBoxAppend = textBox.AppendText;
                _buffer = new StringBuilder();

                TextBox.HandleCreated += _textBox_HandleCreated;
            }

            void _textBox_HandleCreated(object sender, EventArgs e)
            {
                if (_buffer.Length > 0)
                    Write(_buffer.ToString());
                _buffer.Clear();
            }

            /// <summary>
            /// Write a message to the text box.
            /// </summary>
            /// <param name="message">Message to write.</param>
            public override void Write(string message)
            {
                if (TextBox.IsDisposed)
                    return;

                if (!TextBox.IsHandleCreated)
                {
                    _buffer.Append(message);
                    return;
                }

                if (TextBox.InvokeRequired)
                    TextBox.Invoke(_textBoxAppend, message);
                else
                    _textBoxAppend(message);
            }

            /// <summary>
            /// Write a message to the text box, terminated with a NewLine character.
            /// </summary>
            /// <param name="message">Message to write.</param>
            public override void WriteLine(string message)
            {
                Write(message + Environment.NewLine);
            }
        }

        #endregion
    }
}
