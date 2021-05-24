using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Statistics
{
    public partial class UICharacterViewer : UserControl
    {
        UIAccountViewer viewer;
        CharacterStats stats;
        public UICharacterViewer(UIAccountViewer _viewer, CharacterStats _stats)
        {
            InitializeComponent();
            viewer = _viewer;
            stats = _stats;

            pbReturn.Location = new Point((this.Width - pbReturn.Width) - 15, (this.Height - pbReturn.Height) - 15);
            this.Controls.SetChildIndex(pbReturn, 0);

            LoadUI();

            ApplyTheme(viewer.GetDarkmode());
        }

        private void LoadUI()
        {
            lCurrentLevel.Text = $"Level {stats.level}";

            lAliveFame.Text = string.Format(lAliveFame.Text, stats.fame);

            lClassName.Text = stats.charClass.ToString();
            pbClass.Image = viewer.GetCharacterImage(stats.charClass);
            lHP.Text = stats.maxHP.ToString();            
            lMP.Text = stats.maxMP.ToString();
            lATK.Text = stats.atk.ToString();
            lDEF.Text = stats.def.ToString();
            lSPD.Text = stats.spd.ToString();
            lDEX.Text = stats.dex.ToString();
            lVIT.Text = stats.vit.ToString();
            lWIS.Text = stats.wis.ToString();

            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].maxHP <= stats.maxHP))
                lHP.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].maxMP <= stats.maxMP))
                lMP.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].atk <= stats.atk))
                lATK.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].def <= stats.def))
                lDEF.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].spd <= stats.spd))
                lSPD.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].dex <= stats.dex))
                lDEX.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].vit <= stats.vit))
                lVIT.ForeColor = Color.FromArgb(249, 212, 57);
            if ((CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].wis <= stats.wis))
                lWIS.ForeColor = Color.FromArgb(249, 212, 57);

            lHasBackpack.Text = stats.hasBackpack ? "Yes" : "No";
            lHasAdventurersBelt.Text = stats.hasAdventurersBelt ? "Yes" : "No";

            stats.CalculateXof8();
            lXof8.Text = string.Format(lXof8.Text, stats.xOf8);
            if (stats.xOf8 == 8)
            {
                btnViewDetails.Enabled = false;
                btnViewDetails.Text = "Not available";
            }
        }

        public void ApplyTheme(bool isDarkmode)
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (isDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            ApplyTheme(isDarkmode, def, second, third, font);
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            this.Visible = false;
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;

            this.ForeColor = font;

            lLevel.BackColor = lCurrentLevel.BackColor = 
            lAlive.BackColor = lAliveFame.BackColor = 
            lClassName.BackColor = pbClass.BackColor = lHP.BackColor = lMP.BackColor = lATK.BackColor = lDEF.BackColor = lSPD.BackColor = lDEX.BackColor = lVIT.BackColor = lWIS.BackColor = 
            lExtra.BackColor = lBackpack.BackColor = lHasBackpack.BackColor = lBelt.BackColor = lHasAdventurersBelt.BackColor = pbBackpack.BackColor = pbBelt.BackColor = 
            lMaxedStats.BackColor = lXof8.BackColor = isDarkmode ? Color.FromArgb(45, 20, 20, 20) : def;

            foreach (Panel p in this.Controls.OfType<Panel>())
            {
                foreach (MaterialPanel ui in p.Controls.OfType<MaterialPanel>())
                    ui.ApplyTheme(isDarkmode);
                //p.BackColor = bgColor;
                //foreach (Label l in p.Controls.OfType<Label>())
                //    l.BackColor = bgColor;
                //foreach (PictureBox pb in p.Controls.OfType<PictureBox>())
                //    pb.BackColor = bgColor;
            }
            foreach (MaterialPanel ui in this.Controls.OfType<MaterialPanel>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialTextPanel ui in this.Controls.OfType<MaterialTextPanel>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialSimpelTextPanel ui in this.Controls.OfType<MaterialSimpelTextPanel>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialTopAccount ui in this.Controls.OfType<MaterialTopAccount>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialRadarChars ui in this.Controls.OfType<MaterialRadarChars>())
                ui.ApplyTheme(isDarkmode);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
            this.Visible = true;
        }

        private void pbReturn_MouseEnter(object sender, EventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(128, 98, 0, 238);
        }

        private void pbReturn_MouseLeave(object sender, EventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(175, 98, 0, 238);
        }

        private void pbReturn_MouseDown(object sender, MouseEventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(225, 98, 0, 238);
        }

        private void pbReturn_MouseUp(object sender, MouseEventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(128, 98, 0, 238);
        }

        private void pbReturn_Click(object sender, EventArgs e)
        {
            viewer.CloseCharacterForms();
        }

        bool statsOpen = false;
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (!statsOpen)
            {
                viewer.OpenStatsForms(stats);
                btnViewDetails.Text = "Close Details";
            }
            else
            {
                viewer.CloseStatsForms();
                btnViewDetails.Text = "View Details";
            }

            statsOpen = !statsOpen;
        }
    }
}
