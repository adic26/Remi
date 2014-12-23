using System;
using TsdLib.Configuration;

namespace TsdLib.UI.Controls
{
    public partial class TestSequenceControlBase : TsdLibControl
    {
        public TestSequenceControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        public event EventHandler<TestSequenceEventArgs> ExecuteTestSequence;
        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        public event EventHandler AbortTestSequence;

        protected virtual void OnExecute()
        {
            if (ExecuteTestSequence != null)
                ExecuteTestSequence(
                    this,
                    new TestSequenceEventArgs(
                        SelectedStationConfig,
                        SelectedProductConfig,
                        SelectedTestConfig,
                        SelectedSequenceConfig,
                        PublishResults)
                );
        }

        /// <summary>
        /// Fire the <see cref="AbortTestSequence"/> event.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnAbort(EventArgs e)
        {
            if (AbortTestSequence != null/* && CurrentState == State.TestInProgress*/)
                AbortTestSequence(this, e);
        }

        public IConfigItem[] SelectedStationConfig { get; set; }
        public IConfigItem[] SelectedProductConfig { get; set; }
        public IConfigItem[] SelectedTestConfig { get; set; }
        public IConfigItem[] SelectedSequenceConfig { get; set; }
        public virtual bool PublishResults {
            get { return false; }
        }

    }
}
