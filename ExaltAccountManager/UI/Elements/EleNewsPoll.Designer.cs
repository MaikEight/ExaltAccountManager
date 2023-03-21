namespace ExaltAccountManager.UI.Elements
{
    partial class EleNewsPoll
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
            this.lQuestion = new System.Windows.Forms.Label();
            this.pContent = new System.Windows.Forms.Panel();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lQuestion
            // 
            this.lQuestion.AutoSize = true;
            this.lQuestion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(0, 5);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(64, 17);
            this.lQuestion.TabIndex = 1;
            this.lQuestion.Text = "Question";
            this.lQuestion.UseMnemonic = false;
            // 
            // pContent
            // 
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(0, 32);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(471, 218);
            this.pContent.TabIndex = 2;
            // 
            // pSpacer
            // 
            this.pSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSpacer.Location = new System.Drawing.Point(0, 22);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(471, 10);
            this.pSpacer.TabIndex = 3;
            // 
            // EleNewsPoll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.pSpacer);
            this.Controls.Add(this.lQuestion);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleNewsPoll";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Size = new System.Drawing.Size(471, 250);
            this.SizeChanged += new System.EventHandler(this.EleNewsPoll_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lQuestion;
        private System.Windows.Forms.Panel pContent;
        private System.Windows.Forms.Panel pSpacer;
    }
}
