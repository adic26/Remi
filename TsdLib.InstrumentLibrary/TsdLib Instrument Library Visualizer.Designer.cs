namespace TsdLib.InstrumentLibrary
{
    partial class TsdLibInstrumentLibraryVisualizer
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
            this.comboBox_Instrument = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_RefreshInstruments = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox_Instrument
            // 
            this.comboBox_Instrument.FormattingEnabled = true;
            this.comboBox_Instrument.Location = new System.Drawing.Point(70, 97);
            this.comboBox_Instrument.Name = "comboBox_Instrument";
            this.comboBox_Instrument.Size = new System.Drawing.Size(121, 21);
            this.comboBox_Instrument.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Instrument";
            // 
            // button_RefreshInstruments
            // 
            this.button_RefreshInstruments.Location = new System.Drawing.Point(13, 13);
            this.button_RefreshInstruments.Name = "button_RefreshInstruments";
            this.button_RefreshInstruments.Size = new System.Drawing.Size(110, 23);
            this.button_RefreshInstruments.TabIndex = 2;
            this.button_RefreshInstruments.Text = "Refresh Instruments";
            this.button_RefreshInstruments.UseVisualStyleBackColor = true;
            this.button_RefreshInstruments.Click += new System.EventHandler(this.button_RefreshInstruments_Click);
            // 
            // TsdLibInstrumentLibraryVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_RefreshInstruments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_Instrument);
            this.Name = "TsdLibInstrumentLibraryVisualizer";
            this.Text = "TsdLib Instrument Library Visualizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Instrument;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_RefreshInstruments;
    }
}

