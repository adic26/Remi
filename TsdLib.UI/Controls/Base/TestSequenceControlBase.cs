using System;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to start and stop a test sequence on the UI.
    /// </summary>
    public partial class TestSequenceControlBase : UserControl, ITestSequenceControl
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
        public virtual void SetState(State state)
        {
            
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
