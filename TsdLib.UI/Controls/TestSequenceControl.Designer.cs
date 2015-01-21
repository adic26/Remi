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
            this.SuspendLayout();
            // 
            // button_Execute
            // 
            this.button_Execute.Location = new System.Drawing.Point(4, 33);
            this.button_Execute.Name = "button_Execute";
            this.button_Execute.Size = new System.Drawing.Size(180, 47);
            this.button_Execute.TabIndex = 5;
            this.button_Execute.Text = "Execute Test Sequence";
            this.button_Execute.UseVisualStyleBackColor = true;
            this.button_Execute.Click += new System.EventHandler(this.button_Execute_Click);
            // 
            // button_Abort
            // 
            this.button_Abort.Location = new System.Drawing.Point(4, 99);
            this.button_Abort.Name = "button_Abort";
            this.button_Abort.Size = new System.Drawing.Size(179, 47);
            this.button_Abort.TabIndex = 7;
            this.button_Abort.Text = "Abort Test Sequence";
            this.button_Abort.UseVisualStyleBackColor = true;
            this.button_Abort.Click += new System.EventHandler(this.button_Abort_Click);
            // 
            // checkBox_PublishResults
            // 
            this.checkBox_PublishResults.AutoSize = true;
            this.checkBox_PublishResults.Checked = true;
            this.checkBox_PublishResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_PublishResults.Location = new System.Drawing.Point(39, 3);
            this.checkBox_PublishResults.Name = "checkBox_PublishResults";
            this.checkBox_PublishResults.Size = new System.Drawing.Size(98, 17);
            this.checkBox_PublishResults.TabIndex = 28;
            this.checkBox_PublishResults.Text = "Publish Results";
            this.checkBox_PublishResults.UseVisualStyleBackColor = true;
            // 
            // TestSequenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_PublishResults);
            this.Controls.Add(this.button_Execute);
            this.Controls.Add(this.button_Abort);
            this.Name = "TestSequenceControl";
            this.Size = new System.Drawing.Size(188, 153);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Execute;
        private System.Windows.Forms.Button button_Abort;
        protected System.Windows.Forms.CheckBox checkBox_PublishResults;
    }
}
