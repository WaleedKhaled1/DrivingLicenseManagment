namespace MyDVLD
{
    partial class frmShowPersonInfo
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
            this.lblForm = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ctrlpersoninfo2 = new MyDVLD.ctrlpersoninfo();
            this.SuspendLayout();
            // 
            // lblForm
            // 
            this.lblForm.AutoSize = true;
            this.lblForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForm.ForeColor = System.Drawing.Color.Red;
            this.lblForm.Location = new System.Drawing.Point(539, 9);
            this.lblForm.Name = "lblForm";
            this.lblForm.Size = new System.Drawing.Size(213, 32);
            this.lblForm.TabIndex = 93;
            this.lblForm.Text = "Person Datails";
            // 
            // button1
            // 
            this.button1.Image = global::MyDVLD.Properties.Resources.Close_32;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(1039, 375);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 36);
            this.button1.TabIndex = 96;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ctrlpersoninfo2
            // 
            this.ctrlpersoninfo2.Location = new System.Drawing.Point(12, 52);
            this.ctrlpersoninfo2.Name = "ctrlpersoninfo2";
            this.ctrlpersoninfo2.Size = new System.Drawing.Size(1162, 317);
            this.ctrlpersoninfo2.TabIndex = 98;
            // 
            // PersonInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 423);
            this.Controls.Add(this.ctrlpersoninfo2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblForm);
            this.Name = "PersonInfo";
            this.Text = "PersonInfo";
            this.Load += new System.EventHandler(this.PersonInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblForm;
        private System.Windows.Forms.Button button1;
        private ctrlpersoninfo ctrlpersoninfo2;
    }
}