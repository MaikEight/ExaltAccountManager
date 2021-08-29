
namespace EAM_PingChecker.UI
{
    partial class ServerPingGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerPingGraph));
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.lPingInterval = new System.Windows.Forms.Label();
            this.dropInterval = new Bunifu.UI.WinForms.BunifuDropdown();
            this.lAvgPing = new System.Windows.Forms.Label();
            this.lAvgPingText = new System.Windows.Forms.Label();
            this.lMaxPing = new System.Windows.Forms.Label();
            this.lMaxPingText = new System.Windows.Forms.Label();
            this.lMinPing = new System.Windows.Forms.Label();
            this.lMinPingText = new System.Windows.Forms.Label();
            this.lLastPing = new System.Windows.Forms.Label();
            this.lLastPingText = new System.Windows.Forms.Label();
            this.canvas = new Bunifu.Charts.WinForms.BunifuChartCanvas();
            this.separator = new Bunifu.UI.WinForms.BunifuSeparator();
            this.lServerName = new System.Windows.Forms.Label();
            this.lineChart = new Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart(this.components);
            this.timerPing = new System.Windows.Forms.Timer(this.components);
            this.bunifuElipse = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.lineTransparent = new Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart(this.components);
            this.shadow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadow.BorderRadius = 5;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.pbClose);
            this.shadow.Controls.Add(this.lPingInterval);
            this.shadow.Controls.Add(this.dropInterval);
            this.shadow.Controls.Add(this.lAvgPing);
            this.shadow.Controls.Add(this.lAvgPingText);
            this.shadow.Controls.Add(this.lMaxPing);
            this.shadow.Controls.Add(this.lMaxPingText);
            this.shadow.Controls.Add(this.lMinPing);
            this.shadow.Controls.Add(this.lMinPingText);
            this.shadow.Controls.Add(this.lLastPing);
            this.shadow.Controls.Add(this.lLastPingText);
            this.shadow.Controls.Add(this.canvas);
            this.shadow.Controls.Add(this.separator);
            this.shadow.Controls.Add(this.lServerName);
            this.shadow.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.shadow.Size = new System.Drawing.Size(606, 300);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 2;
            // 
            // pbClose
            // 
            this.pbClose.Image = global::EAM_PingChecker.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(580, 10);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(18, 18);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClose.TabIndex = 22;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // lPingInterval
            // 
            this.lPingInterval.AutoSize = true;
            this.lPingInterval.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPingInterval.Location = new System.Drawing.Point(322, 14);
            this.lPingInterval.Name = "lPingInterval";
            this.lPingInterval.Size = new System.Drawing.Size(132, 21);
            this.lPingInterval.TabIndex = 21;
            this.lPingInterval.Text = "Ping interval (s)";
            // 
            // dropInterval
            // 
            this.dropInterval.BackColor = System.Drawing.Color.Transparent;
            this.dropInterval.BackgroundColor = System.Drawing.Color.White;
            this.dropInterval.BorderColor = System.Drawing.Color.Silver;
            this.dropInterval.BorderRadius = 1;
            this.dropInterval.Color = System.Drawing.Color.Silver;
            this.dropInterval.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropInterval.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropInterval.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropInterval.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropInterval.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropInterval.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropInterval.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropInterval.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropInterval.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropInterval.FillDropDown = true;
            this.dropInterval.FillIndicator = false;
            this.dropInterval.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropInterval.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropInterval.ForeColor = System.Drawing.Color.Black;
            this.dropInterval.FormattingEnabled = true;
            this.dropInterval.Icon = null;
            this.dropInterval.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropInterval.IndicatorColor = System.Drawing.Color.Gray;
            this.dropInterval.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropInterval.ItemBackColor = System.Drawing.Color.White;
            this.dropInterval.ItemBorderColor = System.Drawing.Color.White;
            this.dropInterval.ItemForeColor = System.Drawing.Color.Black;
            this.dropInterval.ItemHeight = 16;
            this.dropInterval.ItemHighLightColor = System.Drawing.Color.DodgerBlue;
            this.dropInterval.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropInterval.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "30",
            "60"});
            this.dropInterval.ItemTopMargin = 3;
            this.dropInterval.Location = new System.Drawing.Point(460, 14);
            this.dropInterval.Name = "dropInterval";
            this.dropInterval.Size = new System.Drawing.Size(50, 22);
            this.dropInterval.TabIndex = 20;
            this.dropInterval.Text = null;
            this.dropInterval.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropInterval.TextLeftMargin = 5;
            this.dropInterval.SelectedIndexChanged += new System.EventHandler(this.dropInterval_SelectedIndexChanged);
            // 
            // lAvgPing
            // 
            this.lAvgPing.AutoSize = true;
            this.lAvgPing.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAvgPing.Location = new System.Drawing.Point(515, 214);
            this.lAvgPing.Name = "lAvgPing";
            this.lAvgPing.Size = new System.Drawing.Size(46, 21);
            this.lAvgPing.TabIndex = 19;
            this.lAvgPing.Text = "9999";
            // 
            // lAvgPingText
            // 
            this.lAvgPingText.AutoSize = true;
            this.lAvgPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAvgPingText.Location = new System.Drawing.Point(514, 193);
            this.lAvgPingText.Name = "lAvgPingText";
            this.lAvgPingText.Size = new System.Drawing.Size(83, 21);
            this.lAvgPingText.TabIndex = 18;
            this.lAvgPingText.Text = "Avg. Ping";
            // 
            // lMaxPing
            // 
            this.lMaxPing.AutoSize = true;
            this.lMaxPing.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMaxPing.Location = new System.Drawing.Point(515, 168);
            this.lMaxPing.Name = "lMaxPing";
            this.lMaxPing.Size = new System.Drawing.Size(46, 21);
            this.lMaxPing.TabIndex = 16;
            this.lMaxPing.Text = "9999";
            // 
            // lMaxPingText
            // 
            this.lMaxPingText.AutoSize = true;
            this.lMaxPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMaxPingText.Location = new System.Drawing.Point(514, 147);
            this.lMaxPingText.Name = "lMaxPingText";
            this.lMaxPingText.Size = new System.Drawing.Size(86, 21);
            this.lMaxPingText.TabIndex = 15;
            this.lMaxPingText.Text = "Max. Ping";
            // 
            // lMinPing
            // 
            this.lMinPing.AutoSize = true;
            this.lMinPing.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMinPing.Location = new System.Drawing.Point(515, 122);
            this.lMinPing.Name = "lMinPing";
            this.lMinPing.Size = new System.Drawing.Size(46, 21);
            this.lMinPing.TabIndex = 13;
            this.lMinPing.Text = "9999";
            // 
            // lMinPingText
            // 
            this.lMinPingText.AutoSize = true;
            this.lMinPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMinPingText.Location = new System.Drawing.Point(514, 101);
            this.lMinPingText.Name = "lMinPingText";
            this.lMinPingText.Size = new System.Drawing.Size(83, 21);
            this.lMinPingText.TabIndex = 12;
            this.lMinPingText.Text = "Min. Ping";
            // 
            // lLastPing
            // 
            this.lLastPing.AutoSize = true;
            this.lLastPing.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLastPing.Location = new System.Drawing.Point(515, 76);
            this.lLastPing.Name = "lLastPing";
            this.lLastPing.Size = new System.Drawing.Size(46, 21);
            this.lLastPing.TabIndex = 11;
            this.lLastPing.Text = "9999";
            // 
            // lLastPingText
            // 
            this.lLastPingText.AutoSize = true;
            this.lLastPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLastPingText.Location = new System.Drawing.Point(514, 55);
            this.lLastPingText.Name = "lLastPingText";
            this.lLastPingText.Size = new System.Drawing.Size(79, 21);
            this.lLastPingText.TabIndex = 10;
            this.lLastPingText.Text = "Last Ping";
            // 
            // canvas
            // 
            this.canvas.AnimationDuration = 300;
            this.canvas.AnimationType = Bunifu.Charts.WinForms.BunifuChartCanvas.AnimationOptions.linear;
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.CanvasPadding = new System.Windows.Forms.Padding(-5, 0, 23, 13);
            this.canvas.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.canvas.Labels = new string[] {
        "30",
        "27",
        "24",
        "21",
        "18",
        "15",
        "12",
        "09",
        "06",
        "03",
        "00"};
            this.canvas.LegendAlignment = Bunifu.Charts.WinForms.BunifuChartCanvas.LegendAlignmentOptions.center;
            this.canvas.LegendDisplay = false;
            this.canvas.LegendFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.canvas.LegendForeColor = System.Drawing.Color.DarkGray;
            this.canvas.LegendFullWidth = true;
            this.canvas.LegendPosition = Bunifu.Charts.WinForms.BunifuChartCanvas.PositionOptions.top;
            this.canvas.LegendRevese = false;
            this.canvas.LegendRTL = false;
            this.canvas.Location = new System.Drawing.Point(6, 42);
            this.canvas.Name = "canvas";
            this.canvas.ShowXAxis = true;
            this.canvas.ShowYAxis = true;
            this.canvas.Size = new System.Drawing.Size(533, 250);
            this.canvas.TabIndex = 7;
            this.canvas.Title = "";
            this.canvas.TitleLineHeight = 1.2D;
            this.canvas.TitlePadding = 10;
            this.canvas.TitlePosition = Bunifu.Charts.WinForms.BunifuChartCanvas.PositionOptions.top;
            this.canvas.TooltipBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.canvas.TooltipFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.canvas.TooltipForeColor = System.Drawing.Color.WhiteSmoke;
            this.canvas.TooltipMode = Bunifu.Charts.WinForms.BunifuChartCanvas.TooltipModeOptions.nearest;
            this.canvas.TooltipsEnabled = true;
            this.canvas.XAxesBeginAtZero = true;
            this.canvas.XAxesDrawTicks = true;
            this.canvas.XAxesFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.canvas.XAxesForeColor = System.Drawing.SystemColors.ControlText;
            this.canvas.XAxesGridColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.canvas.XAxesGridLines = true;
            this.canvas.XAxesLabel = "Time past since the ping";
            this.canvas.XAxesLabelFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.canvas.XAxesLabelForeColor = System.Drawing.SystemColors.ControlText;
            this.canvas.XAxesLineWidth = 1;
            this.canvas.XAxesStacked = false;
            this.canvas.XAxesZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.canvas.XAxesZeroLineWidth = 1;
            this.canvas.YAxesBeginAtZero = true;
            this.canvas.YAxesDrawTicks = true;
            this.canvas.YAxesFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.canvas.YAxesForeColor = System.Drawing.SystemColors.ControlText;
            this.canvas.YAxesGridColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.canvas.YAxesGridLines = true;
            this.canvas.YAxesLabel = "Ping in ms";
            this.canvas.YAxesLabelFont = new System.Drawing.Font("Segoe UI", 12F);
            this.canvas.YAxesLabelForeColor = System.Drawing.SystemColors.ControlText;
            this.canvas.YAxesLineWidth = 1;
            this.canvas.YAxesStacked = false;
            this.canvas.YAxesZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.canvas.YAxesZeroLineWidth = 1;
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
            this.separator.Size = new System.Drawing.Size(300, 5);
            this.separator.TabIndex = 2;
            // 
            // lServerName
            // 
            this.lServerName.AutoSize = true;
            this.lServerName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lServerName.Location = new System.Drawing.Point(7, 7);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(122, 25);
            this.lServerName.TabIndex = 6;
            this.lServerName.Text = "ServerName";
            // 
            // lineChart
            // 
            this.lineChart.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(179)))), ((int)(((byte)(219)))));
            this.lineChart.BorderCapStyle = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.LineCaps.Butt;
            this.lineChart.BorderColor = System.Drawing.Color.DodgerBlue;
            this.lineChart.BorderDash = null;
            this.lineChart.BorderDashOffset = 0D;
            this.lineChart.BorderJoin = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.BorderJoinStyles.Miter;
            this.lineChart.BorderWidth = 3;
            this.lineChart.CubicInterpolationMode = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.CubicInterpolationModes.Default;
            this.lineChart.Data = ((System.Collections.Generic.List<double>)(resources.GetObject("lineChart.Data")));
            this.lineChart.Fill = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.FillOptions.Origin;
            this.lineChart.Label = "Ping (ms)";
            this.lineChart.LineTension = 0.4D;
            this.lineChart.Order = 0;
            this.lineChart.PointBackgroundColor = System.Drawing.Color.Empty;
            this.lineChart.PointBorderColor = System.Drawing.Color.Empty;
            this.lineChart.PointBorderWidth = 1;
            this.lineChart.PointHitRadius = 5;
            this.lineChart.PointHoverBackgroundColor = System.Drawing.Color.Empty;
            this.lineChart.PointHoverBorderColor = System.Drawing.Color.Empty;
            this.lineChart.PointHoverBorderWidth = 4;
            this.lineChart.PointHoverRadius = 1;
            this.lineChart.PointRadius = 3;
            this.lineChart.PointRotation = 0;
            this.lineChart.PointStyle = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.PointStyles.Circle;
            this.lineChart.ShowLine = true;
            this.lineChart.SpanGaps = false;
            this.lineChart.SteppedLine = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.SteppedLineStyles.False;
            this.lineChart.TargetCanvas = this.canvas;
            // 
            // timerPing
            // 
            this.timerPing.Interval = 3000;
            this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
            // 
            // bunifuElipse
            // 
            this.bunifuElipse.ElipseRadius = 5;
            this.bunifuElipse.TargetControl = this.pbClose;
            // 
            // lineTransparent
            // 
            this.lineTransparent.BackgroundColor = System.Drawing.Color.Transparent;
            this.lineTransparent.BorderCapStyle = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.LineCaps.Butt;
            this.lineTransparent.BorderColor = System.Drawing.Color.Transparent;
            this.lineTransparent.BorderDash = null;
            this.lineTransparent.BorderDashOffset = 0D;
            this.lineTransparent.BorderJoin = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.BorderJoinStyles.Miter;
            this.lineTransparent.BorderWidth = 3;
            this.lineTransparent.CubicInterpolationMode = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.CubicInterpolationModes.Default;
            this.lineTransparent.Data = ((System.Collections.Generic.List<double>)(resources.GetObject("lineTransparent.Data")));
            this.lineTransparent.Fill = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.FillOptions.Origin;
            this.lineTransparent.Label = "Ping (ms)";
            this.lineTransparent.LineTension = 0.4D;
            this.lineTransparent.Order = 0;
            this.lineTransparent.PointBackgroundColor = System.Drawing.Color.Empty;
            this.lineTransparent.PointBorderColor = System.Drawing.Color.Empty;
            this.lineTransparent.PointBorderWidth = 0;
            this.lineTransparent.PointHitRadius = 0;
            this.lineTransparent.PointHoverBackgroundColor = System.Drawing.Color.Empty;
            this.lineTransparent.PointHoverBorderColor = System.Drawing.Color.Empty;
            this.lineTransparent.PointHoverBorderWidth = 0;
            this.lineTransparent.PointHoverRadius = 0;
            this.lineTransparent.PointRadius = 0;
            this.lineTransparent.PointRotation = 0;
            this.lineTransparent.PointStyle = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.PointStyles.Circle;
            this.lineTransparent.ShowLine = true;
            this.lineTransparent.SpanGaps = false;
            this.lineTransparent.SteppedLine = Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart.SteppedLineStyles.False;
            this.lineTransparent.TargetCanvas = this.canvas;
            // 
            // ServerPingGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.Name = "ServerPingGraph";
            this.Size = new System.Drawing.Size(606, 300);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuSeparator separator;
        private System.Windows.Forms.Label lServerName;
        private Bunifu.Charts.WinForms.BunifuChartCanvas canvas;
        private System.Windows.Forms.Label lAvgPing;
        private System.Windows.Forms.Label lAvgPingText;
        private System.Windows.Forms.Label lMaxPing;
        private System.Windows.Forms.Label lMaxPingText;
        private System.Windows.Forms.Label lMinPing;
        private System.Windows.Forms.Label lMinPingText;
        private System.Windows.Forms.Label lLastPing;
        private System.Windows.Forms.Label lLastPingText;
        private Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart lineChart;
        private System.Windows.Forms.Label lPingInterval;
        private Bunifu.UI.WinForms.BunifuDropdown dropInterval;
        private System.Windows.Forms.Timer timerPing;
        private System.Windows.Forms.PictureBox pbClose;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse;
        private Bunifu.Charts.WinForms.ChartTypes.BunifuLineChart lineTransparent;
    }
}
