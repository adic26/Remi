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
            this.label_ProductConfig = new System.Windows.Forms.Label();
            this.label_StationConfig = new System.Windows.Forms.Label();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.checkedListBox_SequenceConfig = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox_TestConfig = new System.Windows.Forms.CheckedListBox();
            this.label_SequenceConfig = new System.Windows.Forms.Label();
            this.label_TestConfig = new System.Windows.Forms.Label();
            this.button_TestConfigSelectAll = new System.Windows.Forms.Button();
            this.button_TestConfigSelectNone = new System.Windows.Forms.Button();
            this.button_SequenceConfigSelectNone = new System.Windows.Forms.Button();
            this.button_SequenceConfigSelectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_ViewEditConfiguration
            // 
            this.button_ViewEditConfiguration.Location = new System.Drawing.Point(10, 23);
            this.button_ViewEditConfiguration.Name = "button_ViewEditConfiguration";
            this.button_ViewEditConfiguration.Size = new System.Drawing.Size(174, 53);
            this.button_ViewEditConfiguration.TabIndex = 35;
            this.button_ViewEditConfiguration.Text = "View/Edit Configuration";
            this.button_ViewEditConfiguration.UseVisualStyleBackColor = true;
            this.button_ViewEditConfiguration.Click += new System.EventHandler(this.button_ViewEditConfiguration_Click);
            // 
            // label_ProductConfig
            // 
            this.label_ProductConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ProductConfig.AutoSize = true;
            this.label_ProductConfig.Location = new System.Drawing.Point(203, 47);
            this.label_ProductConfig.Name = "label_ProductConfig";
            this.label_ProductConfig.Size = new System.Drawing.Size(77, 13);
            this.label_ProductConfig.TabIndex = 34;
            this.label_ProductConfig.Text = "Product Config";
            // 
            // label_StationConfig
            // 
            this.label_StationConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_StationConfig.AutoSize = true;
            this.label_StationConfig.Location = new System.Drawing.Point(203, 8);
            this.label_StationConfig.Name = "label_StationConfig";
            this.label_StationConfig.Size = new System.Drawing.Size(73, 13);
            this.label_StationConfig.TabIndex = 33;
            this.label_StationConfig.Text = "Station Config";
            // 
            // comboBox_ProductConfig
            // 
            this.comboBox_ProductConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_ProductConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ProductConfig.FormattingEnabled = true;
            this.comboBox_ProductConfig.Location = new System.Drawing.Point(206, 62);
            this.comboBox_ProductConfig.Name = "comboBox_ProductConfig";
            this.comboBox_ProductConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_ProductConfig.TabIndex = 32;
            // 
            // comboBox_StationConfig
            // 
            this.comboBox_StationConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_StationConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StationConfig.FormattingEnabled = true;
            this.comboBox_StationConfig.Location = new System.Drawing.Point(206, 23);
            this.comboBox_StationConfig.Name = "comboBox_StationConfig";
            this.comboBox_StationConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_StationConfig.TabIndex = 31;
            // 
            // checkedListBox_SequenceConfig
            // 
            this.checkedListBox_SequenceConfig.CheckOnClick = true;
            this.checkedListBox_SequenceConfig.FormattingEnabled = true;
            this.checkedListBox_SequenceConfig.Location = new System.Drawing.Point(10, 101);
            this.checkedListBox_SequenceConfig.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.checkedListBox_SequenceConfig.Name = "checkedListBox_SequenceConfig";
            this.checkedListBox_SequenceConfig.Size = new System.Drawing.Size(174, 79);
            this.checkedListBox_SequenceConfig.TabIndex = 36;
            // 
            // checkedListBox_TestConfig
            // 
            this.checkedListBox_TestConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox_TestConfig.CheckOnClick = true;
            this.checkedListBox_TestConfig.FormattingEnabled = true;
            this.checkedListBox_TestConfig.Location = new System.Drawing.Point(206, 101);
            this.checkedListBox_TestConfig.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.checkedListBox_TestConfig.Name = "checkedListBox_TestConfig";
            this.checkedListBox_TestConfig.Size = new System.Drawing.Size(174, 79);
            this.checkedListBox_TestConfig.TabIndex = 37;
            // 
            // label_SequenceConfig
            // 
            this.label_SequenceConfig.AutoSize = true;
            this.label_SequenceConfig.Location = new System.Drawing.Point(7, 86);
            this.label_SequenceConfig.Name = "label_SequenceConfig";
            this.label_SequenceConfig.Size = new System.Drawing.Size(89, 13);
            this.label_SequenceConfig.TabIndex = 38;
            this.label_SequenceConfig.Text = "Sequence Config";
            // 
            // label_TestConfig
            // 
            this.label_TestConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_TestConfig.AutoSize = true;
            this.label_TestConfig.Location = new System.Drawing.Point(203, 86);
            this.label_TestConfig.Name = "label_TestConfig";
            this.label_TestConfig.Size = new System.Drawing.Size(61, 13);
            this.label_TestConfig.TabIndex = 39;
            this.label_TestConfig.Text = "Test Config";
            // 
            // button_TestConfigSelectAll
            // 
            this.button_TestConfigSelectAll.Location = new System.Drawing.Point(206, 182);
            this.button_TestConfigSelectAll.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.button_TestConfigSelectAll.Name = "button_TestConfigSelectAll";
            this.button_TestConfigSelectAll.Size = new System.Drawing.Size(81, 23);
            this.button_TestConfigSelectAll.TabIndex = 40;
            this.button_TestConfigSelectAll.Text = "Select All";
            this.button_TestConfigSelectAll.UseVisualStyleBackColor = true;
            this.button_TestConfigSelectAll.Click += new System.EventHandler(this.button_TestConfigSelectAll_Click);
            // 
            // button_TestConfigSelectNone
            // 
            this.button_TestConfigSelectNone.Location = new System.Drawing.Point(299, 182);
            this.button_TestConfigSelectNone.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.button_TestConfigSelectNone.Name = "button_TestConfigSelectNone";
            this.button_TestConfigSelectNone.Size = new System.Drawing.Size(81, 23);
            this.button_TestConfigSelectNone.TabIndex = 41;
            this.button_TestConfigSelectNone.Text = "Select None";
            this.button_TestConfigSelectNone.UseVisualStyleBackColor = true;
            this.button_TestConfigSelectNone.Click += new System.EventHandler(this.button_TestConfigSelectNone_Click);
            // 
            // button_SequenceConfigSelectNone
            // 
            this.button_SequenceConfigSelectNone.Location = new System.Drawing.Point(103, 182);
            this.button_SequenceConfigSelectNone.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.button_SequenceConfigSelectNone.Name = "button_SequenceConfigSelectNone";
            this.button_SequenceConfigSelectNone.Size = new System.Drawing.Size(81, 23);
            this.button_SequenceConfigSelectNone.TabIndex = 43;
            this.button_SequenceConfigSelectNone.Text = "Select None";
            this.button_SequenceConfigSelectNone.UseVisualStyleBackColor = true;
            this.button_SequenceConfigSelectNone.Click += new System.EventHandler(this.button_SequenceConfigSelectNone_Click);
            // 
            // button_SequenceConfigSelectAll
            // 
            this.button_SequenceConfigSelectAll.Location = new System.Drawing.Point(10, 182);
            this.button_SequenceConfigSelectAll.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.button_SequenceConfigSelectAll.Name = "button_SequenceConfigSelectAll";
            this.button_SequenceConfigSelectAll.Size = new System.Drawing.Size(81, 23);
            this.button_SequenceConfigSelectAll.TabIndex = 42;
            this.button_SequenceConfigSelectAll.Text = "Select All";
            this.button_SequenceConfigSelectAll.UseVisualStyleBackColor = true;
            this.button_SequenceConfigSelectAll.Click += new System.EventHandler(this.button_SequenceConfigSelectAll_Click);
            // 
            // MultiConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_SequenceConfigSelectNone);
            this.Controls.Add(this.button_SequenceConfigSelectAll);
            this.Controls.Add(this.button_TestConfigSelectNone);
            this.Controls.Add(this.button_TestConfigSelectAll);
            this.Controls.Add(this.label_TestConfig);
            this.Controls.Add(this.label_SequenceConfig);
            this.Controls.Add(this.checkedListBox_TestConfig);
            this.Controls.Add(this.checkedListBox_SequenceConfig);
            this.Controls.Add(this.button_ViewEditConfiguration);
            this.Controls.Add(this.label_ProductConfig);
            this.Controls.Add(this.label_StationConfig);
            this.Controls.Add(this.comboBox_ProductConfig);
            this.Controls.Add(this.comboBox_StationConfig);
            this.Name = "MultiConfigControl";
            this.SelectedProductConfig = new TsdLib.Configuration.IProductConfig[0];
            this.SelectedSequenceConfig = new TsdLib.Configuration.ISequenceConfig[0];
            this.SelectedStationConfig = new TsdLib.Configuration.IStationConfig[0];
            this.SelectedTestConfig = new TsdLib.Configuration.ITestConfig[0];
            this.Size = new System.Drawing.Size(387, 213);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button button_ViewEditConfiguration;
        protected System.Windows.Forms.Label label_ProductConfig;
        protected System.Windows.Forms.Label label_StationConfig;
        protected System.Windows.Forms.ComboBox comboBox_ProductConfig;
        protected System.Windows.Forms.ComboBox comboBox_StationConfig;
        private System.Windows.Forms.CheckedListBox checkedListBox_SequenceConfig;
        private System.Windows.Forms.CheckedListBox checkedListBox_TestConfig;
        protected System.Windows.Forms.Label label_SequenceConfig;
        protected System.Windows.Forms.Label label_TestConfig;
        private System.Windows.Forms.Button button_TestConfigSelectAll;
        private System.Windows.Forms.Button button_TestConfigSelectNone;
        private System.Windows.Forms.Button button_SequenceConfigSelectNone;
        private System.Windows.Forms.Button button_SequenceConfigSelectAll;
    }
}
