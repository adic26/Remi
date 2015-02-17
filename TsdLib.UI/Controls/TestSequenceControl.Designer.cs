namespace TsdLib.UI.Controls
{
    partial class TestSequenceControl
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
            this.button_Execute = new System.Windows.Forms.Button();
            this.button_Abort = new System.Windows.Forms.Button();
            this.checkBox_PublishResults = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Execute
            // 
            this.button_Execute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Execute.Location = new System.Drawing.Point(3, 33);
            this.button_Execute.Name = "button_Execute";
            this.button_Execute.Size = new System.Drawing.Size(286, 35);
            this.button_Execute.TabIndex = 5;
            this.button_Execute.Text = "Execute Test Sequence";
            this.button_Execute.UseVisualStyleBackColor = true;
            this.button_Execute.Click += new System.EventHandler(this.button_Execute_Click);
            // 
            // button_Abort
            // 
            this.button_Abort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Abort.Location = new System.Drawing.Point(3, 74);
            this.button_Abort.Name = "button_Abort";
            this.button_Abort.Size = new System.Drawing.Size(286, 35);
            this.button_Abort.TabIndex = 7;
            this.button_Abort.Text = "Abort Test Sequence";
            this.button_Abort.UseVisualStyleBackColor = true;
            this.button_Abort.Click += new System.EventHandler(this.button_Abort_Click);
            // 
            // checkBox_PublishResults
            // 
            this.checkBox_PublishResults.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox_PublishResults.AutoSize = true;
            this.checkBox_PublishResults.Checked = true;
            this.checkBox_PublishResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_PublishResults.Location = new System.Drawing.Point(97, 6);
            this.checkBox_PublishResults.Name = "checkBox_PublishResults";
            this.checkBox_PublishResults.Size = new System.Drawing.Size(98, 17);
            this.checkBox_PublishResults.TabIndex = 28;
            this.checkBox_PublishResults.Text = "Publish Results";
            this.checkBox_PublishResults.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.checkBox_PublishResults, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.button_Abort, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.button_Execute, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(292, 112);
            this.tableLayoutPanel.TabIndex = 29;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(298, 131);
            this.groupBox.TabIndex = 30;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Test Sequence";
            // 
            // TestSequenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "TestSequenceControl";
            this.Size = new System.Drawing.Size(298, 131);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Execute;
        private System.Windows.Forms.Button button_Abort;
        protected System.Windows.Forms.CheckBox checkBox_PublishResults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox;
    }
}
