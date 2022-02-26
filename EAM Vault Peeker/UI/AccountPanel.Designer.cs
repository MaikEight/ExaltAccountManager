
namespace EAM_Vault_Peeker.UI
{
    partial class AccountPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountPanel));
            this.shadowPanel = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lEmail = new System.Windows.Forms.Label();
            this.pLists = new System.Windows.Forms.Panel();
            this.pGifts = new System.Windows.Forms.Panel();
            this.lGiftsShown = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.layoutGifts = new System.Windows.Forms.FlowLayoutPanel();
            this.bunifuSeparator3 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pPotions = new System.Windows.Forms.Panel();
            this.lPotionsShown = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.layoutPotions = new System.Windows.Forms.FlowLayoutPanel();
            this.bunifuSeparator2 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pVault = new System.Windows.Forms.Panel();
            this.lVaultShown = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.layoutVault = new System.Windows.Forms.FlowLayoutPanel();
            this.bunifuSeparator1 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pChars = new System.Windows.Forms.Panel();
            this.pbChars = new System.Windows.Forms.PictureBox();
            this.lChars = new System.Windows.Forms.Label();
            this.pChar = new System.Windows.Forms.Panel();
            this.pAccountName = new System.Windows.Forms.Panel();
            this.pbRefreshData = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbToggleVisible = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lAccountName = new System.Windows.Forms.Label();
            this.lDate = new System.Windows.Forms.Label();
            this.shadowPanel.SuspendLayout();
            this.pLists.SuspendLayout();
            this.pGifts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pPotions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pVault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pChars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbChars)).BeginInit();
            this.pAccountName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefreshData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleVisible)).BeginInit();
            this.SuspendLayout();
            // 
            // shadowPanel
            // 
            this.shadowPanel.BackColor = System.Drawing.Color.Transparent;
            this.shadowPanel.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadowPanel.BorderRadius = 9;
            this.shadowPanel.BorderThickness = 1;
            this.shadowPanel.Controls.Add(this.lEmail);
            this.shadowPanel.Controls.Add(this.pLists);
            this.shadowPanel.Controls.Add(this.pAccountName);
            this.shadowPanel.Controls.Add(this.lDate);
            this.shadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadowPanel.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadowPanel.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadowPanel.Location = new System.Drawing.Point(0, 0);
            this.shadowPanel.Name = "shadowPanel";
            this.shadowPanel.PanelColor = System.Drawing.Color.White;
            this.shadowPanel.PanelColor2 = System.Drawing.Color.White;
            this.shadowPanel.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadowPanel.ShadowDept = 2;
            this.shadowPanel.ShadowDepth = 4;
            this.shadowPanel.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadowPanel.ShadowTopLeftVisible = false;
            this.shadowPanel.Size = new System.Drawing.Size(400, 581);
            this.shadowPanel.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadowPanel.TabIndex = 0;
            // 
            // lEmail
            // 
            this.lEmail.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEmail.Location = new System.Drawing.Point(3, 32);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new System.Drawing.Size(394, 15);
            this.lEmail.TabIndex = 11;
            this.lEmail.Text = "Account Name";
            this.lEmail.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pLists
            // 
            this.pLists.Controls.Add(this.pGifts);
            this.pLists.Controls.Add(this.pPotions);
            this.pLists.Controls.Add(this.pVault);
            this.pLists.Controls.Add(this.pChars);
            this.pLists.Location = new System.Drawing.Point(8, 65);
            this.pLists.Name = "pLists";
            this.pLists.Size = new System.Drawing.Size(384, 513);
            this.pLists.TabIndex = 10;
            // 
            // pGifts
            // 
            this.pGifts.Controls.Add(this.lGiftsShown);
            this.pGifts.Controls.Add(this.pictureBox3);
            this.pGifts.Controls.Add(this.label4);
            this.pGifts.Controls.Add(this.layoutGifts);
            this.pGifts.Controls.Add(this.bunifuSeparator3);
            this.pGifts.Dock = System.Windows.Forms.DockStyle.Top;
            this.pGifts.Location = new System.Drawing.Point(0, 369);
            this.pGifts.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pGifts.Name = "pGifts";
            this.pGifts.Size = new System.Drawing.Size(384, 138);
            this.pGifts.TabIndex = 10;
            // 
            // lGiftsShown
            // 
            this.lGiftsShown.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lGiftsShown.Location = new System.Drawing.Point(191, 13);
            this.lGiftsShown.Name = "lGiftsShown";
            this.lGiftsShown.Size = new System.Drawing.Size(190, 15);
            this.lGiftsShown.TabIndex = 8;
            this.lGiftsShown.Text = "9999 / 9999 Items shown";
            this.lGiftsShown.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::EAM_Vault_Peeker.Properties.Resources.shtrs_Loot_Balloon_Bridge;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(36, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "Gift chest";
            // 
            // layoutGifts
            // 
            this.layoutGifts.Location = new System.Drawing.Point(0, 38);
            this.layoutGifts.Margin = new System.Windows.Forms.Padding(0);
            this.layoutGifts.Name = "layoutGifts";
            this.layoutGifts.Size = new System.Drawing.Size(384, 100);
            this.layoutGifts.TabIndex = 4;
            // 
            // bunifuSeparator3
            // 
            this.bunifuSeparator3.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator3.BackgroundImage")));
            this.bunifuSeparator3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator3.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.bunifuSeparator3.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator3.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.bunifuSeparator3.LineThickness = 1;
            this.bunifuSeparator3.Location = new System.Drawing.Point(0, 31);
            this.bunifuSeparator3.Margin = new System.Windows.Forms.Padding(7);
            this.bunifuSeparator3.Name = "bunifuSeparator3";
            this.bunifuSeparator3.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator3.Size = new System.Drawing.Size(384, 10);
            this.bunifuSeparator3.TabIndex = 7;
            // 
            // pPotions
            // 
            this.pPotions.Controls.Add(this.lPotionsShown);
            this.pPotions.Controls.Add(this.pictureBox2);
            this.pPotions.Controls.Add(this.label3);
            this.pPotions.Controls.Add(this.layoutPotions);
            this.pPotions.Controls.Add(this.bunifuSeparator2);
            this.pPotions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pPotions.Location = new System.Drawing.Point(0, 231);
            this.pPotions.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pPotions.Name = "pPotions";
            this.pPotions.Size = new System.Drawing.Size(384, 138);
            this.pPotions.TabIndex = 9;
            // 
            // lPotionsShown
            // 
            this.lPotionsShown.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPotionsShown.Location = new System.Drawing.Point(191, 17);
            this.lPotionsShown.Name = "lPotionsShown";
            this.lPotionsShown.Size = new System.Drawing.Size(190, 15);
            this.lPotionsShown.TabIndex = 7;
            this.lPotionsShown.Text = "9999 / 9999 Items shown";
            this.lPotionsShown.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::EAM_Vault_Peeker.Properties.Resources.Potions;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Potions";
            // 
            // layoutPotions
            // 
            this.layoutPotions.Location = new System.Drawing.Point(0, 38);
            this.layoutPotions.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPotions.Name = "layoutPotions";
            this.layoutPotions.Size = new System.Drawing.Size(384, 100);
            this.layoutPotions.TabIndex = 4;
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator2.BackgroundImage")));
            this.bunifuSeparator2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator2.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.bunifuSeparator2.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator2.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(0, 31);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator2.Size = new System.Drawing.Size(384, 10);
            this.bunifuSeparator2.TabIndex = 6;
            // 
            // pVault
            // 
            this.pVault.Controls.Add(this.lVaultShown);
            this.pVault.Controls.Add(this.pictureBox1);
            this.pVault.Controls.Add(this.label1);
            this.pVault.Controls.Add(this.layoutVault);
            this.pVault.Controls.Add(this.bunifuSeparator1);
            this.pVault.Dock = System.Windows.Forms.DockStyle.Top;
            this.pVault.Location = new System.Drawing.Point(0, 93);
            this.pVault.Margin = new System.Windows.Forms.Padding(0);
            this.pVault.Name = "pVault";
            this.pVault.Size = new System.Drawing.Size(384, 138);
            this.pVault.TabIndex = 8;
            // 
            // lVaultShown
            // 
            this.lVaultShown.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVaultShown.Location = new System.Drawing.Point(194, 17);
            this.lVaultShown.Name = "lVaultShown";
            this.lVaultShown.Size = new System.Drawing.Size(187, 15);
            this.lVaultShown.TabIndex = 6;
            this.lVaultShown.Text = "9999 / 9999 Items shown";
            this.lVaultShown.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EAM_Vault_Peeker.Properties.Resources.Vault_Portal;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Vault";
            // 
            // layoutVault
            // 
            this.layoutVault.Location = new System.Drawing.Point(0, 38);
            this.layoutVault.Margin = new System.Windows.Forms.Padding(0);
            this.layoutVault.Name = "layoutVault";
            this.layoutVault.Size = new System.Drawing.Size(384, 100);
            this.layoutVault.TabIndex = 4;
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
            this.bunifuSeparator1.Location = new System.Drawing.Point(0, 31);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator1.Size = new System.Drawing.Size(384, 10);
            this.bunifuSeparator1.TabIndex = 5;
            // 
            // pChars
            // 
            this.pChars.Controls.Add(this.pbChars);
            this.pChars.Controls.Add(this.lChars);
            this.pChars.Controls.Add(this.pChar);
            this.pChars.Dock = System.Windows.Forms.DockStyle.Top;
            this.pChars.Location = new System.Drawing.Point(0, 0);
            this.pChars.Name = "pChars";
            this.pChars.Size = new System.Drawing.Size(384, 93);
            this.pChars.TabIndex = 11;
            // 
            // pbChars
            // 
            this.pbChars.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbChars.Image = global::EAM_Vault_Peeker.Properties.Resources.ic_group_black_36dp;
            this.pbChars.Location = new System.Drawing.Point(0, 0);
            this.pbChars.Name = "pbChars";
            this.pbChars.Size = new System.Drawing.Size(32, 32);
            this.pbChars.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbChars.TabIndex = 13;
            this.pbChars.TabStop = false;
            this.pbChars.Click += new System.EventHandler(this.lChars_Click);
            // 
            // lChars
            // 
            this.lChars.AutoSize = true;
            this.lChars.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lChars.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lChars.Location = new System.Drawing.Point(36, 7);
            this.lChars.Name = "lChars";
            this.lChars.Size = new System.Drawing.Size(90, 21);
            this.lChars.TabIndex = 14;
            this.lChars.Text = "Characters";
            this.lChars.Click += new System.EventHandler(this.lChars_Click);
            // 
            // pChar
            // 
            this.pChar.Location = new System.Drawing.Point(0, 38);
            this.pChar.Name = "pChar";
            this.pChar.Size = new System.Drawing.Size(384, 55);
            this.pChar.TabIndex = 12;
            // 
            // pAccountName
            // 
            this.pAccountName.Controls.Add(this.pbRefreshData);
            this.pAccountName.Controls.Add(this.pbToggleVisible);
            this.pAccountName.Controls.Add(this.lAccountName);
            this.pAccountName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pAccountName.Location = new System.Drawing.Point(0, 0);
            this.pAccountName.Name = "pAccountName";
            this.pAccountName.Size = new System.Drawing.Size(400, 40);
            this.pAccountName.TabIndex = 0;
            // 
            // pbRefreshData
            // 
            this.pbRefreshData.AllowFocused = false;
            this.pbRefreshData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbRefreshData.AutoSizeHeight = true;
            this.pbRefreshData.BorderRadius = 10;
            this.pbRefreshData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbRefreshData.Image = global::EAM_Vault_Peeker.Properties.Resources.ic_refresh_black_18dp;
            this.pbRefreshData.IsCircle = true;
            this.pbRefreshData.Location = new System.Drawing.Point(345, 9);
            this.pbRefreshData.Name = "pbRefreshData";
            this.pbRefreshData.Size = new System.Drawing.Size(20, 20);
            this.pbRefreshData.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbRefreshData.TabIndex = 3;
            this.pbRefreshData.TabStop = false;
            this.pbRefreshData.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbRefreshData.Visible = false;
            this.pbRefreshData.Click += new System.EventHandler(this.pbRefreshData_Click);
            // 
            // pbToggleVisible
            // 
            this.pbToggleVisible.AllowFocused = false;
            this.pbToggleVisible.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbToggleVisible.AutoSizeHeight = true;
            this.pbToggleVisible.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pbToggleVisible.BorderRadius = 10;
            this.pbToggleVisible.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbToggleVisible.Image = global::EAM_Vault_Peeker.Properties.Resources.ic_visibility_off_black_18dp;
            this.pbToggleVisible.IsCircle = true;
            this.pbToggleVisible.Location = new System.Drawing.Point(370, 9);
            this.pbToggleVisible.Name = "pbToggleVisible";
            this.pbToggleVisible.Size = new System.Drawing.Size(20, 20);
            this.pbToggleVisible.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbToggleVisible.TabIndex = 2;
            this.pbToggleVisible.TabStop = false;
            this.pbToggleVisible.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbToggleVisible.Click += new System.EventHandler(this.pbToggleVisible_Click);
            // 
            // lAccountName
            // 
            this.lAccountName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAccountName.Location = new System.Drawing.Point(3, 10);
            this.lAccountName.Name = "lAccountName";
            this.lAccountName.Size = new System.Drawing.Size(394, 27);
            this.lAccountName.TabIndex = 1;
            this.lAccountName.Text = "Account Name";
            this.lAccountName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lDate
            // 
            this.lDate.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDate.Location = new System.Drawing.Point(8, 47);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(384, 15);
            this.lDate.TabIndex = 12;
            this.lDate.Text = "Data from {0}";
            this.lDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AccountPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadowPanel);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AccountPanel";
            this.Size = new System.Drawing.Size(400, 581);
            this.shadowPanel.ResumeLayout(false);
            this.pLists.ResumeLayout(false);
            this.pGifts.ResumeLayout(false);
            this.pGifts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pPotions.ResumeLayout(false);
            this.pPotions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pVault.ResumeLayout(false);
            this.pVault.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pChars.ResumeLayout(false);
            this.pChars.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbChars)).EndInit();
            this.pAccountName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbRefreshData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleVisible)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadowPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pAccountName;
        private System.Windows.Forms.Label lAccountName;
        private System.Windows.Forms.FlowLayoutPanel layoutVault;
        private System.Windows.Forms.Panel pVault;
        private System.Windows.Forms.Panel pPotions;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel layoutPotions;
        private System.Windows.Forms.Panel pGifts;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel layoutGifts;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator1;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator2;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator3;
        private System.Windows.Forms.Panel pLists;
        private System.Windows.Forms.Label lGiftsShown;
        private System.Windows.Forms.Label lPotionsShown;
        private System.Windows.Forms.Label lVaultShown;
        private System.Windows.Forms.Label lEmail;
        private Bunifu.UI.WinForms.BunifuPictureBox pbToggleVisible;
        private System.Windows.Forms.Label lDate;
        private Bunifu.UI.WinForms.BunifuPictureBox pbRefreshData;
        private System.Windows.Forms.Panel pChars;
        private System.Windows.Forms.PictureBox pbChars;
        private System.Windows.Forms.Label lChars;
        private System.Windows.Forms.Panel pChar;
    }
}
