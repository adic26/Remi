using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace TsdLib.View
{
    /// <summary>
    /// Subscribe a text box to monitor the Trace and Debug output messages.
    /// </summary>
    public class TextBoxTraceListener : TraceListener
    {
        readonly TextBoxBase _textBox;
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
            _textBox = textBox;
            _textBoxAppend = textBox.AppendText;
            _buffer = new StringBuilder();

            _textBox.HandleCreated += _textBox_HandleCreated;
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
            if (_textBox.IsDisposed)
                return;

            if (!_textBox.IsHandleCreated)
            {
                _buffer.Append(message);
                return;
            }

            if (_textBox.InvokeRequired)
                _textBox.Invoke(_textBoxAppend, message);
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
}
