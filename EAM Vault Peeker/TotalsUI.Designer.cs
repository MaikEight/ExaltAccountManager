namespace EAM_Vault_Peeker
{
    partial class TotalsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalsUI));
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState1 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState2 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState3 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            this.pTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toggleShowSelectedAccountsOnly = new Bunifu.UI.WinForms.BunifuToggleSwitch();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSearch = new Bunifu.UI.WinForms.BunifuTextBox();
            this.bunifuSeparator2 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbFilter = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bunifuSeparator1 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.label5 = new System.Windows.Forms.Label();
            this.dropTier = new Bunifu.UI.WinForms.BunifuDropdown();
            this.label4 = new System.Windows.Forms.Label();
            this.dropFeedpower = new Bunifu.UI.WinForms.BunifuDropdown();
            this.label2 = new System.Windows.Forms.Label();
            this.dropItemType = new Bunifu.UI.WinForms.BunifuDropdown();
            this.pContent = new System.Windows.Forms.Panel();
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.pSpacerLeft = new System.Windows.Forms.Panel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.timerSearch = new System.Windows.Forms.Timer(this.components);
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFilter)).BeginInit();
            this.pContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.label1);
            this.pTop.Controls.Add(this.toggleShowSelectedAccountsOnly);
            this.pTop.Controls.Add(this.label6);
            this.pTop.Controls.Add(this.tbSearch);
            this.pTop.Controls.Add(this.bunifuSeparator2);
            this.pTop.Controls.Add(this.pbFilter);
            this.pTop.Controls.Add(this.label3);
            this.pTop.Controls.Add(this.bunifuSeparator1);
            this.pTop.Controls.Add(this.label5);
            this.pTop.Controls.Add(this.dropTier);
            this.pTop.Controls.Add(this.label4);
            this.pTop.Controls.Add(this.dropFeedpower);
            this.pTop.Controls.Add(this.label2);
            this.pTop.Controls.Add(this.dropItemType);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(808, 100);
            this.pTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(667, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "Selected accounts";
            // 
            // toggleShowSelectedAccountsOnly
            // 
            this.toggleShowSelectedAccountsOnly.Animation = 5;
            this.toggleShowSelectedAccountsOnly.AnimationSpeed = 5;
            this.toggleShowSelectedAccountsOnly.BackColor = System.Drawing.Color.Transparent;
            this.toggleShowSelectedAccountsOnly.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toggleShowSelectedAccountsOnly.BackgroundImage")));
            this.toggleShowSelectedAccountsOnly.Checked = false;
            this.toggleShowSelectedAccountsOnly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toggleShowSelectedAccountsOnly.InnerCirclePadding = 3;
            this.toggleShowSelectedAccountsOnly.Location = new System.Drawing.Point(670, 75);
            this.toggleShowSelectedAccountsOnly.Margin = new System.Windows.Forms.Padding(5);
            this.toggleShowSelectedAccountsOnly.Name = "toggleShowSelectedAccountsOnly";
            this.toggleShowSelectedAccountsOnly.Size = new System.Drawing.Size(40, 16);
            this.toggleShowSelectedAccountsOnly.TabIndex = 23;
            this.toggleShowSelectedAccountsOnly.ThumbMargin = 3;
            toggleState1.BackColor = System.Drawing.Color.DarkGray;
            toggleState1.BackColorInner = System.Drawing.Color.White;
            toggleState1.BorderColor = System.Drawing.Color.DarkGray;
            toggleState1.BorderColorInner = System.Drawing.Color.White;
            toggleState1.BorderRadius = 17;
            toggleState1.BorderRadiusInner = 11;
            toggleState1.BorderThickness = 1;
            toggleState1.BorderThicknessInner = 1;
            this.toggleShowSelectedAccountsOnly.ToggleStateDisabled = toggleState1;
            toggleState2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            toggleState2.BackColorInner = System.Drawing.Color.White;
            toggleState2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            toggleState2.BorderColorInner = System.Drawing.Color.White;
            toggleState2.BorderRadius = 15;
            toggleState2.BorderRadiusInner = 9;
            toggleState2.BorderThickness = 1;
            toggleState2.BorderThicknessInner = 1;
            this.toggleShowSelectedAccountsOnly.ToggleStateOff = toggleState2;
            toggleState3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            toggleState3.BackColorInner = System.Drawing.Color.White;
            toggleState3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            toggleState3.BorderColorInner = System.Drawing.Color.White;
            toggleState3.BorderRadius = 15;
            toggleState3.BorderRadiusInner = 9;
            toggleState3.BorderThickness = 1;
            toggleState3.BorderThicknessInner = 1;
            this.toggleShowSelectedAccountsOnly.ToggleStateOn = toggleState3;
            this.toggleShowSelectedAccountsOnly.Value = false;
            this.toggleShowSelectedAccountsOnly.CheckedChanged += new System.EventHandler<Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs>(this.toggleShowSelectedAccountsOnly_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(373, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 17);
            this.label6.TabIndex = 22;
            this.label6.Text = "Search items by name";
            // 
            // tbSearch
            // 
            this.tbSearch.AcceptsReturn = false;
            this.tbSearch.AcceptsTab = false;
            this.tbSearch.AnimationSpeed = 200;
            this.tbSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbSearch.AutoSizeHeight = true;
            this.tbSearch.BackColor = System.Drawing.Color.White;
            this.tbSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbSearch.BackgroundImage")));
            this.tbSearch.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.tbSearch.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.tbSearch.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.tbSearch.BorderColorIdle = System.Drawing.Color.Silver;
            this.tbSearch.BorderRadius = 1;
            this.tbSearch.BorderThickness = 2;
            this.tbSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tbSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSearch.DefaultFont = new System.Drawing.Font("Segoe UI", 9.25F);
            this.tbSearch.DefaultText = "";
            this.tbSearch.FillColor = System.Drawing.Color.White;
            this.tbSearch.ForeColor = System.Drawing.Color.Black;
            this.tbSearch.HideSelection = true;
            this.tbSearch.IconLeft = global::EAM_Vault_Peeker.Properties.Resources.ic_search_black_24dp;
            this.tbSearch.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSearch.IconPadding = 5;
            this.tbSearch.IconRight = null;
            this.tbSearch.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSearch.Lines = new string[0];
            this.tbSearch.Location = new System.Drawing.Point(376, 65);
            this.tbSearch.MaximumSize = new System.Drawing.Size(283, 32);
            this.tbSearch.MaxLength = 32767;
            this.tbSearch.MinimumSize = new System.Drawing.Size(283, 32);
            this.tbSearch.Modified = false;
            this.tbSearch.Multiline = false;
            this.tbSearch.Name = "tbSearch";
            stateProperties1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbSearch.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            stateProperties2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            stateProperties2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.tbSearch.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbSearch.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.Silver;
            stateProperties4.FillColor = System.Drawing.Color.White;
            stateProperties4.ForeColor = System.Drawing.Color.Black;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbSearch.OnIdleState = stateProperties4;
            this.tbSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tbSearch.PasswordChar = '\0';
            this.tbSearch.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.tbSearch.PlaceholderText = "Search for Item";
            this.tbSearch.ReadOnly = false;
            this.tbSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSearch.SelectedText = "";
            this.tbSearch.SelectionLength = 0;
            this.tbSearch.SelectionStart = 0;
            this.tbSearch.ShortcutsEnabled = true;
            this.tbSearch.Size = new System.Drawing.Size(283, 32);
            this.tbSearch.Style = Bunifu.UI.WinForms.BunifuTextBox._Style.Material;
            this.tbSearch.TabIndex = 21;
            this.tbSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbSearch.TextMarginBottom = 0;
            this.tbSearch.TextMarginLeft = 3;
            this.tbSearch.TextMarginTop = 1;
            this.tbSearch.TextPlaceholder = "Search for Item";
            this.tbSearch.UseSystemPasswordChar = false;
            this.tbSearch.WordWrap = true;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator2.BackgroundImage")));
            this.bunifuSeparator2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator2.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.bunifuSeparator2.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator2.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(0, 35);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator2.Size = new System.Drawing.Size(808, 1);
            this.bunifuSeparator2.TabIndex = 20;
            // 
            // pbFilter
            // 
            this.pbFilter.Image = global::EAM_Vault_Peeker.Properties.Resources.ic_filter_list_black_36dp;
            this.pbFilter.Location = new System.Drawing.Point(0, 0);
            this.pbFilter.Name = "pbFilter";
            this.pbFilter.Size = new System.Drawing.Size(36, 36);
            this.pbFilter.TabIndex = 19;
            this.pbFilter.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(37, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 25);
            this.label3.TabIndex = 18;
            this.label3.Text = "Filter ";
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator1.BackgroundImage")));
            this.bunifuSeparator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator1.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.bunifuSeparator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator1.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(0, 99);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator1.Size = new System.Drawing.Size(808, 1);
            this.bunifuSeparator1.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "Tier";
            // 
            // dropTier
            // 
            this.dropTier.BackColor = System.Drawing.Color.Transparent;
            this.dropTier.BackgroundColor = System.Drawing.Color.White;
            this.dropTier.BorderColor = System.Drawing.Color.Silver;
            this.dropTier.BorderRadius = 9;
            this.dropTier.Color = System.Drawing.Color.Silver;
            this.dropTier.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropTier.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropTier.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropTier.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropTier.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropTier.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropTier.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropTier.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropTier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropTier.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropTier.FillDropDown = true;
            this.dropTier.FillIndicator = false;
            this.dropTier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropTier.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dropTier.ForeColor = System.Drawing.Color.Black;
            this.dropTier.FormattingEnabled = true;
            this.dropTier.Icon = null;
            this.dropTier.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropTier.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.dropTier.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropTier.IndicatorThickness = 2;
            this.dropTier.IsDropdownOpened = false;
            this.dropTier.ItemBackColor = System.Drawing.Color.White;
            this.dropTier.ItemBorderColor = System.Drawing.Color.White;
            this.dropTier.ItemForeColor = System.Drawing.Color.Black;
            this.dropTier.ItemHeight = 26;
            this.dropTier.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dropTier.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropTier.Items.AddRange(new object[] {
            "All",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "UT / ST"});
            this.dropTier.ItemTopMargin = 3;
            this.dropTier.Location = new System.Drawing.Point(310, 65);
            this.dropTier.Name = "dropTier";
            this.dropTier.Size = new System.Drawing.Size(60, 32);
            this.dropTier.TabIndex = 15;
            this.dropTier.Text = null;
            this.dropTier.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropTier.TextLeftMargin = 5;
            this.dropTier.SelectedIndexChanged += new System.EventHandler(this.dropTier_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Feedpower   >=";
            // 
            // dropFeedpower
            // 
            this.dropFeedpower.BackColor = System.Drawing.Color.Transparent;
            this.dropFeedpower.BackgroundColor = System.Drawing.Color.White;
            this.dropFeedpower.BorderColor = System.Drawing.Color.Silver;
            this.dropFeedpower.BorderRadius = 9;
            this.dropFeedpower.Color = System.Drawing.Color.Silver;
            this.dropFeedpower.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropFeedpower.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropFeedpower.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropFeedpower.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropFeedpower.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropFeedpower.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropFeedpower.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropFeedpower.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropFeedpower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropFeedpower.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropFeedpower.FillDropDown = true;
            this.dropFeedpower.FillIndicator = false;
            this.dropFeedpower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropFeedpower.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dropFeedpower.ForeColor = System.Drawing.Color.Black;
            this.dropFeedpower.FormattingEnabled = true;
            this.dropFeedpower.Icon = null;
            this.dropFeedpower.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropFeedpower.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.dropFeedpower.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropFeedpower.IndicatorThickness = 2;
            this.dropFeedpower.IsDropdownOpened = false;
            this.dropFeedpower.ItemBackColor = System.Drawing.Color.White;
            this.dropFeedpower.ItemBorderColor = System.Drawing.Color.White;
            this.dropFeedpower.ItemForeColor = System.Drawing.Color.Black;
            this.dropFeedpower.ItemHeight = 26;
            this.dropFeedpower.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dropFeedpower.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropFeedpower.Items.AddRange(new object[] {
            "All",
            "200",
            "300",
            "400",
            "450",
            "500",
            "550",
            "600",
            "650",
            "700",
            "750",
            "800",
            "900",
            "1000",
            "1300"});
            this.dropFeedpower.ItemTopMargin = 3;
            this.dropFeedpower.Location = new System.Drawing.Point(179, 65);
            this.dropFeedpower.Name = "dropFeedpower";
            this.dropFeedpower.Size = new System.Drawing.Size(125, 32);
            this.dropFeedpower.TabIndex = 13;
            this.dropFeedpower.Text = null;
            this.dropFeedpower.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropFeedpower.TextLeftMargin = 5;
            this.dropFeedpower.SelectedIndexChanged += new System.EventHandler(this.dropFeedpower_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "Type";
            // 
            // dropItemType
            // 
            this.dropItemType.BackColor = System.Drawing.Color.Transparent;
            this.dropItemType.BackgroundColor = System.Drawing.Color.White;
            this.dropItemType.BorderColor = System.Drawing.Color.Silver;
            this.dropItemType.BorderRadius = 9;
            this.dropItemType.Color = System.Drawing.Color.Silver;
            this.dropItemType.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropItemType.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropItemType.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropItemType.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropItemType.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropItemType.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropItemType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropItemType.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropItemType.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropItemType.FillDropDown = true;
            this.dropItemType.FillIndicator = false;
            this.dropItemType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropItemType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dropItemType.ForeColor = System.Drawing.Color.Black;
            this.dropItemType.FormattingEnabled = true;
            this.dropItemType.Icon = null;
            this.dropItemType.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropItemType.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.dropItemType.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropItemType.IndicatorThickness = 2;
            this.dropItemType.IsDropdownOpened = false;
            this.dropItemType.ItemBackColor = System.Drawing.Color.White;
            this.dropItemType.ItemBorderColor = System.Drawing.Color.White;
            this.dropItemType.ItemForeColor = System.Drawing.Color.Black;
            this.dropItemType.ItemHeight = 26;
            this.dropItemType.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(40)))), ((int)(((byte)(203)))));
            this.dropItemType.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropItemType.ItemTopMargin = 3;
            this.dropItemType.Location = new System.Drawing.Point(3, 65);
            this.dropItemType.Name = "dropItemType";
            this.dropItemType.Size = new System.Drawing.Size(170, 32);
            this.dropItemType.TabIndex = 11;
            this.dropItemType.Text = null;
            this.dropItemType.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropItemType.TextLeftMargin = 5;
            this.dropItemType.SelectedIndexChanged += new System.EventHandler(this.dropItemType_SelectedIndexChanged);
            // 
            // pContent
            // 
            this.pContent.Controls.Add(this.flow);
            this.pContent.Controls.Add(this.pSpacerLeft);
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(0, 100);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(800, 526);
            this.pContent.TabIndex = 1;
            // 
            // flow
            // 
            this.flow.Dock = System.Windows.Forms.DockStyle.Top;
            this.flow.Location = new System.Drawing.Point(0, 0);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(800, 100);
            this.flow.TabIndex = 0;
            // 
            // pSpacerLeft
            // 
            this.pSpacerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pSpacerLeft.Location = new System.Drawing.Point(0, 0);
            this.pSpacerLeft.Name = "pSpacerLeft";
            this.pSpacerLeft.Size = new System.Drawing.Size(0, 526);
            this.pSpacerLeft.TabIndex = 1;
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
            this.scrollbar.BorderThickness = 1;
            this.scrollbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(800, 100);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(0);
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
            this.scrollbar.Size = new System.Drawing.Size(8, 526);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 4;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 51;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.scrollbar.Value = 0;
            // 
            // timerSearch
            // 
            this.timerSearch.Interval = 250;
            this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
            // 
            // TotalsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.scrollbar);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TotalsUI";
            this.Size = new System.Drawing.Size(808, 626);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFilter)).EndInit();
            this.pContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pContent;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private System.Windows.Forms.FlowLayoutPanel flow;
        private System.Windows.Forms.Label label5;
        private Bunifu.UI.WinForms.BunifuDropdown dropTier;
        private System.Windows.Forms.Label label4;
        private Bunifu.UI.WinForms.BunifuDropdown dropFeedpower;
        private System.Windows.Forms.Label label2;
        private Bunifu.UI.WinForms.BunifuDropdown dropItemType;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator1;
        private System.Windows.Forms.Panel pSpacerLeft;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator2;
        private System.Windows.Forms.PictureBox pbFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private Bunifu.UI.WinForms.BunifuTextBox tbSearch;
        private System.Windows.Forms.Timer timerSearch;
        private System.Windows.Forms.Label label1;
        private Bunifu.UI.WinForms.BunifuToggleSwitch toggleShowSelectedAccountsOnly;
    }
}
