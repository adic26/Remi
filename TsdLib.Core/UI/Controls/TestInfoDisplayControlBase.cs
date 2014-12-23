using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    public partial class TestInfoDisplayControlBase : TsdLibLabelledControl
    {
        public TestInfoDisplayControlBase()
        {
            InitializeComponent();
            Text = "Test Info";
        }

        public virtual void AddTestInfo(TestInfo testInfo)
        {

        }
    }
}
