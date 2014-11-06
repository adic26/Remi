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
            this.columnHeader_PackageName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PackageVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_Packages
            // 
            this.listView_Packages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Packages.CheckBoxes = true;
            this.listView_Packages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_PackageName,
            this.columnHeader_PackageVersion,
            this.columnHeader_Description});
            this.listView_Packages.Location = new System.Drawing.Point(12, 12);
            this.listView_Packages.Name = "listView_Packages";
            this.listView_Packages.Size = new System.Drawing.Size(745, 236);
            this.listView_Packages.TabIndex = 0;
            this.listView_Packages.UseCompatibleStateImageBehavior = false;
            this.listView_Packages.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_PackageName
            // 
            this.columnHeader_PackageName.Text = "Package Name";
            this.columnHeader_PackageName.Width = 185;
            // 
            // columnHeader_PackageVersion
            // 
            this.columnHeader_PackageVersion.Text = "Package Version";
            this.columnHeader_PackageVersion.Width = 134;
            // 
            // columnHeader_Description
            // 
            this.columnHeader_Description.Text = "Description";
            this.columnHeader_Description.Width = 422;
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(12, 254);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(101, 32);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // SelectPackagesForm
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 296);
            this.ControlBox = false;
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.listView_Packages);
            this.Name = "SelectPackagesForm";
            this.Text = "Select Packages To Reference";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_Packages;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.ColumnHeader columnHeader_PackageName;
        private System.Windows.Forms.ColumnHeader columnHeader_PackageVersion;
        private System.Windows.Forms.ColumnHeader columnHeader_Description;
    }
}