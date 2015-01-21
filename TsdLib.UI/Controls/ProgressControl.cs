using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class ProgressControl : ProgressControlBase
    {
        public ProgressControl()
        {
            InitializeComponent();
        }

        public override void UpdateProgress(int currentStep, int numberOfSteps)
        {
            progressBar.Maximum = numberOfSteps;
            progressBar.Value = currentStep;
        }

        public override void SetMaxValue(int maxValue)
        {
            progressBar.Maximum = maxValue;
        }
    }
}
