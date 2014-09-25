namespace TsdLib.Configuration
{
    partial class ConfigManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox_TestSystemName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_TestSystemVersion = new System.Windows.Forms.ComboBox();
            this.panel_SelectControls = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_ConfigType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_EditControls = new System.Windows.Forms.Panel();
            this.propertyGrid_Settings = new System.Windows.Forms.PropertyGrid();
            this.button_CreateNew = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_ConfigItem = new System.Windows.Forms.ComboBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.panel_SelectControls.SuspendLayout();
            this.panel_EditControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_TestSystemName
            // 
            this.comboBox_TestSystemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TestSystemName.FormattingEnabled = true;
            this.comboBox_TestSystemName.Location = new System.Drawing.Point(6, 23);
            this.comboBox_TestSystemName.Name = "comboBox_TestSystemName";
            this.comboBox_TestSystemName.Size = new System.Drawing.Size(187, 21);
            this.comboBox_TestSystemName.TabIndex = 7;
            this.comboBox_TestSystemName.SelectedValueChanged += new System.EventHandler(this.comboBox_TestSystemName_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Test System Name";
            // 
            // comboBox_TestSystemVersion
            // 
            this.comboBox_TestSystemVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TestSystemVersion.FormattingEnabled = true;
            this.comboBox_TestSystemVersion.Location = new System.Drawing.Point(6, 66);
            this.comboBox_TestSystemVersion.Name = "comboBox_TestSystemVersion";
            this.comboBox_TestSystemVersion.Size = new System.Drawing.Size(187, 21);
            this.comboBox_TestSystemVersion.TabIndex = 9;
            this.comboBox_TestSystemVersion.SelectedValueChanged += new System.EventHandler(this.comboBox_TestSystemVersion_SelectedValueChanged);
            // 
            // panel_SelectControls
            // 
            this.panel_SelectControls.Controls.Add(this.label4);
            this.panel_SelectControls.Controls.Add(this.comboBox_ConfigType);
            this.panel_SelectControls.Controls.Add(this.label3);
            this.panel_SelectControls.Controls.Add(this.comboBox_TestSystemName);
            this.panel_SelectControls.Controls.Add(this.comboBox_TestSystemVersion);
            this.panel_SelectControls.Controls.Add(this.label2);
            this.panel_SelectControls.Location = new System.Drawing.Point(6, 3);
            this.panel_SelectControls.Name = "panel_SelectControls";
            this.panel_SelectControls.Size = new System.Drawing.Size(400, 104);
            this.panel_SelectControls.TabIndex = 10;
            this.panel_SelectControls.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(253, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Config Type";
            // 
            // comboBox_ConfigType
            // 
            this.comboBox_ConfigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ConfigType.FormattingEnabled = true;
            this.comboBox_ConfigType.Location = new System.Drawing.Point(256, 23);
            this.comboBox_ConfigType.Name = "comboBox_ConfigType";
            this.comboBox_ConfigType.Size = new System.Drawing.Size(138, 21);
            this.comboBox_ConfigType.TabIndex = 12;
            this.comboBox_ConfigType.SelectedValueChanged += new System.EventHandler(this.comboBox_ConfigType_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Test System Version";
            // 
            // panel_EditControls
            // 
            this.panel_EditControls.Controls.Add(this.propertyGrid_Settings);
            this.panel_EditControls.Controls.Add(this.button_CreateNew);
            this.panel_EditControls.Controls.Add(this.label5);
            this.panel_EditControls.Controls.Add(this.comboBox_ConfigItem);
            this.panel_EditControls.Location = new System.Drawing.Point(6, 113);
            this.panel_EditControls.Name = "panel_EditControls";
            this.panel_EditControls.Size = new System.Drawing.Size(400, 471);
            this.panel_EditControls.TabIndex = 11;
            // 
            // propertyGrid_Settings
            // 
            this.propertyGrid_Settings.Location = new System.Drawing.Point(6, 48);
            this.propertyGrid_Settings.Name = "propertyGrid_Settings";
            this.propertyGrid_Settings.Size = new System.Drawing.Size(388, 420);
            this.propertyGrid_Settings.TabIndex = 0;
            // 
            // button_CreateNew
            // 
            this.button_CreateNew.Location = new System.Drawing.Point(256, 19);
            this.button_CreateNew.Name = "button_CreateNew";
            this.button_CreateNew.Size = new System.Drawing.Size(138, 23);
            this.button_CreateNew.TabIndex = 5;
            this.button_CreateNew.Text = "Create New";
            this.button_CreateNew.UseVisualStyleBackColor = true;
            this.button_CreateNew.Click += new System.EventHandler(this.button_CreateNew_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Config Item";
            // 
            // comboBox_ConfigItem
            // 
            this.comboBox_ConfigItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ConfigItem.FormattingEnabled = true;
            this.comboBox_ConfigItem.Location = new System.Drawing.Point(6, 19);
            this.comboBox_ConfigItem.Name = "comboBox_ConfigItem";
            this.comboBox_ConfigItem.Size = new System.Drawing.Size(187, 21);
            this.comboBox_ConfigItem.TabIndex = 3;
            this.comboBox_ConfigItem.SelectedValueChanged += new System.EventHandler(this.comboBox_ConfigItem_SelectedValueChanged);
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(9, 590);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(124, 39);
            this.button_OK.TabIndex = 12;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(282, 590);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(124, 39);
            this.button_Cancel.TabIndex = 13;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // ConfigManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(412, 639);
            this.ControlBox = false;
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.panel_EditControls);
            this.Controls.Add(this.panel_SelectControls);
            this.Name = "ConfigManagerForm";
            this.panel_SelectControls.ResumeLayout(false);
            this.panel_SelectControls.PerformLayout();
            this.panel_EditControls.ResumeLayout(false);
            this.panel_EditControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_TestSystemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_TestSystemVersion;
        private System.Windows.Forms.Panel panel_SelectControls;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_ConfigType;
        protected System.Windows.Forms.Panel panel_EditControls;
        protected System.Windows.Forms.PropertyGrid propertyGrid_Settings;
        protected System.Windows.Forms.Button button_CreateNew;
        protected System.Windows.Forms.Label label5;
        protected System.Windows.Forms.ComboBox comboBox_ConfigItem;
        protected System.Windows.Forms.Button button_OK;
        protected System.Windows.Forms.Button button_Cancel;

    }
}