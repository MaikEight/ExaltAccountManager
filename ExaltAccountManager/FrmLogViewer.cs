using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmLogViewer : Form
    {
        FrmMain frm;
        List<LogData> logs;
        List<LogDataUI> logUIs = new List<LogDataUI>();
        public bool isDarkmode = true;
        private int site = 1;
        private int pageSize = 50;

        public FrmLogViewer(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            isDarkmode = frm.useDarkmode;
            ApplyTheme(isDarkmode);

            LogDataUI head = new LogDataUI(this, null, true, true);
            this.Controls.Add(head);
            head.Location = new Point(1, 48);
            head.Width = this.Width - 2;

            LoadLogs(site, true);
            this.Controls.Remove(pLoad);
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(230, 230, 230);
                Color font = Color.White;

                this.ForeColor = font;
                this.BackColor = def;
                pTop.BackColor = def;
                pBox.BackColor = def;
                pLoad.BackColor = def;

                //tbName.ForeColor = font;
                //tbName.BackColor = def;

                //tbEmail.ForeColor = font;
                //tbEmail.BackColor = def;

                //tbPassword.ForeColor = font;
                //tbPassword.BackColor = def;

                pbLogo.Image = Properties.Resources.ic_assignment_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                p = new Pen(Color.White);
            }
        }

        public void LoadLogs(int page, bool clearFlow = false)
        {
            if (File.Exists(frm.pathLogs))
            {
                try
                {
                    logs = ((List<LogData>)frm.ByteArrayToObject(File.ReadAllBytes(frm.pathLogs)));
                    logs = logs.OrderByDescending(o => o.time).ToList();

                    if (logs.Count > 0)
                    {
                        LogDataUI lastUI = null;
                        if (clearFlow)
                            flow.Controls.Clear();
                        else
                            lastUI = flow.Controls.OfType<LogDataUI>().Last();

                        flow.Visible = false;

                        FormsUtils.SuspendDrawing(flow);
                        bool isSecond = false;
                        Color colSecond = Color.FromArgb(240, 240, 240);
                        if (isDarkmode)
                            colSecond = Color.FromArgb(23, 23, 23);
                        DateTime lastDate = new DateTime();
                        page--;

                        int sIndex = page * pageSize;

                        foreach (LogEntryButton btn in flow.Controls.OfType<LogEntryButton>())
                        {
                            flow.Controls.Remove(btn);
                            btn.Dispose();
                        }

                        for (int i = 0; i < ((logs.Count - sIndex) < pageSize ? (logs.Count - sIndex) : pageSize); i++)
                        {
                            bool showDate = (lastDate.Date != logs[i + sIndex].time.Date || lastDate == new DateTime());
                            LogDataUI ui = new LogDataUI(this, logs[i + sIndex], showDate);
                            if (showDate) lastDate = logs[i + sIndex].time.Date;

                            logUIs.Add(ui);
                            if (isSecond)
                                ui.BackColor = colSecond;

                            flow.Controls.Add(ui);
                            flow.SetFlowBreak(ui, true);
                            isSecond = !isSecond;
                        }

                        if (sIndex + pageSize < logs.Count)
                        { // Next Page
                            LogEntryButton logBtn = new LogEntryButton(this, (sIndex + pageSize < logs.Count) ? sIndex + pageSize : logs.Count, logs.Count);
                            flow.Controls.Add(logBtn);
                            flow.SetFlowBreak(logBtn, true);
                        }
                        flow.Visible = true;

                        if (lastUI != null)
                        {
                            if (flow.Controls.Contains(lastUI))
                                flow.ScrollControlIntoView(lastUI);
                        }

                        FormsUtils.ResumeDrawing(flow);
                    }

                }
                catch
                {
                    try
                    {
                        LogDataUI ui = new LogDataUI(this, new LogData(0, "LogViewer", LogEventType.LogsError, "Failed to load logs."));
                        flow.Controls.Add(ui);
                        flow.SetFlowBreak(ui, true);
                    }
                    catch { }
                }
            }
            else
            {
                LogDataUI ui = new LogDataUI(this, new LogData(0, "LogViewer", LogEventType.NoLogs, "No logs found."));
                flow.Controls.Add(ui);
                flow.SetFlowBreak(ui, true);
            }
        }

        public void ShowMore()
        {
            site++;
            LoadLogs(site);
        }

        #region OnPainT
        Pen p = new Pen(Color.Black);
        private void pTop_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
        }

        private void FrmLogViewer_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void pbLogo_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void pbClose_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
        }

        private void pbMinimize_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);

            e.Graphics.DrawLine(p, topLeft, topRight);
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
        }

        #endregion

        #region Button Close / Minimize
        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            frm.CloseFrmLog(this);
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbClose.BackColor = Color.FromArgb(225, 50, 50);
            else
                pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbMinimize.BackColor = Color.DimGray;
            else
                pbMinimize.BackColor = Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.Transparent;
        }
        #endregion

        #region Drag Form
        private Point MouseDownLocation;
        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new Point(e.X + this.Left - MouseDownLocation.X, e.Y + this.Top - MouseDownLocation.Y);

            }
        }
        #endregion
    }
}
