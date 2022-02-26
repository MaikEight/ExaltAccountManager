
namespace ExaltAccountManager
{
    partial class FrmTokenViewer
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
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.lHeadline = new System.Windows.Forms.Label();
            this.Headline = new System.Windows.Forms.Panel();
            this.pHeadline = new System.Windows.Forms.Panel();
            this.pName = new System.Windows.Forms.Panel();
            this.lName = new System.Windows.Forms.Label();
            this.lMail = new System.Windows.Forms.Label();
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pWarning = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.roundPictureBox1 = new ExaltAccountManager.RoundPictureBox();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            this.Headline.SuspendLayout();
            this.pHeadline.SuspendLayout();
            this.pName.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.White;
            this.pTop.Controls.Add(this.pBox);
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Controls.Add(this.lHeadline);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(450, 48);
            this.pTop.TabIndex = 3;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(402, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(48, 48);
            this.pBox.TabIndex = 4;
            this.pBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pBox_Paint);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.ForeColor = System.Drawing.Color.Black;
            this.lHeadline.Location = new System.Drawing.Point(54, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(144, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Token viewer";
            // 
            // Headline
            // 
            this.Headline.Controls.Add(this.pHeadline);
            this.Headline.Location = new System.Drawing.Point(1, 78);
            this.Headline.Name = "Headline";
            this.Headline.Size = new System.Drawing.Size(448, 30);
            this.Headline.TabIndex = 0;
            // 
            // pHeadline
            // 
            this.pHeadline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pHeadline.Controls.Add(this.pName);
            this.pHeadline.Controls.Add(this.lMail);
            this.pHeadline.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeadline.Location = new System.Drawing.Point(0, 0);
            this.pHeadline.Name = "pHeadline";
            this.pHeadline.Size = new System.Drawing.Size(448, 30);
            this.pHeadline.TabIndex = 3;
            // 
            // pName
            // 
            this.pName.Controls.Add(this.lName);
            this.pName.Dock = System.Windows.Forms.DockStyle.Left;
            this.pName.Location = new System.Drawing.Point(0, 0);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(200, 30);
            this.pName.TabIndex = 0;
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(1, 5);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(125, 19);
            this.lName.TabIndex = 0;
            this.lName.Text = "AccountName";
            // 
            // lMail
            // 
            this.lMail.AutoSize = true;
            this.lMail.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMail.Location = new System.Drawing.Point(201, 5);
            this.lMail.Name = "lMail";
            this.lMail.Size = new System.Drawing.Size(64, 19);
            this.lMail.TabIndex = 1;
            this.lMail.Text = "E-Mail";
            // 
            // flow
            // 
            this.flow.AutoScroll = true;
            this.flow.Location = new System.Drawing.Point(1, 108);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(448, 441);
            this.flow.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pWarning);
            this.panel1.Location = new System.Drawing.Point(1, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 30);
            this.panel1.TabIndex = 5;
            // 
            // pWarning
            // 
            this.pWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pWarning.Controls.Add(this.roundPictureBox1);
            this.pWarning.Controls.Add(this.label1);
            this.pWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.pWarning.Location = new System.Drawing.Point(0, 0);
            this.pWarning.Name = "pWarning";
            this.pWarning.Size = new System.Drawing.Size(448, 30);
            this.pWarning.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(369, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please re-new the token if you use a webrequest here";
            // 
            // pbMinimize
            // 
            this.pbMinimize.Image = global::ExaltAccountManager.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(0, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMinimize.TabIndex = 1;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Visible = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMinimize_Paint);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(24, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.Paint += new System.Windows.Forms.PaintEventHandler(this.pbClose_Paint);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_visibility_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // roundPictureBox1
            // 
            this.roundPictureBox1.BackColor = System.Drawing.Color.LightCoral;
            this.roundPictureBox1.Image = global::ExaltAccountManager.Properties.Resources.ic_warning_black_24dp;
            this.roundPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.roundPictureBox1.Name = "roundPictureBox1";
            this.roundPictureBox1.Size = new System.Drawing.Size(30, 30);
            this.roundPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.roundPictureBox1.TabIndex = 1;
            this.roundPictureBox1.TabStop = false;
            // 
            // FrmTokenViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(450, 550);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flow);
            this.Controls.Add(this.Headline);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTokenViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Token viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmTokenViewer_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            this.Headline.ResumeLayout(false);
            this.pHeadline.ResumeLayout(false);
            this.pHeadline.PerformLayout();
            this.pName.ResumeLayout(false);
            this.pName.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pWarning.ResumeLayout(false);
            this.pWarning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.Panel Headline;
        private System.Windows.Forms.Panel pHeadline;
        private System.Windows.Forms.Panel pName;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lMail;
        private System.Windows.Forms.FlowLayoutPanel flow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pWarning;
        private System.Windows.Forms.Label label1;
        private RoundPictureBox roundPictureBox1;
    }
}