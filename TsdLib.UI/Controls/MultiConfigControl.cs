using System;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class MultiConfigControl : ConfigControlBase
    {
        public MultiConfigControl()
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
            get { return (IConfigManager)checkedListBox_TestConfig.DataSource; }
            set
            {
                checkedListBox_TestConfig.DataSource = value;
                if (value != null)
                    checkedListBox_TestConfig.SetItemChecked(0, true);
            }
        }
        /// <summary>
        /// Sets the list of available Sequence Config instances.
        /// </summary>
        public override IConfigManager SequenceConfigManager
        {
            get { return (IConfigManager)checkedListBox_SequenceConfig.DataSource; }
            set
            {
                checkedListBox_SequenceConfig.DataSource = value;
                if (value != null)
                    checkedListBox_SequenceConfig.SetItemChecked(0, true);
            }
        }

        /// <summary>
        /// Gets the selected station configuration instance.
        /// </summary>
        public override IConfigItem[] SelectedStationConfig
        {
            get { return new[] { (IConfigItem)comboBox_StationConfig.SelectedItem }; }
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
            get { return checkedListBox_TestConfig.CheckedItems.Cast<IConfigItem>().ToArray() ; }
        }
        /// <summary>
        /// Gets the selected sequence configuration instance.
        /// </summary>
        public override IConfigItem[] SelectedSequenceConfig
        {
            get { return checkedListBox_SequenceConfig.CheckedItems.Cast<IConfigItem>().ToArray(); }
        }

        private void button_ViewEditConfiguration_Click(object sender, EventArgs e)
        {
            OnViewEditConfiguration(new[] { StationConfigManager, ProductConfigManager, TestConfigManager, SequenceConfigManager });
        }

        private void config_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnConfigSelectionChanged(e);
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
                    OnConfigSelectionChanged(e);
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
