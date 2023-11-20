namespace LAB_EXAM
{
    partial class Form2
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
            this.BtnGenPDF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnGenPDF
            // 
            this.BtnGenPDF.Location = new System.Drawing.Point(168, 24);
            this.BtnGenPDF.Name = "BtnGenPDF";
            this.BtnGenPDF.Size = new System.Drawing.Size(80, 54);
            this.BtnGenPDF.TabIndex = 0;
            this.BtnGenPDF.Text = "GEN PDF";
            this.BtnGenPDF.UseVisualStyleBackColor = true;
            this.BtnGenPDF.Click += new System.EventHandler(this.BtnGenPDF_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnGenPDF);
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnGenPDF;
    }
}