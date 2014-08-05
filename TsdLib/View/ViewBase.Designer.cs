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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_ExecuteTestSequence = new System.Windows.Forms.Button();
            this.button_AbortTestSequence = new System.Windows.Forms.Button();
            this.button_StationConfig = new System.Windows.Forms.Button();
            this.button_ProductConfig = new System.Windows.Forms.Button();
            this.button_TestConfig = new System.Windows.Forms.Button();
            this.comboBox_StationConfig = new System.Windows.Forms.ComboBox();
            this.comboBox_ProductConfig = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 421);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Status";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Measurements";
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
            // button_TestConfig
            // 
            this.button_TestConfig.Location = new System.Drawing.Point(402, 70);
            this.button_TestConfig.Name = "button_TestConfig";
            this.button_TestConfig.Size = new System.Drawing.Size(125, 23);
            this.button_TestConfig.TabIndex = 10;
            this.button_TestConfig.Text = "Edit Test Config";
            this.button_TestConfig.UseVisualStyleBackColor = true;
            this.button_TestConfig.Click += new System.EventHandler(this.button_TestConfig_Click);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Station Config";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Product Config";
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
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_ProductConfig);
            this.Controls.Add(this.comboBox_StationConfig);
            this.Controls.Add(this.button_TestConfig);
            this.Controls.Add(this.button_ProductConfig);
            this.Controls.Add(this.measurementDataGridView);
            this.Controls.Add(this.button_StationConfig);
            this.Controls.Add(this.button_AbortTestSequence);
            this.Controls.Add(this.button_ExecuteTestSequence);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        protected System.Windows.Forms.TextBox textBox_Status;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Button button_ExecuteTestSequence;
        protected System.Windows.Forms.Button button_AbortTestSequence;
        protected System.Windows.Forms.Button button_StationConfig;
        protected MeasurementDataGridView measurementDataGridView;
        protected System.Windows.Forms.Button button_ProductConfig;
        protected System.Windows.Forms.Button button_TestConfig;
        private System.Windows.Forms.ComboBox comboBox_StationConfig;
        private System.Windows.Forms.ComboBox comboBox_ProductConfig;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}