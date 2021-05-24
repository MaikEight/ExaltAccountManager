
namespace ExaltAccountManager
{
    partial class MoreToolsUI
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
            this.pStatistics = new System.Windows.Forms.Panel();
            this.lStatistics = new System.Windows.Forms.Label();
            this.pLeft = new System.Windows.Forms.Panel();
            this.pRight = new System.Windows.Forms.Panel();
            this.pBottom = new System.Windows.Forms.Panel();
            this.pChangelog = new System.Windows.Forms.Panel();
            this.lChangelog = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.pAbout = new System.Windows.Forms.Panel();
            this.lAbout = new System.Windows.Forms.Label();
            this.pTokenViewer = new System.Windows.Forms.Panel();
            this.lTokenViewer = new System.Windows.Forms.Label();
            this.pbAbout = new System.Windows.Forms.PictureBox();
            this.pbTokenViewer = new System.Windows.Forms.PictureBox();
            this.pbChangelog = new System.Windows.Forms.PictureBox();
            this.pbStatistics = new System.Windows.Forms.PictureBox();
            this.pStatistics.SuspendLayout();
            this.pChangelog.SuspendLayout();
            this.pAbout.SuspendLayout();
            this.pTokenViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTokenViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChangelog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatistics)).BeginInit();
            this.SuspendLayout();
            // 
            // pStatistics
            // 
            this.pStatistics.Controls.Add(this.pbStatistics);
            this.pStatistics.Controls.Add(this.lStatistics);
            this.pStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.pStatistics.Location = new System.Drawing.Point(1, 1);
            this.pStatistics.Name = "pStatistics";
            this.pStatistics.Size = new System.Drawing.Size(123, 25);
            this.pStatistics.TabIndex = 0;
            this.pStatistics.Click += new System.EventHandler(this.pStatistics_Click);
            this.pStatistics.MouseEnter += new System.EventHandler(this.pStatistics_MouseEnter);
            this.pStatistics.MouseLeave += new System.EventHandler(this.pStatistics_MouseLeave);
            // 
            // lStatistics
            // 
            this.lStatistics.AutoSize = true;
            this.lStatistics.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatistics.Location = new System.Drawing.Point(26, 5);
            this.lStatistics.Name = "lStatistics";
            this.lStatistics.Size = new System.Drawing.Size(94, 15);
            this.lStatistics.TabIndex = 0;
            this.lStatistics.Text = "Statistics Tool";
            this.lStatistics.Click += new System.EventHandler(this.pStatistics_Click);
            this.lStatistics.MouseEnter += new System.EventHandler(this.pStatistics_MouseEnter);
            this.lStatistics.MouseLeave += new System.EventHandler(this.pStatistics_MouseLeave);
            // 
            // pLeft
            // 
            this.pLeft.BackColor = System.Drawing.Color.Black;
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 1);
            this.pLeft.Name = "pLeft";
            this.pLeft.Size = new System.Drawing.Size(1, 102);
            this.pLeft.TabIndex = 1;
            // 
            // pRight
            // 
            this.pRight.BackColor = System.Drawing.Color.Black;
            this.pRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pRight.Location = new System.Drawing.Point(124, 1);
            this.pRight.Name = "pRight";
            this.pRight.Size = new System.Drawing.Size(1, 102);
            this.pRight.TabIndex = 2;
            // 
            // pBottom
            // 
            this.pBottom.BackColor = System.Drawing.Color.Black;
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(1, 102);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(123, 1);
            this.pBottom.TabIndex = 3;
            // 
            // pChangelog
            // 
            this.pChangelog.Controls.Add(this.pbChangelog);
            this.pChangelog.Controls.Add(this.lChangelog);
            this.pChangelog.Dock = System.Windows.Forms.DockStyle.Top;
            this.pChangelog.Location = new System.Drawing.Point(1, 26);
            this.pChangelog.Name = "pChangelog";
            this.pChangelog.Size = new System.Drawing.Size(123, 25);
            this.pChangelog.TabIndex = 4;
            this.pChangelog.Click += new System.EventHandler(this.pChangelog_Click);
            this.pChangelog.MouseEnter += new System.EventHandler(this.pChangelog_MouseEnter);
            this.pChangelog.MouseLeave += new System.EventHandler(this.pChangelog_MouseLeave);
            // 
            // lChangelog
            // 
            this.lChangelog.AutoSize = true;
            this.lChangelog.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lChangelog.Location = new System.Drawing.Point(26, 5);
            this.lChangelog.Name = "lChangelog";
            this.lChangelog.Size = new System.Drawing.Size(71, 15);
            this.lChangelog.TabIndex = 0;
            this.lChangelog.Text = "Changelog";
            this.lChangelog.Click += new System.EventHandler(this.pChangelog_Click);
            this.lChangelog.MouseEnter += new System.EventHandler(this.pChangelog_MouseEnter);
            this.lChangelog.MouseLeave += new System.EventHandler(this.pChangelog_MouseLeave);
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.Black;
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(125, 1);
            this.pTop.TabIndex = 5;
            // 
            // pAbout
            // 
            this.pAbout.Controls.Add(this.pbAbout);
            this.pAbout.Controls.Add(this.lAbout);
            this.pAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.pAbout.Location = new System.Drawing.Point(1, 76);
            this.pAbout.Name = "pAbout";
            this.pAbout.Size = new System.Drawing.Size(123, 25);
            this.pAbout.TabIndex = 6;
            this.pAbout.Click += new System.EventHandler(this.pAbout_Click);
            this.pAbout.MouseEnter += new System.EventHandler(this.pAbout_MouseEnter);
            this.pAbout.MouseLeave += new System.EventHandler(this.pAbout_MouseLeave);
            // 
            // lAbout
            // 
            this.lAbout.AutoSize = true;
            this.lAbout.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAbout.Location = new System.Drawing.Point(26, 5);
            this.lAbout.Name = "lAbout";
            this.lAbout.Size = new System.Drawing.Size(77, 15);
            this.lAbout.TabIndex = 0;
            this.lAbout.Text = "About EAM";
            this.lAbout.Click += new System.EventHandler(this.pAbout_Click);
            this.lAbout.MouseEnter += new System.EventHandler(this.pAbout_MouseEnter);
            this.lAbout.MouseLeave += new System.EventHandler(this.pAbout_MouseLeave);
            // 
            // pTokenViewer
            // 
            this.pTokenViewer.Controls.Add(this.pbTokenViewer);
            this.pTokenViewer.Controls.Add(this.lTokenViewer);
            this.pTokenViewer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTokenViewer.Location = new System.Drawing.Point(1, 51);
            this.pTokenViewer.Name = "pTokenViewer";
            this.pTokenViewer.Size = new System.Drawing.Size(123, 25);
            this.pTokenViewer.TabIndex = 7;
            this.pTokenViewer.Click += new System.EventHandler(this.pTokenViewer_Click);
            this.pTokenViewer.MouseEnter += new System.EventHandler(this.pTokenViewer_MouseEnter);
            this.pTokenViewer.MouseLeave += new System.EventHandler(this.pTokenViewer_MouseLeave);
            // 
            // lTokenViewer
            // 
            this.lTokenViewer.AutoSize = true;
            this.lTokenViewer.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTokenViewer.Location = new System.Drawing.Point(26, 5);
            this.lTokenViewer.Name = "lTokenViewer";
            this.lTokenViewer.Size = new System.Drawing.Size(91, 15);
            this.lTokenViewer.TabIndex = 0;
            this.lTokenViewer.Text = "Token viewer";
            this.lTokenViewer.Click += new System.EventHandler(this.pTokenViewer_Click);
            this.lTokenViewer.MouseEnter += new System.EventHandler(this.pTokenViewer_MouseEnter);
            this.lTokenViewer.MouseLeave += new System.EventHandler(this.pTokenViewer_MouseLeave);
            // 
            // pbAbout
            // 
            this.pbAbout.Image = global::ExaltAccountManager.Properties.Resources.ic_info_outline_black_24dp;
            this.pbAbout.Location = new System.Drawing.Point(1, 0);
            this.pbAbout.Name = "pbAbout";
            this.pbAbout.Size = new System.Drawing.Size(24, 24);
            this.pbAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAbout.TabIndex = 1;
            this.pbAbout.TabStop = false;
            this.pbAbout.Click += new System.EventHandler(this.pAbout_Click);
            this.pbAbout.MouseEnter += new System.EventHandler(this.pAbout_MouseEnter);
            this.pbAbout.MouseLeave += new System.EventHandler(this.pAbout_MouseLeave);
            // 
            // pbTokenViewer
            // 
            this.pbTokenViewer.Image = global::ExaltAccountManager.Properties.Resources.ic_visibility_black_24dp;
            this.pbTokenViewer.Location = new System.Drawing.Point(1, 0);
            this.pbTokenViewer.Name = "pbTokenViewer";
            this.pbTokenViewer.Size = new System.Drawing.Size(24, 24);
            this.pbTokenViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTokenViewer.TabIndex = 1;
            this.pbTokenViewer.TabStop = false;
            this.pbTokenViewer.Click += new System.EventHandler(this.pTokenViewer_Click);
            this.pbTokenViewer.MouseEnter += new System.EventHandler(this.pTokenViewer_MouseEnter);
            this.pbTokenViewer.MouseLeave += new System.EventHandler(this.pTokenViewer_MouseLeave);
            // 
            // pbChangelog
            // 
            this.pbChangelog.Image = global::ExaltAccountManager.Properties.Resources.baseline_history_edu_black_48dp;
            this.pbChangelog.Location = new System.Drawing.Point(1, 0);
            this.pbChangelog.Name = "pbChangelog";
            this.pbChangelog.Size = new System.Drawing.Size(24, 24);
            this.pbChangelog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbChangelog.TabIndex = 1;
            this.pbChangelog.TabStop = false;
            this.pbChangelog.Click += new System.EventHandler(this.pChangelog_Click);
            this.pbChangelog.MouseEnter += new System.EventHandler(this.pChangelog_MouseEnter);
            this.pbChangelog.MouseLeave += new System.EventHandler(this.pChangelog_MouseLeave);
            // 
            // pbStatistics
            // 
            this.pbStatistics.Image = global::ExaltAccountManager.Properties.Resources.ic_assessment_black_24dp;
            this.pbStatistics.Location = new System.Drawing.Point(1, 0);
            this.pbStatistics.Name = "pbStatistics";
            this.pbStatistics.Size = new System.Drawing.Size(24, 24);
            this.pbStatistics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStatistics.TabIndex = 1;
            this.pbStatistics.TabStop = false;
            this.pbStatistics.Click += new System.EventHandler(this.pStatistics_Click);
            this.pbStatistics.MouseEnter += new System.EventHandler(this.pStatistics_MouseEnter);
            this.pbStatistics.MouseLeave += new System.EventHandler(this.pStatistics_MouseLeave);
            // 
            // MoreToolsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pAbout);
            this.Controls.Add(this.pTokenViewer);
            this.Controls.Add(this.pChangelog);
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.pStatistics);
            this.Controls.Add(this.pLeft);
            this.Controls.Add(this.pRight);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MoreToolsUI";
            this.Size = new System.Drawing.Size(125, 103);
            this.pStatistics.ResumeLayout(false);
            this.pStatistics.PerformLayout();
            this.pChangelog.ResumeLayout(false);
            this.pChangelog.PerformLayout();
            this.pAbout.ResumeLayout(false);
            this.pAbout.PerformLayout();
            this.pTokenViewer.ResumeLayout(false);
            this.pTokenViewer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTokenViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChangelog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatistics)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pStatistics;
        private System.Windows.Forms.Label lStatistics;
        private System.Windows.Forms.PictureBox pbStatistics;
        private System.Windows.Forms.Panel pLeft;
        private System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.Panel pChangelog;
        private System.Windows.Forms.PictureBox pbChangelog;
        private System.Windows.Forms.Label lChangelog;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pAbout;
        private System.Windows.Forms.PictureBox pbAbout;
        private System.Windows.Forms.Label lAbout;
        private System.Windows.Forms.Panel pTokenViewer;
        private System.Windows.Forms.PictureBox pbTokenViewer;
        private System.Windows.Forms.Label lTokenViewer;
    }
}
