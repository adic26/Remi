namespace TsdLibStarterKitInstaller
{
    partial class SelectPackagesForm
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
            this.listView_Packages = new System.Windows.Forms.ListView();
            this.button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_Packages
            // 
            this.listView_Packages.Location = new System.Drawing.Point(45, 60);
            this.listView_Packages.Name = "listView_Packages";
            this.listView_Packages.Size = new System.Drawing.Size(187, 97);
            this.listView_Packages.TabIndex = 0;
            this.listView_Packages.UseCompatibleStateImageBehavior = false;
            this.listView_Packages.View = System.Windows.Forms.View.List;
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(99, 207);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // SelectPackagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.listView_Packages);
            this.Name = "SelectPackagesForm";
            this.Text = "Select Packages To Reference";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_Packages;
        private System.Windows.Forms.Button button_OK;
    }
}