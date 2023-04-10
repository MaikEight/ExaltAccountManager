using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleCustomExport : UserControl
    {
        private FrmMain frm;
        private EleExport eleExport;

        private Mini.MiniAccountDataOrderEntry miniEmail;
        private Mini.MiniAccountDataOrderEntry miniPassword;
        private Mini.MiniAccountDataOrderEntry miniAccountname;
        private Mini.MiniAccountDataOrderEntry miniGroup;

        private Mini.MiniAccountDataOrderEntry SelectedMini
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

        private int miniAmount = 2;
        private bool isInit = true;

        private List<MK_EAM_Lib.AccountInfo> accounts = new List<MK_EAM_Lib.AccountInfo>();

        public EleCustomExport(FrmMain _frm, EleExport _eleExport, List<MK_EAM_Lib.AccountInfo> _accounts)
        {
            InitializeComponent();

            frm = _frm;
            eleExport = _eleExport;
            accounts = _accounts;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

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

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);


            tbValueSeperationChar.ResetColors();
            tbOutput.ResetColors();

            this.BackColor =
            tbValueSeperationChar.BackColor = tbValueSeperationChar.FillColor = tbValueSeperationChar.OnIdleState.FillColor = tbValueSeperationChar.OnHoverState.FillColor = tbValueSeperationChar.OnActiveState.FillColor = tbValueSeperationChar.OnDisabledState.FillColor =
            tbOutput.BackColor = tbOutput.FillColor = tbOutput.OnIdleState.FillColor = tbOutput.OnHoverState.FillColor = tbOutput.OnActiveState.FillColor = tbOutput.OnDisabledState.FillColor =
            second;

            this.ForeColor =
            tbValueSeperationChar.ForeColor = tbValueSeperationChar.OnActiveState.ForeColor = tbValueSeperationChar.OnDisabledState.ForeColor = tbValueSeperationChar.OnHoverState.ForeColor = tbValueSeperationChar.OnIdleState.ForeColor =
            tbOutput.ForeColor = tbOutput.OnActiveState.ForeColor = tbOutput.OnDisabledState.ForeColor = tbOutput.OnHoverState.ForeColor = tbOutput.OnIdleState.ForeColor =
            font;

            miniEmail.UpdateUI();
            miniPassword.UpdateUI();
            miniAccountname.UpdateUI();
            miniGroup.UpdateUI();

            toolTip.BackColor = def;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = frm.UseDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);
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

        private void toggleEmail_CheckedChanged(object sender, EventArgs e)
        {
            miniEmail.Visible = toggleEmail.Checked;
            miniAmount += toggleEmail.Checked ? 1 : -1;

            UpdateMiniIndices();
        }

        private void togglePassword_CheckedChanged(object sender, EventArgs e)
        {
            miniPassword.Visible = togglePassword.Checked;
            miniAmount += togglePassword.Checked ? 1 : -1;

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

            for (int i = 0; i < entries.Count; i++)
            {
                flowData.Controls.SetChildIndex(entries[i].Item1, i);
            }

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

        private void btnTestExport_Click(object sender, EventArgs e)
        {
            AddAccountsToTestUI();
        }

        private void AddAccountsToTestUI()
        {
            if (accounts.Count == 0) return;

            lOutputHeadlines.Text = string.Empty;
            tbOutput.Text = GetExportString(3);
            tbOutput.Visible = true;

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

        private string GetExportString(int amount)
        {
            List<Tuple<Mini.MiniAccountDataOrderEntry, int>> entries = new List<Tuple<Mini.MiniAccountDataOrderEntry, int>>()
            {
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniEmail, GetMiniIndex(miniEmail)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniPassword, GetMiniIndex(miniPassword)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniAccountname, GetMiniIndex(miniAccountname)),
                new Tuple<Mini.MiniAccountDataOrderEntry, int>(miniGroup, GetMiniIndex(miniGroup)),
            }.OrderByDescending(e => e.Item1.Visible).ThenBy(e => e.Item2).ToList();

            StringBuilder sb = new StringBuilder();

            if (amount > accounts.Count)
                amount = accounts.Count;

            for (int x = 0; x < amount; x++)
            {
                for (int i = 0; i < entries.Count; i++)
                    if (entries[i].Item1.Visible)
                        sb.Append(GetValueOfAccount(accounts[x], entries[i].Item1.EntryName) + tbValueSeperationChar.Text);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (accounts.Count > 0)
            {
                eleExport.WriteStringToFile(GetExportString(accounts.Count));
                eleExport.CloseEleCustomImport();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            eleExport.CloseEleCustomImport();
        }
    }
}
