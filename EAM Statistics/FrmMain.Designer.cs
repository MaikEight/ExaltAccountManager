
namespace EAM_Statistics
{
    partial class FrmMain
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pSide = new System.Windows.Forms.Panel();
            this.pButtons = new System.Windows.Forms.Panel();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.pHeader = new System.Windows.Forms.Panel();
            this.lHeaderStatistics = new System.Windows.Forms.Label();
            this.lEAMHead = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.lTitle = new System.Windows.Forms.Label();
            this.btnSwitchDesign = new System.Windows.Forms.Button();
            this.pRight = new System.Windows.Forms.Panel();
            this.bunifuDragControlPTop = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuDragControlPHeader = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuDragControlPbHeader = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuDragControlLEAMHead = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuDragControlLHeaderStatistics = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuElipseFrm = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.pMain = new System.Windows.Forms.Panel();
            this.bunifuDragControlLTitle = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.pAccountButtons = new System.Windows.Forms.Panel();
            this.pAccountBtnSpacer = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.pSideBar = new System.Windows.Forms.PictureBox();
            this.btnAccountView = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pbHeader = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.pSide.SuspendLayout();
            this.pButtons.SuspendLayout();
            this.pHeader.SuspendLayout();
            this.pTop.SuspendLayout();
            this.pRight.SuspendLayout();
            this.pAccountButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSideBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // pSide
            // 
            this.pSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pSide.Controls.Add(this.pButtons);
            this.pSide.Controls.Add(this.pSpacer);
            this.pSide.Controls.Add(this.pHeader);
            this.pSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pSide.Location = new System.Drawing.Point(0, 0);
            this.pSide.Name = "pSide";
            this.pSide.Size = new System.Drawing.Size(175, 624);
            this.pSide.TabIndex = 0;
            // 
            // pButtons
            // 
            this.pButtons.Controls.Add(this.btnAbout);
            this.pButtons.Controls.Add(this.scrollbar);
            this.pButtons.Controls.Add(this.pAccountButtons);
            this.pButtons.Controls.Add(this.pSideBar);
            this.pButtons.Controls.Add(this.btnAccountView);
            this.pButtons.Controls.Add(this.btnDashboard);
            this.pButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pButtons.Location = new System.Drawing.Point(0, 66);
            this.pButtons.Name = "pButtons";
            this.pButtons.Size = new System.Drawing.Size(175, 558);
            this.pButtons.TabIndex = 6;
            // 
            // pSpacer
            // 
            this.pSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSpacer.Location = new System.Drawing.Point(0, 64);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(175, 2);
            this.pSpacer.TabIndex = 3;
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pHeader.Controls.Add(this.lHeaderStatistics);
            this.pHeader.Controls.Add(this.lEAMHead);
            this.pHeader.Controls.Add(this.pbHeader);
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(175, 64);
            this.pHeader.TabIndex = 2;
            // 
            // lHeaderStatistics
            // 
            this.lHeaderStatistics.AutoSize = true;
            this.lHeaderStatistics.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeaderStatistics.ForeColor = System.Drawing.Color.White;
            this.lHeaderStatistics.Location = new System.Drawing.Point(70, 24);
            this.lHeaderStatistics.Name = "lHeaderStatistics";
            this.lHeaderStatistics.Size = new System.Drawing.Size(83, 21);
            this.lHeaderStatistics.TabIndex = 4;
            this.lHeaderStatistics.Text = "Statistics";
            // 
            // lEAMHead
            // 
            this.lEAMHead.AutoSize = true;
            this.lEAMHead.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEAMHead.ForeColor = System.Drawing.Color.White;
            this.lEAMHead.Location = new System.Drawing.Point(63, 14);
            this.lEAMHead.Name = "lEAMHead";
            this.lEAMHead.Size = new System.Drawing.Size(97, 12);
            this.lEAMHead.TabIndex = 2;
            this.lEAMHead.Text = "Exalt Account Manager";
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pTop.Controls.Add(this.lTitle);
            this.pTop.Controls.Add(this.btnSwitchDesign);
            this.pTop.Controls.Add(this.pRight);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(175, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(620, 24);
            this.pTop.TabIndex = 1;
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(6, 3);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(89, 21);
            this.lTitle.TabIndex = 5;
            this.lTitle.Text = "Dashboard";
            // 
            // btnSwitchDesign
            // 
            this.btnSwitchDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitchDesign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchDesign.Location = new System.Drawing.Point(469, -2);
            this.btnSwitchDesign.Name = "btnSwitchDesign";
            this.btnSwitchDesign.Size = new System.Drawing.Size(103, 28);
            this.btnSwitchDesign.TabIndex = 1;
            this.btnSwitchDesign.Text = "Switch Design";
            this.btnSwitchDesign.UseVisualStyleBackColor = true;
            this.btnSwitchDesign.Click += new System.EventHandler(this.btnSwitchDesign_Click);
            // 
            // pRight
            // 
            this.pRight.Controls.Add(this.pbMinimize);
            this.pRight.Controls.Add(this.pbClose);
            this.pRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pRight.Location = new System.Drawing.Point(572, 0);
            this.pRight.Name = "pRight";
            this.pRight.Size = new System.Drawing.Size(48, 24);
            this.pRight.TabIndex = 2;
            // 
            // bunifuDragControlPTop
            // 
            this.bunifuDragControlPTop.Fixed = true;
            this.bunifuDragControlPTop.Horizontal = true;
            this.bunifuDragControlPTop.TargetControl = this.pTop;
            this.bunifuDragControlPTop.Vertical = true;
            // 
            // bunifuDragControlPHeader
            // 
            this.bunifuDragControlPHeader.Fixed = true;
            this.bunifuDragControlPHeader.Horizontal = true;
            this.bunifuDragControlPHeader.TargetControl = this.pHeader;
            this.bunifuDragControlPHeader.Vertical = true;
            // 
            // bunifuDragControlPbHeader
            // 
            this.bunifuDragControlPbHeader.Fixed = true;
            this.bunifuDragControlPbHeader.Horizontal = true;
            this.bunifuDragControlPbHeader.TargetControl = this.pbHeader;
            this.bunifuDragControlPbHeader.Vertical = true;
            // 
            // bunifuDragControlLEAMHead
            // 
            this.bunifuDragControlLEAMHead.Fixed = true;
            this.bunifuDragControlLEAMHead.Horizontal = true;
            this.bunifuDragControlLEAMHead.TargetControl = this.lEAMHead;
            this.bunifuDragControlLEAMHead.Vertical = true;
            // 
            // bunifuDragControlLHeaderStatistics
            // 
            this.bunifuDragControlLHeaderStatistics.Fixed = true;
            this.bunifuDragControlLHeaderStatistics.Horizontal = true;
            this.bunifuDragControlLHeaderStatistics.TargetControl = this.lHeaderStatistics;
            this.bunifuDragControlLHeaderStatistics.Vertical = true;
            // 
            // bunifuElipseFrm
            // 
            this.bunifuElipseFrm.ElipseRadius = 5;
            this.bunifuElipseFrm.TargetControl = this;
            // 
            // pMain
            // 
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(175, 24);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(620, 600);
            this.pMain.TabIndex = 2;
            // 
            // bunifuDragControlLTitle
            // 
            this.bunifuDragControlLTitle.Fixed = true;
            this.bunifuDragControlLTitle.Horizontal = true;
            this.bunifuDragControlLTitle.TargetControl = this.lTitle;
            this.bunifuDragControlLTitle.Vertical = true;
            // 
            // pAccountButtons
            // 
            this.pAccountButtons.Controls.Add(this.pAccountBtnSpacer);
            this.pAccountButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pAccountButtons.Location = new System.Drawing.Point(0, 72);
            this.pAccountButtons.Name = "pAccountButtons";
            this.pAccountButtons.Size = new System.Drawing.Size(175, 87);
            this.pAccountButtons.TabIndex = 7;
            this.pAccountButtons.Visible = false;
            // 
            // pAccountBtnSpacer
            // 
            this.pAccountBtnSpacer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pAccountBtnSpacer.Location = new System.Drawing.Point(0, 0);
            this.pAccountBtnSpacer.Name = "pAccountBtnSpacer";
            this.pAccountBtnSpacer.Size = new System.Drawing.Size(10, 87);
            this.pAccountBtnSpacer.TabIndex = 8;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbMinimize.Image = global::EAM_Statistics.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(0, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMinimize.TabIndex = 3;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbClose.Image = global::EAM_Statistics.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(24, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 2;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // btnAbout
            // 
            this.btnAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Image = global::EAM_Statistics.Properties.Resources.ic_info_outline_black_24dp;
            this.btnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.Location = new System.Drawing.Point(0, 159);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(175, 36);
            this.btnAbout.TabIndex = 6;
            this.btnAbout.Text = "   About";
            this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // pSideBar
            // 
            this.pSideBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pSideBar.Location = new System.Drawing.Point(0, 3);
            this.pSideBar.Name = "pSideBar";
            this.pSideBar.Size = new System.Drawing.Size(5, 30);
            this.pSideBar.TabIndex = 5;
            this.pSideBar.TabStop = false;
            // 
            // btnAccountView
            // 
            this.btnAccountView.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAccountView.FlatAppearance.BorderSize = 0;
            this.btnAccountView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountView.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccountView.Image = global::EAM_Statistics.Properties.Resources.ic_account_box_black_24dp;
            this.btnAccountView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccountView.Location = new System.Drawing.Point(0, 36);
            this.btnAccountView.Name = "btnAccountView";
            this.btnAccountView.Size = new System.Drawing.Size(175, 36);
            this.btnAccountView.TabIndex = 4;
            this.btnAccountView.Text = "   Accounts";
            this.btnAccountView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccountView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAccountView.UseVisualStyleBackColor = true;
            this.btnAccountView.Click += new System.EventHandler(this.btnAccountView_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.Image = global::EAM_Statistics.Properties.Resources.ic_dashboard_black_24dp;
            this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Location = new System.Drawing.Point(0, 0);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(175, 36);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "   Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // pbHeader
            // 
            this.pbHeader.AllowFocused = false;
            this.pbHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbHeader.AutoSizeHeight = true;
            this.pbHeader.BorderRadius = 0;
            this.pbHeader.Image = global::EAM_Statistics.Properties.Resources.ic_assessment_white_48dp;
            this.pbHeader.IsCircle = true;
            this.pbHeader.Location = new System.Drawing.Point(6, 6);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(48, 48);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHeader.TabIndex = 3;
            this.pbHeader.TabStop = false;
            this.pbHeader.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Square;
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
            this.scrollbar.BackgroundColor = System.Drawing.Color.Silver;
            this.scrollbar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scrollbar.BackgroundImage")));
            this.scrollbar.BindingContainer = null;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 14;
            this.scrollbar.BorderThickness = 1;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(172, -2);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scrollbar.Maximum = 100;
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(3, 560);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 0;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 55;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.scrollbar.Value = 0;
            this.scrollbar.Visible = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(795, 624);
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.pTop);
            this.Controls.Add(this.pSide);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EAM Statistics";
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.pSide.ResumeLayout(false);
            this.pButtons.ResumeLayout(false);
            this.pHeader.ResumeLayout(false);
            this.pHeader.PerformLayout();
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pRight.ResumeLayout(false);
            this.pAccountButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSideBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pSide;
        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Label lEAMHead;
        private Bunifu.UI.WinForms.BunifuPictureBox pbHeader;
        private System.Windows.Forms.Label lHeaderStatistics;
        private System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbMinimize;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControlPTop;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControlPHeader;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControlPbHeader;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControlLEAMHead;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControlLHeaderStatistics;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipseFrm;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel pSpacer;
        private System.Windows.Forms.Button btnSwitchDesign;
        private System.Windows.Forms.Button btnAccountView;
        private System.Windows.Forms.Panel pButtons;
        private System.Windows.Forms.PictureBox pSideBar;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Button btnAbout;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControlLTitle;
        private System.Windows.Forms.Panel pAccountButtons;
        private System.Windows.Forms.Panel pAccountBtnSpacer;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
    }
}

