namespace ExaltAccountManager.UI
{
    partial class UILogs
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
            Bunifu.Framework.UI.BunifuElipse bunifuElipse;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UILogs));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges();
            this.pDatagrid = new System.Windows.Forms.Panel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.dataGridView = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.pB = new System.Windows.Forms.Panel();
            this.pT = new System.Windows.Forms.Panel();
            this.pR = new System.Windows.Forms.Panel();
            this.pL = new System.Windows.Forms.Panel();
            this.bunifuCards = new Bunifu.Framework.UI.BunifuCards();
            this.pLoader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton();
            this.dropExportMode = new Bunifu.UI.WinForms.BunifuDropdown();
            this.btnRefresh = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton();
            this.pData = new System.Windows.Forms.Panel();
            this.pCardsTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            bunifuElipse = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.pDatagrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.bunifuCards.SuspendLayout();
            this.pData.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuElipse
            // 
            bunifuElipse.ElipseRadius = 11;
            bunifuElipse.TargetControl = this.pDatagrid;
            // 
            // pDatagrid
            // 
            this.pDatagrid.Controls.Add(this.scrollbar);
            this.pDatagrid.Controls.Add(this.dataGridView);
            this.pDatagrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDatagrid.Location = new System.Drawing.Point(0, 0);
            this.pDatagrid.Name = "pDatagrid";
            this.pDatagrid.Size = new System.Drawing.Size(617, 435);
            this.pDatagrid.TabIndex = 1;
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
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 2;
            this.scrollbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(609, 0);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(5);
            this.scrollbar.Maximum = 100;
            this.scrollbar.MaximumSize = new System.Drawing.Size(8, 0);
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumSize = new System.Drawing.Size(8, 0);
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(8, 435);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 10;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 42;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.scrollbar.Value = 0;
            this.scrollbar.Scroll += new System.EventHandler<Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs>(this.scrollbar_Scroll);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowCustomTheming = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.ColumnHeadersHeight = 40;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.dataGridView.CurrentTheme.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.dataGridView.CurrentTheme.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridView.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.dataGridView.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView.CurrentTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dataGridView.CurrentTheme.GridColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView.CurrentTheme.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dataGridView.CurrentTheme.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            this.dataGridView.CurrentTheme.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dataGridView.CurrentTheme.HeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView.CurrentTheme.Name = null;
            this.dataGridView.CurrentTheme.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(127)))), ((int)(((byte)(246)))));
            this.dataGridView.CurrentTheme.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.dataGridView.CurrentTheme.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridView.CurrentTheme.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.dataGridView.CurrentTheme.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(127)))), ((int)(((byte)(246)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.GridColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dataGridView.HeaderBgColor = System.Drawing.Color.Empty;
            this.dataGridView.HeaderForeColor = System.Drawing.Color.White;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidth = 30;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowTemplate.Height = 40;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ShowCellErrors = false;
            this.dataGridView.ShowCellToolTips = false;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.ShowRowErrors = false;
            this.dataGridView.Size = new System.Drawing.Size(617, 435);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.DarkViolet;
            // 
            // pB
            // 
            this.pB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pB.Location = new System.Drawing.Point(10, 540);
            this.pB.Name = "pB";
            this.pB.Size = new System.Drawing.Size(657, 10);
            this.pB.TabIndex = 24;
            // 
            // pT
            // 
            this.pT.Dock = System.Windows.Forms.DockStyle.Top;
            this.pT.Location = new System.Drawing.Point(10, 0);
            this.pT.Name = "pT";
            this.pT.Size = new System.Drawing.Size(657, 10);
            this.pT.TabIndex = 23;
            // 
            // pR
            // 
            this.pR.Dock = System.Windows.Forms.DockStyle.Right;
            this.pR.Location = new System.Drawing.Point(667, 0);
            this.pR.Name = "pR";
            this.pR.Size = new System.Drawing.Size(10, 550);
            this.pR.TabIndex = 22;
            // 
            // pL
            // 
            this.pL.Dock = System.Windows.Forms.DockStyle.Left;
            this.pL.Location = new System.Drawing.Point(0, 0);
            this.pL.Name = "pL";
            this.pL.Size = new System.Drawing.Size(10, 550);
            this.pL.TabIndex = 21;
            // 
            // bunifuCards
            // 
            this.bunifuCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.bunifuCards.BorderRadius = 11;
            this.bunifuCards.BottomSahddow = true;
            this.bunifuCards.color = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.bunifuCards.Controls.Add(this.pLoader);
            this.bunifuCards.Controls.Add(this.label1);
            this.bunifuCards.Controls.Add(this.btnSave);
            this.bunifuCards.Controls.Add(this.dropExportMode);
            this.bunifuCards.Controls.Add(this.btnRefresh);
            this.bunifuCards.Controls.Add(this.pData);
            this.bunifuCards.Controls.Add(this.pCardsTop);
            this.bunifuCards.Controls.Add(this.label2);
            this.bunifuCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bunifuCards.LeftSahddow = true;
            this.bunifuCards.Location = new System.Drawing.Point(10, 10);
            this.bunifuCards.Name = "bunifuCards";
            this.bunifuCards.RightSahddow = true;
            this.bunifuCards.ShadowDepth = 20;
            this.bunifuCards.Size = new System.Drawing.Size(657, 530);
            this.bunifuCards.TabIndex = 25;
            // 
            // pLoader
            // 
            this.pLoader.Location = new System.Drawing.Point(638, 512);
            this.pLoader.Name = "pLoader";
            this.pLoader.Size = new System.Drawing.Size(200, 100);
            this.pLoader.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(415, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Exportmode for selected entries";
            // 
            // btnSave
            // 
            this.btnSave.AllowAnimations = true;
            this.btnSave.AllowBorderColorChanges = true;
            this.btnSave.AllowMouseEffects = true;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AnimationSpeed = 100;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnSave.BorderRadius = 1;
            this.btnSave.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderStyles.Solid;
            this.btnSave.BorderThickness = 1;
            this.btnSave.ColorContrastOnClick = 30;
            this.btnSave.ColorContrastOnHover = 20;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnSave.CustomizableEdges = borderEdges1;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSave.Image = global::ExaltAccountManager.Properties.Resources.send_file_white_32px;
            this.btnSave.ImageMargin = new System.Windows.Forms.Padding(0);
            this.btnSave.Location = new System.Drawing.Point(603, 33);
            this.btnSave.Name = "btnSave";
            this.btnSave.RoundBorders = false;
            this.btnSave.ShowBorders = true;
            this.btnSave.Size = new System.Drawing.Size(34, 34);
            this.btnSave.Style = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.ButtonStyles.Round;
            this.btnSave.TabIndex = 19;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dropExportMode
            // 
            this.dropExportMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropExportMode.BackColor = System.Drawing.Color.Transparent;
            this.dropExportMode.BackgroundColor = System.Drawing.Color.White;
            this.dropExportMode.BorderColor = System.Drawing.Color.Silver;
            this.dropExportMode.BorderRadius = 9;
            this.dropExportMode.Color = System.Drawing.Color.Silver;
            this.dropExportMode.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropExportMode.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropExportMode.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropExportMode.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropExportMode.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropExportMode.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropExportMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropExportMode.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropExportMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropExportMode.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropExportMode.FillDropDown = true;
            this.dropExportMode.FillIndicator = false;
            this.dropExportMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropExportMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dropExportMode.ForeColor = System.Drawing.Color.Black;
            this.dropExportMode.FormattingEnabled = true;
            this.dropExportMode.Icon = null;
            this.dropExportMode.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropExportMode.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.dropExportMode.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropExportMode.IndicatorThickness = 3;
            this.dropExportMode.IsDropdownOpened = false;
            this.dropExportMode.ItemBackColor = System.Drawing.Color.White;
            this.dropExportMode.ItemBorderColor = System.Drawing.Color.White;
            this.dropExportMode.ItemForeColor = System.Drawing.Color.Black;
            this.dropExportMode.ItemHeight = 26;
            this.dropExportMode.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dropExportMode.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropExportMode.Items.AddRange(new object[] {
            "Copy to clipboard",
            "Save as .PNG",
            "Save as .CSV"});
            this.dropExportMode.ItemTopMargin = 3;
            this.dropExportMode.Location = new System.Drawing.Point(415, 34);
            this.dropExportMode.Name = "dropExportMode";
            this.dropExportMode.Size = new System.Drawing.Size(184, 32);
            this.dropExportMode.TabIndex = 18;
            this.dropExportMode.Text = null;
            this.dropExportMode.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropExportMode.TextLeftMargin = 5;
            this.dropExportMode.SelectedIndexChanged += new System.EventHandler(this.dropExportMode_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AllowAnimations = true;
            this.btnRefresh.AllowBorderColorChanges = true;
            this.btnRefresh.AllowMouseEffects = true;
            this.btnRefresh.AnimationSpeed = 100;
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnRefresh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnRefresh.BorderRadius = 1;
            this.btnRefresh.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderStyles.Solid;
            this.btnRefresh.BorderThickness = 1;
            this.btnRefresh.ColorContrastOnClick = 30;
            this.btnRefresh.ColorContrastOnHover = 20;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnRefresh.CustomizableEdges = borderEdges2;
            this.btnRefresh.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnRefresh.Image = global::ExaltAccountManager.Properties.Resources.ic_refresh_white_24dp;
            this.btnRefresh.ImageMargin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Location = new System.Drawing.Point(116, 25);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.RoundBorders = false;
            this.btnRefresh.ShowBorders = true;
            this.btnRefresh.Size = new System.Drawing.Size(32, 32);
            this.btnRefresh.Style = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.ButtonStyles.Round;
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pData
            // 
            this.pData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pData.Controls.Add(this.pDatagrid);
            this.pData.Location = new System.Drawing.Point(20, 75);
            this.pData.Name = "pData";
            this.pData.Size = new System.Drawing.Size(617, 435);
            this.pData.TabIndex = 10;
            // 
            // pCardsTop
            // 
            this.pCardsTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pCardsTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pCardsTop.Location = new System.Drawing.Point(-2, 0);
            this.pCardsTop.Name = "pCardsTop";
            this.pCardsTop.Size = new System.Drawing.Size(659, 10);
            this.pCardsTop.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 30);
            this.label2.TabIndex = 8;
            this.label2.Text = "EAM-Log";
            // 
            // UILogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bunifuCards);
            this.Controls.Add(this.pB);
            this.Controls.Add(this.pT);
            this.Controls.Add(this.pR);
            this.Controls.Add(this.pL);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UILogs";
            this.Size = new System.Drawing.Size(677, 550);
            this.Load += new System.EventHandler(this.UILogs_Load);
            this.pDatagrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.bunifuCards.ResumeLayout(false);
            this.bunifuCards.PerformLayout();
            this.pData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pB;
        private System.Windows.Forms.Panel pT;
        private System.Windows.Forms.Panel pR;
        private System.Windows.Forms.Panel pL;
        private Bunifu.Framework.UI.BunifuCards bunifuCards;
        private System.Windows.Forms.Panel pData;
        private System.Windows.Forms.Panel pDatagrid;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private Bunifu.UI.WinForms.BunifuDataGridView dataGridView;
        private System.Windows.Forms.Panel pCardsTop;
        private System.Windows.Forms.Label label2;
        private Bunifu.UI.WinForms.BunifuButton.BunifuIconButton btnRefresh;
        private System.Windows.Forms.Label label1;
        private Bunifu.UI.WinForms.BunifuButton.BunifuIconButton btnSave;
        private Bunifu.UI.WinForms.BunifuDropdown dropExportMode;
        private System.Windows.Forms.Panel pLoader;
    }
}
