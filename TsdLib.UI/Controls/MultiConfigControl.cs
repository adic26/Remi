using System;
using System.Linq;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    /// <summary>
    /// Contains functionality to select and manage configuration on the UI, supporting multiple test and sequence configs.
    /// </summary>
    public partial class MultiConfigControl : ConfigControlBase
    {

        /// <summary>
        /// Initialize a new <see cref="MultiConfigControl"/>
        /// </summary>
        public MultiConfigControl()
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
            get { return (IConfigManager<ITestConfig>)checkedListBox_TestConfig.DataSource; }
            set
            {
                if (value == null) return;
                checkedListBox_TestConfig.DataSource = value;
                checkedListBox_TestConfig.SetItemChecked(0, true);
            }
        }
        /// <summary>
        /// Sets the list of available Sequence Config instances.
        /// </summary>
        public override IConfigManager<ISequenceConfig> SequenceConfigManager
        {
            get { return (IConfigManager<ISequenceConfig>)checkedListBox_SequenceConfig.DataSource; }
            set
            {
                if (value == null) return;
                checkedListBox_SequenceConfig.DataSource = value;
                checkedListBox_SequenceConfig.SetItemChecked(0, true);
            }
        }

        /// <summary>
        /// Gets the selected station configuration instance.
        /// </summary>
        public override IStationConfig[] SelectedStationConfig
        {
            get { return new[] { (IStationConfig)comboBox_StationConfig.SelectedItem }; }
            set { comboBox_StationConfig.SelectedItem = value[0]; }
        }
        /// <summary>
        /// Gets the selected product configuration instance.
        /// </summary>
        public override IProductConfig[] SelectedProductConfig
        {
            get { return new[] { (IProductConfig)comboBox_ProductConfig.SelectedItem }; }
            set { comboBox_ProductConfig.SelectedItem = value[0]; }
        }
        /// <summary>
        /// Gets the selected test configuration instance.
        /// </summary>
        public override ITestConfig[] SelectedTestConfig
        {
            get { return checkedListBox_TestConfig.CheckedItems.Cast<ITestConfig>().ToArray(); }
            set
            {
                for (int i = 0; i < checkedListBox_TestConfig.Items.Count; i++)
                    checkedListBox_TestConfig.SetItemChecked(i, value.Contains(checkedListBox_TestConfig.Items[i]));
            }
        }
        /// <summary>
        /// Gets the selected sequence configuration instance.
        /// </summary>
        public override ISequenceConfig[] SelectedSequenceConfig
        {
            get { return checkedListBox_SequenceConfig.CheckedItems.Cast<ISequenceConfig>().ToArray(); }
            set
            {
                for (int i = 0; i < checkedListBox_SequenceConfig.Items.Count; i++)
                    checkedListBox_SequenceConfig.SetItemChecked(i, value.Contains(checkedListBox_SequenceConfig.Items[i]));
            }

        }

        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            OnViewEditConfiguration(new IConfigManager[] { StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager });
        }

        private void button_SequenceConfigSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_SequenceConfig.Items.Count; i++)
                checkedListBox_SequenceConfig.SetItemChecked(i, true);
        }

        private void button_SequenceConfigSelectNone_Click(object sender, EventArgs e)
        {
            foreach (int checkedIndex in checkedListBox_SequenceConfig.CheckedIndices)
                checkedListBox_SequenceConfig.SetItemChecked(checkedIndex, false);
        }

        private void button_TestConfigSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_TestConfig.Items.Count; i++)
                checkedListBox_TestConfig.SetItemChecked(i, true);
        }

        private void button_TestConfigSelectNone_Click(object sender, EventArgs e)
        {
            foreach (int checkedIndex in checkedListBox_TestConfig.CheckedIndices)
                checkedListBox_TestConfig.SetItemChecked(checkedIndex, false);
        }
    }
}
