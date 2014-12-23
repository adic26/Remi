using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.UI.Controls
{
    public partial class ConfigControlBase : TsdLibControl
    {
        protected ConfigControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Override to get and set the station configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        public virtual IConfigManager StationConfigManager { get; set; }
        /// <summary>
        /// Override to get and set the product configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        public virtual IConfigManager ProductConfigManager { get; set; }
        /// <summary>
        /// Override to get and set the test configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        public virtual IConfigManager TestConfigManager { get; set; }
        /// <summary>
        /// Override to get and set the sequence configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        public virtual IConfigManager SequenceConfigManager { get; set; }


        private readonly IConfigItem[] _empty = new IConfigItem[0];

        /// <summary>
        /// Override to expose the selected station configuration instances.
        /// </summary>
        public virtual IConfigItem[] SelectedStationConfig { get { return _empty; } }
        /// <summary>
        /// Override to expose the selected product configuration instances.
        /// </summary>
        public virtual IConfigItem[] SelectedProductConfig { get { return _empty; } }
        /// <summary>
        /// Override to expose the selected test configuration instances.
        /// </summary>
        public virtual IConfigItem[] SelectedTestConfig { get { return _empty; } }
        /// <summary>
        /// Override to expose the selected sequence configuration instances.
        /// </summary>
        public virtual IConfigItem[] SelectedSequenceConfig { get { return _empty; } }
        
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

        public event EventHandler<IConfigItem[]> ConfigSelectionChanged;
        protected virtual void OnConfigSelectionChanged(ListBox control)
        {
            EventHandler<IConfigItem[]> invoker = ConfigSelectionChanged;
            if (invoker != null)
                invoker(this, control.SelectedItems.Cast<IConfigItem>().ToArray());
        }
    }
}
