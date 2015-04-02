namespace TsdLib.Forms
{
    partial class VersionEditorForm
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
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.numericUpDown_Major = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Minor = new System.Windows.Forms.NumericUpDown();
            this.groupBox_Major = new System.Windows.Forms.GroupBox();
            this.groupBox_Minor = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Major)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Minor)).BeginInit();
            this.groupBox_Major.SuspendLayout();
            this.groupBox_Minor.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(12, 98);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(97, 23);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(137, 98);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(94, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_Major
            // 
            this.numericUpDown_Major.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_Major.Location = new System.Drawing.Point(3, 16);
            this.numericUpDown_Major.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_Major.Name = "numericUpDown_Major";
            this.numericUpDown_Major.Size = new System.Drawing.Size(94, 20);
            this.numericUpDown_Major.TabIndex = 3;
            // 
            // numericUpDown_Minor
            // 
            this.numericUpDown_Minor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_Minor.Location = new System.Drawing.Point(3, 16);
            this.numericUpDown_Minor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_Minor.Name = "numericUpDown_Minor";
            this.numericUpDown_Minor.Size = new System.Drawing.Size(107, 20);
            this.numericUpDown_Minor.TabIndex = 4;
            // 
            // groupBox_Major
            // 
            this.groupBox_Major.Controls.Add(this.numericUpDown_Major);
            this.groupBox_Major.Location = new System.Drawing.Point(12, 28);
            this.groupBox_Major.Name = "groupBox_Major";
            this.groupBox_Major.Size = new System.Drawing.Size(100, 42);
            this.groupBox_Major.TabIndex = 5;
            this.groupBox_Major.TabStop = false;
            this.groupBox_Major.Text = "Major";
            // 
            // groupBox_Minor
            // 
            this.groupBox_Minor.Controls.Add(this.numericUpDown_Minor);
            this.groupBox_Minor.Location = new System.Drawing.Point(118, 28);
            this.groupBox_Minor.Name = "groupBox_Minor";
            this.groupBox_Minor.Size = new System.Drawing.Size(113, 43);
            this.groupBox_Minor.TabIndex = 6;
            this.groupBox_Minor.TabStop = false;
            this.groupBox_Minor.Text = "Minor";
            // 
            // PromoteVersionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 133);
            this.Controls.Add(this.groupBox_Minor);
            this.Controls.Add(this.groupBox_Major);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Name = "PromoteVersionForm";
            this.Text = "Enter Target Version";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Major)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Minor)).EndInit();
            this.groupBox_Major.ResumeLayout(false);
            this.groupBox_Minor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.NumericUpDown numericUpDown_Major;
        private System.Windows.Forms.NumericUpDown numericUpDown_Minor;
        private System.Windows.Forms.GroupBox groupBox_Major;
        private System.Windows.Forms.GroupBox groupBox_Minor;
    }
}