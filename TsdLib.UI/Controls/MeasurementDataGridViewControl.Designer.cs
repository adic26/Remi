namespace TsdLib.UI.Controls
{
    partial class MeasurementDataGridViewControl
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Column_MeasurementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_MeasuredValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LowerLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_UpperLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridView);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_MeasurementName,
            this.Column_MeasuredValue,
            this.Column_Units,
            this.Column_LowerLimit,
            this.Column_UpperLimit,
            this.Column_Result});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(680, 269);
            this.dataGridView.TabIndex = 1;
            // 
            // Column_MeasurementName
            // 
            this.Column_MeasurementName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_MeasurementName.FillWeight = 200F;
            this.Column_MeasurementName.HeaderText = "Measurement Name";
            this.Column_MeasurementName.Name = "Column_MeasurementName";
            this.Column_MeasurementName.ReadOnly = true;
            // 
            // Column_MeasuredValue
            // 
            this.Column_MeasuredValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_MeasuredValue.FillWeight = 300F;
            this.Column_MeasuredValue.HeaderText = "Measured Value";
            this.Column_MeasuredValue.Name = "Column_MeasuredValue";
            this.Column_MeasuredValue.ReadOnly = true;
            // 
            // Column_Units
            // 
            this.Column_Units.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Units.FillWeight = 150F;
            this.Column_Units.HeaderText = "Units";
            this.Column_Units.Name = "Column_Units";
            this.Column_Units.ReadOnly = true;
            // 
            // Column_LowerLimit
            // 
            this.Column_LowerLimit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_LowerLimit.FillWeight = 200F;
            this.Column_LowerLimit.HeaderText = "Lower Limit";
            this.Column_LowerLimit.Name = "Column_LowerLimit";
            this.Column_LowerLimit.ReadOnly = true;
            // 
            // Column_UpperLimit
            // 
            this.Column_UpperLimit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_UpperLimit.FillWeight = 200F;
            this.Column_UpperLimit.HeaderText = "Upper Limit";
            this.Column_UpperLimit.Name = "Column_UpperLimit";
            this.Column_UpperLimit.ReadOnly = true;
            // 
            // Column_Result
            // 
            this.Column_Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Result.FillWeight = 150F;
            this.Column_Result.HeaderText = "Result";
            this.Column_Result.Name = "Column_Result";
            this.Column_Result.ReadOnly = true;
            // 
            // MeasurementDisplayControlDataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MeasurementDisplayControlDataGridView";
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_MeasurementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_MeasuredValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LowerLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_UpperLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Result;
    }
}
