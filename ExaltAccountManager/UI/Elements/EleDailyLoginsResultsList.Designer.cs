namespace ExaltAccountManager.UI.Elements
{
    partial class EleDailyLoginsResultsList
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
            Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleDailyLoginsResultsList));
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.btnBack = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton();
            this.pDatagridview = new System.Windows.Forms.Panel();
            this.dataGridView = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lHint = new System.Windows.Forms.Label();
            this.lQuestion = new System.Windows.Forms.Label();
            this.bunifuElipse = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Manuall = new System.Windows.Forms.DataGridViewImageColumn();
            this.Success = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Failed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shadow.SuspendLayout();
            this.pDatagridview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.btnBack);
            this.shadow.Controls.Add(this.pDatagridview);
            this.shadow.Controls.Add(this.spacerQuestion);
            this.shadow.Controls.Add(this.pbClose);
            this.shadow.Controls.Add(this.lHint);
            this.shadow.Controls.Add(this.lQuestion);
            this.shadow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadow.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow.Location = new System.Drawing.Point(0, 0);
            this.shadow.Margin = new System.Windows.Forms.Padding(0);
            this.shadow.Name = "shadow";
            this.shadow.PanelColor = System.Drawing.Color.White;
            this.shadow.PanelColor2 = System.Drawing.Color.White;
            this.shadow.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadow.ShadowDept = 2;
            this.shadow.ShadowDepth = 4;
            this.shadow.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow.ShadowTopLeftVisible = false;
            this.shadow.Size = new System.Drawing.Size(500, 440);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 17;
            // 
            // btnBack
            // 
            this.btnBack.AllowAnimations = true;
            this.btnBack.AllowBorderColorChanges = true;
            this.btnBack.AllowMouseEffects = true;
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.AnimationSpeed = 200;
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnBack.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.btnBack.BorderRadius = 1;
            this.btnBack.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderStyles.Solid;
            this.btnBack.BorderThickness = 1;
            this.btnBack.ColorContrastOnClick = 30;
            this.btnBack.ColorContrastOnHover = 20;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnBack.CustomizableEdges = borderEdges1;
            this.btnBack.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnBack.Image = global::ExaltAccountManager.Properties.Resources.return_36_white;
            this.btnBack.ImageMargin = new System.Windows.Forms.Padding(0);
            this.btnBack.Location = new System.Drawing.Point(454, 42);
            this.btnBack.Name = "btnBack";
            this.btnBack.RoundBorders = false;
            this.btnBack.ShowBorders = true;
            this.btnBack.Size = new System.Drawing.Size(30, 30);
            this.btnBack.Style = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.ButtonStyles.Round;
            this.btnBack.TabIndex = 27;
            this.btnBack.Visible = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pDatagridview
            // 
            this.pDatagridview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pDatagridview.Controls.Add(this.dataGridView);
            this.pDatagridview.Controls.Add(this.scrollbar);
            this.pDatagridview.Location = new System.Drawing.Point(16, 73);
            this.pDatagridview.Name = "pDatagridview";
            this.pDatagridview.Size = new System.Drawing.Size(468, 351);
            this.pDatagridview.TabIndex = 26;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowCustomTheming = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
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
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.Manuall,
            this.Success,
            this.Failed});
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
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
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
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidth = 30;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.RowTemplate.Height = 35;
            this.dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ShowCellErrors = false;
            this.dataGridView.ShowCellToolTips = false;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.ShowRowErrors = false;
            this.dataGridView.Size = new System.Drawing.Size(460, 351);
            this.dataGridView.TabIndex = 11;
            this.dataGridView.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.DarkViolet;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
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
            this.scrollbar.Location = new System.Drawing.Point(460, 0);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(5);
            this.scrollbar.Maximum = 100;
            this.scrollbar.MaximumSize = new System.Drawing.Size(11, 0);
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
            this.scrollbar.Size = new System.Drawing.Size(8, 351);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 10;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 34;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.scrollbar.Value = 0;
            this.scrollbar.Scroll += new System.EventHandler<Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs>(this.scrollbar_Scroll);
            // 
            // spacerQuestion
            // 
            this.spacerQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spacerQuestion.BackColor = System.Drawing.Color.Transparent;
            this.spacerQuestion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("spacerQuestion.BackgroundImage")));
            this.spacerQuestion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.spacerQuestion.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.spacerQuestion.LineColor = System.Drawing.Color.Silver;
            this.spacerQuestion.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.spacerQuestion.LineThickness = 1;
            this.spacerQuestion.Location = new System.Drawing.Point(16, 36);
            this.spacerQuestion.Margin = new System.Windows.Forms.Padding(5);
            this.spacerQuestion.Name = "spacerQuestion";
            this.spacerQuestion.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.spacerQuestion.Size = new System.Drawing.Size(468, 10);
            this.spacerQuestion.TabIndex = 13;
            // 
            // pbClose
            // 
            this.pbClose.AllowFocused = false;
            this.pbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbClose.AutoSizeHeight = true;
            this.pbClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pbClose.BorderRadius = 9;
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_18dp;
            this.pbClose.IsCircle = true;
            this.pbClose.Location = new System.Drawing.Point(473, 9);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(18, 18);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClose.TabIndex = 25;
            this.pbClose.TabStop = false;
            this.pbClose.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // lHint
            // 
            this.lHint.AutoSize = true;
            this.lHint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHint.Location = new System.Drawing.Point(12, 51);
            this.lHint.MaximumSize = new System.Drawing.Size(476, 0);
            this.lHint.Name = "lHint";
            this.lHint.Size = new System.Drawing.Size(255, 20);
            this.lHint.TabIndex = 17;
            this.lHint.Text = "Click on a result to view more details.";
            // 
            // lQuestion
            // 
            this.lQuestion.AutoSize = true;
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(12, 12);
            this.lQuestion.MaximumSize = new System.Drawing.Size(468, 0);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(169, 25);
            this.lQuestion.TabIndex = 12;
            this.lQuestion.Text = "Daily Login results";
            this.lQuestion.UseMnemonic = false;
            // 
            // bunifuElipse
            // 
            this.bunifuElipse.ElipseRadius = 11;
            this.bunifuElipse.TargetControl = this.pDatagridview;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // Manuall
            // 
            this.Manuall.HeaderText = "Manuall";
            this.Manuall.Name = "Manuall";
            this.Manuall.ReadOnly = true;
            // 
            // Success
            // 
            this.Success.HeaderText = "Success";
            this.Success.Name = "Success";
            this.Success.ReadOnly = true;
            // 
            // Failed
            // 
            this.Failed.HeaderText = "Failed";
            this.Failed.Name = "Failed";
            this.Failed.ReadOnly = true;
            // 
            // EleDailyLoginsResultsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleDailyLoginsResultsList";
            this.Size = new System.Drawing.Size(500, 440);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            this.pDatagridview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
        private System.Windows.Forms.Label lHint;
        private System.Windows.Forms.Label lQuestion;
        private System.Windows.Forms.Panel pDatagridview;
        private Bunifu.UI.WinForms.BunifuDataGridView dataGridView;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse;
        private Bunifu.UI.WinForms.BunifuButton.BunifuIconButton btnBack;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewImageColumn Manuall;
        private System.Windows.Forms.DataGridViewTextBoxColumn Success;
        private System.Windows.Forms.DataGridViewTextBoxColumn Failed;
    }
}
