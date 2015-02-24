namespace TsdLib.UI.Controls
{
    partial class TestCasesMenuItem
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
            this.toolStripMenuItem_TestCases_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_TestCases_Load = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // toolStripMenuItem_TestCases_SaveAs
            // 
            this.toolStripMenuItem_TestCases_SaveAs.Name = "toolStripMenuItem_TestCases_SaveAs";
            this.toolStripMenuItem_TestCases_SaveAs.Size = new System.Drawing.Size(123, 22);
            this.toolStripMenuItem_TestCases_SaveAs.Text = "&Save As...";
            this.toolStripMenuItem_TestCases_SaveAs.Click += new System.EventHandler(this.toolStripMenuItem_TestCases_SaveAs_Click);
            // 
            // toolStripMenuItem_TestCases_Load
            // 
            this.toolStripMenuItem_TestCases_Load.Name = "toolStripMenuItem_TestCases_Load";
            this.toolStripMenuItem_TestCases_Load.Size = new System.Drawing.Size(123, 22);
            this.toolStripMenuItem_TestCases_Load.Text = "Load";
            // 
            // TestCasesMenuItem
            // 
            this.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_TestCases_SaveAs,
            this.toolStripMenuItem_TestCases_Load});
            this.Text = "&Test Cases";

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_TestCases_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_TestCases_Load;
    }
}
