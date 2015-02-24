namespace TsdLib.Forms
{
    public partial class ConfigItemCreateForm
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
            this.radioButton_LocalOnly = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.radioButton_StoreInDatabase = new System.Windows.Forms.RadioButton();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // radioButton_LocalOnly
            // 
            this.radioButton_LocalOnly.AutoSize = true;
            this.radioButton_LocalOnly.Location = new System.Drawing.Point(12, 93);
            this.radioButton_LocalOnly.Name = "radioButton_LocalOnly";
            this.radioButton_LocalOnly.Size = new System.Drawing.Size(73, 17);
            this.radioButton_LocalOnly.TabIndex = 11;
            this.radioButton_LocalOnly.Text = "Local-only";
            this.radioButton_LocalOnly.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name";
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(116, 131);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 9;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(12, 131);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 8;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // radioButton_StoreInDatabase
            // 
            this.radioButton_StoreInDatabase.AutoSize = true;
            this.radioButton_StoreInDatabase.Checked = true;
            this.radioButton_StoreInDatabase.Location = new System.Drawing.Point(12, 69);
            this.radioButton_StoreInDatabase.Name = "radioButton_StoreInDatabase";
            this.radioButton_StoreInDatabase.Size = new System.Drawing.Size(110, 17);
            this.radioButton_StoreInDatabase.TabIndex = 7;
            this.radioButton_StoreInDatabase.TabStop = true;
            this.radioButton_StoreInDatabase.Text = "Store in Database";
            this.radioButton_StoreInDatabase.UseVisualStyleBackColor = true;
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(12, 28);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(179, 20);
            this.textBox_Name.TabIndex = 6;
            this.textBox_Name.TextChanged += new System.EventHandler(this.textBox_Name_TextChanged);
            // 
            // ConfigItemCreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 167);
            this.Controls.Add(this.radioButton_LocalOnly);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.radioButton_StoreInDatabase);
            this.Controls.Add(this.textBox_Name);
            this.Name = "ConfigItemCreateForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_LocalOnly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.RadioButton radioButton_StoreInDatabase;
        private System.Windows.Forms.TextBox textBox_Name;
    }
}