namespace ExaltAccountManager.UI.Elements
{
    partial class EleDeleteAccount
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
            Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderEdges();
            this.lDeleteAccount = new System.Windows.Forms.Label();
            this.lKeep = new System.Windows.Forms.Label();
            this.lHint = new System.Windows.Forms.Label();
            this.lRemove = new System.Windows.Forms.Label();
            this.btnBack = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton();
            this.btnDeleteAccount = new Bunifu.UI.WinForms.BunifuButton.BunifuIconButton();
            this.SuspendLayout();
            // 
            // lDeleteAccount
            // 
            this.lDeleteAccount.AutoSize = true;
            this.lDeleteAccount.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDeleteAccount.Location = new System.Drawing.Point(35, 6);
            this.lDeleteAccount.Name = "lDeleteAccount";
            this.lDeleteAccount.Size = new System.Drawing.Size(138, 25);
            this.lDeleteAccount.TabIndex = 19;
            this.lDeleteAccount.Text = "Are you sure?";
            // 
            // lKeep
            // 
            this.lKeep.AutoSize = true;
            this.lKeep.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lKeep.Location = new System.Drawing.Point(67, 81);
            this.lKeep.Name = "lKeep";
            this.lKeep.Size = new System.Drawing.Size(120, 25);
            this.lKeep.TabIndex = 22;
            this.lKeep.Text = "No, keep it.";
            // 
            // lHint
            // 
            this.lHint.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHint.Location = new System.Drawing.Point(25, 33);
            this.lHint.Name = "lHint";
            this.lHint.Size = new System.Drawing.Size(158, 45);
            this.lHint.TabIndex = 23;
            this.lHint.Text = "This will remove the account name@mail.de from EAM.";
            this.lHint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lRemove
            // 
            this.lRemove.AutoSize = true;
            this.lRemove.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRemove.Location = new System.Drawing.Point(67, 127);
            this.lRemove.Name = "lRemove";
            this.lRemove.Size = new System.Drawing.Size(114, 25);
            this.lRemove.TabIndex = 24;
            this.lRemove.Text = "Remove it!";
            // 
            // btnBack
            // 
            this.btnBack.AllowAnimations = false;
            this.btnBack.AllowBorderColorChanges = true;
            this.btnBack.AllowMouseEffects = true;
            this.btnBack.AnimationSpeed = 100;
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
            this.btnBack.Location = new System.Drawing.Point(23, 74);
            this.btnBack.Name = "btnBack";
            this.btnBack.RoundBorders = false;
            this.btnBack.ShowBorders = true;
            this.btnBack.Size = new System.Drawing.Size(40, 40);
            this.btnBack.Style = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.ButtonStyles.Round;
            this.btnBack.TabIndex = 21;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnBack.MouseEnter += new System.EventHandler(this.btnBack_MouseEnter);
            this.btnBack.MouseLeave += new System.EventHandler(this.btnBack_MouseLeave);
            // 
            // btnDeleteAccount
            // 
            this.btnDeleteAccount.AllowAnimations = false;
            this.btnDeleteAccount.AllowBorderColorChanges = true;
            this.btnDeleteAccount.AllowMouseEffects = true;
            this.btnDeleteAccount.AnimationSpeed = 100;
            this.btnDeleteAccount.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteAccount.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this.btnDeleteAccount.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this.btnDeleteAccount.BorderRadius = 1;
            this.btnDeleteAccount.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.BorderStyles.Solid;
            this.btnDeleteAccount.BorderThickness = 1;
            this.btnDeleteAccount.ColorContrastOnClick = 30;
            this.btnDeleteAccount.ColorContrastOnHover = 20;
            this.btnDeleteAccount.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnDeleteAccount.CustomizableEdges = borderEdges2;
            this.btnDeleteAccount.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnDeleteAccount.Image = global::ExaltAccountManager.Properties.Resources.baseline_delete_outline_white_36dp;
            this.btnDeleteAccount.ImageMargin = new System.Windows.Forms.Padding(0);
            this.btnDeleteAccount.Location = new System.Drawing.Point(23, 120);
            this.btnDeleteAccount.Name = "btnDeleteAccount";
            this.btnDeleteAccount.RoundBorders = false;
            this.btnDeleteAccount.ShowBorders = true;
            this.btnDeleteAccount.Size = new System.Drawing.Size(40, 40);
            this.btnDeleteAccount.Style = Bunifu.UI.WinForms.BunifuButton.BunifuIconButton.ButtonStyles.Round;
            this.btnDeleteAccount.TabIndex = 20;
            this.btnDeleteAccount.Click += new System.EventHandler(this.btnDeleteAccount_Click);
            this.btnDeleteAccount.MouseEnter += new System.EventHandler(this.btnDeleteAccount_MouseEnter);
            this.btnDeleteAccount.MouseLeave += new System.EventHandler(this.btnDeleteAccount_MouseLeave);
            // 
            // EleDeleteAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lRemove);
            this.Controls.Add(this.lKeep);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnDeleteAccount);
            this.Controls.Add(this.lDeleteAccount);
            this.Controls.Add(this.lHint);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EleDeleteAccount";
            this.Size = new System.Drawing.Size(208, 183);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuButton.BunifuIconButton btnDeleteAccount;
        private System.Windows.Forms.Label lDeleteAccount;
        private Bunifu.UI.WinForms.BunifuButton.BunifuIconButton btnBack;
        private System.Windows.Forms.Label lKeep;
        private System.Windows.Forms.Label lHint;
        private System.Windows.Forms.Label lRemove;
    }
}
