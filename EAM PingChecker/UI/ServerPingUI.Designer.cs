
namespace EAM_PingChecker.UI
{
    partial class ServerPingUI
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
            this.components = new System.ComponentModel.Container();
            Bunifu.UI.WinForms.BunifuAnimatorNS.Animation animation4 = new Bunifu.UI.WinForms.BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerPingUI));
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lLoad = new System.Windows.Forms.Label();
            this.pButtons = new System.Windows.Forms.Panel();
            this.lPing = new System.Windows.Forms.Label();
            this.lLoadText = new System.Windows.Forms.Label();
            this.lPingText = new System.Windows.Forms.Label();
            this.lServerName = new System.Windows.Forms.Label();
            this.timerHideShadowSide = new System.Windows.Forms.Timer(this.components);
            this.transition = new Bunifu.UI.WinForms.BunifuTransition(this.components);
            this.timerAllowRefresh = new System.Windows.Forms.Timer(this.components);
            this.timerSwitchFav = new System.Windows.Forms.Timer(this.components);
            this.pbFavorite = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbGraphShown = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbShowGraph = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbRenew = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.separator = new Bunifu.UI.WinForms.BunifuSeparator();
            this.shadow.SuspendLayout();
            this.pButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFavorite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphShown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShowGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRenew)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow.BorderRadius = 5;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.pbFavorite);
            this.shadow.Controls.Add(this.pbGraphShown);
            this.shadow.Controls.Add(this.lLoad);
            this.shadow.Controls.Add(this.pButtons);
            this.shadow.Controls.Add(this.lPing);
            this.shadow.Controls.Add(this.separator);
            this.shadow.Controls.Add(this.lLoadText);
            this.shadow.Controls.Add(this.lPingText);
            this.shadow.Controls.Add(this.lServerName);
            this.transition.SetDecoration(this.shadow, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.shadow.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow.Location = new System.Drawing.Point(0, 0);
            this.shadow.Name = "shadow";
            this.shadow.PanelColor = System.Drawing.Color.White;
            this.shadow.PanelColor2 = System.Drawing.Color.White;
            this.shadow.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow.ShadowDept = 2;
            this.shadow.ShadowDepth = 2;
            this.shadow.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow.ShadowTopLeftVisible = false;
            this.shadow.Size = new System.Drawing.Size(200, 100);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 1;
            this.shadow.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.shadow.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // lLoad
            // 
            this.lLoad.AutoSize = true;
            this.transition.SetDecoration(this.lLoad, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLoad.Location = new System.Drawing.Point(63, 64);
            this.lLoad.Name = "lLoad";
            this.lLoad.Size = new System.Drawing.Size(28, 21);
            this.lLoad.TabIndex = 10;
            this.lLoad.Text = "---";
            this.lLoad.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.lLoad.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // pButtons
            // 
            this.pButtons.BackColor = System.Drawing.Color.White;
            this.pButtons.Controls.Add(this.pbShowGraph);
            this.pButtons.Controls.Add(this.pbRenew);
            this.transition.SetDecoration(this.pButtons, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pButtons.Location = new System.Drawing.Point(168, 38);
            this.pButtons.Name = "pButtons";
            this.pButtons.Size = new System.Drawing.Size(25, 57);
            this.pButtons.TabIndex = 2;
            this.pButtons.Visible = false;
            this.pButtons.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.pButtons.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // lPing
            // 
            this.lPing.AutoSize = true;
            this.transition.SetDecoration(this.lPing, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lPing.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPing.Location = new System.Drawing.Point(62, 40);
            this.lPing.Name = "lPing";
            this.lPing.Size = new System.Drawing.Size(28, 21);
            this.lPing.TabIndex = 9;
            this.lPing.Text = "---";
            this.lPing.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.lPing.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // lLoadText
            // 
            this.lLoadText.AutoSize = true;
            this.transition.SetDecoration(this.lLoadText, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lLoadText.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLoadText.Location = new System.Drawing.Point(9, 64);
            this.lLoadText.Name = "lLoadText";
            this.lLoadText.Size = new System.Drawing.Size(46, 21);
            this.lLoadText.TabIndex = 8;
            this.lLoadText.Text = "Load";
            this.lLoadText.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.lLoadText.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // lPingText
            // 
            this.lPingText.AutoSize = true;
            this.transition.SetDecoration(this.lPingText, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPingText.Location = new System.Drawing.Point(9, 38);
            this.lPingText.Name = "lPingText";
            this.lPingText.Size = new System.Drawing.Size(45, 21);
            this.lPingText.TabIndex = 7;
            this.lPingText.Text = "Ping";
            this.lPingText.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.lPingText.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // lServerName
            // 
            this.lServerName.AutoSize = true;
            this.transition.SetDecoration(this.lServerName, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lServerName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lServerName.Location = new System.Drawing.Point(7, 7);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(122, 25);
            this.lServerName.TabIndex = 6;
            this.lServerName.Text = "ServerName";
            this.lServerName.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.lServerName.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // timerHideShadowSide
            // 
            this.timerHideShadowSide.Interval = 10;
            this.timerHideShadowSide.Tick += new System.EventHandler(this.timerHideShadowSide_Tick);
            // 
            // transition
            // 
            this.transition.AnimationType = Bunifu.UI.WinForms.BunifuAnimatorNS.AnimationType.HorizSlide;
            this.transition.Cursor = null;
            animation4.AnimateOnlyDifferences = true;
            animation4.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.BlindCoeff")));
            animation4.LeafCoeff = 0F;
            animation4.MaxTime = 1F;
            animation4.MinTime = 0F;
            animation4.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.MosaicCoeff")));
            animation4.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation4.MosaicShift")));
            animation4.MosaicSize = 0;
            animation4.Padding = new System.Windows.Forms.Padding(0);
            animation4.RotateCoeff = 0F;
            animation4.RotateLimit = 0F;
            animation4.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.ScaleCoeff")));
            animation4.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.SlideCoeff")));
            animation4.TimeCoeff = 0F;
            animation4.TransparencyCoeff = 0F;
            this.transition.DefaultAnimation = animation4;
            this.transition.MaxAnimationTime = 1000;
            this.transition.TimeStep = 0.035F;
            // 
            // timerAllowRefresh
            // 
            this.timerAllowRefresh.Interval = 3000;
            this.timerAllowRefresh.Tick += new System.EventHandler(this.timerAllowRefresh_Tick);
            // 
            // timerSwitchFav
            // 
            this.timerSwitchFav.Interval = 15;
            this.timerSwitchFav.Tick += new System.EventHandler(this.timerSwitchFav_Tick);
            // 
            // pbFavorite
            // 
            this.pbFavorite.AllowFocused = false;
            this.pbFavorite.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbFavorite.AutoSizeHeight = true;
            this.pbFavorite.BorderRadius = 12;
            this.transition.SetDecoration(this.pbFavorite, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbFavorite.Image = global::EAM_PingChecker.Properties.Resources.ic_star_border_black_24dp;
            this.pbFavorite.IsCircle = true;
            this.pbFavorite.Location = new System.Drawing.Point(168, 8);
            this.pbFavorite.Name = "pbFavorite";
            this.pbFavorite.Size = new System.Drawing.Size(24, 24);
            this.pbFavorite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFavorite.TabIndex = 12;
            this.pbFavorite.TabStop = false;
            this.pbFavorite.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbFavorite.Click += new System.EventHandler(this.pbFavorite_Click);
            this.pbFavorite.MouseEnter += new System.EventHandler(this.pbFavorite_MouseEnter);
            this.pbFavorite.MouseLeave += new System.EventHandler(this.pbFavorite_MouseLeave);
            // 
            // pbGraphShown
            // 
            this.pbGraphShown.AllowFocused = false;
            this.pbGraphShown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbGraphShown.AutoSizeHeight = true;
            this.pbGraphShown.BorderRadius = 10;
            this.transition.SetDecoration(this.pbGraphShown, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbGraphShown.Image = global::EAM_PingChecker.Properties.Resources.ic_expand_more_black_36dp;
            this.pbGraphShown.IsCircle = true;
            this.pbGraphShown.Location = new System.Drawing.Point(90, 78);
            this.pbGraphShown.Name = "pbGraphShown";
            this.pbGraphShown.Size = new System.Drawing.Size(20, 20);
            this.pbGraphShown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGraphShown.TabIndex = 11;
            this.pbGraphShown.TabStop = false;
            this.pbGraphShown.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbGraphShown.Visible = false;
            // 
            // pbShowGraph
            // 
            this.pbShowGraph.AllowFocused = false;
            this.pbShowGraph.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbShowGraph.AutoSizeHeight = true;
            this.pbShowGraph.BackColor = System.Drawing.Color.White;
            this.pbShowGraph.BorderRadius = 12;
            this.transition.SetDecoration(this.pbShowGraph, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbShowGraph.Image = global::EAM_PingChecker.Properties.Resources.icons8_ecg_24px;
            this.pbShowGraph.IsCircle = true;
            this.pbShowGraph.Location = new System.Drawing.Point(0, 27);
            this.pbShowGraph.Name = "pbShowGraph";
            this.pbShowGraph.Size = new System.Drawing.Size(24, 24);
            this.pbShowGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbShowGraph.TabIndex = 11;
            this.pbShowGraph.TabStop = false;
            this.pbShowGraph.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbShowGraph.Click += new System.EventHandler(this.pbShowGraph_Click);
            this.pbShowGraph.MouseEnter += new System.EventHandler(this.pbShowGraph_MouseEnter);
            this.pbShowGraph.MouseLeave += new System.EventHandler(this.pbShowGraph_MouseLeave);
            // 
            // pbRenew
            // 
            this.pbRenew.AllowFocused = false;
            this.pbRenew.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbRenew.AutoSizeHeight = true;
            this.pbRenew.BorderRadius = 12;
            this.transition.SetDecoration(this.pbRenew, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbRenew.Image = global::EAM_PingChecker.Properties.Resources.ic_cached_black_24dp;
            this.pbRenew.IsCircle = true;
            this.pbRenew.Location = new System.Drawing.Point(0, 0);
            this.pbRenew.Name = "pbRenew";
            this.pbRenew.Size = new System.Drawing.Size(24, 24);
            this.pbRenew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbRenew.TabIndex = 2;
            this.pbRenew.TabStop = false;
            this.pbRenew.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbRenew.Click += new System.EventHandler(this.pbRenew_Click);
            this.pbRenew.MouseEnter += new System.EventHandler(this.pbRenew_MouseEnter);
            this.pbRenew.MouseLeave += new System.EventHandler(this.pbRenew_MouseLeave);
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.Color.Transparent;
            this.separator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("separator.BackgroundImage")));
            this.separator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.separator.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.transition.SetDecoration(this.separator, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.separator.LineColor = System.Drawing.Color.Silver;
            this.separator.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.separator.LineThickness = 1;
            this.separator.Location = new System.Drawing.Point(10, 30);
            this.separator.Margin = new System.Windows.Forms.Padding(4);
            this.separator.Name = "separator";
            this.separator.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.separator.Size = new System.Drawing.Size(180, 5);
            this.separator.TabIndex = 2;
            this.separator.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.separator.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            // 
            // ServerPingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.transition.SetDecoration(this, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.Name = "ServerPingUI";
            this.Size = new System.Drawing.Size(200, 100);
            this.MouseEnter += new System.EventHandler(this.ServerPingUI_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ServerPingUI_MouseLeave);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            this.pButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbFavorite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraphShown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShowGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRenew)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private System.Windows.Forms.Panel pButtons;
        private Bunifu.UI.WinForms.BunifuPictureBox pbShowGraph;
        private Bunifu.UI.WinForms.BunifuPictureBox pbRenew;
        private System.Windows.Forms.Label lLoad;
        private System.Windows.Forms.Label lPing;
        private Bunifu.UI.WinForms.BunifuSeparator separator;
        private System.Windows.Forms.Label lLoadText;
        private System.Windows.Forms.Label lPingText;
        private System.Windows.Forms.Label lServerName;
        private Bunifu.UI.WinForms.BunifuTransition transition;
        private System.Windows.Forms.Timer timerHideShadowSide;
        private Bunifu.UI.WinForms.BunifuPictureBox pbGraphShown;
        private System.Windows.Forms.Timer timerAllowRefresh;
        private Bunifu.UI.WinForms.BunifuPictureBox pbFavorite;
        private System.Windows.Forms.Timer timerSwitchFav;
    }
}
