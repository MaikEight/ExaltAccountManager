using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleCredits : UserControl
    {
        private const string licenseDotNet = "https://dotnet.microsoft.com/en-us/platform/free";
        private const string licenseNewtonsoftJson = "Newtonsoft.Json.License";
        private const string licenseBunifu = "https://bunifuframework.com/";
        private const string licenseFody = "Fody.License";
        private const string licenseCostura = "Costura.License";
        private const string licenseCsvHelper = "CsvHelper.License";
        private const string licenseTaskScheduler = "TaskScheduler.License";
        private const string licenseWixSharp = "wixsharp.License";
        private const string licenseDiscordRPC = "discord-rpc-csharp.License";

        private const string resIcons8 = "https://icons8.com/";
        private const string resDeca = "https://decagames.com/";
        private const string resRotmg = "https://www.realmofthemadgod.com/";
        private const string resMuledump = "https://github.com/BR-/muledump";

        private FrmMain frm;

        public EleCredits(FrmMain frm)
        {
            InitializeComponent();
            this.frm = frm;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);

            scrollbar.Height = pMain.Height;
            scrollbar.BindTo(pMain);
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            scrollbar.BorderColor = frm.UseDarkmode ? second : Color.Silver;
            scrollbar.BackgroundColor = frm.UseDarkmode ? def : third;
            scrollbar.ThumbColor = frm.UseDarkmode ? third : Color.Gray;
        }

        public void ResetScrollbar()
        {
            scrollbar.Value = scrollbar.Minimum;
            scrollbar.BindTo(pMain);
        }

        private void lDotNet_Click(object sender, EventArgs e)
        {
            Process.Start(licenseDotNet);
        }

        private void lNewtonsoftJson_Click(object sender, EventArgs e)
        {
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseNewtonsoftJson));
        }

        private void lBunifu_Click(object sender, EventArgs e)
        {
            Process.Start(licenseBunifu);
        }

        private void lFody_Click(object sender, EventArgs e)
        {
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseFody));
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseCostura));
        }

        private void lCsvHelper_Click(object sender, EventArgs e)
        {
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseCsvHelper));
        }

        private void lTaskScheduler_Click(object sender, EventArgs e)
        {
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseTaskScheduler));
        }

        private void lWixSharp_Click(object sender, EventArgs e)
        {
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseWixSharp));
        }

        private void lDiscordrichPresence_Click(object sender, EventArgs e)
        {
            OpenLicense(Path.Combine(Application.StartupPath, "Credits", licenseDiscordRPC));
        }

        private void OpenLicense(string license)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "notepad.exe",
                Arguments = license
            });
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e) => frm.RemoveShadowForm();

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_18dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = frm.UseDarkmode ? ColorScheme.GetColorSecond(frm.UseDarkmode) : ColorScheme.GetColorThird(frm.UseDarkmode);
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;

        #endregion

        private void lIcons8_Click(object sender, EventArgs e)
        {
            Process.Start(resIcons8);
        }

        private void lDeca_Click(object sender, EventArgs e)
        {
            Process.Start(resDeca);
        }

        private void pbRotmg_Click(object sender, EventArgs e)
        {
            Process.Start(resRotmg);
        }

        private void lMuledump_Click(object sender, EventArgs e)
        {
            Process.Start(resMuledump);
        }
    }
}
