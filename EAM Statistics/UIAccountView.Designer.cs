
namespace EAM_Statistics
{
    partial class UIAccountView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIAccountView));
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.SuspendLayout();
            // 
            // flow
            // 
            this.flow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow.Location = new System.Drawing.Point(0, 0);
            this.flow.Margin = new System.Windows.Forms.Padding(0);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(620, 600);
            this.flow.TabIndex = 0;
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
            this.scrollbar.AllowScrollOptionsMenu = false;
            this.scrollbar.AllowShrinkingOnFocusLost = false;
            this.scrollbar.BackgroundColor = System.Drawing.Color.Silver;
            this.scrollbar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scrollbar.BackgroundImage")));
            this.scrollbar.BindingContainer = null;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 1;
            this.scrollbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(615, 0);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scrollbar.Maximum = 100;
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 5;
            this.scrollbar.Size = new System.Drawing.Size(5, 600);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 0;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 59;
            this.scrollbar.ThumbMargin = 0;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.scrollbar.Value = 0;
            this.scrollbar.Visible = false;
            // 
            // UIAccountView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.scrollbar);
            this.Controls.Add(this.flow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIAccountView";
            this.Size = new System.Drawing.Size(620, 600);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flow;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
    }
}
