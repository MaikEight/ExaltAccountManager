
namespace ExaltAccountManager
{
    partial class FrmImportAskPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImportAskPass));
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            this.btnImportOK = new System.Windows.Forms.Button();
            this.tbImportPassword = new Bunifu.UI.WinForms.BunifuTextBox();
            this.lPass = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImportOK
            // 
            this.btnImportOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportOK.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportOK.Location = new System.Drawing.Point(189, 27);
            this.btnImportOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportOK.Name = "btnImportOK";
            this.btnImportOK.Size = new System.Drawing.Size(36, 30);
            this.btnImportOK.TabIndex = 14;
            this.btnImportOK.Text = "OK";
            this.btnImportOK.UseVisualStyleBackColor = true;
            this.btnImportOK.Click += new System.EventHandler(this.btnImportOK_Click);
            // 
            // tbImportPassword
            // 
            this.tbImportPassword.AcceptsReturn = false;
            this.tbImportPassword.AcceptsTab = false;
            this.tbImportPassword.AnimationSpeed = 200;
            this.tbImportPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbImportPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbImportPassword.BackColor = System.Drawing.Color.Transparent;
            this.tbImportPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbImportPassword.BackgroundImage")));
            this.tbImportPassword.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.tbImportPassword.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.tbImportPassword.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.tbImportPassword.BorderColorIdle = System.Drawing.Color.Silver;
            this.tbImportPassword.BorderRadius = 1;
            this.tbImportPassword.BorderThickness = 1;
            this.tbImportPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tbImportPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbImportPassword.DefaultFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbImportPassword.DefaultText = "";
            this.tbImportPassword.FillColor = System.Drawing.Color.White;
            this.tbImportPassword.HideSelection = true;
            this.tbImportPassword.IconLeft = null;
            this.tbImportPassword.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbImportPassword.IconPadding = 10;
            this.tbImportPassword.IconRight = null;
            this.tbImportPassword.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbImportPassword.Lines = new string[0];
            this.tbImportPassword.Location = new System.Drawing.Point(5, 26);
            this.tbImportPassword.Margin = new System.Windows.Forms.Padding(6);
            this.tbImportPassword.MaximumSize = new System.Drawing.Size(184, 32);
            this.tbImportPassword.MaxLength = 32767;
            this.tbImportPassword.MinimumSize = new System.Drawing.Size(2, 2);
            this.tbImportPassword.Modified = false;
            this.tbImportPassword.Multiline = false;
            this.tbImportPassword.Name = "tbImportPassword";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbImportPassword.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            stateProperties2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            stateProperties2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.tbImportPassword.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbImportPassword.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.Silver;
            stateProperties4.FillColor = System.Drawing.Color.White;
            stateProperties4.ForeColor = System.Drawing.Color.Empty;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbImportPassword.OnIdleState = stateProperties4;
            this.tbImportPassword.Padding = new System.Windows.Forms.Padding(6);
            this.tbImportPassword.PasswordChar = '\0';
            this.tbImportPassword.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.tbImportPassword.PlaceholderText = "Enter password";
            this.tbImportPassword.ReadOnly = false;
            this.tbImportPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbImportPassword.SelectedText = "";
            this.tbImportPassword.SelectionLength = 0;
            this.tbImportPassword.SelectionStart = 0;
            this.tbImportPassword.ShortcutsEnabled = true;
            this.tbImportPassword.Size = new System.Drawing.Size(184, 32);
            this.tbImportPassword.Style = Bunifu.UI.WinForms.BunifuTextBox._Style.Bunifu;
            this.tbImportPassword.TabIndex = 15;
            this.tbImportPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbImportPassword.TextMarginBottom = 0;
            this.tbImportPassword.TextMarginLeft = 3;
            this.tbImportPassword.TextMarginTop = 0;
            this.tbImportPassword.TextPlaceholder = "Enter password";
            this.tbImportPassword.UseSystemPasswordChar = false;
            this.tbImportPassword.WordWrap = true;
            this.tbImportPassword.TextChanged += new System.EventHandler(this.tbImportPassword_TextChanged);
            // 
            // lPass
            // 
            this.lPass.Dock = System.Windows.Forms.DockStyle.Top;
            this.lPass.Font = new System.Drawing.Font("Century Schoolbook", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPass.Location = new System.Drawing.Point(0, 0);
            this.lPass.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lPass.Name = "lPass";
            this.lPass.Size = new System.Drawing.Size(230, 20);
            this.lPass.TabIndex = 16;
            this.lPass.Text = "Please enter the import-password";
            this.lPass.UseMnemonic = false;
            this.lPass.Paint += new System.Windows.Forms.PaintEventHandler(this.lPass_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(5, 60);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 22);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel import";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmImportAskPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(230, 85);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lPass);
            this.Controls.Add(this.btnImportOK);
            this.Controls.Add(this.tbImportPassword);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmImportAskPass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Import Password";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmImportAskPass_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportOK;
        private Bunifu.UI.WinForms.BunifuTextBox tbImportPassword;
        private System.Windows.Forms.Label lPass;
        private System.Windows.Forms.Button btnCancel;
    }
}