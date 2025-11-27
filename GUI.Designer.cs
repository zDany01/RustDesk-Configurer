namespace RustDesk_Configurer
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.installBtn = new System.Windows.Forms.Button();
            this.descLbl = new System.Windows.Forms.Label();
            this.statusLbl = new System.Windows.Forms.Label();
            this.statusPbr = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // installBtn
            // 
            this.installBtn.Enabled = false;
            this.installBtn.Location = new System.Drawing.Point(12, 75);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(222, 32);
            this.installBtn.TabIndex = 0;
            this.installBtn.Text = "Install";
            this.installBtn.UseVisualStyleBackColor = true;
            // 
            // descLbl
            // 
            this.descLbl.AutoSize = true;
            this.descLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLbl.Location = new System.Drawing.Point(10, 16);
            this.descLbl.Name = "descLbl";
            this.descLbl.Size = new System.Drawing.Size(103, 20);
            this.descLbl.TabIndex = 1;
            this.descLbl.Text = "Server status";
            // 
            // statusLbl
            // 
            this.statusLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.statusLbl.Location = new System.Drawing.Point(118, 11);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(120, 29);
            this.statusLbl.TabIndex = 2;
            this.statusLbl.Text = "Checking...";
            this.statusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusPbr
            // 
            this.statusPbr.Location = new System.Drawing.Point(14, 46);
            this.statusPbr.Name = "statusPbr";
            this.statusPbr.Size = new System.Drawing.Size(220, 23);
            this.statusPbr.TabIndex = 3;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 119);
            this.Controls.Add(this.statusPbr);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.descLbl);
            this.Controls.Add(this.installBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GUI";
            this.Text = "RustDesk Configurer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button installBtn;
        private System.Windows.Forms.Label descLbl;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.ProgressBar statusPbr;
    }
}

