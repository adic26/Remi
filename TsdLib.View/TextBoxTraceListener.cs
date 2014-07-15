using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsdLib.View
{
    delegate void AppendTextDelegate(string text);

    public class TextBoxTraceListener : TraceListener
    {
        readonly TextBoxBase _textBox;
        readonly AppendTextDelegate _textBoxAppend;

        public TextBoxTraceListener(TextBoxBase textBox)
        {
            _textBox = textBox;
            _textBoxAppend = textBox.AppendText;
        }

        public override void Write(string message)
        {
            _textBox.Invoke(_textBoxAppend, message);
        }

        public override void WriteLine(string message)
        {
            _textBox.Invoke(_textBoxAppend, message + Environment.NewLine);
        }
    }
}
