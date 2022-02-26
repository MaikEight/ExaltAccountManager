using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleCustomImport : UserControl
    {
        FrmMain frm;
        EleImport eleImport;

        Mini.MiniAccountDataOrderEntry miniEmail;
        Mini.MiniAccountDataOrderEntry miniPassword;
        Mini.MiniAccountDataOrderEntry miniAccountname;
        Mini.MiniAccountDataOrderEntry miniGroup;

        Mini.MiniAccountDataOrderEntry SelectedMini
        {
            get => selectedMiniValue;
            set
            {
                selectedMiniValue = value;
                if (value != null)
                {
                    int index = GetMiniIndex(value);
                    btnBack.Visible =
                    btnBack.Enabled = index > 0;
                    btnForward.Visible =
                    btnForward.Enabled = index < (miniAmount - 1);
                    lMiniIndex.Visible = true;
                    lMiniIndex.Text = (index + 1).ToString();
                }
                else
                {
                    btnBack.Visible =
                    btnBack.Enabled =
                    btnForward.Visible =
                    btnForward.Enabled =
                    lMiniIndex.Visible = false;
                }
            }
        }
        Mini.MiniAccountDataOrderEntry selectedMiniValue = null;

        int miniAmount = 2;
        bool isInit = true;
        string path = string.Empty;
        List<MK_EAM_Lib.AccountInfo> accounts = new List<MK_EAM_Lib.AccountInfo>();

        public EleCustomImport(FrmMain _frm, EleImport _eleImport, string _path)
        {
            InitializeComponent();
            frm = _frm;
            eleImport = _eleImport;
            path = _path;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += EleCustomImport_Disposed;

            miniEmail = new Mini.MiniAccountDataOrderEntry(frm, "Email", "e@mail.com");
            miniPassword = new Mini.MiniAccountDataOrderEntry(frm, "Password", "**********");
            miniAccountname = new Mini.MiniAccountDataOrderEntry(frm, "Accountname", "Maik8") { Visible = false };
            miniGroup = new Mini.MiniAccountDataOrderEntry(frm, "Group", " #00b3db ") { Visible = false };

            miniEmail.SelectedChanged += MiniClicked;
            miniPassword.SelectedChanged += MiniClicked;
            miniAccountname.SelectedChanged += MiniClicked;
            miniGroup.SelectedChanged += MiniClicked;

            flowData.Controls.Add(miniEmail);
            flowData.Controls.Add(miniPassword);
            flowData.Controls.Add(miniAccountname);
            flowData.Controls.Add(miniGroup);

            ApplyTheme(frm, null);
        }

        private void EleCustomImport_Disposed(object sender, EventArgs e)
        {
            frm.ThemeChanged -= ApplyTheme;
        }

        private void EleCustomImport_Load(object sender, EventArgs e)
        {
            radioAccountPerLine.Checked = true;
            radioAccountPerValue.Checked = false;

            isInit = false;
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            tbValuesPerAccount.Enabled =
            tbSkipLines.Enabled = true;

            tbValuesPerAccount.ResetColors();
            tbValueSeperationChar.ResetColors();
            tbSkipLines.ResetColors();

            this.BackColor =
            tbValuesPerAccount.BackColor = tbValuesPerAccount.FillColor = tbValuesPerAccount.OnIdleState.FillColor = tbValuesPerAccount.OnHoverState.FillColor = tbValuesPerAccount.OnActiveState.FillColor = tbValuesPerAccount.OnDisabledState.FillColor =
            tbValueSeperationChar.BackColor = tbValueSeperationChar.FillColor = tbValueSeperationChar.OnIdleState.FillColor = tbValueSeperationChar.OnHoverState.FillColor = tbValueSeperationChar.OnActiveState.FillColor = tbValueSeperationChar.OnDisabledState.FillColor =
            tbSkipLines.BackColor = tbSkipLines.FillColor = tbSkipLines.OnIdleState.FillColor = tbSkipLines.OnHoverState.FillColor = tbSkipLines.OnActiveState.FillColor = tbSkipLines.OnDisabledState.FillColor =
            second;
            this.ForeColor =
            tbValuesPerAccount.ForeColor = tbValuesPerAccount.OnActiveState.ForeColor = tbValuesPerAccount.OnDisabledState.ForeColor = tbValuesPerAccount.OnHoverState.ForeColor = tbValuesPerAccount.OnIdleState.ForeColor =
            tbValueSeperationChar.ForeColor = tbValueSeperationChar.OnActiveState.ForeColor = tbValueSeperationChar.OnDisabledState.ForeColor = tbValueSeperationChar.OnHoverState.ForeColor = tbValueSeperationChar.OnIdleState.ForeColor =
            tbSkipLines.ForeColor = tbSkipLines.OnActiveState.ForeColor = tbSkipLines.OnDisabledState.ForeColor = tbSkipLines.OnHoverState.ForeColor = tbSkipLines.OnIdleState.ForeColor =
            font;

            tbValuesPerAccount.Enabled = radioAccountPerValue.Checked;
            tbSkipLines.Enabled = !radioAccountPerValue.Checked;

            miniEmail.UpdateUI();
            miniPassword.UpdateUI();
            miniAccountname.UpdateUI();
            miniGroup.UpdateUI();

            toolTip.BackColor = def;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = frm.UseDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);
        }

        private void radioAccountPerValue_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            tbValuesPerAccount.Enabled = radioAccountPerValue.Checked;
            tbSkipLines.Enabled = !radioAccountPerValue.Checked;

            if (isInit) return;

            tbValuesPerAccount.ResetColors();
            tbSkipLines.ResetColors();

            tbValuesPerAccount.BackColor = tbValuesPerAccount.FillColor = tbValuesPerAccount.OnIdleState.FillColor = tbValuesPerAccount.OnHoverState.FillColor = tbValuesPerAccount.OnActiveState.FillColor = tbValuesPerAccount.OnDisabledState.FillColor =
            tbSkipLines.BackColor = tbSkipLines.FillColor = tbSkipLines.OnIdleState.FillColor = tbSkipLines.OnHoverState.FillColor = tbSkipLines.OnActiveState.FillColor = tbSkipLines.OnDisabledState.FillColor =
            ColorScheme.GetColorSecond(frm.UseDarkmode);
            tbValuesPerAccount.ForeColor = tbValuesPerAccount.OnActiveState.ForeColor = tbValuesPerAccount.OnDisabledState.ForeColor = tbValuesPerAccount.OnHoverState.ForeColor = tbValuesPerAccount.OnIdleState.ForeColor =
            tbSkipLines.ForeColor = tbSkipLines.OnActiveState.ForeColor = tbSkipLines.OnDisabledState.ForeColor = tbSkipLines.OnHoverState.ForeColor = tbSkipLines.OnIdleState.ForeColor =
            ColorScheme.GetColorFont(frm.UseDarkmode);
        }

        private void toggleAccountname_CheckedChanged(object sender, EventArgs e)
        {
            miniAccountname.Visible = toggleAccountname.Checked;
            miniAmount += toggleAccountname.Checked ? 1 : -1;

            UpdateMiniIndices();
        }

        private void toggleGroup_CheckedChanged(object sender, EventArgs e)
        {
            miniGroup.Visible = toggleGroup.Checked;
            miniAmount += toggleGroup.Checked ? 1 : -1;

            UpdateMiniIndices();
        }

        private void UpdateMiniIndices()
        {
            if (SelectedMini != null)
                SelectedMini.SelectionOverride(false);
            SelectedMini = null;

            List<Tuple<Mini.MiniAccountDataOrderEntry, int>> entries = new List<Tuple<Mini.MiniAccountDataOrderEntry, int>>()
            {
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniEmail, GetMiniIndex(miniEmail)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniPassword, GetMiniIndex(miniPassword)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniAccountname, GetMiniIndex(miniAccountname)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniGroup, GetMiniIndex(miniGroup)),
            }.OrderByDescending(e => e.Item1.Visible).ThenBy(e => e.Item2).ToList();

            //var list = entries.OrderByDescending(e => e.Item1.Visible).ThenBy(e => e.Item2).ToList();
            for (int i = 0; i < entries.Count; i++)
            {
                flowData.Controls.SetChildIndex(entries[i].Item1, i);
            }

            UpdateSelectionTextAndButtons();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (SelectedMini == null) return;

            flowData.Controls.SetChildIndex(SelectedMini, GetMiniIndex(SelectedMini) - 1);
            UpdateSelectionTextAndButtons();
        }


        private void btnForward_Click(object sender, EventArgs e)
        {
            if (SelectedMini == null) return;

            flowData.Controls.SetChildIndex(SelectedMini, GetMiniIndex(SelectedMini) + 1);
            UpdateSelectionTextAndButtons();
        }

        private void UpdateSelectionTextAndButtons()
        {
            if (SelectedMini == null) return;

            int index = GetMiniIndex(SelectedMini);
            btnBack.Visible =
            btnBack.Enabled = index > 0;
            btnForward.Visible =
            btnForward.Enabled = index < (miniAmount - 1);
            lMiniIndex.Visible = true;
            lMiniIndex.Text = (index + 1).ToString();
        }

        private int GetMiniIndex(Mini.MiniAccountDataOrderEntry mini) => flowData.Controls.GetChildIndex(mini);

        private void MiniClicked(object sender, EventArgs e)
        {
            Mini.MiniAccountDataOrderEntry s = sender as Mini.MiniAccountDataOrderEntry;
            if (SelectedMini == s)
            {
                SelectedMini = null;
                return;
            }
            if (SelectedMini != null)
            {
                SelectedMini.SelectionOverride(false);
                SelectedMini = s;
                return;
            }
            SelectedMini = s;
        }

        private void btnTestImport_Click(object sender, EventArgs e)
        {
            lOutputValues.Visible =
            btnSave.Visible =
            btnSave.Enabled = false;
            btnClose.Top = 379;

            lOutputHeadlines.Text = string.Empty;

            try
            {
                accounts = new List<MK_EAM_Lib.AccountInfo>();
                string[] lines = new string[0];

                if (!radioAccountPerLine.Checked)
                {//One per x values
                    string data = File.ReadAllText(path);
                    string[] values = Regex.Split(data, tbValueSeperationChar.Text);
                    List<string> listLines = new List<string>();
                    int.TryParse(tbValuesPerAccount.Text, out int valuesPerAccount);

                    bool working = true;
                    int valueIndex = 0;
                    StringBuilder sb = new StringBuilder();
                    while (working)
                    {
                        string[] vs = new string[valuesPerAccount];
                        sb.Clear();
                        for (int i = 0; i < valuesPerAccount; i++)
                        {
                            sb.Append(values[valueIndex] + tbValueSeperationChar.Text);
                            vs[i] = values[valueIndex];
                            valueIndex++;
                        }
                        string li = sb.ToString();
                        listLines.Add(li.Substring(0, li.Length - tbValueSeperationChar.Text.Length));

                        if (valueIndex + valuesPerAccount > values.Length)
                            working = false;
                    }
                    lines = listLines.ToArray();
                }
                else
                {   //One per line
                    lines = File.ReadAllLines(path);
                }

                if (lines.Length > 0)
                {
                    int.TryParse(tbSkipLines.Text, out int skipLines);
                    int failedImports = 0;

                    int[] indices = new int[4]
                    {
                        GetMiniIndex(miniEmail),
                        GetMiniIndex(miniPassword),
                        GetMiniIndex(miniAccountname),
                        GetMiniIndex(miniGroup),
                    };

                    bool[] valueStates = new bool[4]
                    {
                        miniEmail.Visible,
                        miniPassword.Visible,
                        miniAccountname.Visible,
                        miniGroup.Visible
                    };

                    List<string> emails = new List<string>();
                    string[] values = null;
                    bool useSplit = true;
                    char splitChar = ' ';
                    int doubleAccounts = 0;
                    if (tbValueSeperationChar.Text.Length == 1)
                    {
                        useSplit = false;
                        splitChar = tbValueSeperationChar.Text[0];
                    }

                    for (int i = skipLines; i < lines.Length; i++)
                    {
                        if (useSplit)
                            values = Regex.Split(lines[i], tbValueSeperationChar.Text);
                        else
                            values = lines[i].Split(splitChar);
                        try
                        {
                            if (emails.Contains(values[indices[0]]))
                            {
                                doubleAccounts++;
                                continue;
                            }

                            MK_EAM_Lib.AccountInfo acc = new MK_EAM_Lib.AccountInfo()
                            {
                                email = values[indices[0]],
                                password = values[indices[1]],
                                performSave = true
                            };
                            if (valueStates[2])
                                acc.name = values[indices[2]];
                            if (valueStates[3])
                            {
                                if (!values[indices[3]].StartsWith("#"))
                                    values[indices[3]] = $"#{values[indices[3]]}";
                                acc.Color = System.Drawing.ColorTranslator.FromHtml(values[indices[3]]);
                            }
                            accounts.Add(acc);
                            emails.Add(acc.Email);
                        }
                        catch { failedImports++; }
                    }

                    if (failedImports > 0)
                        frm.ShowSnackbar($"Failed to parse {failedImports} accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000); 
                    
                    if (doubleAccounts > 0)
                        frm.ShowSnackbar($"Found {doubleAccounts} accounts multiple times.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                }

                AddAccountsToTestUI();
            }
            catch
            {
                frm.ShowSnackbar("Failed to parse your provided data into accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void AddAccountsToTestUI()
        {
            if (accounts.Count == 0) return;

            List<Tuple<Mini.MiniAccountDataOrderEntry, int>> entries = new List<Tuple<Mini.MiniAccountDataOrderEntry, int>>()
            {
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniEmail, GetMiniIndex(miniEmail)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniPassword, GetMiniIndex(miniPassword)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniAccountname, GetMiniIndex(miniAccountname)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniGroup, GetMiniIndex(miniGroup)),
            }.OrderByDescending(e => e.Item1.Visible).ThenBy(e => e.Item2).ToList();

            int[] indices = new int[4]
            {
                GetMiniIndex(miniEmail),
                GetMiniIndex(miniPassword),
                GetMiniIndex(miniAccountname),
                GetMiniIndex(miniGroup),
            };//15   10     15  8

            StringBuilder sb = new StringBuilder();

            StringBuilder sbA1 = new StringBuilder();
            StringBuilder sbA2 = new StringBuilder();
            StringBuilder sbA3 = new StringBuilder();

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Item1.Visible)
                {
                    if (entries[i].Item1.EntryName.Length < 15)
                        sb.Append(entries[i].Item1.EntryName + new string(' ', 15 - entries[i].Item1.EntryName.Length));
                    else
                        sb.Append(entries[i].Item1.EntryName.Substring(0, 15));
                    sb.Append(' ');

                    string data = GetValueOfAccount(accounts[0], entries[i].Item1.EntryName);
                    if (data.Length < 15)
                        sbA1.Append(data + new string(' ', 15 - data.Length));
                    else
                        sbA1.Append(data.Substring(0, 15));
                    sbA1.Append(' ');
                    if (accounts.Count > 1)
                    {
                        data = GetValueOfAccount(accounts[1], entries[i].Item1.EntryName);
                        if (data.Length < 15)
                            sbA2.Append(data + new string(' ', 15 - data.Length));
                        else
                            sbA2.Append(data.Substring(0, 15));
                        sbA2.Append(' ');
                    }
                    if (accounts.Count > 2)
                    {
                        data = GetValueOfAccount(accounts[2], entries[i].Item1.EntryName);
                        if (data.Length < 15)
                            sbA3.Append(data + new string(' ', 15 - data.Length));
                        else
                            sbA3.Append(data.Substring(0, 15));
                        sbA3.Append(' ');
                    }
                }
            }
            if (accounts.Count > 1)
                sbA1.Append(Environment.NewLine + sbA2.ToString());
            if (accounts.Count > 2)
                sbA1.Append(Environment.NewLine + sbA3.ToString());
            lOutputHeadlines.Text = sb.ToString();
            lOutputValues.Text = sbA1.ToString();

            lOutputValues.Visible =
            btnSave.Visible =
            btnSave.Enabled = true;
            btnClose.Top = 333;
        }

        private string GetValueOfAccount(MK_EAM_Lib.AccountInfo acc, string miniName)
        {
            switch (miniName)
            {
                case "Email":
                    return acc.email;
                case "Password":
                    return acc.password;
                case "Accountname":
                    return acc.name;
                case "Group":
                    return "#" + acc.Color.R.ToString("X2") + acc.Color.G.ToString("X2") + acc.Color.B.ToString("X2");
                default:
                    return string.Empty;
            }
        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            btnSave.Image = Properties.Resources.save_36px;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.Image = Properties.Resources.save_Outline_36px;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (accounts.Count > 0)
            {
                eleImport.AddAccountsToDatagrid(accounts);
                eleImport.CloseEleCustomImport();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            eleImport.CloseEleCustomImport(true);
        }
    }
}
