using System.Diagnostics;

namespace TsdLib.View
{
    partial class ViewBase
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
                components.Dispose();

            Trace.Listeners.Remove(Listener);
            Listener.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_Status = new System.Windows.Forms.TextBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.label_Measurements = new System.Windows.Forms.Label();
            this.button_ExecuteTestSequence = new System.Windows.Forms.Button();
            this.button_AbortTestSequence = new System.Windows.Forms.Button();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.label_StationConfig = new System.Windows.Forms.Label();
            this.label_ProductConfig = new System.Windows.Forms.Label();
            this.label_TestConfig = new System.Windows.Forms.Label();
            this.comboBox_TestConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_SequenceConfig = new System.Windows.Forms.ComboBox();
            this.label_SequenceConfig = new System.Windows.Forms.Label();
            this.button_ViewEditConfiguration = new System.Windows.Forms.Button();
            this.panel_Controls = new System.Windows.Forms.Panel();
            this.button_EditTestDetails = new System.Windows.Forms.Button();
            this.checkBox_GetDetailsFromDatabase = new System.Windows.Forms.CheckBox();
            this.measurementDataGridView = new TsdLib.View.MeasurementDataGridView();
            this.MeasurementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasuredValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Controls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_Status
            // 
            this.textBox_Status.Location = new System.Drawing.Point(12, 437);
            this.textBox_Status.Multiline = true;
            this.textBox_Status.Name = "textBox_Status";
            this.textBox_Status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Status.Size = new System.Drawing.Size(804, 127);
            this.textBox_Status.TabIndex = 0;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(12, 421);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(37, 13);
            this.label_Status.TabIndex = 1;
            this.label_Status.Text = "Status";
            // 
            // label_Measurements
            // 
            this.label_Measurements.AutoSize = true;
            this.label_Measurements.Location = new System.Drawing.Point(12, 189);
            this.label_Measurements.Name = "label_Measurements";
            this.label_Measurements.Size = new System.Drawing.Size(76, 13);
            this.label_Measurements.TabIndex = 3;
            this.label_Measurements.Text = "Measurements";
            // 
            // button_ExecuteTestSequence
            // 
            this.button_ExecuteTestSequence.Location = new System.Drawing.Point(636, 31);
            this.button_ExecuteTestSequence.Name = "button_ExecuteTestSequence";
            this.button_ExecuteTestSequence.Size = new System.Drawing.Size(180, 47);
            this.button_ExecuteTestSequence.TabIndex = 5;
            this.button_ExecuteTestSequence.Text = "Execute Test Sequence";
            this.button_ExecuteTestSequence.UseVisualStyleBackColor = true;
            this.button_ExecuteTestSequence.Click += new System.EventHandler(this.button_ExecuteTestSequence_Click);
            // 
            // button_AbortTestSequence
            // 
            this.button_AbortTestSequence.Location = new System.Drawing.Point(637, 127);
            this.button_AbortTestSequence.Name = "button_AbortTestSequence";
            this.button_AbortTestSequence.Size = new System.Drawing.Size(179, 47);
            this.button_AbortTestSequence.TabIndex = 7;
            this.button_AbortTestSequence.Text = "Abort Test Sequence";
            this.button_AbortTestSequence.UseVisualStyleBackColor = true;
            this.button_AbortTestSequence.Click += new System.EventHandler(this.button_AbortTestSequence_Click);
            // 
            // comboBox_StationConfig
            // 
            this.comboBox_StationConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StationConfig.FormattingEnabled = true;
            this.comboBox_StationConfig.Location = new System.Drawing.Point(11, 29);
            this.comboBox_StationConfig.Name = "comboBox_StationConfig";
            this.comboBox_StationConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_StationConfig.TabIndex = 11;
            // 
            // comboBox_ProductConfig
            // 
            this.comboBox_ProductConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ProductConfig.FormattingEnabled = true;
            this.comboBox_ProductConfig.Location = new System.Drawing.Point(11, 68);
            this.comboBox_ProductConfig.Name = "comboBox_ProductConfig";
            this.comboBox_ProductConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_ProductConfig.TabIndex = 12;
            // 
            // label_StationConfig
            // 
            this.label_StationConfig.AutoSize = true;
            this.label_StationConfig.Location = new System.Drawing.Point(8, 13);
            this.label_StationConfig.Name = "label_StationConfig";
            this.label_StationConfig.Size = new System.Drawing.Size(73, 13);
            this.label_StationConfig.TabIndex = 13;
            this.label_StationConfig.Text = "Station Config";
            // 
            // label_ProductConfig
            // 
            this.label_ProductConfig.AutoSize = true;
            this.label_ProductConfig.Location = new System.Drawing.Point(8, 52);
            this.label_ProductConfig.Name = "label_ProductConfig";
            this.label_ProductConfig.Size = new System.Drawing.Size(77, 13);
            this.label_ProductConfig.TabIndex = 14;
            this.label_ProductConfig.Text = "Product Config";
            // 
            // label_TestConfig
            // 
            this.label_TestConfig.AutoSize = true;
            this.label_TestConfig.Location = new System.Drawing.Point(8, 93);
            this.label_TestConfig.Name = "label_TestConfig";
            this.label_TestConfig.Size = new System.Drawing.Size(61, 13);
            this.label_TestConfig.TabIndex = 16;
            this.label_TestConfig.Text = "Test Config";
            // 
            // comboBox_TestConfig
            // 
            this.comboBox_TestConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TestConfig.FormattingEnabled = true;
            this.comboBox_TestConfig.Location = new System.Drawing.Point(11, 109);
            this.comboBox_TestConfig.Name = "comboBox_TestConfig";
            this.comboBox_TestConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_TestConfig.TabIndex = 17;
            // 
            // comboBox_SequenceConfig
            // 
            this.comboBox_SequenceConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SequenceConfig.FormattingEnabled = true;
            this.comboBox_SequenceConfig.Location = new System.Drawing.Point(11, 149);
            this.comboBox_SequenceConfig.Name = "comboBox_SequenceConfig";
            this.comboBox_SequenceConfig.Size = new System.Drawing.Size(174, 21);
            this.comboBox_SequenceConfig.TabIndex = 19;
            // 
            // label_SequenceConfig
            // 
            this.label_SequenceConfig.AutoSize = true;
            this.label_SequenceConfig.Location = new System.Drawing.Point(8, 133);
            this.label_SequenceConfig.Name = "label_SequenceConfig";
            this.label_SequenceConfig.Size = new System.Drawing.Size(56, 13);
            this.label_SequenceConfig.TabIndex = 20;
            this.label_SequenceConfig.Text = "Sequence";
            // 
            // button_ViewEditConfiguration
            // 
            this.button_ViewEditConfiguration.Location = new System.Drawing.Point(223, 69);
            this.button_ViewEditConfiguration.Name = "button_ViewEditConfiguration";
            this.button_ViewEditConfiguration.Size = new System.Drawing.Size(133, 47);
            this.button_ViewEditConfiguration.TabIndex = 21;
            this.button_ViewEditConfiguration.Text = "View/Edit Configuration";
            this.button_ViewEditConfiguration.UseVisualStyleBackColor = true;
            this.button_ViewEditConfiguration.Click += new System.EventHandler(this.button_ViewEditConfiguration_Click);
            // 
            // panel_Controls
            // 
            this.panel_Controls.Controls.Add(this.checkBox_GetDetailsFromDatabase);
            this.panel_Controls.Controls.Add(this.button_EditTestDetails);
            this.panel_Controls.Controls.Add(this.button_ViewEditConfiguration);
            this.panel_Controls.Controls.Add(this.label_SequenceConfig);
            this.panel_Controls.Controls.Add(this.comboBox_SequenceConfig);
            this.panel_Controls.Controls.Add(this.comboBox_TestConfig);
            this.panel_Controls.Controls.Add(this.label_TestConfig);
            this.panel_Controls.Controls.Add(this.label_ProductConfig);
            this.panel_Controls.Controls.Add(this.label_StationConfig);
            this.panel_Controls.Controls.Add(this.comboBox_ProductConfig);
            this.panel_Controls.Controls.Add(this.comboBox_StationConfig);
            this.panel_Controls.Location = new System.Drawing.Point(4, 2);
            this.panel_Controls.Name = "panel_Controls";
            this.panel_Controls.Size = new System.Drawing.Size(542, 171);
            this.panel_Controls.TabIndex = 22;
            // 
            // button_EditTestDetails
            // 
            this.button_EditTestDetails.Location = new System.Drawing.Point(392, 69);
            this.button_EditTestDetails.Name = "button_EditTestDetails";
            this.button_EditTestDetails.Size = new System.Drawing.Size(133, 47);
            this.button_EditTestDetails.TabIndex = 22;
            this.button_EditTestDetails.Text = "Edit Test Details";
            this.button_EditTestDetails.UseVisualStyleBackColor = true;
            this.button_EditTestDetails.Click += new System.EventHandler(this.button_EditTestDetails_Click);
            // 
            // checkBox_GetDetailsFromDatabase
            // 
            this.checkBox_GetDetailsFromDatabase.AutoSize = true;
            this.checkBox_GetDetailsFromDatabase.Checked = true;
            this.checkBox_GetDetailsFromDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_GetDetailsFromDatabase.Location = new System.Drawing.Point(400, 122);
            this.checkBox_GetDetailsFromDatabase.Name = "checkBox_GetDetailsFromDatabase";
            this.checkBox_GetDetailsFromDatabase.Size = new System.Drawing.Size(115, 17);
            this.checkBox_GetDetailsFromDatabase.TabIndex = 23;
            this.checkBox_GetDetailsFromDatabase.Text = "Get from Database";
            this.checkBox_GetDetailsFromDatabase.UseVisualStyleBackColor = true;
            // 
            // measurementDataGridView
            // 
            this.measurementDataGridView.AllowUserToAddRows = false;
            this.measurementDataGridView.AllowUserToDeleteRows = false;
            this.measurementDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.measurementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measurementDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MeasurementName,
            this.MeasuredValue,
            this.Units,
            this.LowerLimit,
            this.UpperLimit,
            this.Result});
            this.measurementDataGridView.Location = new System.Drawing.Point(12, 205);
            this.measurementDataGridView.Name = "measurementDataGridView";
            this.measurementDataGridView.ReadOnly = true;
            this.measurementDataGridView.RowHeadersVisible = false;
            this.measurementDataGridView.Size = new System.Drawing.Size(804, 197);
            this.measurementDataGridView.TabIndex = 6;
            // 
            // MeasurementName
            // 
            this.MeasurementName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MeasurementName.FillWeight = 200F;
            this.MeasurementName.HeaderText = "MeasurementName";
            this.MeasurementName.Name = "MeasurementName";
            this.MeasurementName.ReadOnly = true;
            // 
            // MeasuredValue
            // 
            this.MeasuredValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MeasuredValue.FillWeight = 300F;
            this.MeasuredValue.HeaderText = "MeasuredValue";
            this.MeasuredValue.Name = "MeasuredValue";
            this.MeasuredValue.ReadOnly = true;
            // 
            // Units
            // 
            this.Units.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Units.FillWeight = 150F;
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            this.Units.ReadOnly = true;
            // 
            // LowerLimit
            // 
            this.LowerLimit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LowerLimit.FillWeight = 200F;
            this.LowerLimit.HeaderText = "LowerLimit";
            this.LowerLimit.Name = "LowerLimit";
            this.LowerLimit.ReadOnly = true;
            // 
            // UpperLimit
            // 
            this.UpperLimit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UpperLimit.FillWeight = 200F;
            this.UpperLimit.HeaderText = "UpperLimit";
            this.UpperLimit.Name = "UpperLimit";
            this.UpperLimit.ReadOnly = true;
            // 
            // Result
            // 
            this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Result.FillWeight = 150F;
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            // 
            // ViewBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(833, 576);
            this.Controls.Add(this.panel_Controls);
            this.Controls.Add(this.measurementDataGridView);
            this.Controls.Add(this.button_AbortTestSequence);
            this.Controls.Add(this.button_ExecuteTestSequence);
            this.Controls.Add(this.label_Measurements);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.textBox_Status);
            this.Name = "ViewBase";
            this.Text = "TsdLib Generic Test System";
            this.panel_Controls.ResumeLayout(false);
            this.panel_Controls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Textbox to show status updates posted by the Controller or Test Sequence.
        /// </summary>
        protected System.Windows.Forms.TextBox textBox_Status;
        /// <summary>
        /// Status label.
        /// </summary>
        protected System.Windows.Forms.Label label_Status;
        /// <summary>
        /// Measurements label.
        /// </summary>
        protected System.Windows.Forms.Label label_Measurements;
        /// <summary>
        /// Request to execute the Test Sequence.
        /// </summary>
        protected System.Windows.Forms.Button button_ExecuteTestSequence;
        /// <summary>
        /// Request to abort the Test Sequence currently in progress.
        /// </summary>
        protected System.Windows.Forms.Button button_AbortTestSequence;
        /// <summary>
        /// Displays measurements as they are generated by the Test Sequence.
        /// </summary>
        protected MeasurementDataGridView measurementDataGridView;
        /// <summary>
        /// Station Config label.
        /// </summary>
        protected System.Windows.Forms.Label label_StationConfig;
        /// <summary>
        /// Product Config label.
        /// </summary>
        protected System.Windows.Forms.Label label_ProductConfig;
        /// <summary>
        /// Test Config label.
        /// </summary>
        protected System.Windows.Forms.Label label_TestConfig;
        /// <summary>
        /// Sequence Config label.
        /// </summary>
        protected System.Windows.Forms.Label label_SequenceConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasurementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasuredValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowerLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpperLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        /// <summary>
        /// ComboBox to select the Station Config instance.
        /// </summary>
        protected System.Windows.Forms.ComboBox comboBox_StationConfig;
        /// <summary>
        /// ComboBox to select the Product Config instance.
        /// </summary>
        protected System.Windows.Forms.ComboBox comboBox_ProductConfig;
        /// <summary>
        /// ComboBox to select the Test Config instance.
        /// </summary>
        protected System.Windows.Forms.ComboBox comboBox_TestConfig;
        /// <summary>
        /// ComboBox to select the Sequence Config instance.
        /// </summary>
        protected System.Windows.Forms.ComboBox comboBox_SequenceConfig;
        /// <summary>
        /// Panel to easily enable or disable controls related to test setup.
        /// </summary>
        protected System.Windows.Forms.Panel panel_Controls;
        /// <summary>
        /// Request to view or modify the test configuration.
        /// </summary>
        protected System.Windows.Forms.Button button_ViewEditConfiguration;
        /// <summary>
        /// Request to edit the test details.
        /// </summary>
        protected System.Windows.Forms.Button button_EditTestDetails;
        /// <summary>
        /// Switch to control EditTestDetails behaviour.
        /// </summary>
        protected System.Windows.Forms.CheckBox checkBox_GetDetailsFromDatabase;
    }
}