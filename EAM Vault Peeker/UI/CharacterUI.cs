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

namespace EAM_Vault_Peeker.UI
{
    public partial class CharacterUI : UserControl
    {
        FrmMain frm;
        AccountPanel accountPanel;
        public CharacterStats charStats;
        List<ItemUI> itemUIs = new List<ItemUI>();

        public CharacterUI(FrmMain _frm, CharacterStats _charStats)
        {
            InitializeComponent();
            frm = _frm;
            charStats = _charStats;
            charStats.CalculateXof8();
            this.Dock = DockStyle.Top;

            lCharClass.Text = charStats.charClass.ToString();
            pbClass.Image = GetClassImage(charStats.charClass);

            lLevel.Text = string.Format(lLevel.Text, charStats.level);
            lFame.Text = string.Format(lFame.Text, charStats.fame);
            lXOf8.Text = string.Format(lXOf8.Text, charStats.xOf8);

            //Items to UI
            for (int i = 0; i < charStats.equipment.Length; i++)
            {
                if (i < 4) //EQ
                {
                    ItemUI ui = new ItemUI(frm, GetItemByID(charStats.equipment[i]));
                    tableLayoutEQ.Controls.Add(ui);
                    ui.ShowPreview += frm.ShowItemPreview;
                    itemUIs.Add(ui);
                }
                else if (i < 12) //Inventory
                {
                    ItemUI ui = new ItemUI(frm, GetItemByID(charStats.equipment[i]));
                    ui.ShowPreview += frm.ShowItemPreview;

                    layoutVault.Controls.Add(ui);
                    itemUIs.Add(ui);
                }
                else //Backpack
                {
                    ItemUI ui = new ItemUI(frm, GetItemByID(charStats.equipment[i]));
                    ui.ShowPreview += frm.ShowItemPreview;

                    layoutBackpack.Controls.Add(ui);
                    itemUIs.Add(ui);
                }
            }

            if (!charStats.hasBackpack)
            {
                pBackpack.Visible = false;
                this.Controls.Remove(pBackpack);
                pBackpack.Dispose();
                pBackpack = null;

                this.Height -= 70;
            }
        }

        public bool ToggleItemHighlight(int itemID, bool state)
        {
            bool foundItem = !state;

            for (int i = 0; i < itemUIs.Count; i++)
            {
                if (itemUIs[i].item.id == itemID)
                {
                    itemUIs[i].Highlighted = state;
                    foundItem = true;
                }
            }

            return foundItem;
        }

        private Image GetClassImage(CharClasses charClass)
        {
            switch (charClass)
            {
                case CharClasses.Rogue:
                    return Properties.Resources.Rogue;
                case CharClasses.Archer:
                    return Properties.Resources.Archer;
                case CharClasses.Wizard:
                    return Properties.Resources.Wizard;
                case CharClasses.Priest:
                    return Properties.Resources.Priest;
                case CharClasses.Warrior:
                    return Properties.Resources.Warrior;
                case CharClasses.Knight:
                    return Properties.Resources.Knight;
                case CharClasses.Paladin:
                    return Properties.Resources.Paladin;
                case CharClasses.Assassin:
                    return Properties.Resources.Assassin;
                case CharClasses.Necromancer:
                    return Properties.Resources.Necromancer;
                case CharClasses.Huntress:
                    return Properties.Resources.Huntress;
                case CharClasses.Mystic:
                    return Properties.Resources.Mystic;
                case CharClasses.Trickster:
                    return Properties.Resources.Trickster;
                case CharClasses.Sorcerer:
                    return Properties.Resources.Sorcerer;
                case CharClasses.Ninja:
                    return Properties.Resources.Ninja;
                case CharClasses.Samurai:
                    return Properties.Resources.Samurai;
                case CharClasses.Bard:
                    return Properties.Resources.Bard;
                case CharClasses.Summoner:
                    return Properties.Resources.Summoner;
                case CharClasses.Kensei:
                    return Properties.Resources.Kensei;
                default:
                    return null;
            }
        }

        private Item GetItemByID(int id)
        {
            Item item = null;
            try
            {
                item = frm.items.Where(lIt => lIt.id == id).FirstOrDefault();
                if (item == null)
                    item = new Item()
                    {
                        id = id,
                        x = -1,
                        y = -1
                    };
            }
            catch
            {
                item = new Item()
                {
                    id = id,
                    x = -1,
                    y = -1
                };
            }

            //if (item.id == -1)
            //    continue;
            return item;
        }
        public void ApplyTheme(bool useDarkmode)
        {
            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            this.BackColor = def;
            this.ForeColor = font;
            layoutVault.BackColor = layoutBackpack.BackColor = second;

            pbInventory.Image = useDarkmode ? Properties.Resources.bag_24px : Properties.Resources.bag_24px_1;
        }
    }
}
