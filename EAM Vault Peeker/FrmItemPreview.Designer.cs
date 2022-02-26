
namespace EAM_Vault_Peeker
{
    partial class FrmItemPreview
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
            this.lName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pName = new System.Windows.Forms.Panel();
            this.pTier = new System.Windows.Forms.Panel();
            this.lTier = new System.Windows.Forms.Label();
            this.pFeedpower = new System.Windows.Forms.Panel();
            this.lFeedpower = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pAmount = new System.Windows.Forms.Panel();
            this.lAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pFameBonus = new System.Windows.Forms.Panel();
            this.lFameBonus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bunifuFormDock = new Bunifu.UI.WinForms.BunifuFormDock();
            this.pName.SuspendLayout();
            this.pTier.SuspendLayout();
            this.pFeedpower.SuspendLayout();
            this.pAmount.SuspendLayout();
            this.pFameBonus.SuspendLayout();
            this.SuspendLayout();
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(3, 2);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(85, 20);
            this.lName.TabIndex = 0;
            this.lName.Text = "Item name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tier";
            // 
            // pName
            // 
            this.pName.Controls.Add(this.lName);
            this.pName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pName.Location = new System.Drawing.Point(0, 0);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(166, 24);
            this.pName.TabIndex = 2;
            this.pName.Paint += new System.Windows.Forms.PaintEventHandler(this.pName_Paint);
            // 
            // pTier
            // 
            this.pTier.Controls.Add(this.lTier);
            this.pTier.Controls.Add(this.label1);
            this.pTier.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTier.Location = new System.Drawing.Point(0, 24);
            this.pTier.Name = "pTier";
            this.pTier.Size = new System.Drawing.Size(166, 27);
            this.pTier.TabIndex = 3;
            // 
            // lTier
            // 
            this.lTier.AutoSize = true;
            this.lTier.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTier.Location = new System.Drawing.Point(110, 2);
            this.lTier.Name = "lTier";
            this.lTier.Size = new System.Drawing.Size(18, 20);
            this.lTier.TabIndex = 2;
            this.lTier.Text = "0";
            // 
            // pFeedpower
            // 
            this.pFeedpower.Controls.Add(this.lFeedpower);
            this.pFeedpower.Controls.Add(this.label3);
            this.pFeedpower.Dock = System.Windows.Forms.DockStyle.Top;
            this.pFeedpower.Location = new System.Drawing.Point(0, 51);
            this.pFeedpower.Name = "pFeedpower";
            this.pFeedpower.Size = new System.Drawing.Size(166, 27);
            this.pFeedpower.TabIndex = 4;
            // 
            // lFeedpower
            // 
            this.lFeedpower.AutoSize = true;
            this.lFeedpower.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFeedpower.Location = new System.Drawing.Point(110, 3);
            this.lFeedpower.Name = "lFeedpower";
            this.lFeedpower.Size = new System.Drawing.Size(52, 20);
            this.lFeedpower.TabIndex = 2;
            this.lFeedpower.Text = "10000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Feedpower";
            // 
            // pAmount
            // 
            this.pAmount.Controls.Add(this.lAmount);
            this.pAmount.Controls.Add(this.label5);
            this.pAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.pAmount.Location = new System.Drawing.Point(0, 105);
            this.pAmount.Name = "pAmount";
            this.pAmount.Size = new System.Drawing.Size(166, 27);
            this.pAmount.TabIndex = 5;
            // 
            // lAmount
            // 
            this.lAmount.AutoSize = true;
            this.lAmount.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAmount.Location = new System.Drawing.Point(110, 3);
            this.lAmount.Name = "lAmount";
            this.lAmount.Size = new System.Drawing.Size(18, 20);
            this.lAmount.TabIndex = 2;
            this.lAmount.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Amount";
            // 
            // pFameBonus
            // 
            this.pFameBonus.Controls.Add(this.lFameBonus);
            this.pFameBonus.Controls.Add(this.label4);
            this.pFameBonus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pFameBonus.Location = new System.Drawing.Point(0, 78);
            this.pFameBonus.Name = "pFameBonus";
            this.pFameBonus.Size = new System.Drawing.Size(166, 27);
            this.pFameBonus.TabIndex = 6;
            // 
            // lFameBonus
            // 
            this.lFameBonus.AutoSize = true;
            this.lFameBonus.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFameBonus.Location = new System.Drawing.Point(110, 3);
            this.lFameBonus.Name = "lFameBonus";
            this.lFameBonus.Size = new System.Drawing.Size(18, 20);
            this.lFameBonus.TabIndex = 2;
            this.lFameBonus.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Famebonus";
            // 
            // bunifuFormDock
            // 
            this.bunifuFormDock.AllowFormDragging = false;
            this.bunifuFormDock.AllowFormDropShadow = true;
            this.bunifuFormDock.AllowFormResizing = false;
            this.bunifuFormDock.AllowHidingBottomRegion = true;
            this.bunifuFormDock.AllowOpacityChangesWhileDragging = false;
            this.bunifuFormDock.BorderOptions.BottomBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock.BorderOptions.BottomBorder.BorderThickness = 1;
            this.bunifuFormDock.BorderOptions.BottomBorder.ShowBorder = true;
            this.bunifuFormDock.BorderOptions.LeftBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock.BorderOptions.LeftBorder.BorderThickness = 1;
            this.bunifuFormDock.BorderOptions.LeftBorder.ShowBorder = true;
            this.bunifuFormDock.BorderOptions.RightBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock.BorderOptions.RightBorder.BorderThickness = 1;
            this.bunifuFormDock.BorderOptions.RightBorder.ShowBorder = true;
            this.bunifuFormDock.BorderOptions.TopBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock.BorderOptions.TopBorder.BorderThickness = 1;
            this.bunifuFormDock.BorderOptions.TopBorder.ShowBorder = true;
            this.bunifuFormDock.ContainerControl = this;
            this.bunifuFormDock.DockingIndicatorsColor = System.Drawing.Color.Black;
            this.bunifuFormDock.DockingIndicatorsOpacity = 0.5D;
            this.bunifuFormDock.DockingOptions.DockAll = false;
            this.bunifuFormDock.DockingOptions.DockBottomLeft = false;
            this.bunifuFormDock.DockingOptions.DockBottomRight = false;
            this.bunifuFormDock.DockingOptions.DockFullScreen = false;
            this.bunifuFormDock.DockingOptions.DockLeft = false;
            this.bunifuFormDock.DockingOptions.DockRight = false;
            this.bunifuFormDock.DockingOptions.DockTopLeft = false;
            this.bunifuFormDock.DockingOptions.DockTopRight = false;
            this.bunifuFormDock.FormDraggingOpacity = 0.9D;
            this.bunifuFormDock.ParentForm = this;
            this.bunifuFormDock.ShowCursorChanges = false;
            this.bunifuFormDock.ShowDockingIndicators = false;
            this.bunifuFormDock.TitleBarOptions.AllowFormDragging = true;
            this.bunifuFormDock.TitleBarOptions.BunifuFormDock = this.bunifuFormDock;
            this.bunifuFormDock.TitleBarOptions.DoubleClickToExpandWindow = true;
            this.bunifuFormDock.TitleBarOptions.TitleBarControl = this.pName;
            this.bunifuFormDock.TitleBarOptions.UseBackColorOnDockingIndicators = false;
            // 
            // FrmItemPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(166, 132);
            this.ControlBox = false;
            this.Controls.Add(this.pAmount);
            this.Controls.Add(this.pFameBonus);
            this.Controls.Add(this.pFeedpower);
            this.Controls.Add(this.pTier);
            this.Controls.Add(this.pName);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmItemPreview";
            this.Opacity = 0.97D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Item Preview";
            this.pName.ResumeLayout(false);
            this.pName.PerformLayout();
            this.pTier.ResumeLayout(false);
            this.pTier.PerformLayout();
            this.pFeedpower.ResumeLayout(false);
            this.pFeedpower.PerformLayout();
            this.pAmount.ResumeLayout(false);
            this.pAmount.PerformLayout();
            this.pFameBonus.ResumeLayout(false);
            this.pFameBonus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pName;
        private System.Windows.Forms.Panel pTier;
        private System.Windows.Forms.Label lTier;
        private System.Windows.Forms.Panel pFeedpower;
        private System.Windows.Forms.Label lFeedpower;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pAmount;
        private System.Windows.Forms.Label lAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pFameBonus;
        private System.Windows.Forms.Label lFameBonus;
        private System.Windows.Forms.Label label4;
        private Bunifu.UI.WinForms.BunifuFormDock bunifuFormDock;
    }
}