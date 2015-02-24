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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_DetailsFromDatabase
            // 
            this.checkBox_DetailsFromDatabase.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox_DetailsFromDatabase.AutoSize = true;
            this.checkBox_DetailsFromDatabase.Checked = true;
            this.checkBox_DetailsFromDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DetailsFromDatabase.Location = new System.Drawing.Point(46, 6);
            this.checkBox_DetailsFromDatabase.Name = "checkBox_DetailsFromDatabase";
            this.checkBox_DetailsFromDatabase.Size = new System.Drawing.Size(130, 17);
            this.checkBox_DetailsFromDatabase.TabIndex = 26;
            this.checkBox_DetailsFromDatabase.Text = "Details from Database";
            this.checkBox_DetailsFromDatabase.UseVisualStyleBackColor = true;
            // 
            // button_EditTestDetails
            // 
            this.button_EditTestDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_EditTestDetails.Location = new System.Drawing.Point(3, 33);
            this.button_EditTestDetails.Name = "button_EditTestDetails";
            this.button_EditTestDetails.Size = new System.Drawing.Size(216, 30);
            this.button_EditTestDetails.TabIndex = 25;
            this.button_EditTestDetails.Text = "Edit Test Details";
            this.button_EditTestDetails.UseVisualStyleBackColor = true;
            this.button_EditTestDetails.Click += new System.EventHandler(this.button_EditTestDetails_Click);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(228, 85);
            this.groupBox.TabIndex = 27;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Test Details";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.checkBox_DetailsFromDatabase, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.button_EditTestDetails, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(222, 66);
            this.tableLayoutPanel.TabIndex = 28;
            // 
            // TestDetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "TestDetailsControl";
            this.Size = new System.Drawing.Size(228, 85);
            this.groupBox.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.CheckBox checkBox_DetailsFromDatabase;
        protected System.Windows.Forms.Button button_EditTestDetails;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
