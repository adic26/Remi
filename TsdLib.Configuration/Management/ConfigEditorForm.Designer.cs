namespace TsdLib.Configuration.Management
{
    partial class ConfigEditorForm
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
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_TestSystemIdentity = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_TestSystemIdentity = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_TestSystemName = new System.Windows.Forms.TextBox();
            this.textBox_TestSystemVersion = new System.Windows.Forms.TextBox();
            this.textBox_TestSystemMode = new System.Windows.Forms.TextBox();
            this.button_CloneVersion = new System.Windows.Forms.Button();
            this.button_CloneMode = new System.Windows.Forms.Button();
            this.comboBox_ConfigType = new System.Windows.Forms.ComboBox();
            this.propertyGrid_Settings = new System.Windows.Forms.PropertyGrid();
            this.comboBox_ConfigItem = new System.Windows.Forms.ComboBox();
            this.button_CreateNew = new System.Windows.Forms.Button();
            this.groupBox_ConfigSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_ConfigSelection = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox_TestSystemIdentity.SuspendLayout();
            this.tableLayoutPanel_TestSystemIdentity.SuspendLayout();
            this.groupBox_ConfigSelection.SuspendLayout();
            this.tableLayoutPanel_ConfigSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(12, 586);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(124, 39);
            this.button_OK.TabIndex = 13;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(375, 586);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(124, 39);
            this.button_Cancel.TabIndex = 14;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Version";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Name";
            // 
            // groupBox_TestSystemIdentity
            // 
            this.groupBox_TestSystemIdentity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_TestSystemIdentity.Controls.Add(this.tableLayoutPanel_TestSystemIdentity);
            this.groupBox_TestSystemIdentity.Location = new System.Drawing.Point(12, 12);
            this.groupBox_TestSystemIdentity.MinimumSize = new System.Drawing.Size(330, 100);
            this.groupBox_TestSystemIdentity.Name = "groupBox_TestSystemIdentity";
            this.groupBox_TestSystemIdentity.Size = new System.Drawing.Size(487, 100);
            this.groupBox_TestSystemIdentity.TabIndex = 20;
            this.groupBox_TestSystemIdentity.TabStop = false;
            this.groupBox_TestSystemIdentity.Text = "Test System Identity";
            // 
            // tableLayoutPanel_TestSystemIdentity
            // 
            this.tableLayoutPanel_TestSystemIdentity.ColumnCount = 2;
            this.tableLayoutPanel_TestSystemIdentity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel_TestSystemIdentity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_TestSystemIdentity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_TestSystemIdentity.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel_TestSystemIdentity.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel_TestSystemIdentity.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel_TestSystemIdentity.Controls.Add(this.textBox_TestSystemName, 1, 0);
            this.tableLayoutPanel_TestSystemIdentity.Controls.Add(this.textBox_TestSystemVersion, 1, 1);
            this.tableLayoutPanel_TestSystemIdentity.Controls.Add(this.textBox_TestSystemMode, 1, 2);
            this.tableLayoutPanel_TestSystemIdentity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_TestSystemIdentity.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel_TestSystemIdentity.MinimumSize = new System.Drawing.Size(324, 80);
            this.tableLayoutPanel_TestSystemIdentity.Name = "tableLayoutPanel_TestSystemIdentity";
            this.tableLayoutPanel_TestSystemIdentity.RowCount = 3;
            this.tableLayoutPanel_TestSystemIdentity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_TestSystemIdentity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_TestSystemIdentity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_TestSystemIdentity.Size = new System.Drawing.Size(481, 81);
            this.tableLayoutPanel_TestSystemIdentity.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Mode";
            // 
            // textBox_TestSystemName
            // 
            this.textBox_TestSystemName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_TestSystemName.Location = new System.Drawing.Point(75, 3);
            this.textBox_TestSystemName.Name = "textBox_TestSystemName";
            this.textBox_TestSystemName.ReadOnly = true;
            this.textBox_TestSystemName.Size = new System.Drawing.Size(403, 20);
            this.textBox_TestSystemName.TabIndex = 24;
            // 
            // textBox_TestSystemVersion
            // 
            this.textBox_TestSystemVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_TestSystemVersion.Location = new System.Drawing.Point(75, 30);
            this.textBox_TestSystemVersion.Name = "textBox_TestSystemVersion";
            this.textBox_TestSystemVersion.ReadOnly = true;
            this.textBox_TestSystemVersion.Size = new System.Drawing.Size(403, 20);
            this.textBox_TestSystemVersion.TabIndex = 25;
            // 
            // textBox_TestSystemMode
            // 
            this.textBox_TestSystemMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_TestSystemMode.Location = new System.Drawing.Point(75, 57);
            this.textBox_TestSystemMode.Name = "textBox_TestSystemMode";
            this.textBox_TestSystemMode.ReadOnly = true;
            this.textBox_TestSystemMode.Size = new System.Drawing.Size(403, 20);
            this.textBox_TestSystemMode.TabIndex = 26;
            // 
            // button_CloneVersion
            // 
            this.button_CloneVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CloneVersion.Location = new System.Drawing.Point(284, 3);
            this.button_CloneVersion.Name = "button_CloneVersion";
            this.button_CloneVersion.Size = new System.Drawing.Size(94, 21);
            this.button_CloneVersion.TabIndex = 22;
            this.button_CloneVersion.Text = "Clone Version";
            this.button_CloneVersion.UseVisualStyleBackColor = true;
            this.button_CloneVersion.Click += new System.EventHandler(this.button_PromoteVersion_Click);
            // 
            // button_CloneMode
            // 
            this.button_CloneMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CloneMode.Location = new System.Drawing.Point(384, 3);
            this.button_CloneMode.Name = "button_CloneMode";
            this.button_CloneMode.Size = new System.Drawing.Size(94, 21);
            this.button_CloneMode.TabIndex = 23;
            this.button_CloneMode.Text = "Clone Mode";
            this.button_CloneMode.UseVisualStyleBackColor = true;
            this.button_CloneMode.Click += new System.EventHandler(this.button_PromoteMode_Click);
            // 
            // comboBox_ConfigType
            // 
            this.comboBox_ConfigType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_ConfigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ConfigType.FormattingEnabled = true;
            this.comboBox_ConfigType.Location = new System.Drawing.Point(75, 3);
            this.comboBox_ConfigType.Name = "comboBox_ConfigType";
            this.comboBox_ConfigType.Size = new System.Drawing.Size(203, 21);
            this.comboBox_ConfigType.TabIndex = 21;
            this.comboBox_ConfigType.SelectedIndexChanged += new System.EventHandler(this.comboBox_ConfigType_SelectedIndexChanged);
            // 
            // propertyGrid_Settings
            // 
            this.propertyGrid_Settings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid_Settings.Location = new System.Drawing.Point(12, 211);
            this.propertyGrid_Settings.Name = "propertyGrid_Settings";
            this.propertyGrid_Settings.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid_Settings.Size = new System.Drawing.Size(487, 369);
            this.propertyGrid_Settings.TabIndex = 23;
            // 
            // comboBox_ConfigItem
            // 
            this.comboBox_ConfigItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_ConfigItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ConfigItem.FormattingEnabled = true;
            this.comboBox_ConfigItem.Location = new System.Drawing.Point(75, 30);
            this.comboBox_ConfigItem.Name = "comboBox_ConfigItem";
            this.comboBox_ConfigItem.Size = new System.Drawing.Size(203, 21);
            this.comboBox_ConfigItem.TabIndex = 24;
            this.comboBox_ConfigItem.SelectedIndexChanged += new System.EventHandler(this.comboBox_ConfigItem_SelectedIndexChanged);
            // 
            // button_CreateNew
            // 
            this.tableLayoutPanel_ConfigSelection.SetColumnSpan(this.button_CreateNew, 2);
            this.button_CreateNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_CreateNew.Location = new System.Drawing.Point(284, 30);
            this.button_CreateNew.Name = "button_CreateNew";
            this.button_CreateNew.Size = new System.Drawing.Size(194, 21);
            this.button_CreateNew.TabIndex = 26;
            this.button_CreateNew.Text = "Create New";
            this.button_CreateNew.UseVisualStyleBackColor = true;
            this.button_CreateNew.Click += new System.EventHandler(this.button_CreateNew_Click);
            // 
            // groupBox_ConfigSelection
            // 
            this.groupBox_ConfigSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_ConfigSelection.Controls.Add(this.tableLayoutPanel_ConfigSelection);
            this.groupBox_ConfigSelection.Location = new System.Drawing.Point(12, 125);
            this.groupBox_ConfigSelection.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.groupBox_ConfigSelection.MinimumSize = new System.Drawing.Size(0, 71);
            this.groupBox_ConfigSelection.Name = "groupBox_ConfigSelection";
            this.groupBox_ConfigSelection.Size = new System.Drawing.Size(487, 73);
            this.groupBox_ConfigSelection.TabIndex = 28;
            this.groupBox_ConfigSelection.TabStop = false;
            this.groupBox_ConfigSelection.Text = "Config Selection";
            // 
            // tableLayoutPanel_ConfigSelection
            // 
            this.tableLayoutPanel_ConfigSelection.ColumnCount = 4;
            this.tableLayoutPanel_ConfigSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel_ConfigSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_ConfigSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel_ConfigSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.comboBox_ConfigItem, 1, 1);
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.button_CreateNew, 2, 1);
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.comboBox_ConfigType, 1, 0);
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.button_CloneMode, 3, 0);
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.button_CloneVersion, 2, 0);
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel_ConfigSelection.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel_ConfigSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_ConfigSelection.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel_ConfigSelection.Name = "tableLayoutPanel_ConfigSelection";
            this.tableLayoutPanel_ConfigSelection.RowCount = 2;
            this.tableLayoutPanel_ConfigSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_ConfigSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_ConfigSelection.Size = new System.Drawing.Size(481, 54);
            this.tableLayoutPanel_ConfigSelection.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Config Type";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Config Item";
            // 
            // ConfigEditorForm
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(514, 637);
            this.Controls.Add(this.groupBox_ConfigSelection);
            this.Controls.Add(this.propertyGrid_Settings);
            this.Controls.Add(this.groupBox_TestSystemIdentity);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Name = "ConfigEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Editor";
            this.groupBox_TestSystemIdentity.ResumeLayout(false);
            this.tableLayoutPanel_TestSystemIdentity.ResumeLayout(false);
            this.tableLayoutPanel_TestSystemIdentity.PerformLayout();
            this.groupBox_ConfigSelection.ResumeLayout(false);
            this.tableLayoutPanel_ConfigSelection.ResumeLayout(false);
            this.tableLayoutPanel_ConfigSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox_TestSystemIdentity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_TestSystemIdentity;
        private System.Windows.Forms.Button button_CloneVersion;
        private System.Windows.Forms.Button button_CloneMode;
        private System.Windows.Forms.ComboBox comboBox_ConfigType;
        private System.Windows.Forms.PropertyGrid propertyGrid_Settings;
        private System.Windows.Forms.ComboBox comboBox_ConfigItem;
        private System.Windows.Forms.Button button_CreateNew;
        private System.Windows.Forms.GroupBox groupBox_ConfigSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_ConfigSelection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_TestSystemName;
        private System.Windows.Forms.TextBox textBox_TestSystemVersion;
        private System.Windows.Forms.TextBox textBox_TestSystemMode;
    }
}