namespace EAM_Updater
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Updating Exalt Account Manager";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 77);
            this.progressBar.MarqueeAnimationSpeed = 30;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(263, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 1;
            // 
            // lText
            // 
            this.lText.Location = new System.Drawing.Point(16, 43);
            this.lText.Name = "lText";
            this.lText.Size = new System.Drawing.Size(259, 24);
            this.lText.TabIndex = 2;
            this.lText.Text = "Please wait ...";
            this.lText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(171, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "EAM Updater by Maik8";
            // 
            // pbImage
            // 
            this.pbImage.Image = global::EAM_Updater.Properties.Resources.icons8_update_left_rotation_48px;
            this.pbImage.Location = new System.Drawing.Point(66, 40);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(30, 30);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 4;
            this.pbImage.TabStop = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(287, 117);
            this.ControlBox = false;
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lText);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EAM Updater";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbImage;
    }
}