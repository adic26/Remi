namespace TsdLib.Configuration
{
    sealed partial class ConfigForm<T> where T : ConfigItem, new()
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
            this.propertyGrid_Settings = new System.Windows.Forms.PropertyGrid();
            this.button_OK = new System.Windows.Forms.Button();
            this.comboBox_SettingsGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_CreateNew = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid_Settings
            // 
            this.propertyGrid_Settings.Location = new System.Drawing.Point(12, 81);
            this.propertyGrid_Settings.Name = "propertyGrid_Settings";
            this.propertyGrid_Settings.Size = new System.Drawing.Size(451, 233);
            this.propertyGrid_Settings.TabIndex = 0;
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(12, 320);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(124, 39);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.closeForm);
            // 
            // comboBox_SettingsGroup
            // 
            this.comboBox_SettingsGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SettingsGroup.FormattingEnabled = true;
            this.comboBox_SettingsGroup.Location = new System.Drawing.Point(15, 41);
            this.comboBox_SettingsGroup.Name = "comboBox_SettingsGroup";
            this.comboBox_SettingsGroup.Size = new System.Drawing.Size(121, 21);
            this.comboBox_SettingsGroup.TabIndex = 3;
            this.comboBox_SettingsGroup.SelectedValueChanged += new System.EventHandler(this.comboBox_SettingsGroup_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Settings Group";
            // 
            // button_CreateNew
            // 
            this.button_CreateNew.Location = new System.Drawing.Point(339, 40);
            this.button_CreateNew.Name = "button_CreateNew";
            this.button_CreateNew.Size = new System.Drawing.Size(124, 21);
            this.button_CreateNew.TabIndex = 5;
            this.button_CreateNew.Text = "Create New";
            this.button_CreateNew.UseVisualStyleBackColor = true;
            this.button_CreateNew.Click += new System.EventHandler(this.button_CreateNew_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(339, 320);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(124, 39);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.closeForm);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 371);
            this.ControlBox = false;
            this.Controls.Add(this.button_CreateNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_SettingsGroup);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.propertyGrid_Settings);
            this.Name = "ConfigForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid_Settings;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.ComboBox comboBox_SettingsGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_CreateNew;
        private System.Windows.Forms.Button button_Cancel;
    }
}