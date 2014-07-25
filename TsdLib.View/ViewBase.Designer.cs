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
            this.textBox_Status = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_ExecuteTestSequence = new System.Windows.Forms.Button();
            this.button_AbortTestSequence = new System.Windows.Forms.Button();
            this.button_Settings = new System.Windows.Forms.Button();
            this.measurementDataGridView1 = new TsdLib.View.MeasurementDataGridView();
            this.MeasurementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasuredValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView1)).BeginInit();
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
            // button_Settings
            // 
            this.button_Settings.Location = new System.Drawing.Point(15, 12);
            this.button_Settings.Name = "button_Settings";
            this.button_Settings.Size = new System.Drawing.Size(75, 23);
            this.button_Settings.TabIndex = 8;
            this.button_Settings.Text = "Settings";
            this.button_Settings.UseVisualStyleBackColor = true;
            this.button_Settings.Click += new System.EventHandler(this.button_Settings_Click);
            // 
            // measurementDataGridView1
            // 
            this.measurementDataGridView1.AllowUserToAddRows = false;
            this.measurementDataGridView1.AllowUserToDeleteRows = false;
            this.measurementDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.measurementDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measurementDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MeasurementName,
            this.MeasuredValue,
            this.Units,
            this.LowerLimit,
            this.UpperLimit,
            this.Result});
            this.measurementDataGridView1.Location = new System.Drawing.Point(12, 143);
            this.measurementDataGridView1.Name = "measurementDataGridView1";
            this.measurementDataGridView1.ReadOnly = true;
            this.measurementDataGridView1.RowHeadersVisible = false;
            this.measurementDataGridView1.Size = new System.Drawing.Size(515, 136);
            this.measurementDataGridView1.TabIndex = 6;
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
            this.Controls.Add(this.measurementDataGridView1);
            this.Controls.Add(this.button_Settings);
            this.Controls.Add(this.button_AbortTestSequence);
            this.Controls.Add(this.button_ExecuteTestSequence);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Status);
            this.Name = "ViewBase";
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_ExecuteTestSequence;
        private System.Windows.Forms.Button button_AbortTestSequence;
        private System.Windows.Forms.Button button_Settings;
        private MeasurementDataGridView measurementDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasurementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasuredValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowerLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpperLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
    }
}