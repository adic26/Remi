using System;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class MultiConfigControl<TStationConfig, TProductConfig, TTestConfig> : ConfigControlBase<TStationConfig, TProductConfig, TTestConfig>
        where TStationConfig : IStationConfig
        where TProductConfig : IProductConfig
        where TTestConfig : ITestConfig
    {

        /// <summary>
        /// Initialize a new <see cref="MultiConfigControl{TStationConfig, TProductConfig, TTestConfig}"/>
        /// </summary>
        public MultiConfigControl()
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
            get { return (IConfigManager<TTestConfig>)checkedListBox_TestConfig.DataSource; }
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
            get { return checkedListBox_TestConfig.CheckedItems.Cast<TTestConfig>().ToArray(); }
        }
        /// <summary>
        /// Gets the selected sequence configuration instance.
        /// </summary>
        public override ISequenceConfig[] SelectedSequenceConfig
        {
            get { return checkedListBox_SequenceConfig.CheckedItems.Cast<ISequenceConfig>().ToArray(); }
        }

        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            OnViewEditConfiguration(new IConfigManager[] { StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager });
        }

        private void config_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnConfigSelectionChanged();
        }

        private void config_SelectionChangeCommittedCheckBox(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox checkedListBox = sender as CheckedListBox;
            if (checkedListBox != null)
            {
                try
                {
                    checkedListBox.ItemCheck -= config_SelectionChangeCommittedCheckBox;
                    checkedListBox.SetItemCheckState(e.Index, e.NewValue);
                    OnConfigSelectionChanged();
                }
                finally
                {
                    checkedListBox.ItemCheck += config_SelectionChangeCommittedCheckBox;
                }
            }
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
