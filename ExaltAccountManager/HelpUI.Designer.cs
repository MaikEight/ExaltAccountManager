
namespace ExaltAccountManager
{
    partial class HelpUI
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.pAnswer = new System.Windows.Forms.Panel();
            this.lAnswer = new System.Windows.Forms.Label();
            this.pQuestion = new System.Windows.Forms.Panel();
            this.pbOpen = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lQuestion = new System.Windows.Forms.Label();
            this.pAnswer.SuspendLayout();
            this.pQuestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOpen)).BeginInit();
            this.SuspendLayout();
            // 
            // pAnswer
            // 
            this.pAnswer.Controls.Add(this.lAnswer);
            this.pAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAnswer.Location = new System.Drawing.Point(0, 36);
            this.pAnswer.Name = "pAnswer";
            this.pAnswer.Size = new System.Drawing.Size(425, 164);
            this.pAnswer.TabIndex = 0;
            this.pAnswer.Click += new System.EventHandler(this.pbOpen_Click);
            this.pAnswer.Paint += new System.Windows.Forms.PaintEventHandler(this.pAnswer_Paint);
            this.pAnswer.MouseEnter += new System.EventHandler(this.pbOpen_MouseEnter);
            this.pAnswer.MouseLeave += new System.EventHandler(this.pbOpen_MouseLeave);
            // 
            // lAnswer
            // 
            this.lAnswer.AutoSize = true;
            this.lAnswer.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAnswer.Location = new System.Drawing.Point(5, 5);
            this.lAnswer.MaximumSize = new System.Drawing.Size(410, 0);
            this.lAnswer.Name = "lAnswer";
            this.lAnswer.Size = new System.Drawing.Size(43, 16);
            this.lAnswer.TabIndex = 1;
            this.lAnswer.Text = "label2";
            this.lAnswer.Click += new System.EventHandler(this.pbOpen_Click);
            this.lAnswer.MouseEnter += new System.EventHandler(this.pbOpen_MouseEnter);
            this.lAnswer.MouseLeave += new System.EventHandler(this.pbOpen_MouseLeave);
            // 
            // pQuestion
            // 
            this.pQuestion.Controls.Add(this.pbOpen);
            this.pQuestion.Controls.Add(this.lQuestion);
            this.pQuestion.Dock = System.Windows.Forms.DockStyle.Top;
            this.pQuestion.Location = new System.Drawing.Point(0, 0);
            this.pQuestion.Name = "pQuestion";
            this.pQuestion.Size = new System.Drawing.Size(425, 36);
            this.pQuestion.TabIndex = 1;
            this.pQuestion.Click += new System.EventHandler(this.pbOpen_Click);
            this.pQuestion.MouseEnter += new System.EventHandler(this.pbOpen_MouseEnter);
            this.pQuestion.MouseLeave += new System.EventHandler(this.pbOpen_MouseLeave);
            // 
            // pbOpen
            // 
            this.pbOpen.AllowFocused = false;
            this.pbOpen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbOpen.AutoSizeHeight = true;
            this.pbOpen.BackColor = System.Drawing.Color.White;
            this.pbOpen.BorderRadius = 15;
            this.pbOpen.Image = global::ExaltAccountManager.Properties.Resources.ic_arrow_drop_down_black_36dp;
            this.pbOpen.IsCircle = true;
            this.pbOpen.Location = new System.Drawing.Point(392, 3);
            this.pbOpen.Name = "pbOpen";
            this.pbOpen.Size = new System.Drawing.Size(30, 30);
            this.pbOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbOpen.TabIndex = 2;
            this.pbOpen.TabStop = false;
            this.pbOpen.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbOpen.Click += new System.EventHandler(this.pbOpen_Click);
            this.pbOpen.MouseEnter += new System.EventHandler(this.pbOpen_MouseEnter);
            this.pbOpen.MouseLeave += new System.EventHandler(this.pbOpen_MouseLeave);
            // 
            // lQuestion
            // 
            this.lQuestion.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(3, 1);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(361, 30);
            this.lQuestion.TabIndex = 0;
            this.lQuestion.Text = "Question";
            this.lQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lQuestion.Click += new System.EventHandler(this.pbOpen_Click);
            this.lQuestion.MouseEnter += new System.EventHandler(this.pbOpen_MouseEnter);
            this.lQuestion.MouseLeave += new System.EventHandler(this.pbOpen_MouseLeave);
            // 
            // HelpUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.pAnswer);
            this.Controls.Add(this.pQuestion);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Name = "HelpUI";
            this.Size = new System.Drawing.Size(425, 200);
            this.MouseEnter += new System.EventHandler(this.pbOpen_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.pbOpen_MouseLeave);
            this.pAnswer.ResumeLayout(false);
            this.pAnswer.PerformLayout();
            this.pQuestion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbOpen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pAnswer;
        private System.Windows.Forms.Label lAnswer;
        private System.Windows.Forms.Panel pQuestion;
        private System.Windows.Forms.Label lQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbOpen;
    }
}
