using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmSetup : Form
    {
        FrmMain frm;
        private bool isInit = true;

        public FrmSetup(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            toggleUseDarkmode.Checked = frm.useDarkmode;

            ApplyTheme(frm.useDarkmode);

            isInit = false;
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

                pbLogo.Image = Properties.Resources.maintenance_white_48px;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                btnYes.Image = Properties.Resources.ic_folder_open_white_18dp;
                btnNo.Image = Properties.Resources.new_Outline_white_18px;
                btnLoadSaveFile.Image = Properties.Resources.ic_import_export_white_24dp;
                btnYes.FlatAppearance.MouseOverBackColor = btnNo.FlatAppearance.MouseOverBackColor = btnLoadSaveFile.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);

                p = new Pen(Color.White);
            }
            else
            {
                pbLogo.Image = Properties.Resources.maintenance_black_48px;
                pbClose.Image = Properties.Resources.ic_close_black_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_black_24dp;
                btnYes.Image = Properties.Resources.ic_folder_open_black_18dp;
                btnLoadSaveFile.Image = Properties.Resources.ic_import_export_black_24dp;
                btnNo.Image = Properties.Resources.new_Outline_black_18px;

                btnYes.FlatAppearance.MouseOverBackColor = btnNo.FlatAppearance.MouseOverBackColor = btnLoadSaveFile.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 50, 50, 50);

                p = new Pen(Color.Black);
            }

            this.ForeColor = font;
            this.BackColor = second;
            pTop.BackColor = def;
            pBox.BackColor = def;

            toggleUseDarkmode.ForeColor = font;
            toggleUseDarkmode.ToggleStateOn.BorderColor =
            toggleUseDarkmode.ToggleStateOn.BackColor = font;

            toggleUseDarkmode.ToggleStateOn.BorderColorInner =
            toggleUseDarkmode.ToggleStateOn.BackColorInner = def;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            string basePath = string.Empty;
            try
            {
                OpenFileDialog diag = new OpenFileDialog();
                diag.Title = "Select any file in the old EAM folder.";
                diag.Filter = "All files (*.*)|*.*";
                diag.InitialDirectory = Application.StartupPath;
                diag.Multiselect = false;

                if (diag.ShowDialog() == DialogResult.OK)
                {
                    basePath = Path.GetDirectoryName(diag.FileName);
                    foreach (string f in Directory.GetFiles(basePath))
                    {
                        if (Path.GetFileName(f).StartsWith("EAM."))
                        {
                            try
                            {
                                switch (Path.GetFileName(f))
                                {
                                    case "EAM.accounts":
                                        try
                                        {
                                            string p = Path.Combine(basePath, "EAM.accountOrders");
                                            if (File.Exists(p))
                                            {
                                                frm.SetAccountOrders((AccountOrders)frm.ByteArrayToObject(File.ReadAllBytes(p)));
                                                frm.SaveAccountOrders();
                                            }

                                            frm.LoadAccountInfos(f, true);
                                        }
                                        catch { }
                                        break;
                                    case "EAM.options":
                                        try
                                        {
                                            OptionsData opt = (OptionsData)frm.ByteArrayToObject(File.ReadAllBytes(f));
                                            frm.exePath = opt.exePath;
                                            frm.closeAfterConnect = opt.closeAfterConnection;

                                            frm.useDarkmode = !opt.useDarkmode;
                                            frm.SwitchDesign();
                                            toggleUseDarkmode.Checked = frm.useDarkmode;
                                        }
                                        catch { }
                                        break;
                                    case "EAM.NotificationOptions":
                                        try
                                        {
                                            frm.notOpt = (NotificationOptions)frm.ByteArrayToObject(File.ReadAllBytes(f));
                                        }
                                        catch { }
                                        break;
                                    case "EAM.ServerFavorites":
                                        try
                                        {
                                            if (File.Exists(Path.Combine(basePath, "EAM.ServerFavorites")))
                                                File.Delete(Path.Combine(basePath, "EAM.ServerFavorites"));
                                            File.Copy(f, Path.Combine(basePath, "EAM.ServerFavorites"));
                                        }
                                        catch { }
                                        break;
                                    case "EAM.PingSaveFile":
                                        try
                                        {
                                            if (File.Exists(Path.Combine(basePath, "EAM.PingSaveFile")))
                                                File.Delete(Path.Combine(basePath, "EAM.PingSaveFile"));
                                            File.Copy(f, Path.Combine(basePath, "EAM.PingSaveFile"));
                                        }
                                        catch { }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch { }
                        }
                    }
                }
                else
                    return;
            }
            catch { }

            try
            {
                if (!File.Exists(frm.optionsPath))
                {
                    OptionsData opt = new OptionsData()
                    {
                        exePath = frm.exePath,
                        closeAfterConnection = false,
                        useDarkmode = !frm.useDarkmode
                    };
                    File.WriteAllBytes(frm.optionsPath, frm.ObjectToByteArray(opt));
                }
            }
            catch { }
            try
            {
                if (!File.Exists(frm.accountOrdersPath))
                {
                    if (frm.accountOrders == null)
                        frm.accountOrders = new AccountOrders() { orderData = new List<OrderData>() };
                    frm.SaveAccountOrders();
                }
            }
            catch { }
            try
            {
                if (!File.Exists(frm.notificationOptionsPath))
                {
                    if (frm.notOpt == null)
                        frm.notOpt = new NotificationOptions();
                    frm.SaveNotificationoptions(frm.notOpt);
                }
            }
            catch { }

            try
            {
                if (!Directory.Exists(frm.accountStatsPath))
                    Directory.CreateDirectory(frm.accountStatsPath);

                if (string.IsNullOrEmpty(basePath)) return;

                foreach (string f in Directory.GetFiles(Path.Combine(basePath, "Stats")))
                    File.Copy(f, Path.Combine(frm.accountStatsPath, Path.GetFileName(f)));
            }
            catch { }

            frm.lockForm = false;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            frm.useDarkmode = !frm.useDarkmode;

            try
            {
                if (!File.Exists(frm.optionsPath))
                {
                    OptionsData opt = new OptionsData()
                    {
                        exePath = frm.exePath,
                        closeAfterConnection = false,
                        useDarkmode = !frm.useDarkmode
                    };
                    File.WriteAllBytes(frm.optionsPath, frm.ObjectToByteArray(opt));
                }
            }
            catch { }
            try
            {
                if (!File.Exists(frm.accountOrdersPath))
                {
                    if (frm.accountOrders == null)
                        frm.accountOrders = new AccountOrders() { orderData = new List<OrderData>() };
                    frm.SaveAccountOrders();
                }
            }
            catch { }
            try
            {
                if (!File.Exists(frm.notificationOptionsPath))
                {
                    if (frm.notOpt == null)
                        frm.notOpt = new NotificationOptions();
                    frm.SaveNotificationoptions(frm.notOpt);
                }
            }
            catch { }

            FormsUtils.SuspendDrawing(frm);
            FormsUtils.SuspendDrawing(this);
            FrmAddAccount frmAddAccount = new FrmAddAccount(frm, true);
            frmAddAccount.StartPosition = FormStartPosition.Manual;
            frmAddAccount.Location = new Point(frm.Location.X + ((frm.Width - frmAddAccount.Width) / 2), frm.Location.Y + ((frm.Height - frmAddAccount.Height) / 2));
            frmAddAccount.ApplyTheme(toggleUseDarkmode.Checked);
            this.Visible = false;
            frmAddAccount.Show(frm);
            FormsUtils.ResumeDrawing(frm);
            FormsUtils.ResumeDrawing(this);

            frm.useDarkmode = !frm.useDarkmode;
            frm.lockForm = true;

            this.Close();
        }

        private void toggleUseDarkmode_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (isInit) return;

            frm.useDarkmode = !toggleUseDarkmode.Checked;
            frm.SwitchDesign();

            ApplyTheme(frm.useDarkmode);
        }

        Pen p = new Pen(Color.Black);
        private void FrmSetup_Paint(object sender, PaintEventArgs e)
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

        private void pTop_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
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

        private void btnYes_MouseEnter(object sender, EventArgs e)
        {
            btnYes.Image = frm.useDarkmode ? Properties.Resources.ic_folder_white_18dp : Properties.Resources.ic_folder_black_18dp;
        }

        private void btnYes_MouseLeave(object sender, EventArgs e)
        {
            btnYes.Image = frm.useDarkmode ? Properties.Resources.ic_folder_open_white_18dp : Properties.Resources.ic_folder_open_black_18dp;
        }

        private void btnNo_MouseEnter(object sender, EventArgs e)
        {
            btnNo.Image = frm.useDarkmode ? Properties.Resources.ic_fiber_new_white_18dp : Properties.Resources.ic_fiber_new_black_18dp;
        }

        private void btnNo_MouseLeave(object sender, EventArgs e)
        {
            btnNo.Image = frm.useDarkmode ? Properties.Resources.new_Outline_white_18px : Properties.Resources.new_Outline_black_18px;
        }

        private void btnLoadSaveFile_Click(object sender, EventArgs e)
        {
            frm.useDarkmode = !frm.useDarkmode;

            try
            {
                if (!File.Exists(frm.optionsPath))
                {
                    OptionsData opt = new OptionsData()
                    {
                        exePath = frm.exePath,
                        closeAfterConnection = false,
                        useDarkmode = !frm.useDarkmode
                    };
                    File.WriteAllBytes(frm.optionsPath, frm.ObjectToByteArray(opt));
                }
            }
            catch { }
            try
            {
                if (!File.Exists(frm.accountOrdersPath))
                {
                    if (frm.accountOrders == null)
                        frm.accountOrders = new AccountOrders() { orderData = new List<OrderData>() };
                    frm.SaveAccountOrders();
                }
            }
            catch { }
            try
            {
                if (!File.Exists(frm.notificationOptionsPath))
                {
                    if (frm.notOpt == null)
                        frm.notOpt = new NotificationOptions();
                    frm.SaveNotificationoptions(frm.notOpt);
                }
            }
            catch { }

            frm.useDarkmode = !frm.useDarkmode;

            FormsUtils.SuspendDrawing(frm);
            FormsUtils.SuspendDrawing(this);
            FrmImExport frmImExport = new FrmImExport(frm);
            frmImExport.StartPosition = FormStartPosition.Manual;
            frmImExport.Location = new Point(frm.Location.X + ((frm.Width - frmImExport.Width) / 2), frm.Location.Y + ((frm.Height - frmImExport.Height) / 2));
            this.Visible = false;
            frmImExport.Show(frm);
            FormsUtils.ResumeDrawing(frm);
            FormsUtils.ResumeDrawing(this);

            frm.lockForm = true;

            this.Close();
        }
    }
}
