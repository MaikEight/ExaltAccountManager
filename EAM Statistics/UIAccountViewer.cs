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
    public partial class UIAccountViewer : UserControl
    {
        StatsMain stats;
        FrmMain form;
        UIAccountView view;

        List<CharacterStats> charStats;

        FrmShadowHost shadow;
        FrmCharHost charHost;
        FrmStatsLeftHost statsHost;
        UICharacterViewer characterViewer;
        UIStatsLeft uiStats;

        public UIAccountViewer(UIAccountView _view, FrmMain _form, StatsMain _stats)
        {
            InitializeComponent();
            form = _form;
            view = _view;

            LoadUI(_stats);

            ApplyTheme(form.useDarkmode);
        }

        public void LoadUI(StatsMain _stats)
        {
            stats = _stats;

            #region ACCOUNT

            try
            {
                bool found = false;
                for (int i = 0; i < form.accounts.Count; i++)
                {
                    if (form.accounts[i].email.Equals(stats.email))
                    {
                        lAmountName.Text = form.accounts[i].name;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    lAmountName.Text = stats.email;

                lAccountEmail.Text = stats.email;
            }
            catch { }

            #endregion

            #region Data actuality

            try
            {
                lDataActualityValue.Text = $"{stats.charList[stats.charList.Count - 1].date.ToShortDateString()} {stats.charList[stats.charList.Count - 1].date.ToShortTimeString()}";
            }
            catch { }

            #endregion

            #region mtpCharacters

            try
            {
                //string formatString = "{0,-15}{1,-3}";
                string formatString = "{0}{1}";
                charStats = stats.charList[stats.charList.Count - 1].chars.OrderByDescending(o => o.fame).ToList();
                List<string> names = new List<string>();
                List<string> values = new List<string>();

                for (int i = 0; i < charStats.Count; i++)
                {
                    charStats[i].CalculateXof8();
                    names.Add(string.Format(formatString, $"{charStats[i].charClass.ToString()}", $"{charStats[i].xOf8}/8"));
                    values.Add(charStats[i].fame.ToString());
                }

                //names.Insert(0, string.Format(formatString, "Class", "x/8"));
                names.Insert(0, "Class");
                values.Insert(0, "Fame");

                mtpCharacters.Init("Characters", "Click for more infos", names.ToArray(), values.ToArray(), true);
                mtpCharacters.Size = new Size(200, 410);
                mtpCharacters.RowClicked += CharacterClicked;
            }
            catch (Exception e){ string stack = e.StackTrace; }

            #endregion

            #region Total fame

            try
            {
                lTotalFameValue.Text = string.Format(lTotalFameValue.Text, stats.stats[stats.stats.Count - 1].totalFame);
            }
            catch { }

            #endregion

            #region Alive fame

            try
            {
                int aliveFame = 0;
                for (int i = 0; i < stats.charList[form.statsList[form.statsList.Count - 1].charList.Count - 1].chars.Count; i++)
                {
                    aliveFame += stats.charList[form.statsList[form.statsList.Count - 1].charList.Count - 1].chars[i].fame;
                }
                lAliveFameValue.Text = string.Format(lAliveFameValue.Text, aliveFame);
            }
            catch { }

            #endregion

            #region Best class

            try
            {
                List<CharacterClass> charStats = stats.stats[stats.stats.Count - 1].classes.OrderByDescending(o => o.bestFame).ToList();

                lBestClassName.Text = charStats[0].charClass.ToString();
                int index = form.charNames.IndexOf(lBestClassName.Text);
                if (index > -1 && form.charImages.Count > index)
                    pbBestClassImage.Image = form.charImages[index];

                lBestFameValue.Text = charStats[0].bestFame.ToString();
                lBestLevelValue.Text = charStats[0].bestLevel.ToString();
            }
            catch { }

            #endregion

            #region Radar Char

            try
            {
                List<int> values = new List<int>();
                List<string> valueNames = new List<string>();
                List<Image> valueImgs = new List<Image>();

                for (int i = 0; i < stats.stats[stats.stats.Count - 1].classes.Count; i++)
                {
                    values.Add(stats.stats[stats.stats.Count - 1].classes[i].bestFame);
                    valueNames.Add(stats.stats[stats.stats.Count - 1].classes[i].charClass.ToString());
                    valueImgs.Add(form.charImages[(int)stats.stats[stats.stats.Count - 1].classes[i].charClass]);
                }

                mtpRadarChars.SetLabels("Radar chart: Best classes", lAmountName.Text);
                mtpRadarChars.DrawValues(values.ToArray(), valueImgs.ToArray(), 24, valueNames.ToArray(), true);
            }
            catch { }

            #endregion

            pbReturn.Location = new Point((this.Width - pbReturn.Width) - 15, (this.Height - pbReturn.Height) - 15);
            this.Controls.SetChildIndex(mtpRadarChars, 5);
            this.Controls.SetChildIndex(pbReturn, 0);
        }

        public void CharacterClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < charStats.Count)
                {
                    shadow = new FrmShadowHost(this);
                    shadow.StartPosition = FormStartPosition.Manual;
                    shadow.Location = new Point(form.Location.X + 175, form.Location.Y + 24);
                    shadow.Show(form);

                    characterViewer = new UICharacterViewer(this, charStats[e.RowIndex]);
                    charHost = new FrmCharHost(this, characterViewer);
                    charHost.StartPosition = FormStartPosition.Manual;
                    charHost.Location = new Point(shadow.Location.X + ((shadow.Width - charHost.Width) / 2), shadow.Location.Y + ((shadow.Height - charHost.Height) / 2));
                    charHost.Show(shadow);

                    form.SetTitleText($"Accounts > {lAmountName.Text} > {charStats[e.RowIndex].charClass}");
                }
            }
            catch { }
        }

        public void CloseCharacterForms()
        {
            form.SetTitleText($"Accounts > {lAmountName.Text}");
            view.CloseCharacterForms();
            shadow = null;
            charHost = null;
            statsHost = null;
            uiStats = null;
            characterViewer = null;
        }

        public void CloseStatsForms()
        {
            if (statsHost != null)            
                statsHost.Close();                
            
            if (shadow != null && charHost != null)
                charHost.Location = new Point(shadow.Location.X + ((shadow.Width - charHost.Width) / 2), shadow.Location.Y + ((shadow.Height - charHost.Height) / 2));

            statsHost = null;
            uiStats = null;
        }

        public void OpenStatsForms(CharacterStats chara)
        {
            if (shadow != null && charHost != null)
            {
                uiStats = new UIStatsLeft(this, chara);
                statsHost = new FrmStatsLeftHost(this, uiStats);
                charHost.Location = new Point(shadow.Location.X + ((shadow.Width - (charHost.Width + statsHost.Width + 10)) / 2), shadow.Location.Y + ((shadow.Height - charHost.Height) / 2));
                statsHost.StartPosition = FormStartPosition.Manual;
                statsHost.Location = new Point(charHost.Location.X + charHost.Width + 10, charHost.Location.Y);
                statsHost.Show(shadow);
            }
        }

        public Image GetCharacterImage(CharClasses c)
        {
            int index = form.charNames.IndexOf(c.ToString());
            if (index > -1 && form.charImages.Count > index)
                return form.charImages[index];
            return null;
        }

        public bool GetDarkmode() => form.useDarkmode;
        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            this.Visible = false;
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;

            this.ForeColor = font;

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

            if (shadow != null)            
                shadow.ApplyTheme(isDarkmode);
            if (characterViewer != null)
                characterViewer.ApplyTheme(isDarkmode);
            if (statsHost != null)
                statsHost.ApplyTheme(isDarkmode);         
            if (uiStats != null)
                uiStats.ApplyTheme(isDarkmode);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
            this.Visible = true;
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

        private void pbReturn_Click(object sender, EventArgs e)
        {
            view.CloseAccountViewer();
        }

        private void pbReturn_MouseEnter(object sender, EventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(128, 98, 0, 238);
        }

        private void pbReturn_MouseLeave(object sender, EventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(175, 98, 0, 238);
        }        

        private void pbReturn_MouseDown(object sender, MouseEventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(225, 98, 0, 238);
        }

        private void pbReturn_MouseUp(object sender, MouseEventArgs e)
        {
            pbReturn.BackColor = Color.FromArgb(128, 98, 0, 238);
        }
    }
}
