
namespace EAM_PingChecker
{
    partial class UIDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIDashboard));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges3 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.pBottomSpacer = new System.Windows.Forms.Panel();
            this.lServer = new System.Windows.Forms.Label();
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lServerName = new System.Windows.Forms.Label();
            this.separator = new Bunifu.UI.WinForms.BunifuSeparator();
            this.lPingText = new System.Windows.Forms.Label();
            this.shadow2 = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.btnRefreshData = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.label1 = new System.Windows.Forms.Label();
            this.separator2 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.lDate = new System.Windows.Forms.Label();
            this.toolTip = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.shadow3 = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lRefresh = new System.Windows.Forms.Label();
            this.btnRefreshFav = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnRefreshAll = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.label2 = new System.Windows.Forms.Label();
            this.bunifuSeparator1 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.timerRefreshFav = new System.Windows.Forms.Timer(this.components);
            this.timerRefreshAll = new System.Windows.Forms.Timer(this.components);
            this.snackbar = new Bunifu.UI.WinForms.BunifuSnackbar(this.components);
            this.flow.SuspendLayout();
            this.shadow.SuspendLayout();
            this.shadow2.SuspendLayout();
            this.shadow3.SuspendLayout();
            this.SuspendLayout();
            // 
            // flow
            // 
            this.flow.Controls.Add(this.pBottomSpacer);
            this.flow.Location = new System.Drawing.Point(0, 110);
            this.flow.Margin = new System.Windows.Forms.Padding(0);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(610, 500);
            this.flow.TabIndex = 1;
            this.toolTip.SetToolTip(this.flow, "");
            this.toolTip.SetToolTipIcon(this.flow, null);
            this.toolTip.SetToolTipTitle(this.flow, "");
            // 
            // pBottomSpacer
            // 
            this.pBottomSpacer.Location = new System.Drawing.Point(3, 3);
            this.pBottomSpacer.Name = "pBottomSpacer";
            this.pBottomSpacer.Size = new System.Drawing.Size(604, 10);
            this.pBottomSpacer.TabIndex = 0;
            this.toolTip.SetToolTip(this.pBottomSpacer, "");
            this.toolTip.SetToolTipIcon(this.pBottomSpacer, null);
            this.toolTip.SetToolTipTitle(this.pBottomSpacer, "");
            // 
            // lServer
            // 
            this.lServer.AutoSize = true;
            this.lServer.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lServer.Location = new System.Drawing.Point(3, 85);
            this.lServer.Name = "lServer";
            this.lServer.Size = new System.Drawing.Size(95, 25);
            this.lServer.TabIndex = 7;
            this.lServer.Text = "Serverlist";
            this.toolTip.SetToolTip(this.lServer, "");
            this.toolTip.SetToolTipIcon(this.lServer, null);
            this.toolTip.SetToolTipTitle(this.lServer, "");
            this.lServer.Paint += new System.Windows.Forms.PaintEventHandler(this.lServer_Paint);
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.lServerName);
            this.shadow.Controls.Add(this.separator);
            this.shadow.Controls.Add(this.lPingText);
            this.shadow.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow.Location = new System.Drawing.Point(3, 3);
            this.shadow.Name = "shadow";
            this.shadow.PanelColor = System.Drawing.Color.White;
            this.shadow.PanelColor2 = System.Drawing.Color.White;
            this.shadow.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow.ShadowDept = 2;
            this.shadow.ShadowDepth = 4;
            this.shadow.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow.ShadowTopLeftVisible = false;
            this.shadow.Size = new System.Drawing.Size(175, 75);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 20;
            this.toolTip.SetToolTip(this.shadow, "");
            this.toolTip.SetToolTipIcon(this.shadow, null);
            this.toolTip.SetToolTipTitle(this.shadow, "");
            // 
            // lServerName
            // 
            this.lServerName.AutoSize = true;
            this.lServerName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lServerName.Location = new System.Drawing.Point(7, 7);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(78, 25);
            this.lServerName.TabIndex = 6;
            this.lServerName.Text = "Servers";
            this.toolTip.SetToolTip(this.lServerName, "");
            this.toolTip.SetToolTipIcon(this.lServerName, null);
            this.toolTip.SetToolTipTitle(this.lServerName, "");
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.Color.Transparent;
            this.separator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("separator.BackgroundImage")));
            this.separator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.separator.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.separator.LineColor = System.Drawing.Color.Silver;
            this.separator.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.separator.LineThickness = 1;
            this.separator.Location = new System.Drawing.Point(10, 30);
            this.separator.Margin = new System.Windows.Forms.Padding(4);
            this.separator.Name = "separator";
            this.separator.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.separator.Size = new System.Drawing.Size(155, 5);
            this.separator.TabIndex = 2;
            this.toolTip.SetToolTip(this.separator, "");
            this.toolTip.SetToolTipIcon(this.separator, null);
            this.toolTip.SetToolTipTitle(this.separator, "");
            // 
            // lPingText
            // 
            this.lPingText.AutoSize = true;
            this.lPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPingText.Location = new System.Drawing.Point(9, 38);
            this.lPingText.Name = "lPingText";
            this.lPingText.Size = new System.Drawing.Size(141, 21);
            this.lPingText.TabIndex = 7;
            this.lPingText.Text = "Found {0} servers";
            this.toolTip.SetToolTip(this.lPingText, "");
            this.toolTip.SetToolTipIcon(this.lPingText, null);
            this.toolTip.SetToolTipTitle(this.lPingText, "");
            // 
            // shadow2
            // 
            this.shadow2.BackColor = System.Drawing.Color.White;
            this.shadow2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow2.BorderRadius = 9;
            this.shadow2.BorderThickness = 1;
            this.shadow2.Controls.Add(this.btnRefreshData);
            this.shadow2.Controls.Add(this.label1);
            this.shadow2.Controls.Add(this.separator2);
            this.shadow2.Controls.Add(this.lDate);
            this.shadow2.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow2.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow2.Location = new System.Drawing.Point(181, 3);
            this.shadow2.Name = "shadow2";
            this.shadow2.PanelColor = System.Drawing.Color.White;
            this.shadow2.PanelColor2 = System.Drawing.Color.White;
            this.shadow2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow2.ShadowDept = 2;
            this.shadow2.ShadowDepth = 4;
            this.shadow2.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow2.ShadowTopLeftVisible = false;
            this.shadow2.Size = new System.Drawing.Size(250, 104);
            this.shadow2.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow2.TabIndex = 21;
            this.toolTip.SetToolTip(this.shadow2, "");
            this.toolTip.SetToolTipIcon(this.shadow2, null);
            this.toolTip.SetToolTipTitle(this.shadow2, "");
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.AllowAnimations = true;
            this.btnRefreshData.AllowMouseEffects = true;
            this.btnRefreshData.AllowToggling = false;
            this.btnRefreshData.AnimationSpeed = 200;
            this.btnRefreshData.AutoGenerateColors = false;
            this.btnRefreshData.AutoRoundBorders = false;
            this.btnRefreshData.AutoSizeLeftIcon = true;
            this.btnRefreshData.AutoSizeRightIcon = true;
            this.btnRefreshData.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshData.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshData.BackgroundImage")));
            this.btnRefreshData.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshData.ButtonText = "Refresh data";
            this.btnRefreshData.ButtonTextMarginLeft = 0;
            this.btnRefreshData.ColorContrastOnClick = 45;
            this.btnRefreshData.ColorContrastOnHover = 45;
            this.btnRefreshData.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnRefreshData.CustomizableEdges = borderEdges1;
            this.btnRefreshData.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnRefreshData.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnRefreshData.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRefreshData.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRefreshData.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnRefreshData.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshData.ForeColor = System.Drawing.Color.White;
            this.btnRefreshData.IconLeft = ((System.Drawing.Image)(resources.GetObject("btnRefreshData.IconLeft")));
            this.btnRefreshData.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshData.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnRefreshData.IconLeftPadding = new System.Windows.Forms.Padding(4);
            this.btnRefreshData.IconMarginLeft = 11;
            this.btnRefreshData.IconPadding = 3;
            this.btnRefreshData.IconRight = null;
            this.btnRefreshData.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefreshData.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnRefreshData.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnRefreshData.IconSize = 25;
            this.btnRefreshData.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.IdleBorderRadius = 5;
            this.btnRefreshData.IdleBorderThickness = 1;
            this.btnRefreshData.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.IdleIconLeftImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshData.IdleIconLeftImage")));
            this.btnRefreshData.IdleIconRightImage = null;
            this.btnRefreshData.IndicateFocus = false;
            this.btnRefreshData.Location = new System.Drawing.Point(15, 69);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnRefreshData.OnDisabledState.BorderRadius = 5;
            this.btnRefreshData.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshData.OnDisabledState.BorderThickness = 1;
            this.btnRefreshData.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRefreshData.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRefreshData.OnDisabledState.IconLeftImage = null;
            this.btnRefreshData.OnDisabledState.IconRightImage = null;
            this.btnRefreshData.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.onHoverState.BorderRadius = 5;
            this.btnRefreshData.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshData.onHoverState.BorderThickness = 1;
            this.btnRefreshData.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshData.onHoverState.IconLeftImage = null;
            this.btnRefreshData.onHoverState.IconRightImage = null;
            this.btnRefreshData.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.OnIdleState.BorderRadius = 5;
            this.btnRefreshData.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshData.OnIdleState.BorderThickness = 1;
            this.btnRefreshData.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshData.OnIdleState.IconLeftImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshData.OnIdleState.IconLeftImage")));
            this.btnRefreshData.OnIdleState.IconRightImage = null;
            this.btnRefreshData.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.OnPressedState.BorderRadius = 5;
            this.btnRefreshData.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshData.OnPressedState.BorderThickness = 1;
            this.btnRefreshData.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshData.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshData.OnPressedState.IconLeftImage = null;
            this.btnRefreshData.OnPressedState.IconRightImage = null;
            this.btnRefreshData.Size = new System.Drawing.Size(220, 26);
            this.btnRefreshData.TabIndex = 19;
            this.btnRefreshData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefreshData.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnRefreshData.TextMarginLeft = 0;
            this.btnRefreshData.TextPadding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.toolTip.SetToolTip(this.btnRefreshData, "");
            this.toolTip.SetToolTipIcon(this.btnRefreshData, null);
            this.toolTip.SetToolTipTitle(this.btnRefreshData, "");
            this.btnRefreshData.UseDefaultRadiusAndThickness = true;
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Load information";
            this.toolTip.SetToolTip(this.label1, "");
            this.toolTip.SetToolTipIcon(this.label1, null);
            this.toolTip.SetToolTipTitle(this.label1, "");
            // 
            // separator2
            // 
            this.separator2.BackColor = System.Drawing.Color.Transparent;
            this.separator2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("separator2.BackgroundImage")));
            this.separator2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.separator2.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.separator2.LineColor = System.Drawing.Color.Silver;
            this.separator2.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.separator2.LineThickness = 1;
            this.separator2.Location = new System.Drawing.Point(10, 30);
            this.separator2.Margin = new System.Windows.Forms.Padding(4);
            this.separator2.Name = "separator2";
            this.separator2.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.separator2.Size = new System.Drawing.Size(230, 5);
            this.separator2.TabIndex = 2;
            this.toolTip.SetToolTip(this.separator2, "");
            this.toolTip.SetToolTipIcon(this.separator2, null);
            this.toolTip.SetToolTipTitle(this.separator2, "");
            // 
            // lDate
            // 
            this.lDate.AutoSize = true;
            this.lDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDate.Location = new System.Drawing.Point(9, 38);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(141, 21);
            this.lDate.TabIndex = 7;
            this.lDate.Text = "Data from: {0} {1}";
            this.toolTip.SetToolTip(this.lDate, "");
            this.toolTip.SetToolTipIcon(this.lDate, null);
            this.toolTip.SetToolTipTitle(this.lDate, "");
            // 
            // toolTip
            // 
            this.toolTip.Active = true;
            this.toolTip.AlignTextWithTitle = false;
            this.toolTip.AllowAutoClose = false;
            this.toolTip.AllowFading = true;
            this.toolTip.AutoCloseDuration = 5000;
            this.toolTip.BackColor = System.Drawing.SystemColors.Control;
            this.toolTip.BorderColor = System.Drawing.Color.Gainsboro;
            this.toolTip.ClickToShowDisplayControl = false;
            this.toolTip.ConvertNewlinesToBreakTags = true;
            this.toolTip.DisplayControl = null;
            this.toolTip.EntryAnimationSpeed = 350;
            this.toolTip.ExitAnimationSpeed = 200;
            this.toolTip.GenerateAutoCloseDuration = false;
            this.toolTip.IconMargin = 6;
            this.toolTip.InitialDelay = 0;
            this.toolTip.Name = "toolTip";
            this.toolTip.Opacity = 1D;
            this.toolTip.OverrideToolTipTitles = false;
            this.toolTip.Padding = new System.Windows.Forms.Padding(10);
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            this.toolTip.ShowBorders = false;
            this.toolTip.ShowIcons = true;
            this.toolTip.ShowShadows = true;
            this.toolTip.Tag = null;
            this.toolTip.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.toolTip.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolTip.TextMargin = 2;
            this.toolTip.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolTip.TitleForeColor = System.Drawing.Color.Black;
            this.toolTip.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.toolTip.ToolTipTitle = null;
            // 
            // shadow3
            // 
            this.shadow3.BackColor = System.Drawing.Color.White;
            this.shadow3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow3.BorderRadius = 9;
            this.shadow3.BorderThickness = 1;
            this.shadow3.Controls.Add(this.lRefresh);
            this.shadow3.Controls.Add(this.btnRefreshFav);
            this.shadow3.Controls.Add(this.btnRefreshAll);
            this.shadow3.Controls.Add(this.label2);
            this.shadow3.Controls.Add(this.bunifuSeparator1);
            this.shadow3.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow3.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow3.Location = new System.Drawing.Point(434, 3);
            this.shadow3.Name = "shadow3";
            this.shadow3.PanelColor = System.Drawing.Color.White;
            this.shadow3.PanelColor2 = System.Drawing.Color.White;
            this.shadow3.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow3.ShadowDept = 2;
            this.shadow3.ShadowDepth = 4;
            this.shadow3.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow3.ShadowTopLeftVisible = false;
            this.shadow3.Size = new System.Drawing.Size(183, 104);
            this.shadow3.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow3.TabIndex = 22;
            this.toolTip.SetToolTip(this.shadow3, "");
            this.toolTip.SetToolTipIcon(this.shadow3, null);
            this.toolTip.SetToolTipTitle(this.shadow3, "");
            // 
            // lRefresh
            // 
            this.lRefresh.AutoSize = true;
            this.lRefresh.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRefresh.Location = new System.Drawing.Point(144, 6);
            this.lRefresh.Name = "lRefresh";
            this.lRefresh.Size = new System.Drawing.Size(27, 26);
            this.lRefresh.TabIndex = 23;
            this.lRefresh.Text = "Last\r\n{0}";
            this.lRefresh.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.lRefresh, "");
            this.toolTip.SetToolTipIcon(this.lRefresh, null);
            this.toolTip.SetToolTipTitle(this.lRefresh, "");
            // 
            // btnRefreshFav
            // 
            this.btnRefreshFav.AllowAnimations = true;
            this.btnRefreshFav.AllowMouseEffects = true;
            this.btnRefreshFav.AllowToggling = false;
            this.btnRefreshFav.AnimationSpeed = 200;
            this.btnRefreshFav.AutoGenerateColors = false;
            this.btnRefreshFav.AutoRoundBorders = false;
            this.btnRefreshFav.AutoSizeLeftIcon = true;
            this.btnRefreshFav.AutoSizeRightIcon = true;
            this.btnRefreshFav.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshFav.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshFav.BackgroundImage")));
            this.btnRefreshFav.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshFav.ButtonText = "Refresh favorites";
            this.btnRefreshFav.ButtonTextMarginLeft = 0;
            this.btnRefreshFav.ColorContrastOnClick = 45;
            this.btnRefreshFav.ColorContrastOnHover = 45;
            this.btnRefreshFav.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnRefreshFav.CustomizableEdges = borderEdges2;
            this.btnRefreshFav.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnRefreshFav.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnRefreshFav.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRefreshFav.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRefreshFav.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnRefreshFav.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshFav.ForeColor = System.Drawing.Color.White;
            this.btnRefreshFav.IconLeft = ((System.Drawing.Image)(resources.GetObject("btnRefreshFav.IconLeft")));
            this.btnRefreshFav.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshFav.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnRefreshFav.IconLeftPadding = new System.Windows.Forms.Padding(4);
            this.btnRefreshFav.IconMarginLeft = 11;
            this.btnRefreshFav.IconPadding = 3;
            this.btnRefreshFav.IconRight = null;
            this.btnRefreshFav.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefreshFav.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnRefreshFav.IconRightPadding = new System.Windows.Forms.Padding(4);
            this.btnRefreshFav.IconSize = 25;
            this.btnRefreshFav.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.IdleBorderRadius = 5;
            this.btnRefreshFav.IdleBorderThickness = 1;
            this.btnRefreshFav.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.IdleIconLeftImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshFav.IdleIconLeftImage")));
            this.btnRefreshFav.IdleIconRightImage = null;
            this.btnRefreshFav.IndicateFocus = false;
            this.btnRefreshFav.Location = new System.Drawing.Point(15, 38);
            this.btnRefreshFav.Name = "btnRefreshFav";
            this.btnRefreshFav.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnRefreshFav.OnDisabledState.BorderRadius = 5;
            this.btnRefreshFav.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshFav.OnDisabledState.BorderThickness = 1;
            this.btnRefreshFav.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRefreshFav.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRefreshFav.OnDisabledState.IconLeftImage = null;
            this.btnRefreshFav.OnDisabledState.IconRightImage = null;
            this.btnRefreshFav.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.onHoverState.BorderRadius = 5;
            this.btnRefreshFav.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshFav.onHoverState.BorderThickness = 1;
            this.btnRefreshFav.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshFav.onHoverState.IconLeftImage = null;
            this.btnRefreshFav.onHoverState.IconRightImage = null;
            this.btnRefreshFav.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.OnIdleState.BorderRadius = 5;
            this.btnRefreshFav.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshFav.OnIdleState.BorderThickness = 1;
            this.btnRefreshFav.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshFav.OnIdleState.IconLeftImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshFav.OnIdleState.IconLeftImage")));
            this.btnRefreshFav.OnIdleState.IconRightImage = null;
            this.btnRefreshFav.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.OnPressedState.BorderRadius = 5;
            this.btnRefreshFav.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshFav.OnPressedState.BorderThickness = 1;
            this.btnRefreshFav.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshFav.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshFav.OnPressedState.IconLeftImage = null;
            this.btnRefreshFav.OnPressedState.IconRightImage = null;
            this.btnRefreshFav.Size = new System.Drawing.Size(158, 26);
            this.btnRefreshFav.TabIndex = 23;
            this.btnRefreshFav.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefreshFav.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnRefreshFav.TextMarginLeft = 0;
            this.btnRefreshFav.TextPadding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.toolTip.SetToolTip(this.btnRefreshFav, "");
            this.toolTip.SetToolTipIcon(this.btnRefreshFav, null);
            this.toolTip.SetToolTipTitle(this.btnRefreshFav, "");
            this.btnRefreshFav.UseDefaultRadiusAndThickness = true;
            this.btnRefreshFav.Click += new System.EventHandler(this.btnRefreshFav_Click);
            // 
            // btnRefreshAll
            // 
            this.btnRefreshAll.AllowAnimations = true;
            this.btnRefreshAll.AllowMouseEffects = true;
            this.btnRefreshAll.AllowToggling = false;
            this.btnRefreshAll.AnimationSpeed = 200;
            this.btnRefreshAll.AutoGenerateColors = false;
            this.btnRefreshAll.AutoRoundBorders = false;
            this.btnRefreshAll.AutoSizeLeftIcon = true;
            this.btnRefreshAll.AutoSizeRightIcon = true;
            this.btnRefreshAll.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshAll.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshAll.BackgroundImage")));
            this.btnRefreshAll.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshAll.ButtonText = "Refresh all server";
            this.btnRefreshAll.ButtonTextMarginLeft = 0;
            this.btnRefreshAll.ColorContrastOnClick = 45;
            this.btnRefreshAll.ColorContrastOnHover = 45;
            this.btnRefreshAll.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges3.BottomLeft = true;
            borderEdges3.BottomRight = true;
            borderEdges3.TopLeft = true;
            borderEdges3.TopRight = true;
            this.btnRefreshAll.CustomizableEdges = borderEdges3;
            this.btnRefreshAll.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnRefreshAll.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnRefreshAll.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRefreshAll.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRefreshAll.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnRefreshAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshAll.ForeColor = System.Drawing.Color.White;
            this.btnRefreshAll.IconLeft = ((System.Drawing.Image)(resources.GetObject("btnRefreshAll.IconLeft")));
            this.btnRefreshAll.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshAll.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnRefreshAll.IconLeftPadding = new System.Windows.Forms.Padding(4);
            this.btnRefreshAll.IconMarginLeft = 11;
            this.btnRefreshAll.IconPadding = 3;
            this.btnRefreshAll.IconRight = null;
            this.btnRefreshAll.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefreshAll.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnRefreshAll.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnRefreshAll.IconSize = 25;
            this.btnRefreshAll.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.IdleBorderRadius = 5;
            this.btnRefreshAll.IdleBorderThickness = 1;
            this.btnRefreshAll.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.IdleIconLeftImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshAll.IdleIconLeftImage")));
            this.btnRefreshAll.IdleIconRightImage = null;
            this.btnRefreshAll.IndicateFocus = false;
            this.btnRefreshAll.Location = new System.Drawing.Point(15, 69);
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnRefreshAll.OnDisabledState.BorderRadius = 5;
            this.btnRefreshAll.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshAll.OnDisabledState.BorderThickness = 1;
            this.btnRefreshAll.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRefreshAll.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRefreshAll.OnDisabledState.IconLeftImage = null;
            this.btnRefreshAll.OnDisabledState.IconRightImage = null;
            this.btnRefreshAll.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.onHoverState.BorderRadius = 5;
            this.btnRefreshAll.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshAll.onHoverState.BorderThickness = 1;
            this.btnRefreshAll.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshAll.onHoverState.IconLeftImage = null;
            this.btnRefreshAll.onHoverState.IconRightImage = null;
            this.btnRefreshAll.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.OnIdleState.BorderRadius = 5;
            this.btnRefreshAll.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshAll.OnIdleState.BorderThickness = 1;
            this.btnRefreshAll.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshAll.OnIdleState.IconLeftImage = ((System.Drawing.Image)(resources.GetObject("btnRefreshAll.OnIdleState.IconLeftImage")));
            this.btnRefreshAll.OnIdleState.IconRightImage = null;
            this.btnRefreshAll.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.OnPressedState.BorderRadius = 5;
            this.btnRefreshAll.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRefreshAll.OnPressedState.BorderThickness = 1;
            this.btnRefreshAll.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnRefreshAll.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnRefreshAll.OnPressedState.IconLeftImage = null;
            this.btnRefreshAll.OnPressedState.IconRightImage = null;
            this.btnRefreshAll.Size = new System.Drawing.Size(158, 26);
            this.btnRefreshAll.TabIndex = 18;
            this.btnRefreshAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefreshAll.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnRefreshAll.TextMarginLeft = 0;
            this.btnRefreshAll.TextPadding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.toolTip.SetToolTip(this.btnRefreshAll, "");
            this.toolTip.SetToolTipIcon(this.btnRefreshAll, null);
            this.toolTip.SetToolTipTitle(this.btnRefreshAll, "");
            this.btnRefreshAll.UseDefaultRadiusAndThickness = true;
            this.btnRefreshAll.Click += new System.EventHandler(this.btnRefreshAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Refresh";
            this.toolTip.SetToolTip(this.label2, "");
            this.toolTip.SetToolTipIcon(this.label2, null);
            this.toolTip.SetToolTipTitle(this.label2, "");
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator1.BackgroundImage")));
            this.bunifuSeparator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator1.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator1.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(10, 30);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator1.Size = new System.Drawing.Size(230, 5);
            this.bunifuSeparator1.TabIndex = 2;
            this.toolTip.SetToolTip(this.bunifuSeparator1, "");
            this.toolTip.SetToolTipIcon(this.bunifuSeparator1, null);
            this.toolTip.SetToolTipTitle(this.bunifuSeparator1, "");
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
            this.scrollbar.BindingContainer = this.flow;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 1;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(610, 110);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(0);
            this.scrollbar.Maximum = 100;
            this.scrollbar.MaximumSize = new System.Drawing.Size(10, 490);
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumSize = new System.Drawing.Size(10, 490);
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(10, 490);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 3;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 48;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.toolTip.SetToolTip(this.scrollbar, "");
            this.toolTip.SetToolTipIcon(this.scrollbar, null);
            this.toolTip.SetToolTipTitle(this.scrollbar, "");
            this.scrollbar.Value = 0;
            // 
            // timerRefreshFav
            // 
            this.timerRefreshFav.Interval = 3000;
            this.timerRefreshFav.Tick += new System.EventHandler(this.timerRefreshFav_Tick);
            // 
            // timerRefreshAll
            // 
            this.timerRefreshAll.Interval = 3000;
            this.timerRefreshAll.Tick += new System.EventHandler(this.timerRefreshAll_Tick);
            // 
            // snackbar
            // 
            this.snackbar.AllowDragging = false;
            this.snackbar.AllowMultipleViews = true;
            this.snackbar.ClickToClose = true;
            this.snackbar.DoubleClickToClose = true;
            this.snackbar.DurationAfterIdle = 3000;
            this.snackbar.ErrorOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.ErrorOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.ErrorOptions.ActionBorderRadius = 1;
            this.snackbar.ErrorOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.ErrorOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.ErrorOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.ErrorOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.ErrorOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(199)))));
            this.snackbar.ErrorOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.ErrorOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.ErrorOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon")));
            this.snackbar.ErrorOptions.IconLeftMargin = 12;
            this.snackbar.FadeCloseIcon = false;
            this.snackbar.Host = Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner;
            this.snackbar.InformationOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.InformationOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.InformationOptions.ActionBorderRadius = 1;
            this.snackbar.InformationOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.InformationOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.InformationOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.InformationOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.InformationOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
            this.snackbar.InformationOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.InformationOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.InformationOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon1")));
            this.snackbar.InformationOptions.IconLeftMargin = 12;
            this.snackbar.Margin = 10;
            this.snackbar.MaximumSize = new System.Drawing.Size(0, 0);
            this.snackbar.MaximumViews = 7;
            this.snackbar.MessageRightMargin = 15;
            this.snackbar.MinimumSize = new System.Drawing.Size(0, 0);
            this.snackbar.ShowBorders = false;
            this.snackbar.ShowCloseIcon = false;
            this.snackbar.ShowIcon = true;
            this.snackbar.ShowShadows = true;
            this.snackbar.SuccessOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.SuccessOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.SuccessOptions.ActionBorderRadius = 1;
            this.snackbar.SuccessOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.SuccessOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.SuccessOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.SuccessOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.SuccessOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(237)))));
            this.snackbar.SuccessOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.SuccessOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.SuccessOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon2")));
            this.snackbar.SuccessOptions.IconLeftMargin = 12;
            this.snackbar.ViewsMargin = 7;
            this.snackbar.WarningOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.WarningOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.WarningOptions.ActionBorderRadius = 1;
            this.snackbar.WarningOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.WarningOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.WarningOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.WarningOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.WarningOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(143)))));
            this.snackbar.WarningOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.WarningOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.WarningOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon3")));
            this.snackbar.WarningOptions.IconLeftMargin = 12;
            this.snackbar.ZoomCloseIcon = true;
            // 
            // UIDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow3);
            this.Controls.Add(this.shadow2);
            this.Controls.Add(this.shadow);
            this.Controls.Add(this.lServer);
            this.Controls.Add(this.scrollbar);
            this.Controls.Add(this.flow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIDashboard";
            this.Size = new System.Drawing.Size(620, 600);
            this.toolTip.SetToolTip(this, "");
            this.toolTip.SetToolTipIcon(this, null);
            this.toolTip.SetToolTipTitle(this, "");
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UIDashboard_Paint);
            this.flow.ResumeLayout(false);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            this.shadow2.ResumeLayout(false);
            this.shadow2.PerformLayout();
            this.shadow3.ResumeLayout(false);
            this.shadow3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flow;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private System.Windows.Forms.Label lServer;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnRefreshAll;
        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private System.Windows.Forms.Label lServerName;
        private Bunifu.UI.WinForms.BunifuSeparator separator;
        private System.Windows.Forms.Label lPingText;
        private Bunifu.UI.WinForms.BunifuShadowPanel shadow2;
        private System.Windows.Forms.Label label1;
        private Bunifu.UI.WinForms.BunifuSeparator separator2;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Panel pBottomSpacer;
        public Bunifu.UI.WinForms.BunifuToolTip toolTip;
        private Bunifu.UI.WinForms.BunifuShadowPanel shadow3;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnRefreshFav;
        private System.Windows.Forms.Label label2;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator1;
        private System.Windows.Forms.Timer timerRefreshFav;
        private System.Windows.Forms.Timer timerRefreshAll;
        private System.Windows.Forms.Label lRefresh;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnRefreshData;
        private Bunifu.UI.WinForms.BunifuSnackbar snackbar;
    }
}
