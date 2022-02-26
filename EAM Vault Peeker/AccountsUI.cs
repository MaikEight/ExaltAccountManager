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
    public partial class AccountsUI : UserControl
    {
        FrmMain frm;

        Dictionary<int, List<AccountPanel>> dicHighlightedpanels = new Dictionary<int, List<AccountPanel>>();
        List<AccountPanel> accountPanels = new List<AccountPanel>();
        List<string> activeAccounts = new List<string>();

        public bool isInit = true;

        List<Panel> contentPanels = new List<Panel>();

        bool isResizing = false;
        int widthOffset = 8;

        public AccountsUI(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            dropItemType.Items.AddRange(Enum.GetNames(typeof(ItemType)));
            dropItemType.Items.RemoveAt(1);
            dropItemType.Items.RemoveAt(1);

            dropItemType.SelectedIndex =
            dropFeedpower.SelectedIndex =
            dropTier.SelectedIndex = 0;

            LoadUI();

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);

            isInit = false;
        }

        private void AccountsUI_Load(object sender, EventArgs e)
        {
            timerStart.Start();
        }

        private void LoadUI()
        {
            FormsUtils.SuspendDrawing(pContent);

            if (frm.itemsSaveFile != null && frm.itemsSaveFile.accountItems != null)
            {
                for (int i = 0; i < Math.Min(frm.itemsSaveFile.accountItems.Count, 50); i++)
                {
                    if (!frm.activeVaultPeekerAccounts.Contains(frm.itemsSaveFile.accountItems[i].email) || frm.itemsSaveFile.accountItems[i].name.Equals("FAILED"))
                        continue;

                    //if (UserObjects() > 5000) //10000 Handles are the very max (crash after that) 
                    //    break;

                    AccountPanel ap = new AccountPanel(frm, frm.itemsSaveFile.accountItems[i]);
                    accountPanels.Add(ap);
                    frm.ThemeChanged += ap.ApplyTheme;
                }
            }

            if (accountPanels.Count > 0)
                accountPanels = accountPanels.OrderByDescending(x => x.Height).ToList();
            for (int i = 0; i < accountPanels.Count; i++)
            {
                if (!accountPanels[i].HasItems)
                    accountPanels[i].HideItems();
            }

            FormsUtils.ResumeDrawing(pContent);
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

            pContent.BackColor = pPanels.BackColor = second;
            pbFilter.Image = frm.useDarkmode ? Properties.Resources.ic_filter_list_white_36dp : Properties.Resources.ic_filter_list_black_36dp;
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

        }

        public void ToggleItemHighlight(int itemID, bool state)
        {
            if (dicHighlightedpanels.ContainsKey(itemID))
            {
                if (state)
                    return;
                else
                {
                    dicHighlightedpanels.Remove(itemID);

                    if (dicHighlightedpanels.Count > 0)
                    {
                        for (int i = 0; i < accountPanels.Count; i++)
                        {
                            accountPanels[i].ToggleItemHighlight(itemID, state);
                            bool shouldbeShown = true;

                            foreach (int k in dicHighlightedpanels.Keys)
                            {
                                if (!dicHighlightedpanels[k].Contains(accountPanels[i]))
                                {
                                    shouldbeShown = false;
                                    break;
                                }
                            }

                            accountPanels[i].Visible = shouldbeShown;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < accountPanels.Count; i++)
                        {
                            accountPanels[i].ToggleItemHighlight(itemID, state);
                            accountPanels[i].Visible = true;
                        }
                    }
                }
            }
            else
            {
                dicHighlightedpanels.Add(itemID, new List<AccountPanel>());

                for (int i = 0; i < accountPanels.Count; i++)
                {
                    if (accountPanels[i].ToggleItemHighlight(itemID, state))
                        dicHighlightedpanels[itemID].Add(accountPanels[i]);
                    else
                        accountPanels[i].Visible = false;
                }
            }

            pContent_SizeChanged(null, null);
        }

        private void toggleHideDuplicates_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            FormsUtils.SuspendDrawing(pContent);

            for (int i = 0; i < accountPanels.Count; i++)
            {
                accountPanels[i].ShowHideDuplicates(toggleHideDuplicates.Checked);
            }

            FormsUtils.ResumeDrawing(pContent);
        }

        DateTime lastRefresh = DateTime.Now;
        private void scrollbar_ValueChanged(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ValueChangedEventArgs e)
        {
            if ((DateTime.Now - lastRefresh).TotalMilliseconds < 5)
                return;

            lastRefresh = DateTime.Now;

            pPanels.Update();
        }

        private void dropItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            FormsUtils.SuspendDrawing(pContent);

            for (int i = 0; i < accountPanels.Count; i++)
            {
                accountPanels[i].SetTypeFilter((ItemType)Enum.Parse(typeof(ItemType), (string)dropItemType.SelectedItem));
            }

            FormsUtils.ResumeDrawing(pContent);
        }

        private void dropFeedpower_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            FormsUtils.SuspendDrawing(pContent);
            int filter = -1;
            if (dropFeedpower.SelectedIndex > 0)
                filter = int.Parse(dropFeedpower.SelectedItem.ToString());

            for (int i = 0; i < accountPanels.Count; i++)
            {
                accountPanels[i].SetFeedpowerFilter(filter);
            }

            FormsUtils.ResumeDrawing(pContent);
        }

        private void dropTier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            FormsUtils.SuspendDrawing(pContent);

            for (int i = 0; i < accountPanels.Count; i++)
            {
                accountPanels[i].SetTierFilter(dropTier.SelectedIndex - 1);
            }

            FormsUtils.ResumeDrawing(pContent);
        }

        public void pContent_SizeChanged(object sender, EventArgs e)
        {
            if (frm.WindowState == FormWindowState.Minimized || isInit || isResizing) return;

            if (accountPanels.Count == 0)
                return;

            int w = accountPanels[0].Width;

            int panelsNeeded = pPanels.Width / w;
            if (contentPanels.Count != panelsNeeded || (sender == this && e == null))
            {
                FormsUtils.SuspendDrawing(pContent);

                for (int i = 0; i < contentPanels.Count; i++)
                {
                    contentPanels[i].Controls.Clear();                    
                    contentPanels[i].Dispose();
                }
                contentPanels.Clear();

                while (contentPanels.Count < panelsNeeded)
                {
                    Panel p = new Panel();
                    p.Size = new Size(w, 0);
                    p.Left = w * contentPanels.Count;
                    contentPanels.Add(p);
                    pPanels.Controls.Add(p);
                }

                SortAccountPanelsToContentPanels();

                FormsUtils.ResumeDrawing(pContent);
            }
        }

        public void ResizeEnd()
        {
            isResizing = true;

            if (accountPanels.Count > 0)
            {
                int w = accountPanels[0].Width;
                frm.Width = (frm.Width - this.Width) + ((pPanels.Width / w) * w) + widthOffset;
            }

            isResizing = false;
        }

        private void SortAccountPanelsToContentPanels()
        {
            FormsUtils.SuspendDrawing(pContent);
            FormsUtils.SuspendDrawing(pPanels);

            for (int i = 0; i < contentPanels.Count; i++)
                contentPanels[i].Controls.Clear();

            int maxHeight = 0;

            try
            {
                for (int i = 0; i < accountPanels.Count; i++)
                {
                    contentPanels = contentPanels.OrderBy(p => p.Height).ToList();

                    contentPanels[0].Controls.Add(accountPanels[i]);
                    accountPanels[i].BringToFront();

                    if (accountPanels[i].Dock != DockStyle.Top)
                        accountPanels[i].Dock = DockStyle.Top;

                    contentPanels[0].Height = accountPanels[i].Bottom;

                    maxHeight = accountPanels[i].Bottom > maxHeight ? accountPanels[i].Bottom : maxHeight;
                }
            }
            catch { }

            pPanels.Height = maxHeight;

            FormsUtils.ResumeDrawing(pPanels);
            FormsUtils.ResumeDrawing(pContent);
        }

        public void ResizeContentPanelHeight(Panel p)
        {
            int maxHeight = 0;

            foreach (Control c in p.Controls)
                maxHeight = c.Bottom > maxHeight ? c.Bottom : maxHeight;
            p.Height = maxHeight;


            for (int i = 0; i < contentPanels.Count; i++)
                maxHeight = contentPanels[i].Height > maxHeight ? contentPanels[i].Height : maxHeight;

            pPanels.Height = maxHeight;
        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            timerStart.Stop();

            FormsUtils.SuspendDrawing(pContent);

            pPanels.Visible = true;
            scrollbar.BindTo(pContent);

            FormsUtils.ResumeDrawing(pContent);

            pContent_SizeChanged(null, null);

            timerStart.Dispose();
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
            FormsUtils.SuspendDrawing(pContent);

            for (int i = 0; i < accountPanels.Count; i++)
            {
                accountPanels[i].SetNameFilter(tbSearch.Text.ToLower());
            }

            FormsUtils.ResumeDrawing(pContent);
        }

        //[System.Runtime.InteropServices.DllImport("User32")]
        //private extern static int GetGuiResources(IntPtr hProcess, int uiFlags);

        //public static int UserObjects()
        //{
        //    using (var process = System.Diagnostics.Process.GetCurrentProcess())
        //    {
        //        var gdiHandles = GetGuiResources(process.Handle, 0);
        //        var userHandles = GetGuiResources(process.Handle, 1);
        //        return Convert.ToInt32(userHandles + gdiHandles);
        //    }
        //}
    }
}
