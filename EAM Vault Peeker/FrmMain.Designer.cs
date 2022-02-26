
namespace EAM_Vault_Peeker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pSide = new System.Windows.Forms.Panel();
            this.pButtons = new System.Windows.Forms.Panel();
            this.pSideBar = new System.Windows.Forms.PictureBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnAccountView = new System.Windows.Forms.Button();
            this.btnTotals = new System.Windows.Forms.Button();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.pHeader = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lHeaderStatistics = new System.Windows.Forms.Label();
            this.lEAMHead = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.lTitle = new System.Windows.Forms.Label();
            this.btnSwitchDesign = new System.Windows.Forms.Button();
            this.pRight = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbMaximize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pContent = new System.Windows.Forms.Panel();
            this.bunifuFormDock1 = new Bunifu.UI.WinForms.BunifuFormDock();
            this.pSide.SuspendLayout();
            this.pButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSideBar)).BeginInit();
            this.pHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pTop.SuspendLayout();
            this.pRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMaximize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
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
            this.pSide.Size = new System.Drawing.Size(175, 650);
            this.pSide.TabIndex = 1;
            // 
            // pButtons
            // 
            this.pButtons.Controls.Add(this.pSideBar);
            this.pButtons.Controls.Add(this.btnAbout);
            this.pButtons.Controls.Add(this.btnAccountView);
            this.pButtons.Controls.Add(this.btnTotals);
            this.pButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pButtons.Location = new System.Drawing.Point(0, 66);
            this.pButtons.Name = "pButtons";
            this.pButtons.Size = new System.Drawing.Size(175, 584);
            this.pButtons.TabIndex = 6;
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
            // btnAbout
            // 
            this.btnAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.Location = new System.Drawing.Point(0, 72);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(175, 36);
            this.btnAbout.TabIndex = 6;
            this.btnAbout.Text = "   About";
            this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnAccountView
            // 
            this.btnAccountView.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAccountView.FlatAppearance.BorderSize = 0;
            this.btnAccountView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountView.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // btnTotals
            // 
            this.btnTotals.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTotals.FlatAppearance.BorderSize = 0;
            this.btnTotals.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnTotals.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnTotals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTotals.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTotals.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTotals.Location = new System.Drawing.Point(0, 0);
            this.btnTotals.Name = "btnTotals";
            this.btnTotals.Size = new System.Drawing.Size(175, 36);
            this.btnTotals.TabIndex = 0;
            this.btnTotals.Text = "   Totals";
            this.btnTotals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTotals.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTotals.UseVisualStyleBackColor = true;
            this.btnTotals.Click += new System.EventHandler(this.btnTotals_Click);
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
            this.pHeader.Controls.Add(this.pictureBox1);
            this.pHeader.Controls.Add(this.lHeaderStatistics);
            this.pHeader.Controls.Add(this.lEAMHead);
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(175, 64);
            this.pHeader.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EAM_Vault_Peeker.Properties.Resources.btn_icon_chest_1;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lHeaderStatistics
            // 
            this.lHeaderStatistics.AutoSize = true;
            this.lHeaderStatistics.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeaderStatistics.ForeColor = System.Drawing.Color.White;
            this.lHeaderStatistics.Location = new System.Drawing.Point(60, 24);
            this.lHeaderStatistics.Name = "lHeaderStatistics";
            this.lHeaderStatistics.Size = new System.Drawing.Size(109, 21);
            this.lHeaderStatistics.TabIndex = 4;
            this.lHeaderStatistics.Text = "Vault Peeker";
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
            this.pTop.Size = new System.Drawing.Size(809, 24);
            this.pTop.TabIndex = 2;
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(6, 3);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(78, 21);
            this.lTitle.TabIndex = 5;
            this.lTitle.Text = "Accounts";
            // 
            // btnSwitchDesign
            // 
            this.btnSwitchDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitchDesign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchDesign.Location = new System.Drawing.Point(634, -2);
            this.btnSwitchDesign.Name = "btnSwitchDesign";
            this.btnSwitchDesign.Size = new System.Drawing.Size(103, 28);
            this.btnSwitchDesign.TabIndex = 1;
            this.btnSwitchDesign.Text = "Switch Theme";
            this.btnSwitchDesign.UseVisualStyleBackColor = true;
            this.btnSwitchDesign.Click += new System.EventHandler(this.btnSwitchDesign_Click);
            // 
            // pRight
            // 
            this.pRight.Controls.Add(this.pbMinimize);
            this.pRight.Controls.Add(this.pbMaximize);
            this.pRight.Controls.Add(this.pbClose);
            this.pRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pRight.Location = new System.Drawing.Point(737, 0);
            this.pRight.Name = "pRight";
            this.pRight.Size = new System.Drawing.Size(72, 24);
            this.pRight.TabIndex = 2;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbMinimize.Image = global::EAM_Vault_Peeker.Properties.Resources.baseline_minimize_black_24dp;
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
            // pbMaximize
            // 
            this.pbMaximize.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbMaximize.Image = global::EAM_Vault_Peeker.Properties.Resources.ic_crop_square_black_24dp;
            this.pbMaximize.Location = new System.Drawing.Point(24, 0);
            this.pbMaximize.Name = "pbMaximize";
            this.pbMaximize.Size = new System.Drawing.Size(24, 24);
            this.pbMaximize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMaximize.TabIndex = 4;
            this.pbMaximize.TabStop = false;
            this.pbMaximize.Click += new System.EventHandler(this.pbMaximize_Click);
            this.pbMaximize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMaximize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbClose.Image = global::EAM_Vault_Peeker.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(48, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 2;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pContent
            // 
            this.pContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pContent.BackColor = System.Drawing.Color.White;
            this.pContent.Location = new System.Drawing.Point(175, 24);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(808, 625);
            this.pContent.TabIndex = 3;
            // 
            // bunifuFormDock1
            // 
            this.bunifuFormDock1.AllowFormDragging = true;
            this.bunifuFormDock1.AllowFormDropShadow = true;
            this.bunifuFormDock1.AllowFormResizing = true;
            this.bunifuFormDock1.AllowHidingBottomRegion = true;
            this.bunifuFormDock1.AllowOpacityChangesWhileDragging = false;
            this.bunifuFormDock1.BorderOptions.BottomBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.BottomBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.BottomBorder.ShowBorder = true;
            this.bunifuFormDock1.BorderOptions.LeftBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.LeftBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.LeftBorder.ShowBorder = true;
            this.bunifuFormDock1.BorderOptions.RightBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.RightBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.RightBorder.ShowBorder = true;
            this.bunifuFormDock1.BorderOptions.TopBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.TopBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.TopBorder.ShowBorder = true;
            this.bunifuFormDock1.ContainerControl = this;
            this.bunifuFormDock1.DockingIndicatorsColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(215)))), ((int)(((byte)(233)))));
            this.bunifuFormDock1.DockingIndicatorsOpacity = 0.5D;
            this.bunifuFormDock1.DockingOptions.DockAll = true;
            this.bunifuFormDock1.DockingOptions.DockBottomLeft = true;
            this.bunifuFormDock1.DockingOptions.DockBottomRight = true;
            this.bunifuFormDock1.DockingOptions.DockFullScreen = true;
            this.bunifuFormDock1.DockingOptions.DockLeft = true;
            this.bunifuFormDock1.DockingOptions.DockRight = true;
            this.bunifuFormDock1.DockingOptions.DockTopLeft = true;
            this.bunifuFormDock1.DockingOptions.DockTopRight = true;
            this.bunifuFormDock1.FormDraggingOpacity = 0.9D;
            this.bunifuFormDock1.ParentForm = this;
            this.bunifuFormDock1.ShowCursorChanges = true;
            this.bunifuFormDock1.ShowDockingIndicators = true;
            this.bunifuFormDock1.TitleBarOptions.AllowFormDragging = true;
            this.bunifuFormDock1.TitleBarOptions.BunifuFormDock = this.bunifuFormDock1;
            this.bunifuFormDock1.TitleBarOptions.DoubleClickToExpandWindow = true;
            this.bunifuFormDock1.TitleBarOptions.TitleBarControl = this.pTop;
            this.bunifuFormDock1.TitleBarOptions.UseBackColorOnDockingIndicators = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(18)))), ((int)(((byte)(217)))));
            this.ClientSize = new System.Drawing.Size(984, 650);
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.pTop);
            this.Controls.Add(this.pSide);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(984, 650);
            this.Name = "FrmMain";
            this.Text = "EAM Vault Peeker";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(18)))), ((int)(((byte)(217)))));
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.ResizeEnd += new System.EventHandler(this.FrmMain_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.pSide.ResumeLayout(false);
            this.pButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pSideBar)).EndInit();
            this.pHeader.ResumeLayout(false);
            this.pHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMaximize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pSide;
        private System.Windows.Forms.Panel pButtons;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.PictureBox pSideBar;
        private System.Windows.Forms.Button btnAccountView;
        private System.Windows.Forms.Button btnTotals;
        private System.Windows.Forms.Panel pSpacer;
        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.Label lHeaderStatistics;
        private System.Windows.Forms.Label lEAMHead;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Button btnSwitchDesign;
        private System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbMaximize;
        private System.Windows.Forms.Panel pContent;
        private Bunifu.UI.WinForms.BunifuFormDock bunifuFormDock1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

