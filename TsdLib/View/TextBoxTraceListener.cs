using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace TsdLib.View
{
    delegate void AppendTextDelegate(string text);

    public class TextBoxTraceListener : TraceListener
    {
        readonly TextBoxBase _textBox;
        readonly AppendTextDelegate _textBoxAppend;

        private readonly StringBuilder _buffer;

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

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
