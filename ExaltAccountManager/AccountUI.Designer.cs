namespace ExaltAccountManager
{
    partial class AccountUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountUI));
            this.lAccountName = new System.Windows.Forms.Label();
            this.lEmail = new System.Windows.Forms.Label();
            this.pAccName = new System.Windows.Forms.Panel();
            this.pEmail = new System.Windows.Forms.Panel();
            this.pScroll = new System.Windows.Forms.Panel();
            this.pCheckBox = new System.Windows.Forms.Panel();
            this.checkBox = new Bunifu.UI.WinForms.BunifuCheckBox();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.pPBPlay = new System.Windows.Forms.Panel();
            this.pbPlay = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pPBGetNewToken = new System.Windows.Forms.Panel();
            this.pbGetNewToken = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timerResetGetToken = new System.Windows.Forms.Timer(this.components);
            this.timerCheckProcess = new System.Windows.Forms.Timer(this.components);
            this.pDrag = new System.Windows.Forms.Panel();
            this.pbDragHandle = new System.Windows.Forms.PictureBox();
            this.pbEdit = new System.Windows.Forms.PictureBox();
            this.pbDelete = new System.Windows.Forms.PictureBox();
            this.pAccName.SuspendLayout();
            this.pEmail.SuspendLayout();
            this.pCheckBox.SuspendLayout();
            this.pPBPlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlay)).BeginInit();
            this.pPBGetNewToken.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGetNewToken)).BeginInit();
            this.pDrag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDragHandle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).BeginInit();
            this.SuspendLayout();
            // 
            // lAccountName
            // 
            this.lAccountName.AutoSize = true;
            this.lAccountName.Font = new System.Drawing.Font("Century Schoolbook", 11.25F);
            this.lAccountName.Location = new System.Drawing.Point(3, 9);
            this.lAccountName.Name = "lAccountName";
            this.lAccountName.Size = new System.Drawing.Size(110, 18);
            this.lAccountName.TabIndex = 0;
            this.lAccountName.Text = "Account Name";
            this.lAccountName.Click += new System.EventHandler(this.lAccountName_Click);
            // 
            // lEmail
            // 
            this.lEmail.AutoSize = true;
            this.lEmail.Font = new System.Drawing.Font("Century Schoolbook", 11.25F);
            this.lEmail.Location = new System.Drawing.Point(6, 9);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new System.Drawing.Size(129, 18);
            this.lEmail.TabIndex = 1;
            this.lEmail.Text = "email@email.com";
            this.lEmail.Click += new System.EventHandler(this.lEmail_Click);
            // 
            // pAccName
            // 
            this.pAccName.Controls.Add(this.lAccountName);
            this.pAccName.Dock = System.Windows.Forms.DockStyle.Left;
            this.pAccName.Location = new System.Drawing.Point(34, 0);
            this.pAccName.Name = "pAccName";
            this.pAccName.Size = new System.Drawing.Size(150, 36);
            this.pAccName.TabIndex = 2;
            // 
            // pEmail
            // 
            this.pEmail.Controls.Add(this.lEmail);
            this.pEmail.Dock = System.Windows.Forms.DockStyle.Left;
            this.pEmail.Location = new System.Drawing.Point(184, 0);
            this.pEmail.Name = "pEmail";
            this.pEmail.Size = new System.Drawing.Size(180, 36);
            this.pEmail.TabIndex = 3;
            // 
            // pScroll
            // 
            this.pScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.pScroll.Location = new System.Drawing.Point(559, 0);
            this.pScroll.Name = "pScroll";
            this.pScroll.Size = new System.Drawing.Size(21, 36);
            this.pScroll.TabIndex = 7;
            this.pScroll.Visible = false;
            // 
            // pCheckBox
            // 
            this.pCheckBox.Controls.Add(this.checkBox);
            this.pCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pCheckBox.Location = new System.Drawing.Point(524, 0);
            this.pCheckBox.Name = "pCheckBox";
            this.pCheckBox.Size = new System.Drawing.Size(35, 36);
            this.pCheckBox.TabIndex = 8;
            // 
            // checkBox
            // 
            this.checkBox.AllowBindingControlAnimation = true;
            this.checkBox.AllowBindingControlColorChanges = false;
            this.checkBox.AllowBindingControlLocation = true;
            this.checkBox.AllowCheckBoxAnimation = true;
            this.checkBox.AllowCheckmarkAnimation = true;
            this.checkBox.AllowOnHoverStates = true;
            this.checkBox.AutoCheck = true;
            this.checkBox.BackColor = System.Drawing.Color.Transparent;
            this.checkBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkBox.BackgroundImage")));
            this.checkBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.checkBox.BindingControlPosition = Bunifu.UI.WinForms.BunifuCheckBox.BindingControlPositions.Right;
            this.checkBox.BorderRadius = 5;
            this.checkBox.Checked = true;
            this.checkBox.CheckState = Bunifu.UI.WinForms.BunifuCheckBox.CheckStates.Checked;
            this.checkBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBox.CustomCheckmarkImage = null;
            this.checkBox.Location = new System.Drawing.Point(1, 7);
            this.checkBox.MinimumSize = new System.Drawing.Size(17, 17);
            this.checkBox.Name = "checkBox";
            this.checkBox.OnCheck.BorderColor = System.Drawing.Color.Black;
            this.checkBox.OnCheck.BorderRadius = 5;
            this.checkBox.OnCheck.BorderThickness = 2;
            this.checkBox.OnCheck.CheckBoxColor = System.Drawing.Color.Black;
            this.checkBox.OnCheck.CheckmarkColor = System.Drawing.Color.White;
            this.checkBox.OnCheck.CheckmarkThickness = 2;
            this.checkBox.OnDisable.BorderColor = System.Drawing.Color.LightGray;
            this.checkBox.OnDisable.BorderRadius = 5;
            this.checkBox.OnDisable.BorderThickness = 2;
            this.checkBox.OnDisable.CheckBoxColor = System.Drawing.Color.Transparent;
            this.checkBox.OnDisable.CheckmarkColor = System.Drawing.Color.LightGray;
            this.checkBox.OnDisable.CheckmarkThickness = 2;
            this.checkBox.OnHoverChecked.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.checkBox.OnHoverChecked.BorderRadius = 5;
            this.checkBox.OnHoverChecked.BorderThickness = 2;
            this.checkBox.OnHoverChecked.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.checkBox.OnHoverChecked.CheckmarkColor = System.Drawing.Color.White;
            this.checkBox.OnHoverChecked.CheckmarkThickness = 2;
            this.checkBox.OnHoverUnchecked.BorderColor = System.Drawing.Color.Black;
            this.checkBox.OnHoverUnchecked.BorderRadius = 5;
            this.checkBox.OnHoverUnchecked.BorderThickness = 1;
            this.checkBox.OnHoverUnchecked.CheckBoxColor = System.Drawing.Color.Transparent;
            this.checkBox.OnUncheck.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox.OnUncheck.BorderRadius = 5;
            this.checkBox.OnUncheck.BorderThickness = 1;
            this.checkBox.OnUncheck.CheckBoxColor = System.Drawing.Color.Transparent;
            this.checkBox.Size = new System.Drawing.Size(21, 21);
            this.checkBox.Style = Bunifu.UI.WinForms.BunifuCheckBox.CheckBoxStyles.Bunifu;
            this.checkBox.TabIndex = 0;
            this.checkBox.ThreeState = false;
            this.checkBox.ToolTipText = null;
            this.checkBox.CheckedChanged += new System.EventHandler<Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs>(this.checkBox_CheckedChanged);
            // 
            // pSpacer
            // 
            this.pSpacer.Dock = System.Windows.Forms.DockStyle.Right;
            this.pSpacer.Location = new System.Drawing.Point(514, 0);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(10, 36);
            this.pSpacer.TabIndex = 9;
            // 
            // pPBPlay
            // 
            this.pPBPlay.Controls.Add(this.pbPlay);
            this.pPBPlay.Dock = System.Windows.Forms.DockStyle.Right;
            this.pPBPlay.Location = new System.Drawing.Point(370, 0);
            this.pPBPlay.Name = "pPBPlay";
            this.pPBPlay.Size = new System.Drawing.Size(36, 36);
            this.pPBPlay.TabIndex = 10;
            // 
            // pbPlay
            // 
            this.pbPlay.AllowFocused = false;
            this.pbPlay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbPlay.AutoSizeHeight = true;
            this.pbPlay.BorderRadius = 16;
            this.pbPlay.Image = global::ExaltAccountManager.Properties.Resources.ic_play_circle_outline_black_36dp;
            this.pbPlay.IsCircle = true;
            this.pbPlay.Location = new System.Drawing.Point(2, 2);
            this.pbPlay.Name = "pbPlay";
            this.pbPlay.Size = new System.Drawing.Size(32, 32);
            this.pbPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPlay.TabIndex = 1;
            this.pbPlay.TabStop = false;
            this.pbPlay.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbPlay.Click += new System.EventHandler(this.pbPlay_Click);
            this.pbPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_MouseDown);
            this.pbPlay.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.pbPlay.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            this.pbPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_MouseUp);
            // 
            // pPBGetNewToken
            // 
            this.pPBGetNewToken.Controls.Add(this.pbGetNewToken);
            this.pPBGetNewToken.Dock = System.Windows.Forms.DockStyle.Right;
            this.pPBGetNewToken.Location = new System.Drawing.Point(406, 0);
            this.pPBGetNewToken.Name = "pPBGetNewToken";
            this.pPBGetNewToken.Size = new System.Drawing.Size(36, 36);
            this.pPBGetNewToken.TabIndex = 11;
            // 
            // pbGetNewToken
            // 
            this.pbGetNewToken.AllowFocused = false;
            this.pbGetNewToken.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbGetNewToken.AutoSizeHeight = true;
            this.pbGetNewToken.BorderRadius = 16;
            this.pbGetNewToken.Image = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_black_36dp;
            this.pbGetNewToken.IsCircle = true;
            this.pbGetNewToken.Location = new System.Drawing.Point(2, 2);
            this.pbGetNewToken.Name = "pbGetNewToken";
            this.pbGetNewToken.Size = new System.Drawing.Size(32, 32);
            this.pbGetNewToken.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGetNewToken.TabIndex = 0;
            this.pbGetNewToken.TabStop = false;
            this.pbGetNewToken.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbGetNewToken.Click += new System.EventHandler(this.pbGetNewToken_Click);
            this.pbGetNewToken.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_MouseDown);
            this.pbGetNewToken.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.pbGetNewToken.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            this.pbGetNewToken.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_MouseUp);
            // 
            // toolTip
            // 
            this.toolTip.OwnerDraw = true;
            this.toolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.tooltip_Draw);
            // 
            // timerResetGetToken
            // 
            this.timerResetGetToken.Interval = 60000;
            this.timerResetGetToken.Tick += new System.EventHandler(this.timerResetGetToken_Tick);
            // 
            // timerCheckProcess
            // 
            this.timerCheckProcess.Interval = 500;
            // 
            // pDrag
            // 
            this.pDrag.Controls.Add(this.pbDragHandle);
            this.pDrag.Dock = System.Windows.Forms.DockStyle.Left;
            this.pDrag.Location = new System.Drawing.Point(0, 0);
            this.pDrag.Name = "pDrag";
            this.pDrag.Size = new System.Drawing.Size(34, 36);
            this.pDrag.TabIndex = 12;
            // 
            // pbDragHandle
            // 
            this.pbDragHandle.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.pbDragHandle.Image = global::ExaltAccountManager.Properties.Resources.ic_drag_handle_black_36dp;
            this.pbDragHandle.Location = new System.Drawing.Point(2, 9);
            this.pbDragHandle.Name = "pbDragHandle";
            this.pbDragHandle.Size = new System.Drawing.Size(28, 18);
            this.pbDragHandle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbDragHandle.TabIndex = 1;
            this.pbDragHandle.TabStop = false;
            this.pbDragHandle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbDragHandle_MouseDown);
            this.pbDragHandle.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.pbDragHandle.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            this.pbDragHandle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbDragHandle_MouseMove);
            this.pbDragHandle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbDragHandle_MouseUp);
            // 
            // pbEdit
            // 
            this.pbEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbEdit.Image = global::ExaltAccountManager.Properties.Resources.ic_edit_black_36dp;
            this.pbEdit.Location = new System.Drawing.Point(442, 0);
            this.pbEdit.Name = "pbEdit";
            this.pbEdit.Size = new System.Drawing.Size(36, 36);
            this.pbEdit.TabIndex = 5;
            this.pbEdit.TabStop = false;
            this.pbEdit.Click += new System.EventHandler(this.pbEdit_Click);
            this.pbEdit.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.pbEdit.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // pbDelete
            // 
            this.pbDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbDelete.Image = global::ExaltAccountManager.Properties.Resources.ic_delete_forever_black_36dp;
            this.pbDelete.Location = new System.Drawing.Point(478, 0);
            this.pbDelete.Name = "pbDelete";
            this.pbDelete.Size = new System.Drawing.Size(36, 36);
            this.pbDelete.TabIndex = 4;
            this.pbDelete.TabStop = false;
            this.pbDelete.Click += new System.EventHandler(this.pbDelete_Click);
            this.pbDelete.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.pbDelete.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // AccountUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pPBPlay);
            this.Controls.Add(this.pPBGetNewToken);
            this.Controls.Add(this.pEmail);
            this.Controls.Add(this.pAccName);
            this.Controls.Add(this.pbEdit);
            this.Controls.Add(this.pbDelete);
            this.Controls.Add(this.pSpacer);
            this.Controls.Add(this.pCheckBox);
            this.Controls.Add(this.pScroll);
            this.Controls.Add(this.pDrag);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AccountUI";
            this.Size = new System.Drawing.Size(580, 36);
            this.pAccName.ResumeLayout(false);
            this.pAccName.PerformLayout();
            this.pEmail.ResumeLayout(false);
            this.pEmail.PerformLayout();
            this.pCheckBox.ResumeLayout(false);
            this.pPBPlay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPlay)).EndInit();
            this.pPBGetNewToken.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGetNewToken)).EndInit();
            this.pDrag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDragHandle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lAccountName;
        private System.Windows.Forms.Label lEmail;
        private System.Windows.Forms.Panel pAccName;
        private System.Windows.Forms.Panel pEmail;
        private System.Windows.Forms.PictureBox pbDelete;
        private System.Windows.Forms.PictureBox pbEdit;
        private System.Windows.Forms.Panel pScroll;
        private System.Windows.Forms.Panel pCheckBox;
        private System.Windows.Forms.Panel pSpacer;
        private System.Windows.Forms.Panel pPBPlay;
        private System.Windows.Forms.Panel pPBGetNewToken;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer timerResetGetToken;
        private System.Windows.Forms.Timer timerCheckProcess;
        private System.Windows.Forms.Panel pDrag;
        private System.Windows.Forms.PictureBox pbDragHandle;
        private Bunifu.UI.WinForms.BunifuPictureBox pbGetNewToken;
        private Bunifu.UI.WinForms.BunifuPictureBox pbPlay;
        private Bunifu.UI.WinForms.BunifuCheckBox checkBox;
    }
}
