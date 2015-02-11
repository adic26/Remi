using System.ComponentModel;
using System.Diagnostics;
using TsdLib.Configuration;

namespace TsdLib.UI.Forms
{
    partial class ViewBase<TStationConfig, TProductConfig, TTestConfig>
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

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MeasurementName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasuredValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressControl1 = new TsdLib.UI.Controls.ProgressControl();
            this.multiConfigControl = new TsdLib.UI.Controls.MultiConfigControl<TStationConfig, TProductConfig, TTestConfig>();
            this.traceListenerTextBoxControl = new TsdLib.UI.Controls.TraceListenerTextBoxControl();
            this.testDetailsControl = new TsdLib.UI.Controls.TestDetailsControl();
            this.testSequenceControl = new TsdLib.UI.Controls.TestSequenceControl<TStationConfig, TProductConfig, TTestConfig>();
            this.testInfoDataGridViewControl = new TsdLib.UI.Controls.TestInfoDataGridViewControl();
            this.measurementDataGridViewControl = new TsdLib.UI.Controls.MeasurementDataGridViewControl();
            this.SuspendLayout();
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
            // progressControl1
            // 
            this.progressControl1.Location = new System.Drawing.Point(12, 174);
            this.progressControl1.Name = "progressControl1";
            this.progressControl1.Size = new System.Drawing.Size(257, 61);
            this.progressControl1.TabIndex = 7;
            // 
            // multiConfigControl
            // 
            this.multiConfigControl.Location = new System.Drawing.Point(291, 22);
            this.multiConfigControl.Name = "multiConfigControl";
            this.multiConfigControl.ProductConfigManager = null;
            this.multiConfigControl.SequenceConfigManager = null;
            this.multiConfigControl.Size = new System.Drawing.Size(387, 213);
            this.multiConfigControl.StationConfigManager = null;
            this.multiConfigControl.TabIndex = 6;
            this.multiConfigControl.TestConfigManager = null;
            // 
            // traceListenerTextBoxControl
            // 
            this.traceListenerTextBoxControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.traceListenerTextBoxControl.Location = new System.Drawing.Point(12, 499);
            this.traceListenerTextBoxControl.Name = "traceListenerTextBoxControl";
            this.traceListenerTextBoxControl.Size = new System.Drawing.Size(1032, 236);
            this.traceListenerTextBoxControl.TabIndex = 5;
            // 
            // testDetailsControl
            // 
            this.testDetailsControl.Location = new System.Drawing.Point(57, 50);
            this.testDetailsControl.Name = "testDetailsControl";
            this.testDetailsControl.Size = new System.Drawing.Size(140, 89);
            this.testDetailsControl.TabIndex = 4;
            // 
            // testSequenceControl
            // 
            this.testSequenceControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.testSequenceControl.Location = new System.Drawing.Point(767, 32);
            this.testSequenceControl.Name = "testSequenceControl";
            this.testSequenceControl.SelectedProductConfig = null;
            this.testSequenceControl.SelectedSequenceConfig = null;
            this.testSequenceControl.SelectedStationConfig = null;
            this.testSequenceControl.SelectedTestConfig = null;
            this.testSequenceControl.Size = new System.Drawing.Size(188, 153);
            this.testSequenceControl.TabIndex = 3;
            // 
            // testInfoDataGridViewControl
            // 
            this.testInfoDataGridViewControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testInfoDataGridViewControl.Location = new System.Drawing.Point(12, 241);
            this.testInfoDataGridViewControl.Name = "testInfoDataGridViewControl";
            this.testInfoDataGridViewControl.Size = new System.Drawing.Size(257, 252);
            this.testInfoDataGridViewControl.TabIndex = 2;
            // 
            // measurementDataGridViewControl
            // 
            this.measurementDataGridViewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.measurementDataGridViewControl.DisplayLimitsAndResult = true;
            this.measurementDataGridViewControl.Location = new System.Drawing.Point(291, 241);
            this.measurementDataGridViewControl.Name = "measurementDataGridViewControl";
            this.measurementDataGridViewControl.Size = new System.Drawing.Size(753, 252);
            this.measurementDataGridViewControl.TabIndex = 1;
            // 
            // ViewBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 748);
            this.Controls.Add(this.progressControl1);
            this.Controls.Add(this.multiConfigControl);
            this.Controls.Add(this.traceListenerTextBoxControl);
            this.Controls.Add(this.testDetailsControl);
            this.Controls.Add(this.testSequenceControl);
            this.Controls.Add(this.testInfoDataGridViewControl);
            this.Controls.Add(this.measurementDataGridViewControl);
            this.MinimumSize = new System.Drawing.Size(1072, 751);
            this.Name = "ViewBase";
            this.Text = "TsdLib Generic Test System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewBase_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Displays measurements as they are generated by the Test Sequence.
        /// </summary>
        //protected MeasurementDisplayControlDataGridView measurementDisplayControlDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasurementName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasuredValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowerLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpperLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        protected Controls.MeasurementDataGridViewControl measurementDataGridViewControl;
        protected Controls.TestInfoDataGridViewControl testInfoDataGridViewControl;
        protected Controls.TestSequenceControl<TStationConfig, TProductConfig, TTestConfig> testSequenceControl;
        protected Controls.TestDetailsControl testDetailsControl;
        protected Controls.TraceListenerTextBoxControl traceListenerTextBoxControl;
        protected Controls.MultiConfigControl<TStationConfig, TProductConfig, TTestConfig> multiConfigControl;
        private Controls.ProgressControl progressControl1;
    }
}