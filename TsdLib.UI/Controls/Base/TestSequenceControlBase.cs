using System;
using TsdLib.Configuration;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to start and stop a test sequence on the UI.
    /// </summary>
    public partial class TestSequenceControlBase : TsdLibControl, ITestSequenceControl
    {
        public TestSequenceControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        public event EventHandler ExecuteTestSequence;
        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        public event EventHandler AbortTestSequence;

        protected virtual void OnExecute()
        {
            if (ExecuteTestSequence != null)
                ExecuteTestSequence(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fire the <see cref="AbortTestSequence"/> event.
        /// </summary>
        protected virtual void OnAbort()
        {
            if (AbortTestSequence != null)
                AbortTestSequence(this, EventArgs.Empty);
        }

        public IStationConfig[] SelectedStationConfig { get; set; }
        public IProductConfig[] SelectedProductConfig { get; set; }
        public ITestConfig[] SelectedTestConfig { get; set; }
        public ISequenceConfig[] SelectedSequenceConfig { get; set; }
        public virtual bool PublishResults { get { return false; } }

        /// <summary>
        /// Enables the control if the test system is ready to test. Otherwise disables.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public override void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
        }
    }
}
