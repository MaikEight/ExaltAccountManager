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
    public partial class ServerPingGraph : UserControl
    {
        ServerPingUI pingUI;
        UIDashboard dashboard;

        int minPing = int.MaxValue;
        int maxPing = int.MinValue;
        bool useDarkmode = false;

        public ServerPingGraph(UIDashboard dash)
        {
            InitializeComponent();
            dashboard = dash;
            dropInterval.SelectedIndex = 0;
        }

        public ServerPingUI GetPingUI() => pingUI;

        public void ChangeTarget(ServerPingUI _pingUI)
        {
            timerPing.Stop();

            if (pingUI != null)
            {
                pingUI.graph = null;
                pingUI.RemoveGraph();
            }

            pingUI = _pingUI;
            pingUI.graph = this;

            lineChart.Data = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            lineChart.Render();

            minPing = maxPing = pingUI.GetLastPing();

            if (minPing <= 0)
            {
                minPing = int.MaxValue;
                lServerName.Text = pingUI.serverData.name;
                lLastPing.Text =
                lMinPing.Text =
                lMaxPing.Text =
                lAvgPing.Text = "";
            }
            else
            {
                lServerName.Text = pingUI.serverData.name;
                lLastPing.Text =
                lMinPing.Text =
                lMaxPing.Text =
                lAvgPing.Text = minPing.ToString();
            }
            

            timerPing.Start();
        }
        public void ApplyTheme(bool _useDarkmode, Color def, Color second, Color third, Color font)
        {
            useDarkmode = _useDarkmode;

            this.BackColor =
            pbClose.BackColor =
            shadow.BackColor =
            shadow.PanelColor = shadow.PanelColor2 =
            canvas.BackColor = dropInterval.BackgroundColor = def;
            dropInterval.ItemBackColor = dropInterval.ItemBorderColor = third;
            dropInterval.ItemForeColor = font;

            this.ForeColor = canvas.ForeColor =
            canvas.XAxesForeColor = canvas.XAxesLabelForeColor = canvas.XAxesZeroLineColor =
            canvas.YAxesForeColor = canvas.YAxesLabelForeColor = canvas.YAxesZeroLineColor =
            dropInterval.ForeColor = font;

            canvas.XAxesGridColor = useDarkmode ? Color.FromArgb(25, 200, 200, 200) : Color.FromArgb(25, 0, 0, 0);
            canvas.YAxesGridColor = useDarkmode ? Color.FromArgb(25, 200, 200, 200) : Color.FromArgb(100, 0, 0, 0);
            canvas.Update();

            pbClose.Image = useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }

        public void AddNewPing(int ping)
        {
            if (ping == -1)
                ping = 9999;

            lineChart.Data.Add(ping);

            if (lineChart.Data.Count > canvas.Labels.Length)
                lineChart.Data.RemoveAt(0);
            lineChart.Render();

            lLastPing.Text = ping.ToString();

            double min = lineChart.Data.Where(i => i > 0).Min();
            minPing = minPing < min ? minPing : (int)min;

            double max = lineChart.Data.Where(i => i > 0).Max();
            maxPing = maxPing > max ? maxPing : (int)max;

            lMinPing.Text = minPing.ToString();
            lMaxPing.Text = maxPing.ToString();
            lAvgPing.Text = lineChart.Data.Where(i => i > 0).Average().ToString("N0");
        }

        private void dropInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerPing.Stop();
            lineChart.Data = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            lineChart.Render();

            switch (dropInterval.SelectedIndex)
            {
                case 0: //3
                    {
                        canvas.Labels = new string[] { "30", "27", "24", "21", "18", "15", "12", "09", "06", "03", "00" };
                        timerPing.Interval = 3000;
                    }
                    break;
                case 1: //5
                    {
                        canvas.Labels = new string[] { "50", "45", "40", "35", "30", "25", "20", "15", "10", "05", "00" };
                        timerPing.Interval = 5000;
                    }
                    break;
                case 2: //10
                    {
                        canvas.Labels = new string[] { "01:40", "01:30", "01:20", "01:10", "01:0", "00:50", "00:40", "00:30", "00:20", "00:10", "00:00" };
                        timerPing.Interval = 10000;
                    }
                    break;
                case 3: //30
                    {
                        canvas.Labels = new string[] { "05:30", "05:00", "04:30", "04:00", "03:30", "03:00", "02:30", "02:00", "01:30", "01:00", "00:30" };
                        timerPing.Interval = 30000;
                    }
                    break;
                case 4: //60
                    {
                        canvas.Labels = new string[] { "10:00", "09:00", "08:00", "07:00", "06:00", "05:00", "04:00", "03:00", "02:00", "01:00", "00:00" };
                        timerPing.Interval = 60000;
                    }
                    break;
                default:
                    break;
            }
            timerPing.Start();
        }

        private void timerPing_Tick(object sender, EventArgs e)
        {
            if (pingUI != null)
                pingUI.PingServer(true);
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        { 
            pbClose.BackColor = useDarkmode ? Color.FromArgb(225, 225, 50, 50) : Color.FromArgb(255, 205, 82, 82);
            pbClose.Image = Properties.Resources.ic_close_white_24dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        { 
            pbClose.BackColor = this.BackColor;
            pbClose.Image = useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            timerPing.Stop();

            pingUI.graph = null;
            pingUI.RemoveGraph();
            dashboard.HideGraphUI();
        }

        public void SetPingUI(ServerPingUI u) => pingUI = u;
    }
}
