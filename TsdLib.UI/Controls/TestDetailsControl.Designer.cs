namespace TsdLib.UI.Controls
{
    partial class TestDetailsControl
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
            this.checkBox_DetailsFromDatabase = new System.Windows.Forms.CheckBox();
            this.button_EditTestDetails = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox_DetailsFromDatabase
            // 
            this.checkBox_DetailsFromDatabase.AutoSize = true;
            this.checkBox_DetailsFromDatabase.Checked = true;
            this.checkBox_DetailsFromDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DetailsFromDatabase.Location = new System.Drawing.Point(5, 7);
            this.checkBox_DetailsFromDatabase.Name = "checkBox_DetailsFromDatabase";
            this.checkBox_DetailsFromDatabase.Size = new System.Drawing.Size(130, 17);
            this.checkBox_DetailsFromDatabase.TabIndex = 26;
            this.checkBox_DetailsFromDatabase.Text = "Details from Database";
            this.checkBox_DetailsFromDatabase.UseVisualStyleBackColor = true;
            // 
            // button_EditTestDetails
            // 
            this.button_EditTestDetails.Location = new System.Drawing.Point(4, 34);
            this.button_EditTestDetails.Name = "button_EditTestDetails";
            this.button_EditTestDetails.Size = new System.Drawing.Size(133, 47);
            this.button_EditTestDetails.TabIndex = 25;
            this.button_EditTestDetails.Text = "Edit Test Details";
            this.button_EditTestDetails.UseVisualStyleBackColor = true;
            this.button_EditTestDetails.Click += new System.EventHandler(this.button_EditTestDetails_Click);
            // 
            // TestDetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_DetailsFromDatabase);
            this.Controls.Add(this.button_EditTestDetails);
            this.Name = "TestDetailsControl";
            this.Size = new System.Drawing.Size(140, 89);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.CheckBox checkBox_DetailsFromDatabase;
        protected System.Windows.Forms.Button button_EditTestDetails;
    }
}
