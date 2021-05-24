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
using MKRadarChart;

namespace EAM_Statistics
{
    public partial class UIDashboard : UserControl
    {
        FrmMain form;

        public UIDashboard(FrmMain _form)
        {
            InitializeComponent();
            form = _form;

        }

        public void LoadUI()
        {
            this.SuspendLayout();
            try
            {           
                mtpInfo.Init("Info", "Dashboard", "This may not be representive of the current game data.");

                #region Amount of accounts

                lAmountOfAccounts.Text = string.Format(lAmountOfAccounts.Text, form.statsList.Count);

                #endregion

                #region Amount of Chars

                int chars = 0;
                for (int i = 0; i < form.statsList.Count; i++)
                {
                    if (form.statsList[i].charList != null && form.statsList[i].charList.Count > 0)
                        chars += form.statsList[i].charList[form.statsList[i].charList.Count - 1].chars.Count;
                }
                lAmountOfChars.Text = string.Format(lAmountOfChars.Text, chars);

                #endregion

                #region Total fame + total alive

                int totalFame = 0;
                int totalAliveFame = 0;

                AccountStats[] totalStats = new AccountStats[3] { new AccountStats(), new AccountStats(), new AccountStats() };
                Dictionary<string, int> dicMailToLiveFame = new Dictionary<string, int>();

                List<Tuple<string, int>> listLiveFame = new List<Tuple<string, int>>();

                for (int i = 0; i < form.statsList.Count; i++)
                {
                    if (form.statsList[i].stats != null && form.statsList[i].stats.Count > 0)
                    {
                        int f = form.statsList[i].stats[form.statsList[i].stats.Count - 1].totalFame;
                        totalFame += f;

                        if (totalStats[2].totalFame < f)
                        {
                            if (totalStats[1].totalFame < f)
                            {
                                if (totalStats[0].totalFame < f)
                                {
                                    totalStats[2] = totalStats[1];
                                    totalStats[1] = totalStats[0];
                                    totalStats[0] = form.statsList[i].stats[form.statsList[i].stats.Count - 1];
                                }
                                else
                                {
                                    totalStats[2] = totalStats[1];
                                    totalStats[1] = form.statsList[i].stats[form.statsList[i].stats.Count - 1];
                                }
                            }
                            else
                            {
                                totalStats[2] = form.statsList[i].stats[form.statsList[i].stats.Count - 1];
                            }
                        }

                        int val = 0;
                        for (int x = 0; x < form.statsList[i].charList[(form.statsList[i].charList.Count) - 1].chars.Count; x++)
                        {
                            val += form.statsList[i].charList[(form.statsList[i].charList.Count) - 1].chars[x].fame;
                        }

                        listLiveFame.Add(new Tuple<string, int>(form.statsList[i].stats[form.statsList[i].stats.Count - 1].email, val));
                        totalAliveFame += val;
                    }
                }
                lTotalFame.Text = string.Format(lTotalFame.Text, totalFame);
                lTotalAliveFame.Text = string.Format(lTotalAliveFame.Text, totalAliveFame);

                #endregion

                #region LastWeeks total fame

                //Last Weeks total fame
                int differenceTotal = 0;
                int differenceAlive = 0;
                DateTime lastWeek = DateTime.Now.AddDays(-7);
                for (int i = 0; i < form.statsList.Count; i++)
                {
                    for (int x = form.statsList[i].stats.Count; x < 0; x--)
                    {
                        if (form.statsList[i].stats[x].date <= lastWeek.Date)
                        {
                            int dif = form.statsList[i].stats[form.statsList[i].stats.Count - 1].totalFame - form.statsList[i].stats[x].totalFame;
                            if (dif > 0)
                                differenceTotal += dif;
                            break;
                        }
                    }

                    for (int x = 0; x < form.statsList[i].charList.Count; x++)
                    {
                        if (form.statsList[i].charList[x].date <= lastWeek)
                        {
                            for (int y = 0; y < form.statsList[i].charList[x].chars.Count; y++)
                            {
                                //int dif = form.statsList[i].charList[x].chars[y].fame;
                                //if (dif > 0)
                                differenceAlive += form.statsList[i].charList[x].chars[y].fame;
                            }
                            break;
                        }
                    }
                }
                differenceAlive = totalAliveFame - differenceAlive;

                #endregion

                lOverallTotalChange.Left = lTotalFameHeadline.Left;
                lOverallTotalChange.Width = lTotalFame.Width - 13;
                lOverallTotalChange.Text = $"{(differenceTotal >= 0 ? '+' : ' ')}{differenceTotal} since last week";
                lOverallTotalChange.ForeColor = differenceTotal >= 0 ? differenceTotal == 0 ? this.ForeColor : Color.Green : Color.Crimson;

                lTotalAliveChange.Left = lTotalAliveFameHeadline.Left;
                lTotalAliveChange.Width = lTotalAliveFame.Width - 13;
                lTotalAliveChange.Text = $"{(differenceAlive >= 0 ? '+' : ' ')}{differenceAlive} since last week";
                lTotalAliveChange.ForeColor = differenceAlive >= 0 ? differenceAlive == 0 ? this.ForeColor : Color.Green : Color.Crimson;

                List<string> accNames = new List<string>();
                List<string> acctotalFame = new List<string>();

                for (int i = 0; i < totalStats.Length; i++)
                {
                    if (!string.IsNullOrEmpty(totalStats[i].email) && totalStats[i].totalFame > 0)
                    {
                        accNames.Add(totalStats[i].email);

                        for (int x = 0; x < form.accounts.Count; x++)
                        {
                            if (form.accounts[x].email.Equals(totalStats[i].email))
                            {
                                accNames[i] = form.accounts[x].name;
                                break;
                            }
                        }

                        acctotalFame.Add(totalStats[i].totalFame.ToString());
                    }
                }
                accNames.Insert(0, "Name");
                acctotalFame.Insert(0, "Total fame");
                topAccountFame.Init("Top accounts by total fame", "Best total fame", accNames.ToArray(), acctotalFame.ToArray());

                accNames.Clear();
                acctotalFame.Clear();

                List<Tuple<string, int>> sortedLiveFame = listLiveFame.OrderByDescending(o => o.Item2).ToList();
                if (sortedLiveFame.Count > 0)
                {
                    for (int i = 0; i < ((sortedLiveFame.Count < 3) ? sortedLiveFame.Count : 3); i++)
                    {
                        accNames.Add(sortedLiveFame[i].Item1);
                        for (int x = 0; x < form.accounts.Count; x++)
                        {
                            if (form.accounts[x].email.Equals(accNames[i]))
                            {
                                accNames[i] = form.accounts[x].name;
                                break;
                            }
                        }
                        acctotalFame.Add(sortedLiveFame[i].Item2.ToString());
                    }
                    accNames.Insert(0, "Name");
                    acctotalFame.Insert(0, "Alive fame");
                    topAccountLiveFame.Init("Top accounts by alive fame", "Best alive fame", accNames.ToArray(), acctotalFame.ToArray());
                }

                #region Logins

                int totalLogins = 0;
                int loginsLastWeek = 0;
                for (int i = 0; i < form.statsList.Count; i++)
                {
                    if (form.statsList[i].logins == null)
                        continue;

                    totalLogins += form.statsList[i].logins.Count;
                    loginsLastWeek += form.statsList[i].logins.Where(o => o.time > lastWeek).ToList().Count();
                }

                lTotalLogins.Text = string.Format(lTotalLogins.Text, totalLogins);
                lTotalLoginsChange.Text = string.Format(lTotalLoginsChange.Text, loginsLastWeek);
                lTotalLoginsChange.ForeColor = differenceAlive >= 0 ? this.ForeColor : Color.Green;

                #endregion

                #region DataViz Logins

                //var r = new Random();
                //var canvas = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas();

                //var datapoint = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint(Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced._type.Bunifu_area);

                //datapoint.addLabely("SUN", r.Next(0, 100).ToString());
                //datapoint.addLabely("MON", r.Next(0, 100).ToString());
                //datapoint.addLabely("TUE", r.Next(0, 100).ToString());
                //datapoint.addLabely("WED", r.Next(0, 100).ToString());

                //// Add data sets to Canvas
                //canvas.addData(datapoint);
                ////render canvas   
                //dataVizCanvas.Render(canvas);

                #endregion
            }
            catch { }
            this.ResumeLayout();
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;
            lOverallTotalChange.ForeColor = lOverallTotalChange.ForeColor == this.ForeColor ? font : lOverallTotalChange.ForeColor;
            lTotalAliveChange.ForeColor = lTotalAliveChange.ForeColor == this.ForeColor ? font : lTotalAliveChange.ForeColor;
            lTotalLoginsChange.ForeColor = lTotalLoginsChange.ForeColor == this.ForeColor ? font : lTotalLoginsChange.ForeColor;

            this.ForeColor = font;

            if (isDarkmode)
            {

            }
            else
            {

            }

            foreach (Panel p in this.Controls.OfType<Panel>())
            {
                foreach (MaterialPanel ui in p.Controls.OfType<MaterialPanel>())
                    ui.ApplyTheme(isDarkmode);
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
        }
    }
}
