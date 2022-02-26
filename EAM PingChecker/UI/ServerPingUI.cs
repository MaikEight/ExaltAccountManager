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

namespace EAM_PingChecker.UI
{
    public partial class ServerPingUI : UserControl
    {
        UIDashboard dashboard;
        public ServerData serverData;
        public ServerPingGraph graph;
        public bool isFavorite = false;
        bool useDarkmode = false;
        private int lastPing = -1;

        public ServerPingUI()
        {
            InitializeComponent();
        }

        public ServerPingUI(UIDashboard dash, ServerData data, bool isFav = false)
        {
            InitializeComponent();
            dashboard = dash;
            serverData = data;

            lServerName.Text = serverData.name;
            SetLoad(serverData.usage);

            dashboard.toolTip.SetToolTipTitle(pbRenew, "Refresh ping");
            dashboard.toolTip.SetToolTip(pbRenew, "Ping this server again.");
            dashboard.toolTip.SetToolTipIcon(pbRenew, pbRenew.Image);

            dashboard.toolTip.SetToolTipTitle(pbShowGraph, "Show Graph");
            dashboard.toolTip.SetToolTip(pbShowGraph, "Shows the graph for continuous pinging of ths server.");
            dashboard.toolTip.SetToolTipIcon(pbShowGraph, pbShowGraph.Image);

            isFavorite = isFav;
            pbFavorite.Image = useDarkmode ? (isFavorite ? Properties.Resources.ic_star_white_24dp : Properties.Resources.ic_star_border_white_24dp) : (isFavorite ? Properties.Resources.ic_star_black_24dp : Properties.Resources.ic_star_border_black_24dp);
        }

        public void SetLoad(int usage) => lLoad.Text = $"{usage}%";

        public void ApplyTheme(bool _useDarkmode, Color def, Color second, Color third, Color font)
        {
            useDarkmode = _useDarkmode;

            this.BackColor =
            shadow.BackColor =
            shadow.PanelColor = shadow.PanelColor2 =
            pButtons.BackColor =
            pbRenew.BackColor = pbShowGraph.BackColor = def;

            this.ForeColor = font;

            separator.LineColor =
            lPingText.ForeColor = lPing.ForeColor =
            lLoadText.ForeColor = lLoad.ForeColor = ColorScheme.GetColorThird(!useDarkmode);

            if (useDarkmode)
            {
                pbRenew.Image = Properties.Resources.ic_cached_white_24dp;
                pbShowGraph.Image = graph == null ? Properties.Resources.ecg_24px : Properties.Resources.x_coordinate_white_24px;
                pbGraphShown.Image = Properties.Resources.ic_expand_more_white_36dp;
            }
            else
            {
                pbRenew.Image = Properties.Resources.ic_cached_black_24dp;
                pbShowGraph.Image = graph == null ? Properties.Resources.icons8_ecg_24px : Properties.Resources.x_coordinate_black_24px;
                pbGraphShown.Image = Properties.Resources.ic_expand_more_black_36dp;
            }

            pbFavorite.Image = useDarkmode ? (isFavorite ? Properties.Resources.ic_star_white_24dp : Properties.Resources.ic_star_border_white_24dp) : (isFavorite ? Properties.Resources.ic_star_black_24dp : Properties.Resources.ic_star_border_black_24dp);

            dashboard.toolTip.SetToolTipIcon(pbRenew, pbRenew.Image);
            dashboard.toolTip.SetToolTipIcon(pbShowGraph, pbShowGraph.Image);

            foreach (Control c in shadow.Controls)
            {
                c.BackColor = shadow.PanelColor;
            }
            pButtons.BackColor = Color.Transparent;
        }

        public void RemoveGraph()
        {
            pbGraphShown.Visible = false;

            pbShowGraph.Image = useDarkmode ? Properties.Resources.ecg_24px : Properties.Resources.icons8_ecg_24px;
        }

        public int GetLastPing() => lastPing;

        public void PingServer(bool addToGraph = false)
        {
            if (serverData != null && !string.IsNullOrEmpty(serverData.ip))
            {
                Task.Run(() => PerformPingAsync(addToGraph));
            }
        }

        private async Task PerformPingAsync(bool addToGraph)
        {
            lastPing = await MK_EAM_Lib.PingServer.PingServerInfo(serverData);

            SetLastPingToUI(addToGraph);
        }

        private bool SetLastPingToUI(bool addToGraph)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool, bool>)SetLastPingToUI, addToGraph);

            if (lastPing == -1)
                lPing.Text = "---";
            else
                lPing.Text = lastPing.ToString();

            if (addToGraph && graph != null)
                graph.AddNewPing(lastPing);

            return false;
        }

        private void timerHideShadowSide_Tick(object sender, EventArgs e)
        {
            if (transition.IsCompleted)
            {
                transition.HideSync(pButtons, true);
                timerHideShadowSide.Stop();
            }
        }

        private void ServerPingUI_MouseEnter(object sender, EventArgs e)
        {
            if (timerHideShadowSide.Enabled)
                timerHideShadowSide.Stop();

            if (transition.IsCompleted)
                transition.ShowSync(pButtons, true);
        }

        private void ServerPingUI_MouseLeave(object sender, EventArgs e)
        {
            if (!timerHideShadowSide.Enabled)
                timerHideShadowSide.Start();
        }

        private void pbRenew_MouseEnter(object sender, EventArgs e)
        {
            if (!pbRenew.Enabled) return;

            ServerPingUI_MouseEnter(null, null);
            pbRenew.BackColor = useDarkmode ? Color.FromArgb(50, 200, 200, 200) : Color.FromArgb(25, 0, 0, 0);
        }

        private void pbRenew_MouseLeave(object sender, EventArgs e)
        {
            if (!pbRenew.Enabled) return;

            ServerPingUI_MouseLeave(null, null);
            pbRenew.BackColor = pButtons.BackColor;
        }

        private void pbShowGraph_MouseEnter(object sender, EventArgs e)
        {
            ServerPingUI_MouseEnter(null, null);
            pbShowGraph.BackColor = useDarkmode ? Color.FromArgb(50, 200, 200, 200) : Color.FromArgb(25, 0, 0, 0);
        }

        private void pbShowGraph_MouseLeave(object sender, EventArgs e)
        {
            ServerPingUI_MouseLeave(null, null);
            pbShowGraph.BackColor = pButtons.BackColor;
        }


        private void pbRenew_Click(object sender, EventArgs e)
        {
            pbRenew.BackColor = Color.FromArgb(128, 0, 179, 219);
            pbRenew.Enabled = false;
            timerAllowRefresh.Start();

            PingServer();
        }

        private void pbShowGraph_Click(object sender, EventArgs e)
        {
            if (dashboard != null)
            {
                if (graph == null)
                {
                    dashboard.ShowGraphUI(this);
                    pbGraphShown.Visible = true;
                    pbShowGraph.Image = useDarkmode ? Properties.Resources.x_coordinate_white_24px : Properties.Resources.x_coordinate_black_24px;
                }
                else
                {
                    dashboard.HideGraphUI();
                    RemoveGraph();
                    graph = null;
                }
            }
        }

        private void timerAllowRefresh_Tick(object sender, EventArgs e)
        {
            pbRenew.Enabled = true;
            pbRenew.BackColor = pButtons.BackColor;
            timerAllowRefresh.Stop();
        }

        private void pbFavorite_Click(object sender, EventArgs e)
        {
            if (timerSwitchFav.Enabled)
                return;

            isFavorite = !isFavorite;
            pbFavorite.Image = useDarkmode ? Properties.Resources.ic_star_half_white_24dp : Properties.Resources.ic_star_half_black_24dp;

            timerSwitchFav.Start();
        }

        private void timerSwitchFav_Tick(object sender, EventArgs e)
        {
            timerSwitchFav.Stop();

            pbFavorite.Image = useDarkmode ? (isFavorite ? Properties.Resources.ic_star_white_24dp : Properties.Resources.ic_star_border_white_24dp) : (isFavorite ? Properties.Resources.ic_star_black_24dp : Properties.Resources.ic_star_border_black_24dp);
            dashboard.ChangeFavoriteState(this);
        }

        private void pbFavorite_MouseEnter(object sender, EventArgs e)
        {
            ServerPingUI_MouseEnter(null, null);
            pbFavorite.BackColor = useDarkmode ? Color.FromArgb(50, 200, 200, 200) : Color.FromArgb(25, 0, 0, 0);
        }

        private void pbFavorite_MouseLeave(object sender, EventArgs e)
        {
            ServerPingUI_MouseLeave(null, null);
            pbFavorite.BackColor = shadow.PanelColor;
        }
    }
}
