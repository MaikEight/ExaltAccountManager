using EAM_Vault_Peeker.UI;
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

namespace EAM_Vault_Peeker
{
    public partial class FrmItemPreview : Form
    {
        Color utColor = Color.FromArgb(150, 55, 239);
        Color stColor = Color.FromArgb(255, 167, 16);

        bool useDarkmode = false;

        public ItemUI itemUI;

        public FrmItemPreview()
        {
            InitializeComponent();            
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            useDarkmode = (sender as FrmMain).useDarkmode;

            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            this.BackColor =
            bunifuFormDock.DockingIndicatorsColor = second;
            bunifuFormDock.ApplyShadows();
            this.ForeColor = font;

            this.Invalidate();
        }

        public void ChangeItem(ItemUI _item)
        {
            itemUI = _item;

            pTier.Visible = 
            pFeedpower.Visible = 
            pFameBonus.Visible = true;

            this.Height = 132;
            this.Width = 160;

            lName.Text = itemUI.item.name;
            int e = itemUI.amount;
            lAmount.Text = e.ToString();

            if (itemUI.item.tier != -1 || (itemUI.item.tier == -1 && itemUI.item.utSt != -1))
            {
                if (itemUI.item.tier == -1)
                {
                    switch (itemUI.item.utSt)
                    {
                        case 1:
                            lTier.Text = "UT";
                            lTier.ForeColor = utColor;
                            break;
                        case 2:
                            lTier.Text = "ST";
                            lTier.ForeColor = stColor;
                            break;
                        default:
                            pTier.Visible = false;
                            this.Height -= pTier.Height;
                            break;
                    }
                }
                else
                {
                    lTier.Text = itemUI.item.tier.ToString();
                    lTier.ForeColor = ColorScheme.GetColorFont(useDarkmode);
                }
            }

            if (itemUI.item.feedPower != -1)
            {
                lFeedpower.Text = itemUI.item.feedPower.ToString();
            }
            else
            {
                this.Height -= pFameBonus.Height;
                pFeedpower.Visible = false;
            }

            if (itemUI.item.fameBonus > 0)
            {
                lFameBonus.Text = $"{itemUI.item.fameBonus}%";
            }
            else
            {
                this.Height -= pFameBonus.Height;
                pFameBonus.Visible = false;
            }

            this.Width = lName.Width + (lName.Left * 2) > this.Width ? lName.Width + (lName.Left * 2) : this.Width;
        }

        private void pName_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(this.ForeColor))
            {
                e.Graphics.DrawLine(p, 5, pName.Height - 1, this.Width - 5, pName.Height - 1);
            }
        }
    }
}
