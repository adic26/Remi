namespace TsdLib.UI.Controls.Base
{
    public partial class TsdLibLabelledControl : TsdLibControl
    {
        public override sealed string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public TsdLibLabelledControl()
        {
            InitializeComponent();
            Text = GetType().Name + " - set the Text property in this control's constructor to set this text.";
        }
    }
}
