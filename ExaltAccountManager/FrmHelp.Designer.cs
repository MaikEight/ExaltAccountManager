
namespace ExaltAccountManager
{
    partial class FrmHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHelp));
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.pSozial = new Bunifu.UI.WinForms.BunifuPanel();
            this.lMoreHelp = new System.Windows.Forms.Label();
            this.pSozialInner = new System.Windows.Forms.Panel();
            this.pbDiscord = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbMail = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.linkDiscord = new System.Windows.Forms.LinkLabel();
            this.linkMail = new System.Windows.Forms.LinkLabel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pSozial.SuspendLayout();
            this.pSozialInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMail)).BeginInit();
            this.SuspendLayout();
            // 
            // pTop
            // 
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
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_live_help_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.ForeColor = System.Drawing.Color.Black;
            this.lHeadline.Location = new System.Drawing.Point(54, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(60, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Help";
            // 
            // flow
            // 
            this.flow.Location = new System.Drawing.Point(1, 128);
            this.flow.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.flow.Name = "flow";
            this.flow.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.flow.Size = new System.Drawing.Size(440, 421);
            this.flow.TabIndex = 4;
            // 
            // pSozial
            // 
            this.pSozial.BackgroundColor = System.Drawing.Color.Transparent;
            this.pSozial.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pSozial.BackgroundImage")));
            this.pSozial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pSozial.BorderColor = System.Drawing.Color.Black;
            this.pSozial.BorderRadius = 20;
            this.pSozial.BorderThickness = 2;
            this.pSozial.Controls.Add(this.lMoreHelp);
            this.pSozial.Controls.Add(this.pSozialInner);
            this.pSozial.Location = new System.Drawing.Point(5, 53);
            this.pSozial.Name = "pSozial";
            this.pSozial.ShowBorders = true;
            this.pSozial.Size = new System.Drawing.Size(438, 70);
            this.pSozial.TabIndex = 5;
            // 
            // lMoreHelp
            // 
            this.lMoreHelp.AutoSize = true;
            this.lMoreHelp.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMoreHelp.ForeColor = System.Drawing.Color.Black;
            this.lMoreHelp.Location = new System.Drawing.Point(153, 5);
            this.lMoreHelp.Name = "lMoreHelp";
            this.lMoreHelp.Size = new System.Drawing.Size(133, 19);
            this.lMoreHelp.TabIndex = 3;
            this.lMoreHelp.Text = "For more help ";
            // 
            // pSozialInner
            // 
            this.pSozialInner.Controls.Add(this.pbDiscord);
            this.pSozialInner.Controls.Add(this.pbMail);
            this.pSozialInner.Controls.Add(this.linkDiscord);
            this.pSozialInner.Controls.Add(this.linkMail);
            this.pSozialInner.Location = new System.Drawing.Point(15, 21);
            this.pSozialInner.Name = "pSozialInner";
            this.pSozialInner.Size = new System.Drawing.Size(410, 42);
            this.pSozialInner.TabIndex = 0;
            // 
            // pbDiscord
            // 
            this.pbDiscord.AllowFocused = false;
            this.pbDiscord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbDiscord.AutoSizeHeight = true;
            this.pbDiscord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pbDiscord.BorderRadius = 19;
            this.pbDiscord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDiscord.Image = global::ExaltAccountManager.Properties.Resources.UI_Icon_SocialDiscord_black1;
            this.pbDiscord.IsCircle = true;
            this.pbDiscord.Location = new System.Drawing.Point(0, 2);
            this.pbDiscord.Name = "pbDiscord";
            this.pbDiscord.Size = new System.Drawing.Size(38, 38);
            this.pbDiscord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDiscord.TabIndex = 2;
            this.pbDiscord.TabStop = false;
            this.pbDiscord.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbDiscord.Click += new System.EventHandler(this.pbDiscord_Click);
            this.pbDiscord.MouseEnter += new System.EventHandler(this.pbDiscord_MouseEnter);
            this.pbDiscord.MouseLeave += new System.EventHandler(this.pbDiscord_MouseLeave);
            // 
            // pbMail
            // 
            this.pbMail.AllowFocused = false;
            this.pbMail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbMail.AutoSizeHeight = true;
            this.pbMail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pbMail.BorderRadius = 18;
            this.pbMail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbMail.Image = global::ExaltAccountManager.Properties.Resources.ic_email_black_24dp;
            this.pbMail.IsCircle = true;
            this.pbMail.Location = new System.Drawing.Point(230, 2);
            this.pbMail.Name = "pbMail";
            this.pbMail.Size = new System.Drawing.Size(36, 36);
            this.pbMail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbMail.TabIndex = 4;
            this.pbMail.TabStop = false;
            this.pbMail.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbMail.Click += new System.EventHandler(this.pbMail_Click);
            this.pbMail.MouseEnter += new System.EventHandler(this.pbMail_MouseEnter);
            this.pbMail.MouseLeave += new System.EventHandler(this.pbMail_MouseLeave);
            // 
            // linkDiscord
            // 
            this.linkDiscord.ActiveLinkColor = System.Drawing.Color.Blue;
            this.linkDiscord.AutoSize = true;
            this.linkDiscord.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkDiscord.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkDiscord.Location = new System.Drawing.Point(37, 11);
            this.linkDiscord.Name = "linkDiscord";
            this.linkDiscord.Size = new System.Drawing.Size(135, 19);
            this.linkDiscord.TabIndex = 1;
            this.linkDiscord.TabStop = true;
            this.linkDiscord.Text = "Join my Discord";
            this.linkDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDiscord_LinkClicked);
            this.linkDiscord.MouseEnter += new System.EventHandler(this.pbDiscord_MouseEnter);
            this.linkDiscord.MouseLeave += new System.EventHandler(this.pbDiscord_MouseLeave);
            // 
            // linkMail
            // 
            this.linkMail.ActiveLinkColor = System.Drawing.Color.Blue;
            this.linkMail.AutoSize = true;
            this.linkMail.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkMail.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkMail.Location = new System.Drawing.Point(265, 11);
            this.linkMail.Name = "linkMail";
            this.linkMail.Size = new System.Drawing.Size(145, 19);
            this.linkMail.TabIndex = 3;
            this.linkMail.TabStop = true;
            this.linkMail.Text = "Drop me an email";
            this.linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMail_LinkClicked);
            this.linkMail.MouseEnter += new System.EventHandler(this.pbMail_MouseEnter);
            this.linkMail.MouseLeave += new System.EventHandler(this.pbMail_MouseLeave);
            // 
            // scrollbar
            // 
            this.scrollbar.AllowCursorChanges = true;
            this.scrollbar.AllowHomeEndKeysDetection = false;
            this.scrollbar.AllowIncrementalClickMoves = true;
            this.scrollbar.AllowMouseDownEffects = true;
            this.scrollbar.AllowMouseHoverEffects = true;
            this.scrollbar.AllowScrollingAnimations = true;
            this.scrollbar.AllowScrollKeysDetection = true;
            this.scrollbar.AllowScrollOptionsMenu = true;
            this.scrollbar.AllowShrinkingOnFocusLost = false;
            this.scrollbar.BackgroundColor = System.Drawing.Color.White;
            this.scrollbar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scrollbar.BackgroundImage")));
            this.scrollbar.BindingContainer = this.flow;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 1;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(441, 123);
            this.scrollbar.Maximum = 100;
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.White;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(8, 426);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 0;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 42;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.scrollbar.Value = 0;
            // 
            // FrmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(450, 550);
            this.Controls.Add(this.scrollbar);
            this.Controls.Add(this.pSozial);
            this.Controls.Add(this.flow);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmHelp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Help";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmHelp_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pSozial.ResumeLayout(false);
            this.pSozial.PerformLayout();
            this.pSozialInner.ResumeLayout(false);
            this.pSozialInner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.FlowLayoutPanel flow;
        private Bunifu.UI.WinForms.BunifuPanel pSozial;
        private Bunifu.UI.WinForms.BunifuPictureBox pbDiscord;
        private System.Windows.Forms.LinkLabel linkDiscord;
        private Bunifu.UI.WinForms.BunifuPictureBox pbMail;
        private System.Windows.Forms.LinkLabel linkMail;
        private System.Windows.Forms.Panel pSozialInner;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private System.Windows.Forms.Label lMoreHelp;
    }
}