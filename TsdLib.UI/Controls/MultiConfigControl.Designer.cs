namespace TsdLib.UI.Controls
{
    partial class MultiConfigControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_ViewEditConfiguration = new System.Windows.Forms.Button();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.checkedListBox_SequenceConfig = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox_TestConfig = new System.Windows.Forms.CheckedListBox();
            this.button_TestConfigSelectAll = new System.Windows.Forms.Button();
            this.button_TestConfigSelectNone = new System.Windows.Forms.Button();
            this.button_SequenceConfigSelectNone = new System.Windows.Forms.Button();
            this.button_SequenceConfigSelectAll = new System.Windows.Forms.Button();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.groupBox_StationConfig = new System.Windows.Forms.GroupBox();
            this.groupBox_ProductConfig = new System.Windows.Forms.GroupBox();
            this.groupBox_TestConfig = new System.Windows.Forms.GroupBox();
            this.groupBox_SequenceConfig = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_TestConfigButtons = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_SequenceConfigButtons = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox_StationConfig.SuspendLayout();
            this.groupBox_ProductConfig.SuspendLayout();
            this.groupBox_TestConfig.SuspendLayout();
            this.groupBox_SequenceConfig.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel_TestConfigButtons.SuspendLayout();
            this.tableLayoutPanel_SequenceConfigButtons.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ViewEditConfiguration
            // 
            this.tableLayoutPanel.SetColumnSpan(this.button_ViewEditConfiguration, 2);
            this.button_ViewEditConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ViewEditConfiguration.Location = new System.Drawing.Point(3, 3);
            this.button_ViewEditConfiguration.Name = "button_ViewEditConfiguration";
            this.button_ViewEditConfiguration.Size = new System.Drawing.Size(532, 24);
            this.button_ViewEditConfiguration.TabIndex = 35;
            this.button_ViewEditConfiguration.Text = "View/Edit Configuration";
            this.button_ViewEditConfiguration.UseVisualStyleBackColor = true;
            this.button_ViewEditConfiguration.Click += new System.EventHandler(this.button_ViewEditConfiguration_Click);
            // 
            // comboBox_ProductConfig
            // 
            this.comboBox_ProductConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_ProductConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ProductConfig.FormattingEnabled = true;
            this.comboBox_ProductConfig.Location = new System.Drawing.Point(3, 16);
            this.comboBox_ProductConfig.Name = "comboBox_ProductConfig";
            this.comboBox_ProductConfig.Size = new System.Drawing.Size(257, 21);
            this.comboBox_ProductConfig.TabIndex = 32;
            // 
            // checkedListBox_SequenceConfig
            // 
            this.checkedListBox_SequenceConfig.CheckOnClick = true;
            this.checkedListBox_SequenceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_SequenceConfig.FormattingEnabled = true;
            this.checkedListBox_SequenceConfig.Location = new System.Drawing.Point(3, 16);
            this.checkedListBox_SequenceConfig.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.checkedListBox_SequenceConfig.Name = "checkedListBox_SequenceConfig";
            this.checkedListBox_SequenceConfig.Size = new System.Drawing.Size(257, 93);
            this.checkedListBox_SequenceConfig.TabIndex = 36;
            // 
            // checkedListBox_TestConfig
            // 
            this.checkedListBox_TestConfig.CheckOnClick = true;
            this.checkedListBox_TestConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_TestConfig.FormattingEnabled = true;
            this.checkedListBox_TestConfig.Location = new System.Drawing.Point(3, 16);
            this.checkedListBox_TestConfig.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.checkedListBox_TestConfig.Name = "checkedListBox_TestConfig";
            this.checkedListBox_TestConfig.Size = new System.Drawing.Size(257, 93);
            this.checkedListBox_TestConfig.TabIndex = 37;
            // 
            // button_TestConfigSelectAll
            // 
            this.button_TestConfigSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_TestConfigSelectAll.Location = new System.Drawing.Point(0, 0);
            this.button_TestConfigSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.button_TestConfigSelectAll.Name = "button_TestConfigSelectAll";
            this.button_TestConfigSelectAll.Size = new System.Drawing.Size(131, 24);
            this.button_TestConfigSelectAll.TabIndex = 40;
            this.button_TestConfigSelectAll.Text = "Select All";
            this.button_TestConfigSelectAll.UseVisualStyleBackColor = true;
            this.button_TestConfigSelectAll.Click += new System.EventHandler(this.button_TestConfigSelectAll_Click);
            // 
            // button_TestConfigSelectNone
            // 
            this.button_TestConfigSelectNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_TestConfigSelectNone.Location = new System.Drawing.Point(131, 0);
            this.button_TestConfigSelectNone.Margin = new System.Windows.Forms.Padding(0);
            this.button_TestConfigSelectNone.Name = "button_TestConfigSelectNone";
            this.button_TestConfigSelectNone.Size = new System.Drawing.Size(132, 24);
            this.button_TestConfigSelectNone.TabIndex = 41;
            this.button_TestConfigSelectNone.Text = "Select None";
            this.button_TestConfigSelectNone.UseVisualStyleBackColor = true;
            this.button_TestConfigSelectNone.Click += new System.EventHandler(this.button_TestConfigSelectNone_Click);
            // 
            // button_SequenceConfigSelectNone
            // 
            this.button_SequenceConfigSelectNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_SequenceConfigSelectNone.Location = new System.Drawing.Point(131, 0);
            this.button_SequenceConfigSelectNone.Margin = new System.Windows.Forms.Padding(0);
            this.button_SequenceConfigSelectNone.Name = "button_SequenceConfigSelectNone";
            this.button_SequenceConfigSelectNone.Size = new System.Drawing.Size(132, 24);
            this.button_SequenceConfigSelectNone.TabIndex = 43;
            this.button_SequenceConfigSelectNone.Text = "Select None";
            this.button_SequenceConfigSelectNone.UseVisualStyleBackColor = true;
            this.button_SequenceConfigSelectNone.Click += new System.EventHandler(this.button_SequenceConfigSelectNone_Click);
            // 
            // button_SequenceConfigSelectAll
            // 
            this.button_SequenceConfigSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_SequenceConfigSelectAll.Location = new System.Drawing.Point(0, 0);
            this.button_SequenceConfigSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.button_SequenceConfigSelectAll.Name = "button_SequenceConfigSelectAll";
            this.button_SequenceConfigSelectAll.Size = new System.Drawing.Size(131, 24);
            this.button_SequenceConfigSelectAll.TabIndex = 42;
            this.button_SequenceConfigSelectAll.Text = "Select All";
            this.button_SequenceConfigSelectAll.UseVisualStyleBackColor = true;
            this.button_SequenceConfigSelectAll.Click += new System.EventHandler(this.button_SequenceConfigSelectAll_Click);
            // 
            // comboBox_StationConfig
            // 
            this.comboBox_StationConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_StationConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StationConfig.FormattingEnabled = true;
            this.comboBox_StationConfig.Location = new System.Drawing.Point(3, 16);
            this.comboBox_StationConfig.Name = "comboBox_StationConfig";
            this.comboBox_StationConfig.Size = new System.Drawing.Size(257, 21);
            this.comboBox_StationConfig.TabIndex = 31;
            // 
            // groupBox_StationConfig
            // 
            this.groupBox_StationConfig.Controls.Add(this.comboBox_StationConfig);
            this.groupBox_StationConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_StationConfig.Location = new System.Drawing.Point(3, 33);
            this.groupBox_StationConfig.Name = "groupBox_StationConfig";
            this.groupBox_StationConfig.Size = new System.Drawing.Size(263, 44);
            this.groupBox_StationConfig.TabIndex = 44;
            this.groupBox_StationConfig.TabStop = false;
            this.groupBox_StationConfig.Text = "Station Config";
            // 
            // groupBox_ProductConfig
            // 
            this.groupBox_ProductConfig.Controls.Add(this.comboBox_ProductConfig);
            this.groupBox_ProductConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_ProductConfig.Location = new System.Drawing.Point(272, 33);
            this.groupBox_ProductConfig.Name = "groupBox_ProductConfig";
            this.groupBox_ProductConfig.Size = new System.Drawing.Size(263, 44);
            this.groupBox_ProductConfig.TabIndex = 45;
            this.groupBox_ProductConfig.TabStop = false;
            this.groupBox_ProductConfig.Text = "Product Config";
            // 
            // groupBox_TestConfig
            // 
            this.groupBox_TestConfig.Controls.Add(this.checkedListBox_TestConfig);
            this.groupBox_TestConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_TestConfig.Location = new System.Drawing.Point(272, 83);
            this.groupBox_TestConfig.Name = "groupBox_TestConfig";
            this.groupBox_TestConfig.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox_TestConfig.Size = new System.Drawing.Size(263, 109);
            this.groupBox_TestConfig.TabIndex = 46;
            this.groupBox_TestConfig.TabStop = false;
            this.groupBox_TestConfig.Text = "Test Config";
            // 
            // groupBox_SequenceConfig
            // 
            this.groupBox_SequenceConfig.Controls.Add(this.checkedListBox_SequenceConfig);
            this.groupBox_SequenceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_SequenceConfig.Location = new System.Drawing.Point(3, 83);
            this.groupBox_SequenceConfig.Name = "groupBox_SequenceConfig";
            this.groupBox_SequenceConfig.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox_SequenceConfig.Size = new System.Drawing.Size(263, 109);
            this.groupBox_SequenceConfig.TabIndex = 47;
            this.groupBox_SequenceConfig.TabStop = false;
            this.groupBox_SequenceConfig.Text = "Sequence Config";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel_TestConfigButtons, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.button_ViewEditConfiguration, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel_SequenceConfigButtons, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.groupBox_ProductConfig, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.groupBox_SequenceConfig, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.groupBox_StationConfig, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.groupBox_TestConfig, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(538, 225);
            this.tableLayoutPanel.TabIndex = 48;
            // 
            // tableLayoutPanel_TestConfigButtons
            // 
            this.tableLayoutPanel_TestConfigButtons.ColumnCount = 2;
            this.tableLayoutPanel_TestConfigButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_TestConfigButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_TestConfigButtons.Controls.Add(this.button_TestConfigSelectNone, 1, 0);
            this.tableLayoutPanel_TestConfigButtons.Controls.Add(this.button_TestConfigSelectAll, 0, 0);
            this.tableLayoutPanel_TestConfigButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_TestConfigButtons.Location = new System.Drawing.Point(272, 198);
            this.tableLayoutPanel_TestConfigButtons.Name = "tableLayoutPanel_TestConfigButtons";
            this.tableLayoutPanel_TestConfigButtons.RowCount = 1;
            this.tableLayoutPanel_TestConfigButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_TestConfigButtons.Size = new System.Drawing.Size(263, 24);
            this.tableLayoutPanel_TestConfigButtons.TabIndex = 50;
            // 
            // tableLayoutPanel_SequenceConfigButtons
            // 
            this.tableLayoutPanel_SequenceConfigButtons.ColumnCount = 2;
            this.tableLayoutPanel_SequenceConfigButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_SequenceConfigButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_SequenceConfigButtons.Controls.Add(this.button_SequenceConfigSelectAll, 0, 0);
            this.tableLayoutPanel_SequenceConfigButtons.Controls.Add(this.button_SequenceConfigSelectNone, 1, 0);
            this.tableLayoutPanel_SequenceConfigButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_SequenceConfigButtons.Location = new System.Drawing.Point(3, 198);
            this.tableLayoutPanel_SequenceConfigButtons.Name = "tableLayoutPanel_SequenceConfigButtons";
            this.tableLayoutPanel_SequenceConfigButtons.RowCount = 1;
            this.tableLayoutPanel_SequenceConfigButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_SequenceConfigButtons.Size = new System.Drawing.Size(263, 24);
            this.tableLayoutPanel_SequenceConfigButtons.TabIndex = 49;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(544, 244);
            this.groupBox.TabIndex = 49;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Configuration";
            // 
            // MultiConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "MultiConfigControl";
            this.Size = new System.Drawing.Size(544, 244);
            this.groupBox_StationConfig.ResumeLayout(false);
            this.groupBox_ProductConfig.ResumeLayout(false);
            this.groupBox_TestConfig.ResumeLayout(false);
            this.groupBox_SequenceConfig.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel_TestConfigButtons.ResumeLayout(false);
            this.tableLayoutPanel_SequenceConfigButtons.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button button_ViewEditConfiguration;
        protected System.Windows.Forms.ComboBox comboBox_ProductConfig;
        private System.Windows.Forms.CheckedListBox checkedListBox_SequenceConfig;
        private System.Windows.Forms.CheckedListBox checkedListBox_TestConfig;
        private System.Windows.Forms.Button button_TestConfigSelectAll;
        private System.Windows.Forms.Button button_TestConfigSelectNone;
        private System.Windows.Forms.Button button_SequenceConfigSelectNone;
        private System.Windows.Forms.Button button_SequenceConfigSelectAll;
        protected System.Windows.Forms.ComboBox comboBox_StationConfig;
        private System.Windows.Forms.GroupBox groupBox_StationConfig;
        private System.Windows.Forms.GroupBox groupBox_ProductConfig;
        private System.Windows.Forms.GroupBox groupBox_TestConfig;
        private System.Windows.Forms.GroupBox groupBox_SequenceConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_TestConfigButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_SequenceConfigButtons;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
