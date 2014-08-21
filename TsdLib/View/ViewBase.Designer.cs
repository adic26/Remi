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

            Trace.Listeners.Remove(_textBoxTraceListener);
            _textBoxTraceListener.Dispose();
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
            this.button_StationConfig = new System.Windows.Forms.Button();
            this.button_ProductConfig = new System.Windows.Forms.Button();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.label_StationConfig = new System.Windows.Forms.Label();
            this.label_ProductConfig = new System.Windows.Forms.Label();
            this.measurementDataGridView = new TsdLib.View.MeasurementDataGridView();
            this.MeasurementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasuredValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_Status
            // 
            this.textBox_Status.Location = new System.Drawing.Point(12, 437);
            this.textBox_Status.Multiline = true;
            this.textBox_Status.Name = "textBox_Status";
            this.textBox_Status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Status.Size = new System.Drawing.Size(515, 127);
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
            this.label_Measurements.Location = new System.Drawing.Point(12, 127);
            this.label_Measurements.Name = "label_Measurements";
            this.label_Measurements.Size = new System.Drawing.Size(76, 13);
            this.label_Measurements.TabIndex = 3;
            this.label_Measurements.Text = "Measurements";
            // 
            // button_ExecuteTestSequence
            // 
            this.button_ExecuteTestSequence.Location = new System.Drawing.Point(12, 340);
            this.button_ExecuteTestSequence.Name = "button_ExecuteTestSequence";
            this.button_ExecuteTestSequence.Size = new System.Drawing.Size(180, 47);
            this.button_ExecuteTestSequence.TabIndex = 5;
            this.button_ExecuteTestSequence.Text = "Execute Test Sequence";
            this.button_ExecuteTestSequence.UseVisualStyleBackColor = true;
            this.button_ExecuteTestSequence.Click += new System.EventHandler(this.button_ExecuteTestSequence_Click);
            // 
            // button_AbortTestSequence
            // 
            this.button_AbortTestSequence.Location = new System.Drawing.Point(348, 340);
            this.button_AbortTestSequence.Name = "button_AbortTestSequence";
            this.button_AbortTestSequence.Size = new System.Drawing.Size(179, 47);
            this.button_AbortTestSequence.TabIndex = 7;
            this.button_AbortTestSequence.Text = "Abort Test Sequence";
            this.button_AbortTestSequence.UseVisualStyleBackColor = true;
            this.button_AbortTestSequence.Click += new System.EventHandler(this.button_AbortTestSequence_Click);
            // 
            // button_StationConfig
            // 
            this.button_StationConfig.Location = new System.Drawing.Point(402, 12);
            this.button_StationConfig.Name = "button_StationConfig";
            this.button_StationConfig.Size = new System.Drawing.Size(125, 23);
            this.button_StationConfig.TabIndex = 8;
            this.button_StationConfig.Text = "Edit Station Config";
            this.button_StationConfig.UseVisualStyleBackColor = true;
            this.button_StationConfig.Click += new System.EventHandler(this.button_StationConfig_Click);
            // 
            // button_ProductConfig
            // 
            this.button_ProductConfig.Location = new System.Drawing.Point(402, 41);
            this.button_ProductConfig.Name = "button_ProductConfig";
            this.button_ProductConfig.Size = new System.Drawing.Size(125, 23);
            this.button_ProductConfig.TabIndex = 9;
            this.button_ProductConfig.Text = "Edit Product Config";
            this.button_ProductConfig.UseVisualStyleBackColor = true;
            this.button_ProductConfig.Click += new System.EventHandler(this.button_ProductConfig_Click);
            // 
            // comboBox_StationConfig
            // 
            this.comboBox_StationConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StationConfig.FormattingEnabled = true;
            this.comboBox_StationConfig.Location = new System.Drawing.Point(15, 31);
            this.comboBox_StationConfig.Name = "comboBox_StationConfig";
            this.comboBox_StationConfig.Size = new System.Drawing.Size(121, 21);
            this.comboBox_StationConfig.TabIndex = 11;
            // 
            // comboBox_ProductConfig
            // 
            this.comboBox_ProductConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ProductConfig.FormattingEnabled = true;
            this.comboBox_ProductConfig.Location = new System.Drawing.Point(15, 83);
            this.comboBox_ProductConfig.Name = "comboBox_ProductConfig";
            this.comboBox_ProductConfig.Size = new System.Drawing.Size(121, 21);
            this.comboBox_ProductConfig.TabIndex = 12;
            // 
            // label_StationConfig
            // 
            this.label_StationConfig.AutoSize = true;
            this.label_StationConfig.Location = new System.Drawing.Point(12, 15);
            this.label_StationConfig.Name = "label_StationConfig";
            this.label_StationConfig.Size = new System.Drawing.Size(73, 13);
            this.label_StationConfig.TabIndex = 13;
            this.label_StationConfig.Text = "Station Config";
            // 
            // label_ProductConfig
            // 
            this.label_ProductConfig.AutoSize = true;
            this.label_ProductConfig.Location = new System.Drawing.Point(15, 67);
            this.label_ProductConfig.Name = "label_ProductConfig";
            this.label_ProductConfig.Size = new System.Drawing.Size(77, 13);
            this.label_ProductConfig.TabIndex = 14;
            this.label_ProductConfig.Text = "Product Config";
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
            this.measurementDataGridView.Location = new System.Drawing.Point(12, 143);
            this.measurementDataGridView.Name = "measurementDataGridView";
            this.measurementDataGridView.ReadOnly = true;
            this.measurementDataGridView.RowHeadersVisible = false;
            this.measurementDataGridView.Size = new System.Drawing.Size(515, 136);
            this.measurementDataGridView.TabIndex = 6;
            // 
            // MeasurementName
            // 
            this.MeasurementName.HeaderText = "MeasurementName";
            this.MeasurementName.Name = "MeasurementName";
            this.MeasurementName.ReadOnly = true;
            this.MeasurementName.Width = 124;
            // 
            // MeasuredValue
            // 
            this.MeasuredValue.HeaderText = "MeasuredValue";
            this.MeasuredValue.Name = "MeasuredValue";
            this.MeasuredValue.ReadOnly = true;
            this.MeasuredValue.Width = 106;
            // 
            // Units
            // 
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            this.Units.ReadOnly = true;
            this.Units.Width = 56;
            // 
            // LowerLimit
            // 
            this.LowerLimit.HeaderText = "LowerLimit";
            this.LowerLimit.Name = "LowerLimit";
            this.LowerLimit.ReadOnly = true;
            this.LowerLimit.Width = 82;
            // 
            // UpperLimit
            // 
            this.UpperLimit.HeaderText = "UpperLimit";
            this.UpperLimit.Name = "UpperLimit";
            this.UpperLimit.ReadOnly = true;
            this.UpperLimit.Width = 82;
            // 
            // Result
            // 
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Width = 62;
            // 
            // ViewBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 576);
            this.Controls.Add(this.label_ProductConfig);
            this.Controls.Add(this.label_StationConfig);
            this.Controls.Add(this.comboBox_ProductConfig);
            this.Controls.Add(this.comboBox_StationConfig);
            this.Controls.Add(this.button_ProductConfig);
            this.Controls.Add(this.measurementDataGridView);
            this.Controls.Add(this.button_StationConfig);
            this.Controls.Add(this.button_AbortTestSequence);
            this.Controls.Add(this.button_ExecuteTestSequence);
            this.Controls.Add(this.label_Measurements);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.textBox_Status);
            this.Name = "ViewBase";
            this.Text = "TsdLib Generic Test System";
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn MeasurementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasuredValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowerLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpperLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
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
        /// Request to modify the Station Config.
        /// </summary>
        protected System.Windows.Forms.Button button_StationConfig;
        /// <summary>
        /// Displays measurements as they are generated by the Test Sequence.
        /// </summary>
        protected MeasurementDataGridView measurementDataGridView;
        /// <summary>
        /// Request to modify the Product Config.
        /// </summary>
        protected System.Windows.Forms.Button button_ProductConfig;
        private System.Windows.Forms.ComboBox comboBox_StationConfig;
        private System.Windows.Forms.ComboBox comboBox_ProductConfig;
        /// <summary>
        /// Station Config label.
        /// </summary>
        protected System.Windows.Forms.Label label_StationConfig;
        /// <summary>
        /// Product Config label.
        /// </summary>
        protected System.Windows.Forms.Label label_ProductConfig;
    }
}