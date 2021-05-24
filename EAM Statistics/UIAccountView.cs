using EAM_Statistics.MatPanel;
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

namespace EAM_Statistics
{
    public partial class UIAccountView : UserControl
    {
        FrmMain form;
        UIAccountViewer accountViewer = null;

        public UIAccountView(FrmMain _form)
        {
            InitializeComponent();

            form = _form;

            ApplyTheme(form.useDarkmode);

            LoadUI();
        }

        private void LoadUI()
        {
            List<StatsMain> statsList = new List<StatsMain>();
            statsList.AddRange(form.statsList);
            statsList = statsList.OrderByDescending(o => o.stats.Count > 0 ? o.stats[o.stats.Count - 1].totalFame : -1).ToList();
            
            bool needScrollbar = false;
            for (int i = 0; i < statsList.Count; i++)
            {
                MaterialAccountViewInfo info = new MaterialAccountViewInfo(this, statsList[i], i);
                flow.Controls.Add(info);

                if (!needScrollbar == info.Bottom > flow.Height)
                    needScrollbar = true;
            }
            if (needScrollbar)
            {
                scrollbar.BindTo(flow);
                scrollbar.Visible = true;
            }
        }

        public bool GetDarkmode() => form.useDarkmode;

        public string GetAccountName(string mail)
        {
            for (int i = 0; i < form.accounts.Count; i++)
            {
                if (form.accounts[i].email.Equals(mail))
                {
                    return form.accounts[i].name;
                }
            }

            return mail;
        }

        public void OpenAccountViewer(StatsMain stats)
        {
            CloseAccountViewer();

            accountViewer = new UIAccountViewer(this, form, stats);
            this.Controls.Add(accountViewer);
            this.Controls.SetChildIndex(accountViewer, 0);

            string name = stats.email;

            for (int i = 0; i < form.accounts.Count; i++)
            {
                if (name.Equals(form.accounts[i].email))
                {
                    name = form.accounts[i].name;
                    break;
                }
            }
            form.SetTitleText($"Accounts > {name}");
        }

        public void CloseAccountViewer()
        {
            if (accountViewer != null)
            {
                if (this.Controls.Contains(accountViewer))
                    this.Controls.Remove(accountViewer);

                accountViewer.CloseCharacterForms();

                accountViewer.Dispose();
                accountViewer = null;

                form.SetTitleText("Accounts");
            }
        }

        public void CloseCharacterForms() => form.CloseCharacterForms();

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;
            this.ForeColor = font;

            scrollbar.BackgroundColor = scrollbar.BorderColor = third;
            scrollbar.ThumbColor = isDarkmode ? Color.FromArgb(128, 128, 128, 128) : Color.FromArgb(128, 50, 50, 50);

            if (accountViewer != null) accountViewer.ApplyTheme(isDarkmode);

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
            foreach (MaterialAccountViewInfo ui in flow.Controls.OfType<MaterialAccountViewInfo>())
                ui.ApplyTheme(isDarkmode);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }


        public void ApplyTheme(bool isDarkmode)
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (isDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            ApplyTheme(isDarkmode, def, second, third, font);
        }
    }
}
