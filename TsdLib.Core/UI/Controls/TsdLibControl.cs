using System.Windows.Forms;

namespace TsdLib.UI.Controls
{
    public partial class TsdLibControl : UserControl
    {
        public TsdLibControl()
        {
            InitializeComponent();
        }

        public virtual void SetState(State state)
        {

        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
