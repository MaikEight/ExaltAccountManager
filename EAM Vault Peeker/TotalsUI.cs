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
    public partial class TotalsUI : UserControl
    {
        FrmMain frm;
        List<ItemUI> itemUIs = new List<ItemUI>();
        bool isInit = true;

        ItemType filterType = ItemType.All;
        int feedpowerFilter = -1;
        int tierFilter = -1;
        string nameFilter = string.Empty;

        Dictionary<int, int> dicItemIDToAmountSelected = new Dictionary<int, int>();
        Dictionary<int, int> dicItemIDToAmountAll = new Dictionary<int, int>();

        public TotalsUI(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            dropItemType.Items.AddRange(Enum.GetNames(typeof(ItemType)));
            dropItemType.Items.RemoveAt(1);
            dropItemType.Items.RemoveAt(1);

            dropItemType.SelectedIndex =
            dropFeedpower.SelectedIndex =
            dropTier.SelectedIndex = 0;

            LoadItems();

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);

            isInit = false;
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.useDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.useDarkmode);
            Color third = ColorScheme.GetColorThird(frm.useDarkmode);
            Color font = ColorScheme.GetColorFont(frm.useDarkmode);

            this.ForeColor = font;

            tbSearch.ResetColors();
            this.BackColor = pTop.BackColor =
            tbSearch.BackColor = tbSearch.FillColor = tbSearch.OnIdleState.FillColor = tbSearch.OnHoverState.FillColor = tbSearch.OnActiveState.FillColor = tbSearch.OnDisabledState.FillColor =
            def;

            tbSearch.IconLeft = frm.useDarkmode ? Properties.Resources.ic_search_white_24dp : Properties.Resources.ic_search_black_24dp;

            foreach (var drop in pTop.Controls.OfType<Bunifu.UI.WinForms.BunifuDropdown>())
            {
                drop.BackgroundColor =
                drop.ItemBackColor = def;
                drop.ForeColor =
                drop.ItemBorderColor =
                drop.ItemForeColor = font;
                drop.BorderColor = third;
                drop.Invalidate();
            }

            pContent.BackColor = flow.BackColor = second;
            pbFilter.Image = frm.useDarkmode ? Properties.Resources.ic_filter_list_white_36dp : Properties.Resources.ic_filter_list_black_36dp;
        }

        private void LoadItems()
        {
            dicItemIDToAmountSelected = new Dictionary<int, int>();
            dicItemIDToAmountAll = new Dictionary<int, int>();
            bool isSelectedAccount = false;

            if (frm.itemsSaveFile != null && frm.itemsSaveFile.accountItems != null)
            {
                for (int i = 0; i < frm.itemsSaveFile.accountItems.Count; i++)
                {
                    isSelectedAccount = frm.activeVaultPeekerAccounts.Contains(frm.itemsSaveFile.accountItems[i].email);

                    for (int x = 0; x < frm.itemsSaveFile.accountItems[i].vaultItems.Count; x++)
                    {
                        if (frm.itemsSaveFile.accountItems[i].vaultItems[x].id <= 0)
                            continue;

                        if (dicItemIDToAmountAll.ContainsKey(frm.itemsSaveFile.accountItems[i].vaultItems[x].id))
                            dicItemIDToAmountAll[frm.itemsSaveFile.accountItems[i].vaultItems[x].id]++;
                        else
                            dicItemIDToAmountAll.Add(frm.itemsSaveFile.accountItems[i].vaultItems[x].id, 1);

                        if (!isSelectedAccount) continue;
                        if (dicItemIDToAmountSelected.ContainsKey(frm.itemsSaveFile.accountItems[i].vaultItems[x].id))
                            dicItemIDToAmountSelected[frm.itemsSaveFile.accountItems[i].vaultItems[x].id]++;
                        else
                            dicItemIDToAmountSelected.Add(frm.itemsSaveFile.accountItems[i].vaultItems[x].id, 1);
                    }

                    for (int x = 0; x < frm.itemsSaveFile.accountItems[i].potionItems.Count; x++)
                    {
                        if (frm.itemsSaveFile.accountItems[i].potionItems[x].id <= 0)
                            continue;

                        if (dicItemIDToAmountAll.ContainsKey(frm.itemsSaveFile.accountItems[i].potionItems[x].id))
                            dicItemIDToAmountAll[frm.itemsSaveFile.accountItems[i].potionItems[x].id]++;
                        else
                            dicItemIDToAmountAll.Add(frm.itemsSaveFile.accountItems[i].potionItems[x].id, 1);

                        if (!isSelectedAccount) continue;
                        if (dicItemIDToAmountSelected.ContainsKey(frm.itemsSaveFile.accountItems[i].potionItems[x].id))
                            dicItemIDToAmountSelected[frm.itemsSaveFile.accountItems[i].potionItems[x].id]++;
                        else
                            dicItemIDToAmountSelected.Add(frm.itemsSaveFile.accountItems[i].potionItems[x].id, 1);
                    }

                    for (int x = 0; x < frm.itemsSaveFile.accountItems[i].giftItems.Count; x++)
                    {
                        if (frm.itemsSaveFile.accountItems[i].giftItems[x].id <= 0)
                            continue;

                        if (dicItemIDToAmountAll.ContainsKey(frm.itemsSaveFile.accountItems[i].giftItems[x].id))
                            dicItemIDToAmountAll[frm.itemsSaveFile.accountItems[i].giftItems[x].id]++;
                        else
                            dicItemIDToAmountAll.Add(frm.itemsSaveFile.accountItems[i].giftItems[x].id, 1);

                        if (!isSelectedAccount) continue;
                        if (dicItemIDToAmountSelected.ContainsKey(frm.itemsSaveFile.accountItems[i].giftItems[x].id))
                            dicItemIDToAmountSelected[frm.itemsSaveFile.accountItems[i].giftItems[x].id]++;
                        else
                            dicItemIDToAmountSelected.Add(frm.itemsSaveFile.accountItems[i].giftItems[x].id, 1);
                    }
                }

                for (int x = 0; x < frm.statsList.Count; x++)
                {
                    if (frm.statsList[x].charList.Count <= 0)
                        continue;

                    isSelectedAccount = frm.activeVaultPeekerAccounts.Contains(frm.statsList[x].email);

                    for (int y = 0; y < frm.statsList[x].charList[frm.statsList[x].charList.Count - 1].chars.Count; y++)
                    {
                        foreach (int id in frm.statsList[x].charList[frm.statsList[x].charList.Count - 1].chars[y].equipment)
                        {
                            if (id > 0)
                            {
                                if (dicItemIDToAmountAll.ContainsKey(id))
                                    dicItemIDToAmountAll[id]++;
                                else
                                    dicItemIDToAmountAll.Add(id, 1);

                                if (!isSelectedAccount) continue;
                                if (dicItemIDToAmountSelected.ContainsKey(id))
                                    dicItemIDToAmountSelected[id]++;
                                else
                                    dicItemIDToAmountSelected.Add(id, 1);
                            }
                        }
                    }
                }
            }


            FormsUtils.SuspendDrawing(pContent);
            FormsUtils.SuspendDrawing(flow);

            foreach (int k in dicItemIDToAmountAll.Keys)
            {
                Item item = null;
                try
                {
                    item = frm.items.Where(lIt => lIt.id == k).First();
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

                ItemUI it = new UI.ItemUI(frm, item) { amount = dicItemIDToAmountAll[k], HideDuplicates = true };
                it.ShowPreview += frm.ShowItemPreview;
                itemUIs.Add(it);
            }

            flow.Controls.AddRange(itemUIs.ToArray());
            flow.Height = itemUIs.Count > 0 ? itemUIs[itemUIs.Count - 1].Bottom + 3 : 48;

            toggleShowSelectedAccountsOnly.Checked = true;

            FormsUtils.ResumeDrawing(flow);
            FormsUtils.ResumeDrawing(pContent);

            ResizeEnd();
        }

        public void ToggleItemHighlight(int itemID, bool state)
        {
            try
            {
                itemUIs.Where(it => it.item.id == itemID).ToList().ForEach(it => { it.Highlighted = state; });
            }
            catch { }
        }

        public void ResizeEnd()
        {
            pSpacerLeft.Width = (pContent.Width % 48) / 2;

            scrollbar.BindTo(pContent);
        }

        private void dropItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            ItemType type = (ItemType)Enum.Parse(typeof(ItemType), (string)dropItemType.SelectedItem);

            if (type == filterType)
                return;

            FormsUtils.SuspendDrawing(flow);
            flow.SuspendLayout();

            if (filterType != ItemType.All)
            {
                for (int i = 0; i < itemUIs.Count; i++)
                {
                    if (itemUIs[i].filteredByType && flow.Controls.Contains(itemUIs[i]))
                    {
                        itemUIs[i].filteredByType = false;
                        itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName);
                    }
                }
            }

            filterType = type;

            if (filterType == ItemType.All)
            {
                for (int i = 0; i < itemUIs.Count; i++)
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName);

                flow.ResumeLayout();
                FormsUtils.ResumeDrawing(flow);
                return;
            }

            for (int i = 0; i < itemUIs.Count; i++)
            {
                if (itemUIs[i].item.itemType != type && flow.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByType = true;
                    itemUIs[i].Visible = false;
                }
            }

            flow.ResumeLayout();
            FormsUtils.ResumeDrawing(flow);
        }

        private void dropFeedpower_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            int filter = -1;
            if (dropFeedpower.SelectedIndex > 0)
                filter = int.Parse(dropFeedpower.SelectedItem.ToString());

            if (feedpowerFilter == filter)
                return;

            feedpowerFilter = filter;

            FormsUtils.SuspendDrawing(flow);
            flow.SuspendLayout();
            for (int i = 0; i < itemUIs.Count; i++)
            {
                if (itemUIs[i].item.feedPower >= feedpowerFilter && flow.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByFeedpower = false;
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName);
                }
                else
                {
                    itemUIs[i].filteredByFeedpower = true;
                    itemUIs[i].Visible = false;
                }
            }
            flow.ResumeLayout();
            FormsUtils.ResumeDrawing(flow);
        }

        private void dropTier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            int tier = dropTier.SelectedIndex - 1;
            if (tierFilter == tier)
                return;

            tierFilter = tier;

            FormsUtils.SuspendDrawing(flow);
            flow.SuspendLayout();
            for (int i = 0; i < itemUIs.Count; i++)
            {
                if ((tierFilter == -1 || (tierFilter == 15 && itemUIs[i].item.utSt != 0) || itemUIs[i].item.tier == tierFilter) && flow.Controls.Contains(itemUIs[i]))
                {
                    itemUIs[i].filteredByTier = false;
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName);
                }
                else
                {
                    itemUIs[i].filteredByTier = true;
                    itemUIs[i].Visible = false;
                }
            }
            flow.ResumeLayout();
            FormsUtils.ResumeDrawing(flow);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (timerSearch.Enabled)
                timerSearch.Stop();

            if (isInit)
                return;

            timerSearch.Start();
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (nameFilter.Equals(tbSearch.Text))
                return;

            nameFilter = tbSearch.Text;

            FormsUtils.SuspendDrawing(flow);
            flow.SuspendLayout();

            for (int i = 0; i < itemUIs.Count; i++)
            {
                if ((nameFilter.Length == 0 || itemUIs[i].item.name.ToLower().Contains(nameFilter)))
                {
                    itemUIs[i].filteredByName = false;
                    itemUIs[i].Visible = (!itemUIs[i].filteredByFeedpower && !itemUIs[i].filteredByType && !itemUIs[i].filteredByTier && !itemUIs[i].filteredByName);
                }
                else
                {
                    itemUIs[i].filteredByName = true;
                    itemUIs[i].Visible = false;
                }
            }

            flow.ResumeLayout();
            FormsUtils.ResumeDrawing(flow);
        }

        private void toggleShowSelectedAccountsOnly_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            FormsUtils.SuspendDrawing(flow);
            flow.SuspendLayout();

            if (toggleShowSelectedAccountsOnly.Checked)
            {
                for (int i = 0; i < itemUIs.Count; i++)
                {
                    if (dicItemIDToAmountSelected.ContainsKey(itemUIs[i].item.id))
                    {
                        itemUIs[i].amount = dicItemIDToAmountSelected[itemUIs[i].item.id];
                        itemUIs[i].filteredBySelectedAccount = false;
                        itemUIs[i].Visible = true;
                    }
                    else
                    {
                        itemUIs[i].filteredBySelectedAccount = true;
                        itemUIs[i].Visible = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < itemUIs.Count; i++)
                {
                    if (dicItemIDToAmountAll.ContainsKey(itemUIs[i].item.id))
                    {
                        itemUIs[i].amount = dicItemIDToAmountAll[itemUIs[i].item.id];
                        itemUIs[i].filteredBySelectedAccount = false;
                        itemUIs[i].Visible = true;
                    }
                    else
                    {
                        itemUIs[i].filteredBySelectedAccount = true;
                        itemUIs[i].Visible = false;
                    }
                }

            }

            flow.ResumeLayout();
            FormsUtils.ResumeDrawing(flow);
        }

    }
}
