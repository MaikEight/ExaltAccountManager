namespace ExaltAccountManager.UI
{
    partial class UIHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIHelp));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges();
            this.pDatagrid = new System.Windows.Forms.Panel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.dataGridView = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.shadowContact = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.pbMailMini = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbDiscordMini = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pLinkButtons = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pbEmail = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbDiscord = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.btnContactMinimize = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton();
            this.lHelpText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.spacorContact = new Bunifu.UI.WinForms.BunifuSeparator();
            this.bunifuCards = new Bunifu.Framework.UI.BunifuCards();
            this.pData = new System.Windows.Forms.Panel();
            this.pCardsTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pL = new System.Windows.Forms.Panel();
            this.pR = new System.Windows.Forms.Panel();
            this.pT = new System.Windows.Forms.Panel();
            this.pB = new System.Windows.Forms.Panel();
            this.pSpacer = new System.Windows.Forms.Panel();
            bunifuElipse = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.pDatagrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.shadowContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMailMini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscordMini)).BeginInit();
            this.pLinkButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscord)).BeginInit();
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
            this.pDatagrid.Size = new System.Drawing.Size(617, 275);
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
            this.scrollbar.Size = new System.Drawing.Size(8, 275);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 10;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 27;
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
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidth = 30;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.RowTemplate.Height = 40;
            this.dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ShowCellErrors = false;
            this.dataGridView.ShowCellToolTips = false;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.ShowRowErrors = false;
            this.dataGridView.Size = new System.Drawing.Size(617, 275);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.DarkViolet;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // shadowContact
            // 
            this.shadowContact.BackColor = System.Drawing.Color.Transparent;
            this.shadowContact.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadowContact.BorderRadius = 9;
            this.shadowContact.BorderThickness = 1;
            this.shadowContact.Controls.Add(this.pbMailMini);
            this.shadowContact.Controls.Add(this.pbDiscordMini);
            this.shadowContact.Controls.Add(this.pLinkButtons);
            this.shadowContact.Controls.Add(this.btnContactMinimize);
            this.shadowContact.Controls.Add(this.lHelpText);
            this.shadowContact.Controls.Add(this.label4);
            this.shadowContact.Controls.Add(this.spacorContact);
            this.shadowContact.Dock = System.Windows.Forms.DockStyle.Top;
            this.shadowContact.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadowContact.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadowContact.Location = new System.Drawing.Point(10, 10);
            this.shadowContact.Margin = new System.Windows.Forms.Padding(0);
            this.shadowContact.Name = "shadowContact";
            this.shadowContact.PanelColor = System.Drawing.Color.White;
            this.shadowContact.PanelColor2 = System.Drawing.Color.White;
            this.shadowContact.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadowContact.ShadowDept = 2;
            this.shadowContact.ShadowDepth = 4;
            this.shadowContact.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadowContact.ShadowTopLeftVisible = false;
            this.shadowContact.Size = new System.Drawing.Size(657, 170);
            this.shadowContact.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadowContact.TabIndex = 15;
            // 
            // pbMailMini
            // 
            this.pbMailMini.AllowFocused = false;
            this.pbMailMini.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbMailMini.AutoSizeHeight = true;
            this.pbMailMini.BorderRadius = 12;
            this.pbMailMini.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbMailMini.Image = global::ExaltAccountManager.Properties.Resources.ic_email_black_24dp;
            this.pbMailMini.IsCircle = true;
            this.pbMailMini.Location = new System.Drawing.Point(106, 15);
            this.pbMailMini.Name = "pbMailMini";
            this.pbMailMini.Size = new System.Drawing.Size(24, 24);
            this.pbMailMini.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMailMini.TabIndex = 17;
            this.pbMailMini.TabStop = false;
            this.pbMailMini.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbMailMini.Visible = false;
            this.pbMailMini.Click += new System.EventHandler(this.pbEmail_Click);
            this.pbMailMini.MouseEnter += new System.EventHandler(this.pbEmail_MouseEnter);
            this.pbMailMini.MouseLeave += new System.EventHandler(this.pbEmail_MouseLeave);
            // 
            // pbDiscordMini
            // 
            this.pbDiscordMini.AllowFocused = false;
            this.pbDiscordMini.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbDiscordMini.AutoSizeHeight = true;
            this.pbDiscordMini.BorderRadius = 12;
            this.pbDiscordMini.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDiscordMini.Image = global::ExaltAccountManager.Properties.Resources.UI_Icon_SocialDiscord_black1;
            this.pbDiscordMini.IsCircle = true;
            this.pbDiscordMini.Location = new System.Drawing.Point(145, 15);
            this.pbDiscordMini.Name = "pbDiscordMini";
            this.pbDiscordMini.Size = new System.Drawing.Size(24, 24);
            this.pbDiscordMini.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDiscordMini.TabIndex = 16;
            this.pbDiscordMini.TabStop = false;
            this.pbDiscordMini.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbDiscordMini.Visible = false;
            this.pbDiscordMini.Click += new System.EventHandler(this.pbDiscord_Click);
            this.pbDiscordMini.MouseEnter += new System.EventHandler(this.pbDiscord_MouseEnter);
            this.pbDiscordMini.MouseLeave += new System.EventHandler(this.pbDiscord_MouseLeave);
            // 
            // pLinkButtons
            // 
            this.pLinkButtons.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pLinkButtons.BackColor = System.Drawing.Color.White;
            this.pLinkButtons.Controls.Add(this.label3);
            this.pLinkButtons.Controls.Add(this.pbEmail);
            this.pLinkButtons.Controls.Add(this.label1);
            this.pLinkButtons.Controls.Add(this.pbDiscord);
            this.pLinkButtons.Location = new System.Drawing.Point(179, 106);
            this.pLinkButtons.Name = "pLinkButtons";
            this.pLinkButtons.Size = new System.Drawing.Size(300, 55);
            this.pLinkButtons.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(181, 35);
            this.label3.MaximumSize = new System.Drawing.Size(195, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Join the discord";
            // 
            // pbEmail
            // 
            this.pbEmail.AllowFocused = false;
            this.pbEmail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbEmail.AutoSizeHeight = true;
            this.pbEmail.BorderRadius = 18;
            this.pbEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEmail.Image = global::ExaltAccountManager.Properties.Resources.ic_email_black_48dp;
            this.pbEmail.IsCircle = true;
            this.pbEmail.Location = new System.Drawing.Point(37, 0);
            this.pbEmail.Name = "pbEmail";
            this.pbEmail.Size = new System.Drawing.Size(36, 36);
            this.pbEmail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEmail.TabIndex = 15;
            this.pbEmail.TabStop = false;
            this.pbEmail.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbEmail.Click += new System.EventHandler(this.pbEmail_Click);
            this.pbEmail.MouseEnter += new System.EventHandler(this.pbEmail_MouseEnter);
            this.pbEmail.MouseLeave += new System.EventHandler(this.pbEmail_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 35);
            this.label1.MaximumSize = new System.Drawing.Size(195, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "Leave me a Mail";
            // 
            // pbDiscord
            // 
            this.pbDiscord.AllowFocused = false;
            this.pbDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDiscord.AutoSizeHeight = true;
            this.pbDiscord.BorderRadius = 18;
            this.pbDiscord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDiscord.Image = global::ExaltAccountManager.Properties.Resources.UI_Icon_SocialDiscord_black1;
            this.pbDiscord.IsCircle = true;
            this.pbDiscord.Location = new System.Drawing.Point(217, 0);
            this.pbDiscord.Name = "pbDiscord";
            this.pbDiscord.Size = new System.Drawing.Size(36, 36);
            this.pbDiscord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDiscord.TabIndex = 14;
            this.pbDiscord.TabStop = false;
            this.pbDiscord.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbDiscord.Click += new System.EventHandler(this.pbDiscord_Click);
            this.pbDiscord.MouseEnter += new System.EventHandler(this.pbDiscord_MouseEnter);
            this.pbDiscord.MouseLeave += new System.EventHandler(this.pbDiscord_MouseLeave);
            // 
            // btnContactMinimize
            // 
            this.btnContactMinimize.AllowAnimations = true;
            this.btnContactMinimize.AllowBorderColorChanges = true;
            this.btnContactMinimize.AllowMouseEffects = true;
            this.btnContactMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContactMinimize.AnimationSpeed = 200;
            this.btnContactMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnContactMinimize.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnContactMinimize.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnContactMinimize.BorderRadius = 1;
            this.btnContactMinimize.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderStyles.Solid;
            this.btnContactMinimize.BorderThickness = 1;
            this.btnContactMinimize.ColorContrastOnClick = 30;
            this.btnContactMinimize.ColorContrastOnHover = 30;
            this.btnContactMinimize.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnContactMinimize.CustomizableEdges = borderEdges1;
            this.btnContactMinimize.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnContactMinimize.Image = global::ExaltAccountManager.Properties.Resources.ic_arrow_drop_up_white_36dp;
            this.btnContactMinimize.ImageMargin = new System.Windows.Forms.Padding(0);
            this.btnContactMinimize.Location = new System.Drawing.Point(617, 15);
            this.btnContactMinimize.Name = "btnContactMinimize";
            this.btnContactMinimize.RoundBorders = true;
            this.btnContactMinimize.ShowBorders = true;
            this.btnContactMinimize.Size = new System.Drawing.Size(24, 24);
            this.btnContactMinimize.Style = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.ButtonStyles.Round;
            this.btnContactMinimize.TabIndex = 15;
            this.btnContactMinimize.Click += new System.EventHandler(this.btnContactMinimize_Click);
            // 
            // lHelpText
            // 
            this.lHelpText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lHelpText.AutoSize = true;
            this.lHelpText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHelpText.Location = new System.Drawing.Point(26, 50);
            this.lHelpText.MaximumSize = new System.Drawing.Size(638, 0);
            this.lHelpText.Name = "lHelpText";
            this.lHelpText.Size = new System.Drawing.Size(605, 51);
            this.lHelpText.TabIndex = 7;
            this.lHelpText.Text = resources.GetString("lHelpText.Text");
            this.lHelpText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.MaximumSize = new System.Drawing.Size(176, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 30);
            this.label4.TabIndex = 12;
            this.label4.Text = "Contact";
            this.label4.UseMnemonic = false;
            // 
            // spacorContact
            // 
            this.spacorContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spacorContact.BackColor = System.Drawing.Color.Transparent;
            this.spacorContact.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("spacorContact.BackgroundImage")));
            this.spacorContact.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.spacorContact.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.spacorContact.LineColor = System.Drawing.Color.Silver;
            this.spacorContact.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.spacorContact.LineThickness = 1;
            this.spacorContact.Location = new System.Drawing.Point(16, 39);
            this.spacorContact.Margin = new System.Windows.Forms.Padding(5);
            this.spacorContact.Name = "spacorContact";
            this.spacorContact.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.spacorContact.Size = new System.Drawing.Size(625, 10);
            this.spacorContact.TabIndex = 13;
            // 
            // bunifuCards
            // 
            this.bunifuCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.bunifuCards.BorderRadius = 11;
            this.bunifuCards.BottomSahddow = true;
            this.bunifuCards.color = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.bunifuCards.Controls.Add(this.pData);
            this.bunifuCards.Controls.Add(this.pCardsTop);
            this.bunifuCards.Controls.Add(this.label2);
            this.bunifuCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bunifuCards.LeftSahddow = true;
            this.bunifuCards.Location = new System.Drawing.Point(10, 187);
            this.bunifuCards.Name = "bunifuCards";
            this.bunifuCards.RightSahddow = true;
            this.bunifuCards.ShadowDepth = 20;
            this.bunifuCards.Size = new System.Drawing.Size(657, 353);
            this.bunifuCards.TabIndex = 16;
            // 
            // pData
            // 
            this.pData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pData.Controls.Add(this.pDatagrid);
            this.pData.Location = new System.Drawing.Point(20, 58);
            this.pData.Name = "pData";
            this.pData.Size = new System.Drawing.Size(617, 275);
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
            this.label2.Size = new System.Drawing.Size(288, 30);
            this.label2.TabIndex = 8;
            this.label2.Text = "Frequently Asked Questions";
            // 
            // pL
            // 
            this.pL.Dock = System.Windows.Forms.DockStyle.Left;
            this.pL.Location = new System.Drawing.Point(0, 0);
            this.pL.Name = "pL";
            this.pL.Size = new System.Drawing.Size(10, 550);
            this.pL.TabIndex = 17;
            // 
            // pR
            // 
            this.pR.Dock = System.Windows.Forms.DockStyle.Right;
            this.pR.Location = new System.Drawing.Point(667, 0);
            this.pR.Name = "pR";
            this.pR.Size = new System.Drawing.Size(10, 550);
            this.pR.TabIndex = 18;
            // 
            // pT
            // 
            this.pT.Dock = System.Windows.Forms.DockStyle.Top;
            this.pT.Location = new System.Drawing.Point(10, 0);
            this.pT.Name = "pT";
            this.pT.Size = new System.Drawing.Size(657, 10);
            this.pT.TabIndex = 19;
            // 
            // pB
            // 
            this.pB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pB.Location = new System.Drawing.Point(10, 540);
            this.pB.Name = "pB";
            this.pB.Size = new System.Drawing.Size(657, 10);
            this.pB.TabIndex = 20;
            // 
            // pSpacer
            // 
            this.pSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSpacer.Location = new System.Drawing.Point(10, 180);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(657, 7);
            this.pSpacer.TabIndex = 21;
            // 
            // UIHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bunifuCards);
            this.Controls.Add(this.pSpacer);
            this.Controls.Add(this.shadowContact);
            this.Controls.Add(this.pB);
            this.Controls.Add(this.pT);
            this.Controls.Add(this.pR);
            this.Controls.Add(this.pL);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIHelp";
            this.Size = new System.Drawing.Size(677, 550);
            this.Load += new System.EventHandler(this.UIHelp_Load);
            this.pDatagrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.shadowContact.ResumeLayout(false);
            this.shadowContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMailMini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscordMini)).EndInit();
            this.pLinkButtons.ResumeLayout(false);
            this.pLinkButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDiscord)).EndInit();
            this.bunifuCards.ResumeLayout(false);
            this.bunifuCards.PerformLayout();
            this.pData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.UI.WinForms.BunifuShadowPanel shadowContact;
        private Bunifu.UI.WinForms.BunifuSeparator spacorContact;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lHelpText;
        private Bunifu.UI.WinForms.BunifuPictureBox pbEmail;
        private Bunifu.UI.WinForms.BunifuPictureBox pbDiscord;
        private System.Windows.Forms.Panel pLinkButtons;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuCards bunifuCards;
        private System.Windows.Forms.Panel pData;
        private System.Windows.Forms.Panel pDatagrid;
        private Bunifu.UI.WinForms.BunifuDataGridView dataGridView;
        private System.Windows.Forms.Panel pCardsTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pL;
        private System.Windows.Forms.Panel pR;
        private System.Windows.Forms.Panel pT;
        private System.Windows.Forms.Panel pB;
        private Bunifu.UI.WinForms.BunifuButton.BunifuIconButton btnContactMinimize;
        private System.Windows.Forms.Panel pSpacer;
        private Bunifu.UI.WinForms.BunifuPictureBox pbMailMini;
        private Bunifu.UI.WinForms.BunifuPictureBox pbDiscordMini;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
    }
}
