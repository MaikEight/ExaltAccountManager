
namespace ExaltAccountManager
{
    partial class LogDataUI
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
            this.pDate = new System.Windows.Forms.Panel();
            this.lDate = new System.Windows.Forms.Label();
            this.pTime = new System.Windows.Forms.Panel();
            this.lTime = new System.Windows.Forms.Label();
            this.pSender = new System.Windows.Forms.Panel();
            this.lSender = new System.Windows.Forms.Label();
            this.pMessage = new System.Windows.Forms.Panel();
            this.lMessage = new System.Windows.Forms.Label();
            this.pEventType = new System.Windows.Forms.Panel();
            this.lEventType = new System.Windows.Forms.Label();
            this.pDate.SuspendLayout();
            this.pTime.SuspendLayout();
            this.pSender.SuspendLayout();
            this.pMessage.SuspendLayout();
            this.pEventType.SuspendLayout();
            this.SuspendLayout();
            // 
            // pDate
            // 
            this.pDate.Controls.Add(this.lDate);
            this.pDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.pDate.Location = new System.Drawing.Point(0, 0);
            this.pDate.Name = "pDate";
            this.pDate.Size = new System.Drawing.Size(80, 30);
            this.pDate.TabIndex = 0;
            // 
            // lDate
            // 
            this.lDate.AutoSize = true;
            this.lDate.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDate.Location = new System.Drawing.Point(3, 7);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(72, 16);
            this.lDate.TabIndex = 1;
            this.lDate.Text = "10.03.2021";
            // 
            // pTime
            // 
            this.pTime.Controls.Add(this.lTime);
            this.pTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.pTime.Location = new System.Drawing.Point(80, 0);
            this.pTime.Name = "pTime";
            this.pTime.Size = new System.Drawing.Size(48, 30);
            this.pTime.TabIndex = 1;
            // 
            // lTime
            // 
            this.lTime.AutoSize = true;
            this.lTime.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTime.Location = new System.Drawing.Point(3, 7);
            this.lTime.Name = "lTime";
            this.lTime.Size = new System.Drawing.Size(40, 16);
            this.lTime.TabIndex = 1;
            this.lTime.Text = "00:00";
            // 
            // pSender
            // 
            this.pSender.Controls.Add(this.lSender);
            this.pSender.Dock = System.Windows.Forms.DockStyle.Left;
            this.pSender.Location = new System.Drawing.Point(128, 0);
            this.pSender.Name = "pSender";
            this.pSender.Size = new System.Drawing.Size(115, 30);
            this.pSender.TabIndex = 2;
            // 
            // lSender
            // 
            this.lSender.AutoEllipsis = true;
            this.lSender.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSender.Location = new System.Drawing.Point(3, 7);
            this.lSender.MaximumSize = new System.Drawing.Size(110, 17);
            this.lSender.Name = "lSender";
            this.lSender.Size = new System.Drawing.Size(110, 17);
            this.lSender.TabIndex = 1;
            this.lSender.Text = "TaskTrayTool";
            // 
            // pMessage
            // 
            this.pMessage.Controls.Add(this.lMessage);
            this.pMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMessage.Location = new System.Drawing.Point(243, 0);
            this.pMessage.Name = "pMessage";
            this.pMessage.Size = new System.Drawing.Size(413, 30);
            this.pMessage.TabIndex = 3;
            // 
            // lMessage
            // 
            this.lMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMessage.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMessage.Location = new System.Drawing.Point(0, 0);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(413, 30);
            this.lMessage.TabIndex = 1;
            this.lMessage.Text = "Message";
            this.lMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lMessage.UseMnemonic = false;
            // 
            // pEventType
            // 
            this.pEventType.Controls.Add(this.lEventType);
            this.pEventType.Dock = System.Windows.Forms.DockStyle.Right;
            this.pEventType.Location = new System.Drawing.Point(656, 0);
            this.pEventType.Name = "pEventType";
            this.pEventType.Size = new System.Drawing.Size(125, 30);
            this.pEventType.TabIndex = 4;
            // 
            // lEventType
            // 
            this.lEventType.AutoSize = true;
            this.lEventType.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEventType.Location = new System.Drawing.Point(3, 7);
            this.lEventType.Name = "lEventType";
            this.lEventType.Size = new System.Drawing.Size(94, 16);
            this.lEventType.TabIndex = 1;
            this.lEventType.Text = "ServiceStart";
            // 
            // LogDataUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pMessage);
            this.Controls.Add(this.pEventType);
            this.Controls.Add(this.pSender);
            this.Controls.Add(this.pTime);
            this.Controls.Add(this.pDate);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.Name = "LogDataUI";
            this.Size = new System.Drawing.Size(781, 30);
            this.pDate.ResumeLayout(false);
            this.pDate.PerformLayout();
            this.pTime.ResumeLayout(false);
            this.pTime.PerformLayout();
            this.pSender.ResumeLayout(false);
            this.pMessage.ResumeLayout(false);
            this.pEventType.ResumeLayout(false);
            this.pEventType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pDate;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Panel pTime;
        private System.Windows.Forms.Label lTime;
        private System.Windows.Forms.Panel pSender;
        private System.Windows.Forms.Label lSender;
        private System.Windows.Forms.Panel pMessage;
        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.Panel pEventType;
        private System.Windows.Forms.Label lEventType;
    }
}
