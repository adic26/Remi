﻿using System;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
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
            get { return checkedListBox_TestConfig.CheckedItems.Cast<ITestConfig>().ToArray(); }
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
