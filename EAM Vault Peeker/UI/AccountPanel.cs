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
    public partial class AccountPanel : UserControl
    {
        FrmMain frm;
        bool useDarkmode = true;
        public bool isVisible = true;

        List<CharacterUI> characterUIs = new List<CharacterUI>();

        List<ItemUI> itemUIs = new List<ItemUI>();
        List<ItemUI> itemUIsPotions = new List<ItemUI>();
        List<ItemUI> itemUIsGifts = new List<ItemUI>();

        AccountItems accountItem;

        ItemType filterType = ItemType.All;
        int feedpowerFilter = -1;
        int tierFilter = -1;
        string nameFilter = string.Empty;

        public bool HasItems { get { return (itemUIs.Count > 0 || itemUIsPotions.Count > 0 || itemUIsGifts.Count > 0);  } }

        public AccountPanel(FrmMain _frm, AccountItems _accountItem)
        {
            InitializeComponent();

            frm = _frm;
            accountItem = _accountItem;

            lAccountName.Text = accountItem.name;
            lEmail.Text = accountItem.email;
            lDate.Text = $"Data from {accountItem.date.ToString("dd.MM.yyyy HH:mm")}";
            //shadowPanel.Controls.Add(pbRefreshData);
            try
            {
                var q = frm.statsList.Where(s => s.email.Equals(accountItem.email));
                if (q.Count() > 0)
                {
                    StatsMain chars = q.First();

                    for (int i = 0; i < chars.charList[chars.charList.Count - 1].chars.Count; i++)
                    {
                        CharacterUI cui = new CharacterUI(frm, chars.charList[chars.charList.Count - 1].chars[i]);
                        characterUIs.Add(cui);
                    }

                    pChar.Controls.AddRange(characterUIs.ToArray());
                }
            }
            catch { }            

            if (accountItem != null)
            {
                for (int i = 0; i < accountItem.vaultItems.Count; i++)
                {
                    Item item = null;
                    try
                    {
                        //item = frm.items.Where(lIt => lIt.id == accountItem.vaultItems[i].id).First();                       
                        for (int x = 0; x < frm.items.Count; x++)
                        {
                            if (frm.items[x].id == accountItem.vaultItems[i].id)
                            {
                                item = frm.items[x];
                                break;
                            }
                        }
                        if (item == null)
                        {
                            item = new Item()
                            {
                                id = 0
                            };
                        }
                    }
                    catch (Exception e)
                    {
                        item = new Item()
                        {
                            id = 0
                        };
                    }

                    if (item.id == -1)
                        continue;

                    var it = new UI.ItemUI(frm, item);
                    itemUIs.Add(it);
                    layoutVault.Controls.Add(it);
                    it.ShowPreview += frm.ShowItemPreview;

                    try
                    {
                        it.amount = accountItem.vaultItems.Where(vi => vi.id == accountItem.vaultItems[i].id).Count();
                    }
                    catch
                    {
                        it.amount = 1;
                    }
                }

                lVaultShown.Text = $"{itemUIs.Count} / {itemUIs.Count} items shown";
            }


            for (int i = 0; i < accountItem.potionItems.Count; i++)
            {
                Item item = null;
                try
                {
                    item = frm.items.Where(lIt => lIt.id == accountItem.potionItems[i].id).First();
                }
                catch
                {
                    item = new Item()
                    {
                        id = -1
                    };
                }

                if (item.id == -1)
                    continue;

                var it = new UI.ItemUI(frm, item);
                itemUIsPotions.Add(it);
                layoutPotions.Controls.Add(it);
                it.ShowPreview += frm.ShowItemPreview;

                try
                {
                    it.amount = accountItem.potionItems.Where(pi => pi.id == accountItem.potionItems[i].id).Count();
                }
                catch
                {
                    it.amount = 1;
                }
            }

            lPotionsShown.Text = $"{itemUIsPotions.Count} / {itemUIsPotions.Count} items shown";

            for (int i = 0; i < accountItem.giftItems.Count; i++)
            {
                Item item = null;
                try
                {
                    item = frm.items.Where(lIt => lIt.id == accountItem.giftItems[i].id).First();
                }
                catch (Exception)
                {
                    item = new Item()
                    {
                        id = -1
                    };
                }

                if (item.id == -1)
                    continue;

                var it = new UI.ItemUI(frm, item);
                itemUIsGifts.Add(it);
                layoutGifts.Controls.Add(it);
                it.ShowPreview += frm.ShowItemPreview;

                try
                {
                    it.amount = accountItem.giftItems.Where(gi => gi.id == accountItem.giftItems[i].id).Count();
                }
                catch
                {
                    it.amount = 1;
                }
            }

            lGiftsShown.Text = $"{itemUIsGifts.Count} / {itemUIsGifts.Count} items shown";

            ResizeUI();

            ApplyTheme(frm, null);
        }

        public void ShowHideDuplicates(bool state)
        {
            if (state)
            {
                List<Item> knownItems = new List<Item>();

                FormsUtils.SuspendDrawing(layoutVault);
                layoutVault.SuspendLayout();
                layoutVault.Controls.Clear();

                for (int i = 0; i < itemUIs.Count; i++)
                {
                    if (!knownItems.Contains(itemUIs[i].item))
                    {
                        itemUIs[i].HideDuplicates = true;
                        knownItems.Add(itemUIs[i].item);
                        layoutVault.Controls.Add(itemUIs[i]);
                    }
                }
                layoutVault.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutVault);

                knownItems.Clear();
                FormsUtils.SuspendDrawing(layoutPotions);
                layoutPotions.SuspendLayout();
                layoutPotions.Controls.Clear();

                for (int i = 0; i < itemUIsPotions.Count; i++)
                {
                    if (!knownItems.Contains(itemUIsPotions[i].item))
                    {
                        itemUIsPotions[i].HideDuplicates = true;
                        knownItems.Add(itemUIsPotions[i].item);
                        layoutPotions.Controls.Add(itemUIsPotions[i]);
                    }
                }

                layoutPotions.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutPotions);

                knownItems.Clear();
                FormsUtils.SuspendDrawing(layoutGifts);
                layoutGifts.SuspendLayout();

                layoutGifts.Controls.Clear();

                for (int i = 0; i < itemUIsGifts.Count; i++)
                {
                    if (!knownItems.Contains(itemUIsGifts[i].item))
                    {
                        itemUIsGifts[i].HideDuplicates = true;
                        knownItems.Add(itemUIsGifts[i].item);
                        layoutGifts.Controls.Add(itemUIsGifts[i]);
                    }
                }

                layoutGifts.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutGifts);
            }
            else
            {
                FormsUtils.SuspendDrawing(layoutVault);
                layoutVault.SuspendLayout();

                layoutVault.Controls.Clear();
                for (int i = 0; i < itemUIs.Count; i++)
                {
                    itemUIs[i].HideDuplicates = false;
                }
                layoutVault.Controls.AddRange(itemUIs.ToArray());

                layoutVault.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutVault);

                FormsUtils.SuspendDrawing(layoutPotions);
                layoutPotions.SuspendLayout();

                layoutPotions.Controls.Clear();
                for (int i = 0; i < itemUIsPotions.Count; i++)
                {
                    itemUIsPotions[i].HideDuplicates = false;
                }
                layoutPotions.Controls.AddRange(itemUIsPotions.ToArray());

                layoutPotions.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutPotions);

                FormsUtils.SuspendDrawing(layoutGifts);
                layoutGifts.SuspendLayout();

                layoutGifts.Controls.Clear();
                for (int i = 0; i < itemUIsGifts.Count; i++)
                {
                    itemUIsGifts[i].HideDuplicates = false;
                }
                layoutGifts.Controls.AddRange(itemUIsGifts.ToArray());

                layoutGifts.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutGifts);
            }

            //lVaultShown.Text = $"{layoutVault.Controls.Count} / {itemUIs.Count} items shown";
            //lPotionsShown.Text = $"{layoutPotions.Controls.Count} / {itemUIsPotions.Count} items shown";
            //lGiftsShown.Text = $"{layoutGifts.Controls.Count} / {itemUIsGifts.Count} items shown";
            UpdateItemsShownUI();

            ResizeUI();
        }

        private void UpdateItemsShownUI()
        {
            int amount = 0;
            foreach (Control c in layoutVault.Controls)
                amount += c.Visible ? 1 : 0;

            lVaultShown.Text = $"{amount} / {itemUIs.Count} items shown";

            amount = 0;
            foreach (Control c in layoutPotions.Controls)
                amount += c.Visible ? 1 : 0;

            lPotionsShown.Text = $"{amount} / {itemUIsPotions.Count} items shown";

            amount = 0;
            foreach (Control c in layoutGifts.Controls)
                amount += c.Visible ? 1 : 0;

            lGiftsShown.Text = $"{amount} / {itemUIsGifts.Count} items shown";
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

            for (int i = 0; i < itemUIsPotions.Count; i++)
            {
                if (itemUIsPotions[i].item.id == itemID)
                {
                    itemUIsPotions[i].Highlighted = state;
                    foundItem = true;
                }
            }

            for (int i = 0; i < itemUIsGifts.Count; i++)
            {
                if (itemUIsGifts[i].item.id == itemID)
                {
                    itemUIsGifts[i].Highlighted = state;
                    foundItem = true;
                }
            }

            for (int i = 0; i < characterUIs.Count; i++)
            {
                if (characterUIs[i].ToggleItemHighlight(itemID, state))
                    foundItem = true;
            }

            return foundItem;
        }

        public void SetNameFilter(string name)
        {
            if (nameFilter.Equals(name))
                return;

            nameFilter = name;

            FormsUtils.SuspendDrawing(layoutVault);
            layoutVault.SuspendLayout();
            for (int i = 0; i < itemUIs.Count; i++)
            {
                if ((nameFilter.Length == 0 || itemUIs[i].item.name.ToLower().Contains(nameFilter)) && layoutVault.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByName = false;
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName && !itemUIs[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIs[i].filteredByName = true;
                    itemUIs[i].Visible = false;
                }
            }
            layoutVault.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutVault);

            FormsUtils.SuspendDrawing(layoutPotions);
            layoutPotions.SuspendLayout();
            for (int i = 0; i < itemUIsPotions.Count; i++)
            {
                if ((nameFilter.Length == 0 || itemUIsPotions[i].item.name.ToLower().Contains(nameFilter)) && layoutPotions.Controls.Contains(itemUIsPotions[i]))
                {
                    itemUIsPotions[i].filteredByName = false;
                    itemUIsPotions[i].Visible = (!itemUIsPotions[i].filteredByFeedpower && !itemUIsPotions[i].filteredByType && !itemUIsPotions[i].filteredByTier && !itemUIsPotions[i].filteredByName && !itemUIsPotions[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIsPotions[i].filteredByName = true;
                    itemUIsPotions[i].Visible = false;
                }
            }
            layoutPotions.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutPotions);

            FormsUtils.SuspendDrawing(layoutGifts);
            layoutGifts.SuspendLayout();
            for (int i = 0; i < itemUIsGifts.Count; i++)
            {
                if ((nameFilter.Length == 0 || itemUIsGifts[i].item.name.ToLower().Contains(nameFilter)) && layoutGifts.Controls.Contains(itemUIsGifts[i]))
                {
                    itemUIsGifts[i].filteredByName = false;
                    itemUIsGifts[i].Visible = (!itemUIsGifts[i].filteredByFeedpower && !itemUIsGifts[i].filteredByType && !itemUIsGifts[i].filteredByTier && !itemUIsGifts[i].filteredByName && !itemUIsGifts[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIsGifts[i].filteredByName = true;
                    itemUIsGifts[i].Visible = false;
                }
            }
            layoutGifts.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutGifts);

            ResizeUI();
            UpdateItemsShownUI();
        }

        public void SetTierFilter(int tier)
        {
            if (tierFilter == tier)
                return;

            tierFilter = tier;

            FormsUtils.SuspendDrawing(layoutVault);
            layoutVault.SuspendLayout();
            for (int i = 0; i < itemUIs.Count; i++)
            {
                if ((tierFilter == -1 || (tierFilter == 15 && itemUIs[i].item.utSt != 0) || itemUIs[i].item.tier == tierFilter) && layoutVault.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByTier = false;
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName && !itemUIs[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIs[i].filteredByTier = true;
                    itemUIs[i].Visible = false;
                }
            }
            layoutVault.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutVault);

            FormsUtils.SuspendDrawing(layoutPotions);
            layoutPotions.SuspendLayout();
            for (int i = 0; i < itemUIsPotions.Count; i++)
            {
                if ((tierFilter == -1 || itemUIsPotions[i].item.tier == tierFilter) && layoutPotions.Controls.Contains(itemUIsPotions[i]))
                {
                    itemUIsPotions[i].filteredByTier = false;
                    itemUIsPotions[i].Visible = (!itemUIsPotions[i].filteredByFeedpower && !itemUIsPotions[i].filteredByType && !itemUIsPotions[i].filteredByTier && !itemUIsPotions[i].filteredByName && !itemUIsPotions[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIsPotions[i].filteredByTier = true;
                    itemUIsPotions[i].Visible = false;
                }
            }
            layoutPotions.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutPotions);

            FormsUtils.SuspendDrawing(layoutGifts);
            layoutGifts.SuspendLayout();
            for (int i = 0; i < itemUIsGifts.Count; i++)
            {
                if ((tierFilter == -1 || itemUIsGifts[i].item.tier == tierFilter) && layoutGifts.Controls.Contains(itemUIsGifts[i]))
                {
                    itemUIsGifts[i].filteredByTier = false;
                    itemUIsGifts[i].Visible = (!itemUIsGifts[i].filteredByFeedpower && !itemUIsGifts[i].filteredByType && !itemUIsGifts[i].filteredByTier && !itemUIsGifts[i].filteredByName && !itemUIsGifts[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIsGifts[i].filteredByTier = true;
                    itemUIsGifts[i].Visible = false;
                }
            }
            layoutGifts.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutGifts);

            ResizeUI();
            UpdateItemsShownUI();
        }

        public void SetFeedpowerFilter(int minFeedpower)
        {
            if (feedpowerFilter == minFeedpower)
                return;

            feedpowerFilter = minFeedpower;

            FormsUtils.SuspendDrawing(layoutVault);
            layoutVault.SuspendLayout();
            for (int i = 0; i < itemUIs.Count; i++)
            {
                if (itemUIs[i].item.feedPower >= feedpowerFilter && layoutVault.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByFeedpower = false;
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName && !itemUIs[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIs[i].filteredByFeedpower = true;
                    itemUIs[i].Visible = false;
                }
            }
            layoutVault.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutVault);

            FormsUtils.SuspendDrawing(layoutPotions);
            layoutPotions.SuspendLayout();
            for (int i = 0; i < itemUIsPotions.Count; i++)
            {
                if (itemUIsPotions[i].item.feedPower >= feedpowerFilter && layoutPotions.Controls.Contains(itemUIsPotions[i]))
                {
                    itemUIsPotions[i].filteredByFeedpower = false;
                    itemUIsPotions[i].Visible = (!itemUIsPotions[i].filteredByFeedpower && !itemUIsPotions[i].filteredByType && !itemUIsPotions[i].filteredByTier && !itemUIsPotions[i].filteredByName && !itemUIsPotions[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIsPotions[i].filteredByFeedpower = true;
                    itemUIsPotions[i].Visible = false;
                }
            }
            layoutPotions.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutPotions);

            FormsUtils.SuspendDrawing(layoutGifts);
            layoutGifts.SuspendLayout();
            for (int i = 0; i < itemUIsGifts.Count; i++)
            {
                if (itemUIsGifts[i].item.feedPower >= feedpowerFilter && layoutGifts.Controls.Contains(itemUIsGifts[i]))
                {
                    itemUIsGifts[i].filteredByFeedpower = false;
                    itemUIsGifts[i].Visible = (!itemUIsGifts[i].filteredByFeedpower && !itemUIsGifts[i].filteredByType && !itemUIsGifts[i].filteredByTier && !itemUIsGifts[i].filteredByName && !itemUIsGifts[i].filteredBySelectedAccount);
                }
                else
                {
                    itemUIsGifts[i].filteredByFeedpower = true;
                    itemUIsGifts[i].Visible = false;
                }
            }
            layoutGifts.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutGifts);    

            ResizeUI();
            UpdateItemsShownUI();
        }

        public void SetTypeFilter(ItemType type)
        {
            if (filterType == type)
                return;

            if (filterType != ItemType.All)
            {
                FormsUtils.SuspendDrawing(layoutVault);
                layoutVault.SuspendLayout();
                for (int i = 0; i < itemUIs.Count; i++)
                {
                    if (itemUIs[i].filteredByType && layoutVault.Controls.Contains(itemUIs[i]))
                    {
                        itemUIs[i].filteredByType = false;
                        itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName && !itemUIs[i].filteredBySelectedAccount);
                    }
                }
                layoutVault.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutVault);

                FormsUtils.SuspendDrawing(layoutPotions);
                layoutPotions.SuspendLayout();
                for (int i = 0; i < itemUIsPotions.Count; i++)
                {
                    if (itemUIsPotions[i].filteredByType && layoutPotions.Controls.Contains(itemUIsPotions[i]))
                    {
                        itemUIsPotions[i].filteredByType = false;
                        itemUIsPotions[i].Visible = (!itemUIsPotions[i].filteredByFeedpower && !itemUIsPotions[i].filteredByType && !itemUIsPotions[i].filteredByTier && !itemUIsPotions[i].filteredByName && !itemUIsPotions[i].filteredBySelectedAccount);
                    }
                }
                layoutPotions.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutPotions);

                FormsUtils.SuspendDrawing(layoutGifts);
                layoutGifts.SuspendLayout();
                for (int i = 0; i < itemUIsGifts.Count; i++)
                {
                    if (itemUIsGifts[i].filteredByType && layoutGifts.Controls.Contains(itemUIsGifts[i]))
                    {
                        itemUIsGifts[i].filteredByType = false;
                        itemUIsGifts[i].Visible = (!itemUIsGifts[i].filteredByFeedpower && !itemUIsGifts[i].filteredByType && !itemUIsGifts[i].filteredByTier && !itemUIsGifts[i].filteredByName && !itemUIsGifts[i].filteredBySelectedAccount);
                    }
                }
                layoutGifts.ResumeLayout();
                FormsUtils.ResumeDrawing(layoutGifts);
            }
            filterType = type;

            if (filterType == ItemType.All)
            {
                ResizeUI();
                UpdateItemsShownUI();
                return;
            }

            FormsUtils.SuspendDrawing(layoutVault);
            layoutVault.SuspendLayout();
            for (int i = 0; i < itemUIs.Count; i++)
            {
                if (itemUIs[i].item.itemType != type && layoutVault.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByType = true;
                    itemUIs[i].Visible = false;
                }
            }
            layoutVault.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutVault);

            FormsUtils.SuspendDrawing(layoutPotions);
            layoutPotions.SuspendLayout();
            for (int i = 0; i < itemUIsPotions.Count; i++)
            {
                if (itemUIsPotions[i].item.itemType != type && layoutPotions.Controls.Contains(itemUIsPotions[i]))
                {
                    itemUIsPotions[i].filteredByType = true;
                    itemUIsPotions[i].Visible = false;
                }
            }
            layoutPotions.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutPotions);

            FormsUtils.SuspendDrawing(layoutGifts);
            layoutGifts.SuspendLayout();
            for (int i = 0; i < itemUIsGifts.Count; i++)
            {
                if (itemUIsGifts[i].item.itemType != type && layoutGifts.Controls.Contains(itemUIsGifts[i]))
                {
                    itemUIsGifts[i].filteredByType = true;
                    itemUIsGifts[i].Visible = false;
                }
            }
            layoutGifts.ResumeLayout();
            FormsUtils.ResumeDrawing(layoutGifts);

            Application.DoEvents();
            ResizeUI();
            UpdateItemsShownUI();
        }        

        private void ResizeUI()
        {
            if (!pLists.Visible)
            {
                this.Height = lEmail.Bottom + lAccountName.Top;
                return;
            }

            if (characterUIs.Count > 0)
            {
                pChar.Height = characterUIs.Max(c => c.Bottom);
                if (pChar.Visible)
                    pChars.Height = pChar.Bottom;
                else
                    pChars.Height = pChar.Top;
            }
            else
            {
                pChars.Visible = false;
            }

            decimal amount = 0;
            foreach (Control c in layoutVault.Controls)
                amount += c.Visible ? 1 : 0;

            layoutVault.Height = (int)(Math.Ceiling(amount / 8m) * 48);
            pVault.Height = layoutVault.Bottom;

            amount = 0;
            foreach (Control c in layoutPotions.Controls)
                amount += c.Visible ? 1 : 0;

            layoutPotions.Height = (int)(Math.Ceiling(amount / 8m) * 48);
            pPotions.Height = layoutPotions.Bottom;

            amount = 0;
            foreach (Control c in layoutGifts.Controls)
                amount += c.Visible ? 1 : 0;

            layoutGifts.Height = (int)(Math.Ceiling(amount / 8m) * 48);
            pGifts.Height = layoutGifts.Bottom;

            pLists.Height = pGifts.Bottom;
            this.Height = pLists.Bottom + 8;

            if (this.Parent != null)
                frm.accountsUI.ResizeContentPanelHeight((Panel)this.Parent);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            useDarkmode = (sender as FrmMain).useDarkmode;

            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            this.BackColor = second;
            shadowPanel.PanelColor = shadowPanel.PanelColor2 =
            pbToggleVisible.BackColor = def;

            shadowPanel.BorderColor = useDarkmode ? second : Color.WhiteSmoke;
            shadowPanel.ShadowColor = useDarkmode ? second : Color.DarkGray;
            this.ForeColor = font;

            pbToggleVisible.Image = frm.useDarkmode ? (!isVisible ? Properties.Resources.ic_visibility_white_18dp : Properties.Resources.ic_visibility_off_white_18dp) : (!isVisible ? Properties.Resources.ic_visibility_black_18dp : Properties.Resources.ic_visibility_off_black_18dp);
            pbRefreshData.Image = frm.useDarkmode ? Properties.Resources.ic_refresh_white_18dp : Properties.Resources.ic_refresh_black_18dp;
            pbChars.Image = frm.useDarkmode ? Properties.Resources.ic_group_white_36dp : Properties.Resources.ic_group_black_36dp;

            pChars.BackColor = pVault.BackColor = pPotions.BackColor = pGifts.BackColor = second;

            for (int i = 0; i < characterUIs.Count; i++)
                characterUIs[i].ApplyTheme(frm.useDarkmode);

            this.Invalidate();
        }

        private void pbToggleVisible_Click(object sender, EventArgs e)
        {
            pLists.Visible = lDate.Visible /*= pbRefreshData.Visible*/ = isVisible = !isVisible;
            pbToggleVisible.Image = frm.useDarkmode ? (!isVisible ? Properties.Resources.ic_visibility_white_18dp : Properties.Resources.ic_visibility_off_white_18dp) : (!isVisible ? Properties.Resources.ic_visibility_black_18dp : Properties.Resources.ic_visibility_off_black_18dp);

            ResizeUI();

            if (isVisible)
                UpdateItemsShownUI();
        }

        public void HideItems()
        {
            pLists.Visible = lDate.Visible = isVisible = false;
            pbToggleVisible.Image = frm.useDarkmode ? (!isVisible ? Properties.Resources.ic_visibility_white_18dp : Properties.Resources.ic_visibility_off_white_18dp) : (!isVisible ? Properties.Resources.ic_visibility_black_18dp : Properties.Resources.ic_visibility_off_black_18dp);

            ResizeUI();
        }

        private void pbRefreshData_Click(object sender, EventArgs e)
        {

        }

        private void lChars_Click(object sender, EventArgs e)
        {
            pChar.Visible = !pChar.Visible;
            pChars.Height = pChar.Visible ? pChar.Bottom : pbChars.Bottom;

            ResizeUI();
        }
    }
}
