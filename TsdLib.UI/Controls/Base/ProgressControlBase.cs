namespace TsdLib.UI.Controls.Base
{
    public partial class ProgressControlBase : TsdLibLabelledControl, IProgressControl
    {
        public ProgressControlBase()
        {
            InitializeComponent();
            Text = "Progress";
        }

        public virtual void UpdateProgress(int currentStep, int numberOfSteps)
        {
            
        }

        public virtual void SetMaxValue(int maxValue)
        {
            
        }

        /// <summary>
        /// Resets the indicated progress when the test begins.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public override void SetState(State state)
        {
            if (state.HasFlag(State.TestInProgress))
                UpdateProgress(0, 1);
        }
    }
}
