using System.Windows.Forms;

namespace TsdLib.UI.Controls
{
    public partial class ProgressControl : UserControl, IProgressControl
    {
        public ProgressControl()
        {
            InitializeComponent();
        }

        public void UpdateProgress(int currentStep, int numberOfSteps)
        {
            progressBar.Maximum = numberOfSteps;
            progressBar.Value = currentStep;
        }

        public virtual void SetState(State state)
        {
            if (state.HasFlag(State.TestStarting))
                UpdateProgress(0, 1);
        }
    }
}
