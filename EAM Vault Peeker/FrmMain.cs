using EAM_Vault_Peeker.UI;
using MK_EAM_General_Services_Lib;
using MK_EAM_General_Services_Lib.General.Responses;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Vault_Peeker
{
    public partial class FrmMain : Form
    {
        public Version version { get; } = new Version(1, 0, 10);
        public string API_BASE_URL { get; internal set; } = "https://api.exalt-account-manager.eu/";

        public bool useDarkmode = true;
        public Image renders = null;

        public event EventHandler ThemeChanged;

        public List<Item> items = new List<Item>();

        public ItemsSaveFile itemsSaveFile = null;

        private FrmItemPreview frmItemPreview = new FrmItemPreview();

        public AccountsUI accountsUI;
        public TotalsUI totalsUI;
        public AboutUI aboutUI;

        private UIState uiState = UIState.Accounts;

        public List<StatsMain> statsList = new List<StatsMain>();
        public List<string> activeVaultPeekerAccounts = new List<string>();

        private bool didCheckForUpdate = false;

        #region Path

        public static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        private readonly string pathNEWRenders = Path.Combine(saveFilePath, "_NewRenders.png");
        private readonly string pathRenders = Path.Combine(saveFilePath, "renders.png");
        private readonly string pathItems = Path.Combine(saveFilePath, "items.cfg");
        private readonly string pathNEWItems = Path.Combine(saveFilePath, "_NewItems.cfg");
        public string itemsSaveFilePath = Path.Combine(saveFilePath, "EAM.ItemsSaveFile");
        public string accountStatsPath = Path.Combine(saveFilePath, "Stats");
        public string activeVaultPeekerAccountsPath = Path.Combine(saveFilePath, "EAM.ActiveVaultPeekerAccounts");

        #endregion

        #region Form Resize

        ////Fields
        private int borderSize = 2;
        private Size formSize; //Keep form size when it is minimized and restored.Since the form is resized because it takes into account the size of the title bar and borders.

        ////Drag Form
        //[DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        //private extern static void ReleaseCapture();
        //[DllImport("user32.DLL", EntryPoint = "SendMessage")]
        //private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //DateTime lastPTopMouseDown = DateTime.Now;
        //private void pTop_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if ((DateTime.Now - lastPTopMouseDown).TotalMilliseconds <= 200)
        //    {
        //        pbMaximize_Click(pbMaximize, new EventArgs());

        //        lastPTopMouseDown = DateTime.Now;
        //        return;
        //    }

        //    lastPTopMouseDown = DateTime.Now;
        //    ReleaseCapture();
        //    SendMessage(this.Handle, 0x112, 0xf012, 0);
        //}

        //Overridden methods
        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
        //    const int WM_SYSCOMMAND = 0x0112;
        //    const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
        //    const int SC_RESTORE = 0xF120; //Restore form (Before)
        //    const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
        //    const int resizeAreaSize = 10;

        //    #region Form Resize

        //    // Resize/WM_NCHITTEST values
        //    const int HTCLIENT = 1; //Represents the client area of the window
        //    const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
        //    const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
        //    const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
        //    const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
        //    const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
        //    const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
        //    const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
        //    const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right
        //    ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>
        //    if (m.Msg == WM_NCHITTEST)
        //    { //If the windows m is WM_NCHITTEST
        //        base.WndProc(ref m);
        //        if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
        //        {
        //            if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
        //            {
        //                Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
        //                Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          
        //                if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
        //                {
        //                    if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
        //                        m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
        //                    else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
        //                        m.Result = (IntPtr)HTTOP; //Resize vertically up
        //                    else //Resize diagonally to the right
        //                        m.Result = (IntPtr)HTTOPRIGHT;
        //                }
        //                else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
        //                {
        //                    if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
        //                        m.Result = (IntPtr)HTLEFT;
        //                    else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
        //                        m.Result = (IntPtr)HTRIGHT;
        //                }
        //                else
        //                {
        //                    if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
        //                        m.Result = (IntPtr)HTBOTTOMLEFT;
        //                    else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
        //                        m.Result = (IntPtr)HTBOTTOM;
        //                    else //Resize diagonally to the right
        //                        m.Result = (IntPtr)HTBOTTOMRIGHT;
        //                }
        //            }
        //        }
        //        return;
        //    }

        //    //Remove border and keep snap window ################### BROKEN
        //    //if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
        //    //{
        //    //    return;
        //    //}


        //    //Keep form size when it is minimized and restored. Since the form is resized because it takes into account the size of the title bar and borders.
        //    if (m.Msg == WM_SYSCOMMAND)
        //    {
        //        /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
        //        /// Quote:
        //        /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
        //        /// are used internally by the system.To obtain the correct result when testing 
        //        /// the value of wParam, an application must combine the value 0xFFF0 with the 
        //        /// wParam value by using the bitwise AND operator.
        //        int wParam = (m.WParam.ToInt32() & 0xFFF0);
        //        if (wParam == SC_MINIMIZE)  //Before
        //            formSize = this.ClientSize;
        //        if (wParam == SC_RESTORE)// Restored form(Before)
        //            this.Size = formSize;
        //    }
        //    base.WndProc(ref m);
        //}

        //#endregion

        ////Private methods
        //private void AdjustForm()
        //{
        //    switch (this.WindowState)
        //    {
        //        case FormWindowState.Maximized: //Maximized form (After)
        //            this.Padding = new Padding(0, 0, 0, 0);
        //            break;
        //        case FormWindowState.Normal: //Restored form (After)
        //            if (this.Padding.Top != borderSize)
        //                this.Padding = new Padding(borderSize);
        //            accountsUI.isInit = false;
        //            break;
        //    }
        //}

        //private void Frm_Resize(object sender, EventArgs e) => AdjustForm();

        //#region Button Close / Minimize

        //private void pbMaximize_Click(object sender, EventArgs e)
        //{
        //    if (this.WindowState == FormWindowState.Normal)
        //    {
        //        formSize = this.ClientSize;
        //        this.WindowState = FormWindowState.Maximized;

        //        pbMaximize.Image = useDarkmode ? Properties.Resources.ic_filter_none_white_24dp : Properties.Resources.ic_filter_none_black_24dp;
        //    }
        //    else
        //    {
        //        this.WindowState = FormWindowState.Normal;
        //        this.Size = formSize;

        //        pbMaximize.Image = useDarkmode ? Properties.Resources.ic_crop_square_white_24dp : Properties.Resources.ic_crop_square_black_24dp;
        //    }
        //}

        //private void pbMinimize_Click(object sender, EventArgs e)
        //{
        //    formSize = this.ClientSize;
        //    accountsUI.isInit = true;
        //    this.WindowState = FormWindowState.Minimized; 
        //}

        //private void pbClose_Click(object sender, EventArgs e) => this.Close();
        //private void pbClose_MouseEnter(object sender, EventArgs e) => pbClose.BackColor = useDarkmode ? Color.FromArgb(225, 225, 50, 50) : Color.FromArgb(255, 205, 82, 82);
        //private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = pRight.BackColor;
        //private void pbMinimize_MouseEnter(object sender, EventArgs e) => (sender as PictureBox).BackColor = useDarkmode ? Color.FromArgb(128, 105, 105, 105) : Color.FromArgb(128, 169, 169, 169);
        //private void pbMinimize_MouseLeave(object sender, EventArgs e) => (sender as PictureBox).BackColor = pRight.BackColor;

        //#endregion

        #endregion

        #region Borderless Form Minimize On Taskbar Icon Click

        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }

        #endregion

        public FrmMain()
        {
            InitializeComponent();

            //formSize = this.Size;
            // this.Padding = new Padding(borderSize);         //Border size
            this.BackColor = Color.FromArgb(112, 18, 217);  //trannsparent color


            #region API Base URL

            try
            {
                API_BASE_URL = "https://api.exalt-account-manager.eu/";
                string fileName = Path.Combine(Application.StartupPath, "MK_EAM_API_DATA");
                if (File.Exists(fileName))
                    API_BASE_URL = File.ReadAllText(fileName);
            }
            catch { API_BASE_URL = "https://api.exalt-account-manager.eu/"; }

            #endregion

            _ = new GeneralServicesClient(API_BASE_URL);

            ReadItems();

            if (File.Exists(itemsSaveFilePath))
            {
                try
                {
                    itemsSaveFile = (ItemsSaveFile)ByteArrayToObject(File.ReadAllBytes(itemsSaveFilePath));
                }
                catch
                {
                    itemsSaveFile = new ItemsSaveFile();
                }
            }

            ThemeChanged += frmItemPreview.ApplyTheme;

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
            catch { }

            try
            {
                activeVaultPeekerAccounts = (List<string>)ByteArrayToObject(System.IO.File.ReadAllBytes(activeVaultPeekerAccountsPath));
            }
            catch { }

            btnTotals_Click(btnTotals, null);

            ApplyTheme();

            Task.Run(() => CheckForItemUpdate());
        }

        private void CheckForItemUpdate()
        {
            if (didCheckForUpdate)
                return;
            didCheckForUpdate = true;

            try
            {
                Task<MK_EAM_General_Services_Lib.General.Responses.GetVaultPeekerHashOfFilesResponse> result = GeneralServicesClient.Instance?.GetVaultPeekerHashOfFiles();

                GetVaultPeekerHashOfFilesResponse hashOfFiles = result.Result;
                if (string.IsNullOrEmpty(hashOfFiles.rendersPng) || string.IsNullOrEmpty(hashOfFiles.itemsCfg))
                {
                    return;
                }
                bool[] needsUpdate = new bool[] { true, true };

                if (File.Exists(pathRenders))
                {
                    try
                    {
                        string h = GetMD5AsString(pathRenders);
                        needsUpdate[0] = !hashOfFiles.rendersPng.Equals(h);
                    }
                    catch { needsUpdate[0] = true; }
                }

                if (File.Exists(pathItems))
                {
                    try
                    {
                        string h = GetMD5AsString(pathItems);
                        needsUpdate[1] = !hashOfFiles.itemsCfg.Equals(h);
                    }
                    catch { needsUpdate[1] = true; }
                }

                if (needsUpdate.Contains(true))
                {
                    Task<GetFileResponse> pngResponse = GeneralServicesClient.Instance?.GetVaultPeekerRendersPng();
                    Task<GetFileResponse> itemsCfgResponse = GeneralServicesClient.Instance?.GetVaultPeekerItemsConfig();

                    GetFileResponse png = pngResponse.Result;
                    if (png != null && png.data != null)
                    {
                        if (File.Exists(pathNEWRenders))
                            File.Delete(pathNEWRenders);

                        File.WriteAllBytes(pathNEWRenders, png.data);
                    }

                    GetFileResponse items = itemsCfgResponse.Result;
                    if (items != null && items.data != null)
                    {
                        if (File.Exists(pathNEWItems))
                            File.Delete(pathNEWItems);

                        File.WriteAllBytes(pathNEWItems, items.data);
                    }
                }
            }
            catch { }
        }

        private string GetMD5AsString(string filename)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void ReadItems(bool isSecondRun = false)
        {
            bool filesMissing = false;
            try
            {
                if (File.Exists(pathNEWItems))
                {
                    if (File.Exists(pathItems))
                        File.Delete(pathItems);
                    File.Move(pathNEWItems, pathItems);
                }

                if (File.Exists(pathItems))
                {
                    string[] itemsString = File.ReadAllLines(pathItems);

                    for (int i = 0; i < itemsString.Length; i++)
                    {
                        items.Add(new Item(itemsString[i]));
                    }
                } 
                else { filesMissing = true; }

                if (File.Exists(pathNEWRenders))
                {
                    if (File.Exists(pathRenders))
                        File.Delete(pathRenders);
                    File.Move(pathNEWRenders, pathRenders);
                }

                if (File.Exists(pathRenders))
                {
                    renders = Bitmap.FromFile(pathRenders);
                }
                else { filesMissing = true; }
            }
            catch { filesMissing = true; }

            if (!isSecondRun && filesMissing)
            {
                var t = Task.Run(() =>
                {
                    CheckForItemUpdate();
                    ReadItems(true);
                });
                t.Wait();
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            // AdjustForm();
        }

        private void ApplyTheme()
        {
            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            this.ForeColor = font;
            this.BackColor = pContent.BackColor = def;

            pButtons.BackColor = pTop.BackColor = pRight.BackColor = second;

            if (this.ThemeChanged != null)
                this.ThemeChanged(this, new EventArgs());

            pbMinimize.Image = useDarkmode ? Properties.Resources.baseline_minimize_white_24dp : Properties.Resources.baseline_minimize_black_24dp;
            pbMaximize.Image = useDarkmode ? (this.WindowState == FormWindowState.Maximized ? Properties.Resources.ic_filter_none_white_24dp : Properties.Resources.ic_crop_square_white_24dp) : (this.WindowState == FormWindowState.Maximized ? Properties.Resources.ic_filter_none_black_24dp : Properties.Resources.ic_crop_square_black_24dp);
            pbClose.Image = useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
            pbMinimize.BackColor = pbMaximize.BackColor = pbClose.BackColor = pRight.BackColor;

            this.Invalidate();
        }

        public void ToggleItemHighlight(int item, bool state)
        {
            if (uiState == UIState.Accounts)
                accountsUI.ToggleItemHighlight(item, state);
            else
                totalsUI.ToggleItemHighlight(item, state);
        }

        public void ShowItemPreview(object sender, EventArgs e)
        {
            frmItemPreview.Hide();

            ItemUI ui = sender as ItemUI;
            if (frmItemPreview.itemUI == ui)
            {
                frmItemPreview.itemUI = null;
                return;
            }

            frmItemPreview.ChangeItem(ui);
            Point p = ui.GetPointOnScreen();
            if ((p.X + frmItemPreview.Width + ui.Width) > Screen.GetBounds(p).Width)
                p.X -= (frmItemPreview.Width + ui.Width);
            frmItemPreview.Location = p;
            frmItemPreview.Left += ui.Width + 3;
            frmItemPreview.Show(this);
            ui.Focus();
        }

        private void btnSwitchDesign_Click(object sender, EventArgs e)
        {
            useDarkmode = !useDarkmode;
            ApplyTheme();

            lHeaderStatistics.Focus();
        }

        private Point m_PreviousLocation = new Point(int.MinValue, int.MinValue);
        private void FrmMain_LocationChanged(object sender, EventArgs e)
        {
            // All open child forms to be moved
            Form[] formsToAdjust = Application
              .OpenForms
              .OfType<Form>()
              .ToArray();

            // If the main form has been moved...
            if (m_PreviousLocation.X != int.MinValue)
            {
                foreach (var form in formsToAdjust) //... move all child froms aw well
                {
                    if (form == this || form.GetType().Name.Equals("FrmMain")) // Except a few ones that should not
                        continue;
                    form.Location = new Point(
                      form.Location.X + Location.X - m_PreviousLocation.X,
                      form.Location.Y + Location.Y - m_PreviousLocation.Y
                    );
                }
            }
            m_PreviousLocation = Location;
        }

        private void btnAccountView_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Accounts)
            {
                if (accountsUI == null)
                    accountsUI = new AccountsUI(this) { Dock = DockStyle.Fill };

                FormsUtils.SuspendDrawing(pContent);

                pContent.Controls.Clear();
                pContent.Controls.Add(accountsUI);

                pSideBar.Top = btnAccountView.Top + 3;

                uiState = UIState.Accounts;
                accountsUI.ResizeEnd();

                lTitle.Text = uiState.ToString();
                FormsUtils.ResumeDrawing(pContent);
            }
        }

        private void btnTotals_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Totals)
            {
                if (totalsUI == null)
                    totalsUI = new TotalsUI(this) { Dock = DockStyle.Fill };

                FormsUtils.SuspendDrawing(pContent);

                pContent.Controls.Clear();
                pContent.Controls.Add(totalsUI);

                pSideBar.Top = btnTotals.Top + 3;

                uiState = UIState.Totals;

                lTitle.Text = uiState.ToString();
                FormsUtils.ResumeDrawing(pContent);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.About)
            {
                pContent.Controls.Clear();

                if (aboutUI == null)
                    aboutUI = new AboutUI(this);

                pContent.Controls.Add(aboutUI);

                pSideBar.Top = btnAbout.Top + 3;

                uiState = UIState.About;

                lTitle.Text = uiState.ToString();
            }
        }

        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = (object)binForm.Deserialize(memStream);

                return obj;
            }
        }

        private void pbClose_Click(object sender, EventArgs e) => this.Close();
        private void pbMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

            pbMaximize.Image = useDarkmode ? (this.WindowState == FormWindowState.Maximized ? Properties.Resources.ic_filter_none_white_24dp : Properties.Resources.ic_crop_square_white_24dp) : (this.WindowState == FormWindowState.Maximized ? Properties.Resources.ic_filter_none_black_24dp : Properties.Resources.ic_crop_square_black_24dp);
        }
        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private void pbClose_MouseEnter(object sender, EventArgs e) => pbClose.BackColor = useDarkmode ? Color.FromArgb(225, 225, 50, 50) : Color.FromArgb(255, 205, 82, 82);
        private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = pRight.BackColor;
        private void pbMinimize_MouseEnter(object sender, EventArgs e) => (sender as PictureBox).BackColor = useDarkmode ? Color.FromArgb(128, 105, 105, 105) : Color.FromArgb(128, 169, 169, 169);
        private void pbMinimize_MouseLeave(object sender, EventArgs e) => (sender as PictureBox).BackColor = pRight.BackColor;

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            if (uiState == UIState.Accounts)
                accountsUI.ResizeEnd();
            else if (uiState == UIState.Totals)
                totalsUI.ResizeEnd();
        }
    }

    public enum UIState
    {
        Accounts,
        Totals,
        About
    }
}
