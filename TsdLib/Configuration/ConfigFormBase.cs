using System;
using System.Collections;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class ConfigFormBase : Form
    {
        protected IConfigGroup ConfigGroup { get; set; }

        protected ConfigFormBase()
        {
            InitializeComponent();
        }

        //protected ConfigFormBase(IConfigGroup configGroup, bool editable)
        //{
        //    InitializeComponent();
        //    SetConfigGroup(configGroup, editable);

        //}

        //protected void SetConfigGroup(IConfigGroup configGroup, bool editable)
        //{
        //    Text = configGroup.ConfigType;
        //    ConfigGroup = configGroup;
        //    comboBox_SettingsGroup.DataSource = ConfigGroup;
        //    IList list = ConfigGroup.GetList();
        //    if (list.Count == 0)
        //        throw new EmptyConfigGroupException(ConfigGroup);

        //    propertyGrid_Settings.SelectedObject = list[0];

        //    propertyGrid_Settings.Enabled = editable;
        //    button_CreateNew.Enabled = editable;
        //    button_OK.Enabled = editable;
        //}

        //public override sealed string Text
        //{
        //    get { return base.Text; }
        //}

        //private void closeForm(object sender, EventArgs e)
        //{
        //    Close();
        //}

        //private void comboBox_SettingsGroup_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    ComboBox cb = sender as ComboBox;
        //    if (cb != null)
        //        propertyGrid_Settings.SelectedObject = cb.SelectedItem;
        //}

        //private void button_CreateNew_Click(object sender, EventArgs e)
        //{
        //    CreateNew();

        //    comboBox_SettingsGroup.SelectedIndex = comboBox_SettingsGroup.Items.Count - 1;
        //}

        //protected virtual void CreateNew()
        //{
        //    MessageBox.Show("Creating a new config item is not supported by this applcation", "Not Supported");

        //    //using (ConfigFormCreate<T> form = new ConfigFormCreate<T>(_configGroup))
        //    //    form.ShowDialog();
        //}
    }
}
