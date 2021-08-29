
namespace ExaltAccountManager
{
    partial class FrmImExport
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImExport));
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState1 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState2 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState3 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.dropExports = new Bunifu.UI.WinForms.BunifuDropdown();
            this.label1 = new System.Windows.Forms.Label();
            this.pOptions = new System.Windows.Forms.Panel();
            this.pTbPassword = new System.Windows.Forms.Panel();
            this.tbPassword = new Bunifu.UI.WinForms.BunifuTextBox();
            this.pToggleRisk = new System.Windows.Forms.Panel();
            this.lDarkmode = new System.Windows.Forms.Label();
            this.toggleAcceptRisk = new Bunifu.UI.WinForms.BunifuToggleSwitch();
            this.btnExport = new System.Windows.Forms.Button();
            this.seperator = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pImport = new Bunifu.UI.WinForms.BunifuPanel();
            this.pbImport = new System.Windows.Forms.PictureBox();
            this.lImport = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.timerNotSupported = new System.Windows.Forms.Timer(this.components);
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pOptions.SuspendLayout();
            this.pTbPassword.SuspendLayout();
            this.pToggleRisk.SuspendLayout();
            this.pImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImport)).BeginInit();
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
            this.pTop.Size = new System.Drawing.Size(250, 48);
            this.pTop.TabIndex = 2;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(202, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(48, 48);
            this.pBox.TabIndex = 4;
            this.pBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pBox_Paint);
            this.pBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
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
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_import_export_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            this.pbLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pbLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(54, 12);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(142, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Im- & Export";
            this.lHeadline.UseMnemonic = false;
            this.lHeadline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lHeadline.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // dropExports
            // 
            this.dropExports.BackColor = System.Drawing.Color.Transparent;
            this.dropExports.BackgroundColor = System.Drawing.Color.White;
            this.dropExports.BorderColor = System.Drawing.Color.Silver;
            this.dropExports.BorderRadius = 1;
            this.dropExports.Color = System.Drawing.Color.Silver;
            this.dropExports.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropExports.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropExports.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropExports.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropExports.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropExports.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropExports.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropExports.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropExports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropExports.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropExports.FillDropDown = true;
            this.dropExports.FillIndicator = false;
            this.dropExports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropExports.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dropExports.ForeColor = System.Drawing.Color.Black;
            this.dropExports.FormattingEnabled = true;
            this.dropExports.Icon = null;
            this.dropExports.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropExports.IndicatorColor = System.Drawing.Color.Gray;
            this.dropExports.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropExports.ItemBackColor = System.Drawing.Color.White;
            this.dropExports.ItemBorderColor = System.Drawing.Color.White;
            this.dropExports.ItemForeColor = System.Drawing.Color.Black;
            this.dropExports.ItemHeight = 26;
            this.dropExports.ItemHighLightColor = System.Drawing.Color.DodgerBlue;
            this.dropExports.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropExports.Items.AddRange(new object[] {
            "Accounts encrypted with a password",
            "Accounts NOT encrypted (carefull!)",
            "Accounts as CSV. without passwords",
            "Accounts as CSV. with passwords (carefull!)",
            "Complete save file encrypted with a password"});
            this.dropExports.ItemTopMargin = 3;
            this.dropExports.Location = new System.Drawing.Point(10, 182);
            this.dropExports.Name = "dropExports";
            this.dropExports.Size = new System.Drawing.Size(230, 32);
            this.dropExports.TabIndex = 7;
            this.dropExports.Text = null;
            this.dropExports.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropExports.TextLeftMargin = 5;
            this.dropExports.SelectedIndexChanged += new System.EventHandler(this.dropExports_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(90, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Export";
            this.label1.UseMnemonic = false;
            // 
            // pOptions
            // 
            this.pOptions.Controls.Add(this.pTbPassword);
            this.pOptions.Controls.Add(this.pToggleRisk);
            this.pOptions.Location = new System.Drawing.Point(10, 218);
            this.pOptions.Name = "pOptions";
            this.pOptions.Size = new System.Drawing.Size(230, 86);
            this.pOptions.TabIndex = 10;
            // 
            // pTbPassword
            // 
            this.pTbPassword.Controls.Add(this.tbPassword);
            this.pTbPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTbPassword.Location = new System.Drawing.Point(0, 44);
            this.pTbPassword.Name = "pTbPassword";
            this.pTbPassword.Size = new System.Drawing.Size(230, 42);
            this.pTbPassword.TabIndex = 12;
            // 
            // tbPassword
            // 
            this.tbPassword.AcceptsReturn = false;
            this.tbPassword.AcceptsTab = false;
            this.tbPassword.AnimationSpeed = 200;
            this.tbPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbPassword.BackColor = System.Drawing.Color.Transparent;
            this.tbPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbPassword.BackgroundImage")));
            this.tbPassword.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.tbPassword.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.tbPassword.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.tbPassword.BorderColorIdle = System.Drawing.Color.Silver;
            this.tbPassword.BorderRadius = 1;
            this.tbPassword.BorderThickness = 1;
            this.tbPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tbPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPassword.DefaultFont = new System.Drawing.Font("Segoe UI", 9.25F);
            this.tbPassword.DefaultText = "";
            this.tbPassword.FillColor = System.Drawing.Color.White;
            this.tbPassword.HideSelection = true;
            this.tbPassword.IconLeft = null;
            this.tbPassword.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPassword.IconPadding = 10;
            this.tbPassword.IconRight = null;
            this.tbPassword.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPassword.Lines = new string[0];
            this.tbPassword.Location = new System.Drawing.Point(6, 7);
            this.tbPassword.MaxLength = 32767;
            this.tbPassword.MinimumSize = new System.Drawing.Size(1, 1);
            this.tbPassword.Modified = false;
            this.tbPassword.Multiline = false;
            this.tbPassword.Name = "tbPassword";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbPassword.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            stateProperties2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            stateProperties2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.tbPassword.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbPassword.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.Silver;
            stateProperties4.FillColor = System.Drawing.Color.White;
            stateProperties4.ForeColor = System.Drawing.Color.Empty;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbPassword.OnIdleState = stateProperties4;
            this.tbPassword.Padding = new System.Windows.Forms.Padding(3);
            this.tbPassword.PasswordChar = '\0';
            this.tbPassword.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.tbPassword.PlaceholderText = "Password";
            this.tbPassword.ReadOnly = false;
            this.tbPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbPassword.SelectedText = "";
            this.tbPassword.SelectionLength = 0;
            this.tbPassword.SelectionStart = 0;
            this.tbPassword.ShortcutsEnabled = true;
            this.tbPassword.Size = new System.Drawing.Size(218, 30);
            this.tbPassword.Style = Bunifu.UI.WinForms.BunifuTextBox._Style.Bunifu;
            this.tbPassword.TabIndex = 12;
            this.tbPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbPassword.TextMarginBottom = 0;
            this.tbPassword.TextMarginLeft = 3;
            this.tbPassword.TextMarginTop = 0;
            this.tbPassword.TextPlaceholder = "Password";
            this.tbPassword.UseSystemPasswordChar = false;
            this.tbPassword.WordWrap = true;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // pToggleRisk
            // 
            this.pToggleRisk.Controls.Add(this.lDarkmode);
            this.pToggleRisk.Controls.Add(this.toggleAcceptRisk);
            this.pToggleRisk.Dock = System.Windows.Forms.DockStyle.Top;
            this.pToggleRisk.Location = new System.Drawing.Point(0, 0);
            this.pToggleRisk.Name = "pToggleRisk";
            this.pToggleRisk.Size = new System.Drawing.Size(230, 44);
            this.pToggleRisk.TabIndex = 11;
            // 
            // lDarkmode
            // 
            this.lDarkmode.AutoSize = true;
            this.lDarkmode.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDarkmode.Location = new System.Drawing.Point(43, 6);
            this.lDarkmode.Name = "lDarkmode";
            this.lDarkmode.Size = new System.Drawing.Size(181, 32);
            this.lDarkmode.TabIndex = 7;
            this.lDarkmode.Text = "I accept the security risk an \r\nunencrypted savefile can cause.";
            // 
            // toggleAcceptRisk
            // 
            this.toggleAcceptRisk.Animation = 5;
            this.toggleAcceptRisk.BackColor = System.Drawing.Color.Transparent;
            this.toggleAcceptRisk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toggleAcceptRisk.BackgroundImage")));
            this.toggleAcceptRisk.Checked = false;
            this.toggleAcceptRisk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toggleAcceptRisk.InnerCirclePadding = 3;
            this.toggleAcceptRisk.Location = new System.Drawing.Point(6, 15);
            this.toggleAcceptRisk.Margin = new System.Windows.Forms.Padding(6);
            this.toggleAcceptRisk.Name = "toggleAcceptRisk";
            this.toggleAcceptRisk.Size = new System.Drawing.Size(32, 18);
            this.toggleAcceptRisk.TabIndex = 6;
            this.toggleAcceptRisk.ThumbMargin = 3;
            toggleState1.BackColor = System.Drawing.Color.DarkGray;
            toggleState1.BackColorInner = System.Drawing.Color.White;
            toggleState1.BorderColor = System.Drawing.Color.DarkGray;
            toggleState1.BorderColorInner = System.Drawing.Color.White;
            toggleState1.BorderRadius = 17;
            toggleState1.BorderRadiusInner = 11;
            toggleState1.BorderThickness = 1;
            toggleState1.BorderThicknessInner = 1;
            this.toggleAcceptRisk.ToggleStateDisabled = toggleState1;
            toggleState2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            toggleState2.BackColorInner = System.Drawing.Color.White;
            toggleState2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            toggleState2.BorderColorInner = System.Drawing.Color.White;
            toggleState2.BorderRadius = 17;
            toggleState2.BorderRadiusInner = 11;
            toggleState2.BorderThickness = 1;
            toggleState2.BorderThicknessInner = 1;
            this.toggleAcceptRisk.ToggleStateOff = toggleState2;
            toggleState3.BackColor = System.Drawing.Color.Black;
            toggleState3.BackColorInner = System.Drawing.Color.White;
            toggleState3.BorderColor = System.Drawing.Color.Black;
            toggleState3.BorderColorInner = System.Drawing.Color.White;
            toggleState3.BorderRadius = 17;
            toggleState3.BorderRadiusInner = 11;
            toggleState3.BorderThickness = 1;
            toggleState3.BorderThicknessInner = 1;
            this.toggleAcceptRisk.ToggleStateOn = toggleState3;
            this.toggleAcceptRisk.Value = false;
            this.toggleAcceptRisk.CheckedChanged += new System.EventHandler<Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs>(this.toggleAcceptRisk_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Image = global::ExaltAccountManager.Properties.Resources.outline_save_black_18dp;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(10, 310);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(230, 30);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export as selected above";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.btnExport.MouseEnter += new System.EventHandler(this.btnExport_MouseEnter);
            this.btnExport.MouseLeave += new System.EventHandler(this.btnExport_MouseLeave);
            // 
            // seperator
            // 
            this.seperator.BackColor = System.Drawing.Color.Transparent;
            this.seperator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("seperator.BackgroundImage")));
            this.seperator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seperator.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.seperator.LineColor = System.Drawing.Color.Silver;
            this.seperator.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.seperator.LineThickness = 1;
            this.seperator.Location = new System.Drawing.Point(5, 150);
            this.seperator.Margin = new System.Windows.Forms.Padding(6);
            this.seperator.Name = "seperator";
            this.seperator.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.seperator.Size = new System.Drawing.Size(240, 5);
            this.seperator.TabIndex = 8;
            // 
            // pImport
            // 
            this.pImport.AllowDrop = true;
            this.pImport.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pImport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pImport.BackgroundImage")));
            this.pImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pImport.BorderColor = System.Drawing.Color.Black;
            this.pImport.BorderRadius = 20;
            this.pImport.BorderThickness = 2;
            this.pImport.Controls.Add(this.pbImport);
            this.pImport.Controls.Add(this.lImport);
            this.pImport.Controls.Add(this.btnImport);
            this.pImport.Location = new System.Drawing.Point(10, 58);
            this.pImport.Name = "pImport";
            this.pImport.ShowBorders = true;
            this.pImport.Size = new System.Drawing.Size(230, 85);
            this.pImport.TabIndex = 6;
            this.pImport.DragDrop += new System.Windows.Forms.DragEventHandler(this.pImport_DragDrop);
            this.pImport.DragEnter += new System.Windows.Forms.DragEventHandler(this.pImport_DragEnter);
            this.pImport.DragLeave += new System.EventHandler(this.pImport_DragLeave);
            // 
            // pbImport
            // 
            this.pbImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pbImport.Image = global::ExaltAccountManager.Properties.Resources.ic_filter_none_black_48dp;
            this.pbImport.Location = new System.Drawing.Point(9, 28);
            this.pbImport.Name = "pbImport";
            this.pbImport.Size = new System.Drawing.Size(46, 46);
            this.pbImport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbImport.TabIndex = 4;
            this.pbImport.TabStop = false;
            // 
            // lImport
            // 
            this.lImport.AutoSize = true;
            this.lImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lImport.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lImport.Location = new System.Drawing.Point(25, 4);
            this.lImport.Name = "lImport";
            this.lImport.Size = new System.Drawing.Size(181, 19);
            this.lImport.TabIndex = 5;
            this.lImport.Text = "Darg & drop to import";
            this.lImport.UseMnemonic = false;
            // 
            // btnImport
            // 
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Image = global::ExaltAccountManager.Properties.Resources.ic_folder_open_black_18dp;
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(65, 50);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(151, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Or click to choose a file";
            this.btnImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            this.btnImport.MouseEnter += new System.EventHandler(this.btnImport_MouseEnter);
            this.btnImport.MouseLeave += new System.EventHandler(this.btnImport_MouseLeave);
            // 
            // timerNotSupported
            // 
            this.timerNotSupported.Interval = 3000;
            this.timerNotSupported.Tick += new System.EventHandler(this.timerNotSupported_Tick);
            // 
            // FrmImExport
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(250, 350);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.pOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seperator);
            this.Controls.Add(this.dropExports);
            this.Controls.Add(this.pImport);
            this.Controls.Add(this.pTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmImExport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Century Schoolbook; 7,875pt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FmImExport_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pOptions.ResumeLayout(false);
            this.pTbPassword.ResumeLayout(false);
            this.pToggleRisk.ResumeLayout(false);
            this.pToggleRisk.PerformLayout();
            this.pImport.ResumeLayout(false);
            this.pImport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.PictureBox pbImport;
        private System.Windows.Forms.Label lImport;
        private System.Windows.Forms.Button btnImport;
        private Bunifu.UI.WinForms.BunifuPanel pImport;
        private Bunifu.UI.WinForms.BunifuDropdown dropExports;
        private Bunifu.UI.WinForms.BunifuSeparator seperator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pOptions;
        private System.Windows.Forms.Panel pTbPassword;
        private System.Windows.Forms.Panel pToggleRisk;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lDarkmode;
        private Bunifu.UI.WinForms.BunifuToggleSwitch toggleAcceptRisk;
        private Bunifu.UI.WinForms.BunifuTextBox tbPassword;
        private System.Windows.Forms.Timer timerNotSupported;
    }
}