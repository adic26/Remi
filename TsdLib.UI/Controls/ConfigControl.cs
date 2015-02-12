using System;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class ConfigControl : ConfigControlBase
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
        public override IConfigManager<IStationConfig> StationConfigManager
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
        public override IConfigManager<IProductConfig> ProductConfigManager
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
        public override IConfigManager<ITestConfig> TestConfigManager
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
        public override IConfigManager<ISequenceConfig> SequenceConfigManager
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
        public override IStationConfig[] SelectedStationConfig
        {
            get { return new[] { (IStationConfig)comboBox_StationConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected product configuration instance.
        /// </summary>
        public override IProductConfig[] SelectedProductConfig
        {
            get { return new[] { (IProductConfig)comboBox_ProductConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected test configuration instance.
        /// </summary>
        public override ITestConfig[] SelectedTestConfig
        {
            get { return new[] { (ITestConfig)comboBox_TestConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected sequence configuration instance.
        /// </summary>
        public override ISequenceConfig[] SelectedSequenceConfig
        {
            get { return new[] { (ISequenceConfig)comboBox_SequenceConfig.SelectedItem }; }
        }

        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            OnViewEditConfiguration(new IConfigManager[] { StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager });
        }

        private void config_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnConfigSelectionChanged();
        }
    }
}
