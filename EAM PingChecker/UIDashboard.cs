using Bunifu.UI.WinForms;
using EAM_PingChecker.UI;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EAM_PingChecker
{
    public partial class UIDashboard : UserControl
    {
        FrmMain frm;
        List<ServerPingUI> serverPingUIs = new List<ServerPingUI>();
        Dictionary<ServerPingUI, int> dicUItoID = new Dictionary<ServerPingUI, int>();
        ServerPingGraph pingGraph;
        bool useDarkmode = false;

        ServerDataCollection dataCollection = new ServerDataCollection()
        {
            servers = new List<ServerData>()
        };

        public UIDashboard(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            dataCollection = frm.serverData;
            if (dataCollection != null)
            {
                lPingText.Text = string.Format("Found {0} servers", dataCollection.servers.Count);
                lDate.Text = string.Format(lDate.Text, dataCollection.collectionDate.ToString("dd.MM.yyyy"), dataCollection.collectionDate.ToString("HH:mm"));
            }
            else
            {
                lDate.Text = "No serverdata found!";
                lPingText.Text = string.Format("Found {0} servers", 0);
            }

            pingGraph = new ServerPingGraph(this);
            useDarkmode = frm.useDarkmode;
            ApplyTheme(useDarkmode, ColorScheme.GetColorDef(useDarkmode), ColorScheme.GetColorSecond(useDarkmode), ColorScheme.GetColorThird(useDarkmode), ColorScheme.GetColorFont(useDarkmode));

            if (dataCollection != null)
                AddServersToUI();

            if (frm.pingSaveFile == null)
            {
                frm.pingSaveFile = new PingSaveFile();
                frm.SavePingSaveFile();
            }

            if (dataCollection != null)
            {
                switch (frm.pingSaveFile.startupPing)
                {
                    case StartupPing.All:
                        btnRefreshAll_Click(btnRefreshAll, null);
                        break;
                    case StartupPing.Favorites:
                        btnRefreshFav_Click(btnRefreshFav, null);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ApplyTheme(bool _useDarkmode, Color def, Color second, Color third, Color font)
        {
            useDarkmode = _useDarkmode;

            this.BackColor =
            toolTip.BackColor = def;
            this.ForeColor =
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = useDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);
            lRefresh.ForeColor = ColorScheme.GetColorThird(!useDarkmode);

            shadow.BackColor = shadow2.BackColor = shadow3.BackColor =
            shadow.PanelColor = shadow2.PanelColor = shadow3.PanelColor =
            shadow.PanelColor2 = shadow2.PanelColor2 = shadow3.PanelColor2 = def;

            shadow.ForeColor = shadow2.ForeColor = shadow3.ForeColor =
            separator.LineColor = separator2.LineColor = font;

            if (useDarkmode)
            {
                scrollbar.BorderColor = second;
                scrollbar.BackgroundColor = def;
                scrollbar.ThumbColor = third;
            }
            else
            {
                scrollbar.BorderColor = Color.Silver;
                scrollbar.BackgroundColor = def;
                scrollbar.ThumbColor = Color.Gray;
            }

            BunifuSnackbar.CustomizationOptions opt = new BunifuSnackbar.CustomizationOptions()
            {
                ActionBackColor = !useDarkmode ? Color.White : Color.FromArgb(8, 8, 8),
                BackColor = !useDarkmode ? Color.White : Color.FromArgb(8, 8, 8),
                ActionBorderColor = !useDarkmode ? Color.White : Color.FromArgb(15, 15, 15),
                BorderColor = !useDarkmode ? Color.White : Color.FromArgb(15, 15, 15),
                ActionForeColor = !useDarkmode ? Color.Black : Color.FromArgb(170, 170, 170),
                ForeColor = !useDarkmode ? Color.Black : Color.FromArgb(170, 170, 170),
                CloseIconColor = Color.FromArgb(246, 255, 237)
            };

            snackbar.ErrorOptions = opt;
            snackbar.ErrorOptions.CloseIconColor = Color.FromArgb(255, 204, 199);
            snackbar.WarningOptions = opt;
            snackbar.WarningOptions.CloseIconColor = Color.FromArgb(255, 229, 143);
            snackbar.InformationOptions = opt;
            snackbar.InformationOptions.CloseIconColor = Color.FromArgb(145, 213, 255);
            snackbar.SuccessOptions = opt;
            snackbar.SuccessOptions.CloseIconColor = Color.FromArgb(246, 255, 237);

            for (int i = 0; i < serverPingUIs.Count; i++)
                serverPingUIs[i].ApplyTheme(useDarkmode, def, second, third, font);

            pingGraph.ApplyTheme(useDarkmode, def, second, third, font);
        }

        private void AddServersToUI()
        {
            FormsUtils.SuspendDrawing(this);

            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            for (int i = 0; i < dataCollection.servers.Count; i++)
            {
                ServerPingUI sp = new ServerPingUI(this, dataCollection.servers[i], frm.favorites.favorites.Contains(dataCollection.servers[i].name));
                sp.ApplyTheme(useDarkmode, def, second, third, font);

                flow.Controls.Add(sp);
                serverPingUIs.Add(sp);
            }
            flow.Controls.SetChildIndex(pBottomSpacer, flow.Controls.Count + 1);
            scrollbar.BindTo(flow);

            FormsUtils.ResumeDrawing(this);
        }

        public void ShowGraphUI(ServerPingUI pingUI)
        {
            FormsUtils.SuspendDrawing(this);

            for (int i = 0; i < serverPingUIs.Count; i++)
                flow.Controls.SetChildIndex(serverPingUIs[i], dicUItoID[serverPingUIs[i]]);
            flow.Controls.SetChildIndex(pBottomSpacer, flow.Controls.Count + 1);

            if (!flow.Controls.Contains(pingGraph))
                flow.Controls.Add(pingGraph);

            flow.Controls.SetChildIndex(pingGraph, ((dicUItoID[pingUI]/*serverPingUIs.IndexOf(pingUI)*/ / 3) * 3) + 3);
            //flow.Sc(pingGraph);
            ScrollPanelTo(dicUItoID[pingUI]);

            pingGraph.ChangeTarget(pingUI);

            FormsUtils.ResumeDrawing(this);
        }

        public void HideGraphUI()
        {
            FormsUtils.SuspendDrawing(this);

            pingGraph.SetPingUI(null);

            if (flow.Controls.Contains(pingGraph))
                flow.Controls.Remove(pingGraph);

            flow.ScrollControlIntoView(pingGraph.GetPingUI());

            FormsUtils.ResumeDrawing(this);
        }

        private void UIDashboard_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorFont(useDarkmode)))
                e.Graphics.DrawLine(p, new Point(flow.Left, flow.Top - 1), new Point(this.Width, flow.Top - 1));
        }

        private void lServer_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorFont(useDarkmode)))
                e.Graphics.DrawLine(p, new Point(0, (flow.Top - lServer.Top) - 1), new Point(lServer.Width, (flow.Top - lServer.Top) - 1));
        }

        private void btnRefreshAll_Click(object sender, EventArgs e)
        {
            btnRefreshAll.Text = "Refreshing...";
            btnRefreshAll.Enabled = false;

            btnRefreshFav.Text = "Refreshing...";
            btnRefreshFav.Enabled = false;

            btnRefreshData.Enabled = false;

            Application.DoEvents();

            for (int i = 0; i < serverPingUIs.Count; i++)
            {
                serverPingUIs[i].PingServer();
            }
            OrderPingUIs();

            lRefresh.Text = string.Format("Last\n{0}", DateTime.Now.ToString("HH:mm"));
            btnRefreshAll.Text = "On cooldown";
            btnRefreshFav.Text = "On cooldown";
            btnRefreshData.Text = "On cooldown";

            timerRefreshAll.Start();

            snackbar.Show(frm, "Pinged all servers.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, null, BunifuSnackbar.Positions.BottomRight);
        }

        private void OrderPingUIs()
        {
            FormsUtils.SuspendDrawing(this);

            dicUItoID.Clear();
            int nextID = 0;

            var q = serverPingUIs.Where(s => s.isFavorite);
            List<ServerPingUI> favs = new List<ServerPingUI>();
            favs = q.OrderBy(f => f.GetLastPing()).ToList();
            for (int i = 0; i < favs.Count; i++)
            {
                dicUItoID.Add(favs[i], nextID);
                flow.Controls.SetChildIndex(favs[i], nextID);
                nextID++;
            }

            q = serverPingUIs.Where(s => !s.isFavorite);
            favs = q.OrderBy(f => f.GetLastPing()).ToList();
            for (int i = 0; i < favs.Count; i++)
            {
                dicUItoID.Add(favs[i], nextID);
                flow.Controls.SetChildIndex(favs[i], nextID);
                nextID++;
            }

            if (pingGraph != null && flow.Controls.Contains(pingGraph))
                flow.Controls.SetChildIndex(pingGraph, ((dicUItoID[pingGraph.GetPingUI()] / 3) * 3) + 3);

            FormsUtils.ResumeDrawing(this);
        }

        private void btnRefreshFav_Click(object sender, EventArgs e)
        {
            btnRefreshFav.Text = "Refreshing...";
            btnRefreshFav.Enabled = false;
            Application.DoEvents();

            var q = serverPingUIs.Where(s => s.isFavorite);
            List<ServerPingUI> favs = q.ToList();
            for (int i = 0; i < favs.Count; i++)
            {
                favs[i].PingServer();
            }
            OrderPingUIs();

            lRefresh.Text = string.Format("Last\n{0}", DateTime.Now.ToString("HH:mm"));
            btnRefreshFav.Text = "On cooldown";
            timerRefreshFav.Start();

            snackbar.Show(frm, "Pinged favorite servers.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, null, BunifuSnackbar.Positions.BottomRight);
        }

        public void ChangeFavoriteState(ServerPingUI ui)
        {
            if (frm.favorites.favorites.Contains(ui.serverData.name))
                frm.favorites.favorites.Remove(ui.serverData.name);
            else
                frm.favorites.favorites.Add(ui.serverData.name);

            frm.SaveServerFavorites();
            OrderPingUIs();

            FormsUtils.SuspendDrawing(this);

            if (ui.isFavorite)
                ScrollPanelTo(dicUItoID[ui]);

            FormsUtils.ResumeDrawing(this);
        }

        public void UpdateServer()
        {
            dataCollection = frm.serverData;

            if (dataCollection != null)
            {
                lPingText.Text = string.Format("Found {0} servers", dataCollection.servers.Count);
                lDate.Text = string.Format("Data from: {0} {1}", dataCollection.collectionDate.ToString("dd.MM.yyyy"), dataCollection.collectionDate.ToString("HH:mm"));
            }
            else
            {
                lDate.Text = "No serverdata found!";
                lPingText.Text = string.Format("Found {0} servers", 0);
                return;
            }

            bool newServerAdded = false;
            List<string> servers = new List<string>(); //dataCollection.servers.Select(s => s.name).ToList();
            Dictionary<string, ServerData> dicServersToServerData = new Dictionary<string, ServerData>();
            for (int i = 0; i < dataCollection.servers.Count; i++)
            {
                dicServersToServerData.Add(dataCollection.servers[i].name, dataCollection.servers[i]);
                servers.Add(dataCollection.servers[i].name);
            }

            FormsUtils.SuspendDrawing(this);

            for (int i = 0; i < serverPingUIs.Count; i++)
            {
                if (servers.Contains(serverPingUIs[i].serverData.name))
                {
                    serverPingUIs[i].SetLoad(dicServersToServerData[servers[servers.IndexOf(serverPingUIs[i].serverData.name)]].usage);
                    servers.Remove(serverPingUIs[i].serverData.name);
                }
                else
                {
                    //Server removed!?
                    dicUItoID.Remove(serverPingUIs[i]);

                    ServerPingUI ui = serverPingUIs[i];

                    serverPingUIs.RemoveAt(i);
                    i--;

                    flow.Controls.Remove(ui);
                    ui.Dispose();
                }
            }

            newServerAdded = servers.Count > 0;
            for (int i = 0; i < servers.Count; i++)
            {
                ServerPingUI sp = new ServerPingUI(this, dataCollection.servers[i], frm.favorites.favorites.Contains(dataCollection.servers[i].name));
                sp.ApplyTheme(useDarkmode, ColorScheme.GetColorDef(useDarkmode), ColorScheme.GetColorSecond(useDarkmode), ColorScheme.GetColorThird(useDarkmode), ColorScheme.GetColorFont(useDarkmode));

                flow.Controls.Add(sp);
                serverPingUIs.Add(sp);
            }

            if (newServerAdded)
            {
                flow.Controls.SetChildIndex(pBottomSpacer, flow.Controls.Count + 1);
                scrollbar.BindTo(flow);

                OrderPingUIs();
            }

            FormsUtils.ResumeDrawing(this);

            snackbar.Show(frm, "Serverdata has been updated", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 5000, null, BunifuSnackbar.Positions.BottomRight);
        }

        private void timerRefreshFav_Tick(object sender, EventArgs e)
        {
            timerRefreshFav.Stop();

            btnRefreshFav.Text = "Refresh favorites";
            btnRefreshFav.Enabled = true;
        }

        private void timerRefreshAll_Tick(object sender, EventArgs e)
        {
            timerRefreshAll.Stop();

            btnRefreshFav.Text = "Refresh favorites";
            btnRefreshFav.Enabled = true;
            btnRefreshAll.Text = "Refresh all server";
            btnRefreshAll.Enabled = true;

            btnRefreshData.Text = "Refresh data";
            btnRefreshData.Enabled = true;
        }

        private void ScrollPanelTo(int index)
        {
            var ctl = flow.Controls[index];
            var loc = ctl.Location - new Size(flow.AutoScrollPosition);
            loc -= new Size(ctl.Margin.Left, ctl.Margin.Top);
            flow.AutoScrollPosition = loc;
            ctl.Focus();
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            if (frm == null)
                return;

            if (!string.IsNullOrEmpty(frm.pingSaveFile.accountName) && !frm.pingSaveFile.accountName.Equals("I don't need this feature"))
            {
                btnRefreshAll.Text = "On cooldown";
                btnRefreshAll.Enabled = false;
                btnRefreshFav.Text = "On cooldown";
                btnRefreshFav.Enabled = false;
                btnRefreshData.Text = "On cooldown";
                btnRefreshData.Enabled = false;

                frm.timerRefreshData_Tick(null, null);

                timerRefreshAll.Start();
            }
            else
            {
                snackbar.Show(frm, "You need to select an account in the options for this feature to work.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000, null, BunifuSnackbar.Positions.BottomRight);
            }
        }
    }
}
