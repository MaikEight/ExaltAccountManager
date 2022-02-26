
namespace EAM_Vault_Peeker.UI
{
    partial class CharacterUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterUI));
            this.lCharType = new System.Windows.Forms.Label();
            this.pCharEQ = new System.Windows.Forms.Panel();
            this.tableLayoutEQ = new System.Windows.Forms.TableLayoutPanel();
            this.lLevel = new System.Windows.Forms.Label();
            this.lXOf8 = new System.Windows.Forms.Label();
            this.lFame = new System.Windows.Forms.Label();
            this.pbClass = new System.Windows.Forms.PictureBox();
            this.lCharClass = new System.Windows.Forms.Label();
            this.bunifuSeparator1 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pInventory = new System.Windows.Forms.Panel();
            this.pbInventory = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.layoutVault = new System.Windows.Forms.FlowLayoutPanel();
            this.bunifuSeparator2 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pBackpack = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.layoutBackpack = new System.Windows.Forms.FlowLayoutPanel();
            this.bunifuSeparator3 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.bunifuSeparator4 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbChar = new System.Windows.Forms.PictureBox();
            this.pCharEQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).BeginInit();
            this.pInventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInventory)).BeginInit();
            this.pBackpack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChar)).BeginInit();
            this.SuspendLayout();
            // 
            // lCharType
            // 
            this.lCharType.AutoSize = true;
            this.lCharType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCharType.Location = new System.Drawing.Point(36, 7);
            this.lCharType.Name = "lCharType";
            this.lCharType.Size = new System.Drawing.Size(81, 21);
            this.lCharType.TabIndex = 8;
            this.lCharType.Text = "CharType";
            // 
            // pCharEQ
            // 
            this.pCharEQ.Controls.Add(this.tableLayoutEQ);
            this.pCharEQ.Controls.Add(this.lLevel);
            this.pCharEQ.Controls.Add(this.lXOf8);
            this.pCharEQ.Controls.Add(this.lFame);
            this.pCharEQ.Controls.Add(this.pbClass);
            this.pCharEQ.Controls.Add(this.lCharClass);
            this.pCharEQ.Controls.Add(this.bunifuSeparator1);
            this.pCharEQ.Dock = System.Windows.Forms.DockStyle.Top;
            this.pCharEQ.Location = new System.Drawing.Point(0, 0);
            this.pCharEQ.Margin = new System.Windows.Forms.Padding(0);
            this.pCharEQ.Name = "pCharEQ";
            this.pCharEQ.Size = new System.Drawing.Size(384, 90);
            this.pCharEQ.TabIndex = 9;
            // 
            // tableLayoutEQ
            // 
            this.tableLayoutEQ.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutEQ.ColumnCount = 4;
            this.tableLayoutEQ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutEQ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutEQ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutEQ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutEQ.Location = new System.Drawing.Point(100, 33);
            this.tableLayoutEQ.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutEQ.MaximumSize = new System.Drawing.Size(192, 50);
            this.tableLayoutEQ.Name = "tableLayoutEQ";
            this.tableLayoutEQ.RowCount = 1;
            this.tableLayoutEQ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutEQ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutEQ.Size = new System.Drawing.Size(192, 50);
            this.tableLayoutEQ.TabIndex = 8;
            // 
            // lLevel
            // 
            this.lLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLevel.Location = new System.Drawing.Point(3, 35);
            this.lLevel.Name = "lLevel";
            this.lLevel.Size = new System.Drawing.Size(83, 15);
            this.lLevel.TabIndex = 10;
            this.lLevel.Text = "Level: {0}";
            // 
            // lXOf8
            // 
            this.lXOf8.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lXOf8.Location = new System.Drawing.Point(3, 69);
            this.lXOf8.Name = "lXOf8";
            this.lXOf8.Size = new System.Drawing.Size(83, 15);
            this.lXOf8.TabIndex = 7;
            this.lXOf8.Text = "X/8: {0} / 8";
            // 
            // lFame
            // 
            this.lFame.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFame.Location = new System.Drawing.Point(3, 52);
            this.lFame.Name = "lFame";
            this.lFame.Size = new System.Drawing.Size(88, 15);
            this.lFame.TabIndex = 9;
            this.lFame.Text = "Fame: {0}";
            // 
            // pbClass
            // 
            this.pbClass.Image = global::EAM_Vault_Peeker.Properties.Resources.Vault_Portal;
            this.pbClass.Location = new System.Drawing.Point(0, 0);
            this.pbClass.Name = "pbClass";
            this.pbClass.Size = new System.Drawing.Size(32, 32);
            this.pbClass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbClass.TabIndex = 1;
            this.pbClass.TabStop = false;
            // 
            // lCharClass
            // 
            this.lCharClass.AutoSize = true;
            this.lCharClass.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCharClass.Location = new System.Drawing.Point(36, 7);
            this.lCharClass.Name = "lCharClass";
            this.lCharClass.Size = new System.Drawing.Size(91, 21);
            this.lCharClass.TabIndex = 2;
            this.lCharClass.Text = "CharName";
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
            this.bunifuSeparator1.Location = new System.Drawing.Point(0, 28);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator1.Size = new System.Drawing.Size(384, 10);
            this.bunifuSeparator1.TabIndex = 5;
            // 
            // pInventory
            // 
            this.pInventory.Controls.Add(this.pbInventory);
            this.pInventory.Controls.Add(this.label3);
            this.pInventory.Controls.Add(this.layoutVault);
            this.pInventory.Controls.Add(this.bunifuSeparator2);
            this.pInventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.pInventory.Location = new System.Drawing.Point(0, 90);
            this.pInventory.Margin = new System.Windows.Forms.Padding(0);
            this.pInventory.Name = "pInventory";
            this.pInventory.Size = new System.Drawing.Size(384, 70);
            this.pInventory.TabIndex = 10;
            // 
            // pbInventory
            // 
            this.pbInventory.Image = global::EAM_Vault_Peeker.Properties.Resources.bag_24px_1;
            this.pbInventory.Location = new System.Drawing.Point(0, 1);
            this.pbInventory.Name = "pbInventory";
            this.pbInventory.Size = new System.Drawing.Size(16, 16);
            this.pbInventory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInventory.TabIndex = 1;
            this.pbInventory.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Inventory";
            // 
            // layoutVault
            // 
            this.layoutVault.Location = new System.Drawing.Point(0, 20);
            this.layoutVault.Margin = new System.Windows.Forms.Padding(0);
            this.layoutVault.Name = "layoutVault";
            this.layoutVault.Size = new System.Drawing.Size(384, 50);
            this.layoutVault.TabIndex = 4;
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
            this.bunifuSeparator2.Location = new System.Drawing.Point(0, 13);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator2.Size = new System.Drawing.Size(384, 10);
            this.bunifuSeparator2.TabIndex = 5;
            // 
            // pBackpack
            // 
            this.pBackpack.Controls.Add(this.pictureBox3);
            this.pBackpack.Controls.Add(this.label5);
            this.pBackpack.Controls.Add(this.layoutBackpack);
            this.pBackpack.Controls.Add(this.bunifuSeparator3);
            this.pBackpack.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBackpack.Location = new System.Drawing.Point(0, 160);
            this.pBackpack.Margin = new System.Windows.Forms.Padding(0);
            this.pBackpack.Name = "pBackpack";
            this.pBackpack.Size = new System.Drawing.Size(384, 70);
            this.pBackpack.TabIndex = 11;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::EAM_Vault_Peeker.Properties.Resources.Backpack;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(18, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Backpack";
            // 
            // layoutBackpack
            // 
            this.layoutBackpack.Location = new System.Drawing.Point(0, 20);
            this.layoutBackpack.Margin = new System.Windows.Forms.Padding(0);
            this.layoutBackpack.Name = "layoutBackpack";
            this.layoutBackpack.Size = new System.Drawing.Size(384, 50);
            this.layoutBackpack.TabIndex = 4;
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
            this.bunifuSeparator3.Location = new System.Drawing.Point(0, 13);
            this.bunifuSeparator3.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator3.Name = "bunifuSeparator3";
            this.bunifuSeparator3.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator3.Size = new System.Drawing.Size(384, 10);
            this.bunifuSeparator3.TabIndex = 5;
            // 
            // bunifuSeparator4
            // 
            this.bunifuSeparator4.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator4.BackgroundImage")));
            this.bunifuSeparator4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator4.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Triangle;
            this.bunifuSeparator4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bunifuSeparator4.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator4.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Dash;
            this.bunifuSeparator4.LineThickness = 3;
            this.bunifuSeparator4.Location = new System.Drawing.Point(0, 226);
            this.bunifuSeparator4.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator4.Name = "bunifuSeparator4";
            this.bunifuSeparator4.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator4.Size = new System.Drawing.Size(384, 12);
            this.bunifuSeparator4.TabIndex = 12;
            // 
            // pbChar
            // 
            this.pbChar.Image = global::EAM_Vault_Peeker.Properties.Resources.Vault_Portal;
            this.pbChar.Location = new System.Drawing.Point(0, 0);
            this.pbChar.Name = "pbChar";
            this.pbChar.Size = new System.Drawing.Size(32, 32);
            this.pbChar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbChar.TabIndex = 7;
            this.pbChar.TabStop = false;
            // 
            // CharacterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bunifuSeparator4);
            this.Controls.Add(this.pBackpack);
            this.Controls.Add(this.pInventory);
            this.Controls.Add(this.pCharEQ);
            this.Controls.Add(this.pbChar);
            this.Controls.Add(this.lCharType);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CharacterUI";
            this.Size = new System.Drawing.Size(384, 238);
            this.pCharEQ.ResumeLayout(false);
            this.pCharEQ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).EndInit();
            this.pInventory.ResumeLayout(false);
            this.pInventory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInventory)).EndInit();
            this.pBackpack.ResumeLayout(false);
            this.pBackpack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbChar;
        private System.Windows.Forms.Label lCharType;
        private System.Windows.Forms.Panel pCharEQ;
        private System.Windows.Forms.PictureBox pbClass;
        private System.Windows.Forms.Label lCharClass;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator1;
        private System.Windows.Forms.Panel pInventory;
        private System.Windows.Forms.PictureBox pbInventory;
        private System.Windows.Forms.Label label3;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator2;
        private System.Windows.Forms.Panel pBackpack;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label5;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator3;
        public System.Windows.Forms.FlowLayoutPanel layoutVault;
        public System.Windows.Forms.FlowLayoutPanel layoutBackpack;
        private System.Windows.Forms.Label lXOf8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutEQ;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator4;
        private System.Windows.Forms.Label lLevel;
        private System.Windows.Forms.Label lFame;
    }
}
