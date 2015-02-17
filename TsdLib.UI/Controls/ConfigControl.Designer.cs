namespace TsdLib.UI.Controls
{
    partial class ConfigControl
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
            this.label_StationConfig = new System.Windows.Forms.Label();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_SequenceConfig = new System.Windows.Forms.ComboBox();
            this.label_ProductConfig = new System.Windows.Forms.Label();
            this.comboBox_TestConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.label_SequenceConfig = new System.Windows.Forms.Label();
            this.label_TestConfig = new System.Windows.Forms.Label();
            this.button_ViewEditConfiguration = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_StationConfig
            // 
            this.label_StationConfig.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_StationConfig.AutoSize = true;
            this.label_StationConfig.Location = new System.Drawing.Point(3, 8);
            this.label_StationConfig.Name = "label_StationConfig";
            this.label_StationConfig.Size = new System.Drawing.Size(73, 13);
            this.label_StationConfig.TabIndex = 24;
            this.label_StationConfig.Text = "Station Config";
            // 
            // comboBox_ProductConfig
            // 
            this.comboBox_ProductConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_ProductConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ProductConfig.FormattingEnabled = true;
            this.comboBox_ProductConfig.Location = new System.Drawing.Point(98, 33);
            this.comboBox_ProductConfig.Name = "comboBox_ProductConfig";
            this.comboBox_ProductConfig.Size = new System.Drawing.Size(282, 21);
            this.comboBox_ProductConfig.TabIndex = 23;
            // 
            // comboBox_SequenceConfig
            // 
            this.comboBox_SequenceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_SequenceConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SequenceConfig.FormattingEnabled = true;
            this.comboBox_SequenceConfig.Location = new System.Drawing.Point(98, 63);
            this.comboBox_SequenceConfig.Name = "comboBox_SequenceConfig";
            this.comboBox_SequenceConfig.Size = new System.Drawing.Size(282, 21);
            this.comboBox_SequenceConfig.TabIndex = 28;
            // 
            // label_ProductConfig
            // 
            this.label_ProductConfig.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_ProductConfig.AutoSize = true;
            this.label_ProductConfig.Location = new System.Drawing.Point(3, 38);
            this.label_ProductConfig.Name = "label_ProductConfig";
            this.label_ProductConfig.Size = new System.Drawing.Size(77, 13);
            this.label_ProductConfig.TabIndex = 25;
            this.label_ProductConfig.Text = "Product Config";
            // 
            // comboBox_TestConfig
            // 
            this.comboBox_TestConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_TestConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TestConfig.FormattingEnabled = true;
            this.comboBox_TestConfig.Location = new System.Drawing.Point(98, 93);
            this.comboBox_TestConfig.Name = "comboBox_TestConfig";
            this.comboBox_TestConfig.Size = new System.Drawing.Size(282, 21);
            this.comboBox_TestConfig.TabIndex = 27;
            // 
            // comboBox_StationConfig
            // 
            this.comboBox_StationConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_StationConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StationConfig.FormattingEnabled = true;
            this.comboBox_StationConfig.Location = new System.Drawing.Point(98, 3);
            this.comboBox_StationConfig.Name = "comboBox_StationConfig";
            this.comboBox_StationConfig.Size = new System.Drawing.Size(282, 21);
            this.comboBox_StationConfig.TabIndex = 22;
            // 
            // label_SequenceConfig
            // 
            this.label_SequenceConfig.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_SequenceConfig.AutoSize = true;
            this.label_SequenceConfig.Location = new System.Drawing.Point(3, 68);
            this.label_SequenceConfig.Name = "label_SequenceConfig";
            this.label_SequenceConfig.Size = new System.Drawing.Size(56, 13);
            this.label_SequenceConfig.TabIndex = 29;
            this.label_SequenceConfig.Text = "Sequence";
            // 
            // label_TestConfig
            // 
            this.label_TestConfig.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_TestConfig.AutoSize = true;
            this.label_TestConfig.Location = new System.Drawing.Point(3, 98);
            this.label_TestConfig.Name = "label_TestConfig";
            this.label_TestConfig.Size = new System.Drawing.Size(61, 13);
            this.label_TestConfig.TabIndex = 26;
            this.label_TestConfig.Text = "Test Config";
            // 
            // button_ViewEditConfiguration
            // 
            this.tableLayoutPanel.SetColumnSpan(this.button_ViewEditConfiguration, 2);
            this.button_ViewEditConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ViewEditConfiguration.Location = new System.Drawing.Point(3, 123);
            this.button_ViewEditConfiguration.Name = "button_ViewEditConfiguration";
            this.button_ViewEditConfiguration.Size = new System.Drawing.Size(377, 33);
            this.button_ViewEditConfiguration.TabIndex = 30;
            this.button_ViewEditConfiguration.Text = "View/Edit Configuration";
            this.button_ViewEditConfiguration.UseVisualStyleBackColor = true;
            this.button_ViewEditConfiguration.Click += new System.EventHandler(this.button_ViewEditConfiguration_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.button_ViewEditConfiguration, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label_TestConfig, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label_SequenceConfig, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBox_StationConfig, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBox_TestConfig, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.label_ProductConfig, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBox_SequenceConfig, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBox_ProductConfig, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label_StationConfig, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(383, 159);
            this.tableLayoutPanel.TabIndex = 31;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(389, 178);
            this.groupBox.TabIndex = 32;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Configuration";
            // 
            // ConfigControl
            // 
            this.Controls.Add(this.groupBox);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(389, 178);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Label label_StationConfig;
        protected System.Windows.Forms.ComboBox comboBox_ProductConfig;
        protected System.Windows.Forms.ComboBox comboBox_SequenceConfig;
        protected System.Windows.Forms.Label label_ProductConfig;
        protected System.Windows.Forms.ComboBox comboBox_TestConfig;
        protected System.Windows.Forms.ComboBox comboBox_StationConfig;
        protected System.Windows.Forms.Label label_SequenceConfig;
        protected System.Windows.Forms.Label label_TestConfig;
        protected System.Windows.Forms.Button button_ViewEditConfiguration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox;

        //protected System.Windows.Forms.Button button_ViewEditConfiguration;
    }
}
