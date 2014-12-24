using System.Windows.Forms;

namespace TsdLib.UI.Controls.Base
{
    //TODO: should this be a control? Maybe just MarshalByRefObject
    /// <summary>
    /// Base class that all TsdLib controls should derive from.
    /// </summary>
    public partial class TsdLibControl : UserControl, ITsdLibControl
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
