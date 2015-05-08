using System;
using System.ComponentModel;
using System.Windows.Forms;
using TsdLib.Configuration;
using TsdLib.Configuration.Management;

namespace TsdLib.UI.Controls
{
    /// <summary>
    /// Contains functionality to select and manage configuration on the UI.
    /// </summary>
    public partial class ConfigControl : UserControl, IConfigControl
    {
        /// <summary>
        /// Initialize a new <see cref="ConfigControl"/>
        /// </summary>
        public ConfigControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the list of available Station Config instances.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IConfigManager<IStationConfig> StationConfigManager
        {

            get { return (IConfigManager<IStationConfig>)comboBox_StationConfig.DataSource; }
            set
            {
                if (value == null) return;
                comboBox_StationConfig.DataSource = value;
            }
        }
        /// <summary>
        /// Sets the list of available Product Config instances.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IConfigManager<IProductConfig> ProductConfigManager
        {
            get { return (IConfigManager<IProductConfig>)comboBox_ProductConfig.DataSource; }
            set
            {
                if (value == null) return; 
                comboBox_ProductConfig.DataSource = value;
            }
        }
        /// <summary>
        /// Sets the list of available Test Config instances.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IConfigManager<ITestConfig> TestConfigManager
        {
            get { return (IConfigManager<ITestConfig>)comboBox_TestConfig.DataSource; }
            set
            {
                if (value == null) return; 
                comboBox_TestConfig.DataSource = value;
            }
        }
        /// <summary>
        /// Sets the list of available Sequence Config instances.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IConfigManager<ISequenceConfig> SequenceConfigManager
        {
            get { return (IConfigManager<ISequenceConfig>)comboBox_SequenceConfig.DataSource; }
            set
            {
                if (value == null) return; 
                comboBox_SequenceConfig.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the selected station configuration instance.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IStationConfig[] SelectedStationConfig
        {
            get { return new[] { (IStationConfig)comboBox_StationConfig.SelectedItem }; }
            set
            {
                if (value == null || value.Length == 0) return;
                comboBox_StationConfig.SelectedItem = value[0];
            }
        }
        /// <summary>
        /// Gets the selected product configuration instance.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IProductConfig[] SelectedProductConfig
        {
            get { return new[] { (IProductConfig)comboBox_ProductConfig.SelectedItem }; }
            set
            {
                if (value == null || value.Length == 0) return;
                comboBox_ProductConfig.SelectedItem = value[0];
            }
        }
        /// <summary>
        /// Gets the selected test configuration instance.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ITestConfig[] SelectedTestConfig
        {
            get { return new[] { (ITestConfig)comboBox_TestConfig.SelectedItem }; }
            set
            {
                if (value == null || value.Length == 0) return;
                comboBox_TestConfig.SelectedItem = value[0];
            }
        }
        /// <summary>
        /// Gets the selected sequence configuration instance.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ISequenceConfig[] SelectedSequenceConfig
        {
            get { return new[] { (ISequenceConfig)comboBox_SequenceConfig.SelectedItem }; }
            set
            {
                if (value == null || value.Length == 0) return;
                comboBox_SequenceConfig.SelectedItem = value[0];
            }
        }

        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            EventHandler<IConfigManager[]> handler = ViewEditConfiguration;
            if (handler != null)
            {
                var managers = new IConfigManager[] {StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager};
                handler(this, managers);
            }

        }

        /// <summary>
        /// Event fired when requesting to modify the test system configuration.
        /// </summary>
        public event EventHandler<IConfigManager[]> ViewEditConfiguration;

        public virtual void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
        }
    }
}
