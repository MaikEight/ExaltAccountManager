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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExaltAccountManager;

namespace EAM_Statistics
{
    public partial class FrmMain : Form
    {
        public readonly Version version = new Version(1, 0, 0);

        private UIDashboard dashboard;
        private UIAccountView accountView;
        private UIAbout aboutUI;

        private Dictionary<Button, StatsMain> dicBtnToStatsMain = new Dictionary<Button, StatsMain>();

        public bool useDarkmode = false;

        public List<StatsMain> statsList = new List<StatsMain>();
        public List<AccountInfo> accounts = new List<AccountInfo>();

        public string optionsPath = Path.Combine(Application.StartupPath, "EAM.options");
        public string accountsPath = Path.Combine(Application.StartupPath, "EAM.accounts");
        public string accountStatsPath = Path.Combine(Application.StartupPath, "Stats");
        public string pathLogs = Path.Combine(Application.StartupPath, "EAM.Logs");

        public List<LogData> logs = new List<LogData>();

        #region ChartData

        public List<Image> charImages = new List<Image>()
        {
            Properties.Resources.Rogue,
            Properties.Resources.Archer,
            Properties.Resources.Wizard,
            Properties.Resources.Priest,
            Properties.Resources.Warrior,
            Properties.Resources.Knight,
            Properties.Resources.Paladin,
            Properties.Resources.Assassin,
            Properties.Resources.Necromancer,
            Properties.Resources.Huntress,
            Properties.Resources.Mystic,
            Properties.Resources.Trickster,
            Properties.Resources.Sorcerer,
            Properties.Resources.Ninja,
            Properties.Resources.Samurai,
            Properties.Resources.Bard,
            Properties.Resources.Summoner
        };

        public List<string> charNames = new List<string>()
        {
            "Rogue",
            "Archer",
            "Wizard",
            "Priest",
            "Warrior",
            "Knight",
            "Paladin",
            "Assassin",
            "Necromancer",
            "Huntress",
            "Mystic",
            "Trickster",
            "Sorcerer",
            "Ninja",
            "Samurai",
            "Bard",
            "Summoner"
        };

        #endregion

        public FrmMain()
        {
            InitializeComponent();
            pButtons.Controls.SetChildIndex(pSideBar, 0);
            //MessageBox.Show("Test");
            dashboard = new UIDashboard(this);
            this.ShowInTaskbar = true;

            LoadOptions();

            SwitchDesign(false);

            LoadAccountInfos();
            LoadStats();
            this.pMain.Controls.Add(dashboard);
            dashboard.LoadUI();
            CreateAccountButtons();

            pButtons.HorizontalScroll.Visible = false;
            pButtons.VerticalScroll.Visible = false;

            this.BringToFront();
        }

        public void SwitchDesign(bool performSwitch = true)
        {
            if (performSwitch)
                useDarkmode = !useDarkmode;

            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            ApplyTheme(useDarkmode, def, second, third, font);

            foreach (MaterialTextPanel ui in pMain.Controls.OfType<MaterialTextPanel>())
                ui.ApplyTheme(useDarkmode);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void LoadAccountInfos()
        {
            try
            {
                byte[] data = File.ReadAllBytes(accountsPath);
                AesCryptographyService acs = new AesCryptographyService();
                accounts = (List<AccountInfo>)ByteArrayToObject(acs.Decrypt(data));
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM Stats", LogEventType.StatsError, $"Failed to load AccountInfos."));
            }
        }

        public void SetTitleText(string title) => lTitle.Text = title;

        private void LoadOptions()
        {
            if (File.Exists(optionsPath))
            {
                try
                {
                    OptionsData opt = (OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath));
                    useDarkmode = opt.useDarkmode;
                }
                catch { }
            }
            else
            {
                try
                {
                    OptionsData opt = new OptionsData()
                    {
                        exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe"),
                        closeAfterConnection = false,
                        useDarkmode = !useDarkmode
                    };
                    File.WriteAllBytes(optionsPath, ObjectToByteArray(opt));
                }
                catch { }
            }
        }

        private void LoadStats()
        {
            try
            {
                foreach (string f in Directory.GetFiles(accountStatsPath))
                {
                    try
                    {
                        StatsMain stat = (StatsMain)ByteArrayToObject(File.ReadAllBytes(f));
                        statsList.Add(stat);
                    }
                    catch { }
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM Stats", LogEventType.StatsError, $"Failed to load data."));
            }
        }

        public void LogEvent(LogData data)
        {
            if (File.Exists(pathLogs))
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    logs = (List<LogData>)ByteArrayToObject(File.ReadAllBytes(pathLogs));
                    data.ID = logs.Count - 1;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
            else
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    data.ID = 0;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            this.ForeColor = font;
            this.BackColor = def;

            pTop.BackColor = pRight.BackColor = pbMinimize.BackColor = pbClose.BackColor = second;
            pSide.BackColor = second;
            pAccountButtons.BackColor = second;
            pAccountBtnSpacer.BackColor = second;
            pMain.BackColor = def;
            pSpacer.BackColor = third;

            if (isDarkmode)
            {
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;

                //Buttons
                btnDashboard.Image = Properties.Resources.ic_dashboard_white_24dp;
                btnAccountView.Image = Properties.Resources.ic_account_box_white_24dp;
                btnAbout.Image = Properties.Resources.ic_info_outline_white_24dp;
            }
            else
            {
                pbMinimize.Image = Properties.Resources.baseline_minimize_black_24dp;
                pbClose.Image = Properties.Resources.ic_close_black_24dp;

                //Buttons
                btnDashboard.Image = Properties.Resources.ic_dashboard_black_24dp;
                btnAccountView.Image = Properties.Resources.ic_account_box_black_24dp;
                btnAbout.Image = Properties.Resources.ic_info_outline_black_24dp;
            }

            foreach (Button btn in dicBtnToStatsMain.Keys)
            {
                btn.BackColor = second;
                btn.ForeColor = font;
            }
            //    btn.Image = isDarkmode ? Properties.Resources.ic_account_circle_white_24dp : Properties.Resources.ic_account_circle_black_24dp;            

            if (dashboard != null) dashboard.ApplyTheme(isDarkmode, def, second, third, font);
            if (accountView != null) accountView.ApplyTheme(isDarkmode, def, second, third, font);
            if (aboutUI != null) aboutUI.ApplyTheme(isDarkmode, def, second, third, font);

            this.Update();
        }

        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }

        #region Button Close / Minimize

        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        //private void pbClose_Click(object sender, EventArgs e) => Environment.Exit(0);
        private void pbClose_Click(object sender, EventArgs e) => this.Close();

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = useDarkmode ? Color.FromArgb(225, 225, 50, 50) : Color.FromArgb(255, 205, 82, 82);
        }

        private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = pRight.BackColor;

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            pbMinimize.BackColor = useDarkmode ? Color.FromArgb(128, 105, 105, 105) : Color.FromArgb(128, 169, 169, 169);
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = pRight.BackColor;

        #endregion

        private void ClearPMain()
        {
            pMain.Controls.Clear();
            pAccountButtons.Visible = false;
            scrollbar.BindTo(pButtons);

            if (accountView != null)
            {
                accountView.CloseAccountViewer();
            }
            //SetTitleText("");
        }

        private void btnSwitchDesign_Click(object sender, EventArgs e)
        {
            SwitchDesign();
            lEAMHead.Focus();
        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            pSideBar.Location = new Point(0, btnDashboard.Top + 3);

            if (dashboard != null)
            {
                if (!pMain.Controls.Contains(dashboard))
                {
                    ClearPMain();
                    pMain.Controls.Add(dashboard);
                    SetTitleText("Dashboard");
                }
            }
            else
            {
                ClearPMain();
                dashboard = new UIDashboard(this);
                pMain.Controls.Add(dashboard);
                SetTitleText("Dashboard");
            }
        }

        private void btnAccountView_Click(object sender, EventArgs e)
        {
            pSideBar.Location = new Point(0, btnAccountView.Top + 3);

            if (accountView != null)
            {
                if (!pMain.Controls.Contains(accountView))
                {
                    ClearPMain();
                    pMain.Controls.Add(accountView);
                    SetTitleText("Accounts");
                }
                else
                {
                    accountView.CloseAccountViewer();
                }
            }
            else
            {
                ClearPMain();
                accountView = new UIAccountView(this);
                pMain.Controls.Add(accountView);
                SetTitleText("Accounts");
            }
            pAccountButtons.Visible = true;
            scrollbar.BindTo(pButtons);
            pButtons.HorizontalScroll.Visible = false;
            pButtons.HorizontalScroll.Enabled = false;
            pButtons.VerticalScroll.Visible = false;
            pButtons.VerticalScroll.Enabled = false;
        }

        private void CreateAccountButtons()
        {
            dicBtnToStatsMain.Clear();

            List<StatsMain> list = new List<StatsMain>();
            list.AddRange(statsList);
            list = list.OrderByDescending(o => o.stats.Count > 0 ? o.stats[o.stats.Count - 1].totalFame : -1).ToList();
            int h = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string name = GetAccountName(list[i].email);
                Button btn = new Button()
                {
                    FlatStyle = btnDashboard.FlatStyle,
                    ImageAlign = btnDashboard.ImageAlign,
                    TextAlign = btnDashboard.TextAlign,
                    TextImageRelation = btnDashboard.TextImageRelation,
                    UseVisualStyleBackColor = btnDashboard.UseVisualStyleBackColor,
                    Dock = btnDashboard.Dock,
                    BackColor = btnDashboard.BackColor,
                    ForeColor = btnDashboard.ForeColor,
                    Size = new Size(165, 23),
                    Text = $"#{i + 1} {name}"
                };

                btn.FlatAppearance.BorderColor = btnDashboard.FlatAppearance.BorderColor;
                btn.FlatAppearance.BorderSize = btnDashboard.FlatAppearance.BorderSize;
                btn.FlatAppearance.CheckedBackColor = btnDashboard.FlatAppearance.CheckedBackColor;
                btn.FlatAppearance.MouseOverBackColor = btnDashboard.FlatAppearance.MouseOverBackColor;
                btn.FlatAppearance.MouseDownBackColor = btnDashboard.FlatAppearance.MouseDownBackColor;

                btn.Font = new Font(btnDashboard.Font.FontFamily, 7f, FontStyle.Regular);

                btn.Click += AccountButton_Click;
                h += btn.Height;
                dicBtnToStatsMain.Add(btn, list[i]);

                pAccountButtons.Controls.Add(btn);
                pAccountButtons.Controls.SetChildIndex(btn, 0);
            }

            pAccountButtons.Height = h;
            pAccountButtons.Controls.SetChildIndex(pAccountBtnSpacer, list.Count*10 + 1 );
        }

        private void AccountButton_Click(object sender, EventArgs e)
        {
            try
            {
                FormsUtils.SuspendDrawing(pMain);

                if(accountView == null || !pMain.Controls.Contains(accountView))
                    btnAccountView_Click(null, null);
                accountView.OpenAccountViewer(dicBtnToStatsMain[(Button)sender]);

                FormsUtils.ResumeDrawing(pMain);
            }
            catch { }            
        }

        public string GetAccountName(string mail)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].email.Equals(mail))
                {
                    return accounts[i].name;
                }
            }

            return mail;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            if (aboutUI != null)
            {
                ClearPMain();
                pMain.Controls.Add(aboutUI);
                SetTitleText("About");
            }
            else
            {
                ClearPMain();
                aboutUI = new UIAbout(this);
                pMain.Controls.Add(aboutUI);
                SetTitleText("About");
            }

            pSideBar.Location = new Point(0, btnAbout.Top + 3);
        }

        private Point m_PreviousLocation = new Point(int.MinValue, int.MinValue);
        private void FrmMain_LocationChanged(object sender, EventArgs e)
        {
            // All open child forms to be moved
            Form[] formsToAdjust = Application
              .OpenForms
              .OfType<Form>()
              //.Where(f => !f.GetType().Name.Equals("FrmMain"))
              .ToArray();

            // If the main form has been moved...
            if (m_PreviousLocation.X != int.MinValue)
            {
                foreach (var form in formsToAdjust) //... we move all child froms aw well
                {
                    if (form == this || form.GetType().Name.Equals("FrmLogViewer") || form.GetType().Name.Equals("FrmMain"))
                        continue;
                    form.Location = new Point(
                      form.Location.X + Location.X - m_PreviousLocation.X,
                      form.Location.Y + Location.Y - m_PreviousLocation.Y
                    );
                }
            }
            m_PreviousLocation = Location;
        }

        public void CloseCharacterForms()
        {
            Form[] formsToAdjust = Application
              .OpenForms
              .OfType<FrmCharHost>()
              .ToArray();

            for (int i = 0; i < formsToAdjust.Length; i++)
            {
                formsToAdjust[i].Close();
            }

            formsToAdjust = Application
              .OpenForms
              .OfType<FrmShadowHost>()
              .ToArray();

            for (int i = 0; i < formsToAdjust.Length; i++)
            {
                formsToAdjust[i].Close();
            }
        }
    }
}
