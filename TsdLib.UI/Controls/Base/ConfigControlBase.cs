using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to select, view or edit the test configuration on the UI.
    /// </summary>
    public partial class ConfigControlBase : UserControl, IConfigControl
    {
        /// <summary>
        /// Initialize the control.
        /// </summary>
        public ConfigControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Override to get and set the station configuration manager. Suitable for binding to a ListControl.
        /// </summary>
        public virtual IConfigManager<IStationConfig> StationConfigManager { get; set; }
        /// <summary>
        /// Override to get and set the product configuration manager. Suitable for binding to a ListControl.
        /// </summary>
        public virtual IConfigManager<IProductConfig> ProductConfigManager { get; set; }
        /// <summary>
        /// Override to get and set the test configuration manager. Suitable for binding to a ListControl.
        /// </summary>
        public virtual IConfigManager<ITestConfig> TestConfigManager { get; set; }
        /// <summary>
        /// Override to get and set the sequence configuration manager. Suitable for binding to a ListControl.
        /// </summary>
        public virtual IConfigManager<ISequenceConfig> SequenceConfigManager { get; set; }

        /// <summary>
        /// Override to expose the selected station configuration instance(s).
        /// </summary>
        public virtual IStationConfig[] SelectedStationConfig
        {
            get { return new IStationConfig[0]; }
            set { }
        }
        /// <summary>
        /// Override to expose the selected product configuration instance(s).
        /// </summary>
        public virtual IProductConfig[] SelectedProductConfig
        {
            get { return new IProductConfig[0]; }
            set { }
        }
        /// <summary>
        /// Override to expose the selected test configuration instance(s).
        /// </summary>
        public virtual ITestConfig[] SelectedTestConfig
        {
            get { return new ITestConfig[0]; }
            set { }
        }
        /// <summary>
        /// Override to expose the selected sequence configuration instance(s).
        /// </summary>
        public virtual ISequenceConfig[] SelectedSequenceConfig
        {
            get { return new ISequenceConfig[0]; }
            set { }
        }

        /// <summary>
        /// Event fired when requesting to modify the test system configuration.
        /// </summary>
        public event EventHandler<IConfigManager[]> ViewEditConfiguration;
        /// <summary>
        /// Fire the <see cref="ViewEditConfiguration"/> event.
        /// </summary>
        /// <param name="configManagers">Sequence of <see cref="IConfigManager"/> objects that are bound to the UI controls.</param>
        protected virtual void OnViewEditConfiguration(IEnumerable<IConfigManager> configManagers)
        {
            EventHandler<IConfigManager[]> invoker = ViewEditConfiguration;
            IConfigManager[] configManagersArray = configManagers.Where(c => c != null).ToArray();
            if (invoker != null)
                invoker(this, configManagersArray);
        }

        /// <summary>
        /// Enables the control if the test system is ready to test. Otherwise disables.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
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
