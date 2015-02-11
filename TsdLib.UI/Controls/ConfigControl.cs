using System;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class ConfigControl<TStationConfig, TProductConfig, TTestConfig> : ConfigControlBase<TStationConfig, TProductConfig, TTestConfig>
        where TStationConfig : IStationConfig
        where TProductConfig : IProductConfig
        where TTestConfig : ITestConfig
    {
        /// <summary>
        /// Initialize a new <see cref="ConfigControl{TStationConfig, TProductConfig, TTestConfig}"/>
        /// </summary>
        public ConfigControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the list of available Station Config instances.
        /// </summary>
        public override IConfigManager<TStationConfig> StationConfigManager
        {
            get { return (IConfigManager<TStationConfig>)comboBox_StationConfig.DataSource; }
            set
            {
                if (value == null) return;
                comboBox_StationConfig.DataSource = value;
            }
        }
        /// <summary>
        /// Sets the list of available Product Config instances.
        /// </summary>
        public override IConfigManager<TProductConfig> ProductConfigManager
        {
            get { return (IConfigManager<TProductConfig>)comboBox_ProductConfig.DataSource; }
            set
            {
                if (value == null) return; 
                comboBox_ProductConfig.DataSource = value;
            }
        }
        /// <summary>
        /// Sets the list of available Test Config instances.
        /// </summary>
        public override IConfigManager<TTestConfig> TestConfigManager
        {
            get { return (IConfigManager<TTestConfig>)comboBox_TestConfig.DataSource; }
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
        public override TStationConfig[] SelectedStationConfig
        {
            get { return new[] { (TStationConfig)comboBox_StationConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected product configuration instance.
        /// </summary>
        public override TProductConfig[] SelectedProductConfig
        {
            get { return new[] { (TProductConfig)comboBox_ProductConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected test configuration instance.
        /// </summary>
        public override TTestConfig[] SelectedTestConfig
        {
            get { return new[] { (TTestConfig)comboBox_TestConfig.SelectedItem }; }
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
