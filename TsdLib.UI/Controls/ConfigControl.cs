using System;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class ConfigControl : ConfigControlBase
    {
        public override void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
        }

        public ConfigControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the list of available Station Config instances.
        /// </summary>
        public override IConfigManager StationConfigManager
        {
            get { return (IConfigManager)comboBox_StationConfig.DataSource; }
            set { comboBox_StationConfig.DataSource = value; }
        }
        /// <summary>
        /// Sets the list of available Product Config instances.
        /// </summary>
        public override IConfigManager ProductConfigManager
        {
            get { return (IConfigManager)comboBox_ProductConfig.DataSource; }
            set { comboBox_ProductConfig.DataSource = value; }
        }
        /// <summary>
        /// Sets the list of available Test Config instances.
        /// </summary>
        public override IConfigManager TestConfigManager
        {
            get { return (IConfigManager)comboBox_TestConfig.DataSource; }
            set { comboBox_TestConfig.DataSource = value; }
        }
        /// <summary>
        /// Sets the list of available Sequence Config instances.
        /// </summary>
        public override IConfigManager SequenceConfigManager
        {
            get { return (IConfigManager)comboBox_SequenceConfig.DataSource; }
            set { comboBox_SequenceConfig.DataSource = value; }
        }

        /// <summary>
        /// Gets the selected station configuration instance.
        /// </summary>
        public override IConfigItem[] SelectedStationConfig
        {
            get { return new [] { (IConfigItem) comboBox_StationConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected product configuration instance.
        /// </summary>
        public override IConfigItem[] SelectedProductConfig
        {
            get { return new[] { (IConfigItem)comboBox_ProductConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected test configuration instance.
        /// </summary>
        public override IConfigItem[] SelectedTestConfig
        {
            get { return new[] { (IConfigItem)comboBox_TestConfig.SelectedItem }; }
        }
        /// <summary>
        /// Gets the selected sequence configuration instance.
        /// </summary>
        public override IConfigItem[] SelectedSequenceConfig
        {
            get { return new[] { (IConfigItem)comboBox_SequenceConfig.SelectedItem }; }
        }

        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            OnViewEditConfiguration(new []{StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager});
        }

        private void config_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnConfigSelectionChanged(e);
        }
    }
}
