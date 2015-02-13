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
            this.label_SequenceConfig = new System.Windows.Forms.Label();
            this.comboBox_SequenceConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_TestConfig = new System.Windows.Forms.ComboBox();
            this.label_TestConfig = new System.Windows.Forms.Label();
            this.label_ProductConfig = new System.Windows.Forms.Label();
            this.label_StationConfig = new System.Windows.Forms.Label();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.button_ViewEditConfiguration = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_SequenceConfig
            // 
            this.label_SequenceConfig.AutoSize = true;
            this.label_SequenceConfig.Location = new System.Drawing.Point(6, 82);
            this.label_SequenceConfig.Name = "label_SequenceConfig";
            this.label_SequenceConfig.Size = new System.Drawing.Size(56, 13);
            this.label_SequenceConfig.TabIndex = 29;
            this.label_SequenceConfig.Text = "Sequence";
            // 
            // comboBox_SequenceConfig
            // 
            this.comboBox_SequenceConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SequenceConfig.FormattingEnabled = true;
            this.comboBox_SequenceConfig.Location = new System.Drawing.Point(9, 98);
            this.comboBox_SequenceConfig.Name = "comboBox_SequenceConfig";
            this.comboBox_SequenceConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_SequenceConfig.TabIndex = 28;
            // 
            // comboBox_TestConfig
            // 
            this.comboBox_TestConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TestConfig.FormattingEnabled = true;
            this.comboBox_TestConfig.Location = new System.Drawing.Point(9, 138);
            this.comboBox_TestConfig.Name = "comboBox_TestConfig";
            this.comboBox_TestConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_TestConfig.TabIndex = 27;
            // 
            // label_TestConfig
            // 
            this.label_TestConfig.AutoSize = true;
            this.label_TestConfig.Location = new System.Drawing.Point(6, 122);
            this.label_TestConfig.Name = "label_TestConfig";
            this.label_TestConfig.Size = new System.Drawing.Size(61, 13);
            this.label_TestConfig.TabIndex = 26;
            this.label_TestConfig.Text = "Test Config";
            // 
            // label_ProductConfig
            // 
            this.label_ProductConfig.AutoSize = true;
            this.label_ProductConfig.Location = new System.Drawing.Point(6, 42);
            this.label_ProductConfig.Name = "label_ProductConfig";
            this.label_ProductConfig.Size = new System.Drawing.Size(77, 13);
            this.label_ProductConfig.TabIndex = 25;
            this.label_ProductConfig.Text = "Product Config";
            // 
            // label_StationConfig
            // 
            this.label_StationConfig.AutoSize = true;
            this.label_StationConfig.Location = new System.Drawing.Point(6, 3);
            this.label_StationConfig.Name = "label_StationConfig";
            this.label_StationConfig.Size = new System.Drawing.Size(73, 13);
            this.label_StationConfig.TabIndex = 24;
            this.label_StationConfig.Text = "Station Config";
            // 
            // comboBox_ProductConfig
            // 
            this.comboBox_ProductConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ProductConfig.FormattingEnabled = true;
            this.comboBox_ProductConfig.Location = new System.Drawing.Point(9, 58);
            this.comboBox_ProductConfig.Name = "comboBox_ProductConfig";
            this.comboBox_ProductConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_ProductConfig.TabIndex = 23;
            // 
            // comboBox_StationConfig
            // 
            this.comboBox_StationConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StationConfig.FormattingEnabled = true;
            this.comboBox_StationConfig.Location = new System.Drawing.Point(9, 19);
            this.comboBox_StationConfig.Name = "comboBox_StationConfig";
            this.comboBox_StationConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_StationConfig.TabIndex = 22;
            // 
            // button_ViewEditConfiguration
            // 
            this.button_ViewEditConfiguration.Location = new System.Drawing.Point(205, 66);
            this.button_ViewEditConfiguration.Name = "button_ViewEditConfiguration";
            this.button_ViewEditConfiguration.Size = new System.Drawing.Size(109, 44);
            this.button_ViewEditConfiguration.TabIndex = 30;
            this.button_ViewEditConfiguration.Text = "View/Edit Configuration";
            this.button_ViewEditConfiguration.UseVisualStyleBackColor = true;
            this.button_ViewEditConfiguration.Click += new System.EventHandler(this.button_ViewEditConfiguration_Click);
            // 
            // ConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.button_ViewEditConfiguration);
            this.Controls.Add(this.label_SequenceConfig);
            this.Controls.Add(this.comboBox_SequenceConfig);
            this.Controls.Add(this.comboBox_TestConfig);
            this.Controls.Add(this.label_TestConfig);
            this.Controls.Add(this.label_ProductConfig);
            this.Controls.Add(this.label_StationConfig);
            this.Controls.Add(this.comboBox_ProductConfig);
            this.Controls.Add(this.comboBox_StationConfig);
            this.Name = "ConfigControl";
            this.SelectedProductConfig = new TsdLib.Configuration.IProductConfig[0];
            this.SelectedSequenceConfig = new TsdLib.Configuration.ISequenceConfig[0];
            this.SelectedStationConfig = new TsdLib.Configuration.IStationConfig[0];
            this.SelectedTestConfig = new TsdLib.Configuration.ITestConfig[0];
            this.Size = new System.Drawing.Size(320, 162);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //protected System.Windows.Forms.Button button_ViewEditConfiguration;
        protected System.Windows.Forms.Label label_SequenceConfig;
        protected System.Windows.Forms.ComboBox comboBox_SequenceConfig;
        protected System.Windows.Forms.ComboBox comboBox_TestConfig;
        protected System.Windows.Forms.Label label_TestConfig;
        protected System.Windows.Forms.Label label_ProductConfig;
        protected System.Windows.Forms.Label label_StationConfig;
        protected System.Windows.Forms.ComboBox comboBox_ProductConfig;
        protected System.Windows.Forms.ComboBox comboBox_StationConfig;
        protected System.Windows.Forms.Button button_ViewEditConfiguration;
    }
}
