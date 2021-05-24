
namespace ExaltAccountManager
{
    partial class UIDailyLoginsTimings
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbJointTime = new System.Windows.Forms.TextBox();
            this.lStoJoin = new System.Windows.Forms.Label();
            this.lStillKill = new System.Windows.Forms.Label();
            this.tbKillTime = new System.Windows.Forms.TextBox();
            this.lBack = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.pbBack = new Bunifu.UI.WinForms.BunifuPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(52, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Timing settings";
            // 
            // tbJointTime
            // 
            this.tbJointTime.Location = new System.Drawing.Point(12, 43);
            this.tbJointTime.Name = "tbJointTime";
            this.tbJointTime.Size = new System.Drawing.Size(35, 20);
            this.tbJointTime.TabIndex = 11;
            this.tbJointTime.Text = "90";
            this.tbJointTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lStoJoin
            // 
            this.lStoJoin.Location = new System.Drawing.Point(53, 32);
            this.lStoJoin.Name = "lStoJoin";
            this.lStoJoin.Size = new System.Drawing.Size(180, 111);
            this.lStoJoin.TabIndex = 12;
            this.lStoJoin.Text = "seconds to join the Nexus.\r\nThis represents the time you need to join the Nexus v" +
    "ia this Manager.\r\nIt is the time a Client is expected to join the nexus (counts " +
    "as daily login).";
            // 
            // lStillKill
            // 
            this.lStillKill.Location = new System.Drawing.Point(54, 144);
            this.lStillKill.Name = "lStillKill";
            this.lStillKill.Size = new System.Drawing.Size(179, 78);
            this.lStillKill.TabIndex = 14;
            this.lStillKill.Text = "seconds after joining the client disconnects.\r\nAfter joining, this amount of seco" +
    "nds is waited till the process is killed (successfull or not).";
            // 
            // tbKillTime
            // 
            this.tbKillTime.Location = new System.Drawing.Point(13, 156);
            this.tbKillTime.Name = "tbKillTime";
            this.tbKillTime.Size = new System.Drawing.Size(35, 20);
            this.tbKillTime.TabIndex = 13;
            this.tbKillTime.Text = "30";
            this.tbKillTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lBack
            // 
            this.lBack.AutoSize = true;
            this.lBack.Location = new System.Drawing.Point(-1, 23);
            this.lBack.Name = "lBack";
            this.lBack.Size = new System.Drawing.Size(33, 15);
            this.lBack.TabIndex = 16;
            this.lBack.Text = "Back";
            this.lBack.Click += new System.EventHandler(this.pbBack_Click);
            this.lBack.MouseEnter += new System.EventHandler(this.pbBack_MouseEnter);
            this.lBack.MouseLeave += new System.EventHandler(this.pbBack_MouseLeave);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = global::ExaltAccountManager.Properties.Resources.ic_save_black_18dp;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(13, 223);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(220, 24);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pSpacer
            // 
            this.pSpacer.BackColor = System.Drawing.Color.Black;
            this.pSpacer.Location = new System.Drawing.Point(10, 141);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(228, 2);
            this.pSpacer.TabIndex = 18;
            // 
            // pbBack
            // 
            this.pbBack.AllowFocused = false;
            this.pbBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbBack.AutoSizeHeight = true;
            this.pbBack.BorderRadius = 12;
            this.pbBack.Image = global::ExaltAccountManager.Properties.Resources.ic_arrow_back_black_24dp;
            this.pbBack.IsCircle = true;
            this.pbBack.Location = new System.Drawing.Point(3, 2);
            this.pbBack.Name = "pbBack";
            this.pbBack.Size = new System.Drawing.Size(24, 24);
            this.pbBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBack.TabIndex = 19;
            this.pbBack.TabStop = false;
            this.pbBack.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbBack.Click += new System.EventHandler(this.pbBack_Click);
            this.pbBack.MouseEnter += new System.EventHandler(this.pbBack_MouseEnter);
            this.pbBack.MouseLeave += new System.EventHandler(this.pbBack_MouseLeave);
            // 
            // UIDailyLoginsTimings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbBack);
            this.Controls.Add(this.pSpacer);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lBack);
            this.Controls.Add(this.lStillKill);
            this.Controls.Add(this.tbKillTime);
            this.Controls.Add(this.lStoJoin);
            this.Controls.Add(this.tbJointTime);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Name = "UIDailyLoginsTimings";
            this.Size = new System.Drawing.Size(248, 250);
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbJointTime;
        private System.Windows.Forms.Label lStoJoin;
        private System.Windows.Forms.Label lStillKill;
        private System.Windows.Forms.TextBox tbKillTime;
        private System.Windows.Forms.Label lBack;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pSpacer;
        private Bunifu.UI.WinForms.BunifuPictureBox pbBack;
    }
}
