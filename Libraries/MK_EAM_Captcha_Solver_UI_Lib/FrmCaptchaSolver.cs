using MK_EAM_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MK_EAM_Captcha_Solver_UI_Lib
{
    public partial class FrmCaptchaSolver : Form
    {
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

        public bool UseDarkmode = false;

        private AccountInfo accountInfo;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private bool[] pointsSet = new bool[3] { false, false, false };
        private PointF[] points = new PointF[3] { new PointF(), new PointF(), new PointF() };
        private int currentPoint = 0;
        private bool isLoading = false;
        private Image imageOrg = null;

        private DateTime startTime = DateTime.Now;

        private CaptchaUIConfiguration configuration = new CaptchaUIConfiguration();

        private FrmZoom frmZoom = null;

        #region Paths

        public static readonly string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        public readonly string captchaSolverConfigurationPath = Path.Combine(saveFilePath, "EAM.CaptchaAidConf");

        #endregion

        public FrmCaptchaSolver(AccountInfo _accountInfo, bool useDarkmode = false)
        {
            InitializeComponent();

            formDock.SubscribeControlsToDragEvents(new Control[] { pHeader, pbHeader, lEAMHead, lHeaderCaptchaAid, lBeta });

            accountInfo = _accountInfo;
            UseDarkmode = useDarkmode;

            if (accountInfo == null)
            {
                this.DialogResult = DialogResult.Abort;

                _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                startTime,
                MK_EAM_Analytics.Data.CaptchaResult.Aborted,
                null,
                null,
                null);

                return;
            }

            lMessage.Text = string.Format(lMessage.Text, string.IsNullOrEmpty(accountInfo.Name) ? accountInfo.Email : accountInfo.Name);

            ApplyTheme();
            frmZoom = new FrmZoom();

            #region Read configuration

            if (File.Exists(captchaSolverConfigurationPath))
            {
                try
                {
                    configuration = JsonConvert.DeserializeObject<CaptchaUIConfiguration>(File.ReadAllText(captchaSolverConfigurationPath));
                }
                catch
                {
                    configuration = new CaptchaUIConfiguration();
                    try
                    {
                        File.WriteAllText(captchaSolverConfigurationPath, JsonConvert.SerializeObject(configuration));
                    }
                    catch { }
                }
            }

            #endregion

            ApplyConfigurationToUI();

            pbCaptchaMain.LoadCompleted += PbCaptchaMain_LoadCompleted;

            RequestChallenge();
        }

        private void ApplyTheme()
        {
            Color def = ColorScheme.GetColorDef(UseDarkmode);
            Color second = ColorScheme.GetColorSecond(UseDarkmode);
            Color third = ColorScheme.GetColorThird(UseDarkmode);
            Color font = ColorScheme.GetColorFont(UseDarkmode);

            this.ForeColor = font;
            this.BackColor = pTopContent.BackColor = def;
            pTop.BackColor = pMessage.BackColor = pbMinimize.BackColor = pbClose.BackColor = second;

            pbClose.Image = UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
            pbMinimize.Image = UseDarkmode ? Properties.Resources.baseline_minimize_white_24dp : Properties.Resources.baseline_minimize_black_24dp;

            pbShowNumbers.Image = UseDarkmode ? Properties.Resources.numbered_list_white_24px : Properties.Resources.numbered_list_black_24px;
            pbZoom.Image = UseDarkmode ? Properties.Resources.ic_search_white_24dp : Properties.Resources.search_black_black_24px;
            pbGrayScale.Image = UseDarkmode ? Properties.Resources.grayscale_white_24px : Properties.Resources.grayscale_black_24px;
        }

        private void RequestChallenge()
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(5000);
            pContent.Visible = false;

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;
                try
                {
                    Task<CaptchaSolverUtils.CaptchaRefreshResponse> task = CaptchaSolverUtils.RequestChallenge(accountInfo);
                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            this.DialogResult = DialogResult.Abort;

                            _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                            DateTime.Now,
                            MK_EAM_Analytics.Data.CaptchaResult.Error,
                            null,
                            null,
                            null);

                            this.Close();
                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    CaptchaSolverUtils.CaptchaRefreshResponse result = task.Result;
                    RequestChallengeUpdateUIInvoker(result);
                }
                catch (Exception ex)
                {
                    this.DialogResult = DialogResult.Abort;

                    _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                    DateTime.Now,
                    MK_EAM_Analytics.Data.CaptchaResult.Aborted,
                    null,
                    null,
                    null);

                    this.Close();
                    return;
                }
            }), cancellationTokenSource.Token);
        }

        private void RequestChallengeUpdateUIInvoker(CaptchaSolverUtils.CaptchaRefreshResponse captchaRefresh)
        {
            startTime = DateTime.Now;
            RequestChallengeUpdateUI(captchaRefresh);
        }

        private bool RequestChallengeUpdateUI(CaptchaSolverUtils.CaptchaRefreshResponse captchaRefresh)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<CaptchaSolverUtils.CaptchaRefreshResponse, bool>)RequestChallengeUpdateUI, captchaRefresh);

            if (captchaRefresh == null)
            {
                this.DialogResult = DialogResult.No;
                MessageBox.Show("You may have exceeded the amount of tries. Please try again later.", "Tries exceeded", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                startTime,
                MK_EAM_Analytics.Data.CaptchaResult.Failed,
                null,
                null,
                null);

                this.Close();
                return false;
            }

            lTriesLeft.Text = $"Tries left\n{captchaRefresh.submitsLeft}";
            pbCaptchaQuestion.LoadAsync(captchaRefresh.qimg);
            pbCaptchaMain.LoadAsync(captchaRefresh.img);

            pContent.Visible =
            pActions.Visible = true;
            btnSubmit.Enabled = false;

            points = new PointF[3] { new PointF(), new PointF(), new PointF() };
            return false;
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DialogResult = DialogResult.Cancel;

            _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
            startTime,
            MK_EAM_Analytics.Data.CaptchaResult.Aborted,
            null,
            null,
            null);

            this.Close();
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;


        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_24dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = pTop.BackColor;
            pbClose.Image = UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }

        private void pbClose_MouseUp(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Crimson;

        #endregion

        #region Button Minimize

        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private void pbMinimize_MouseDown(object sender, MouseEventArgs e) => pbMinimize.BackColor = UseDarkmode ? Color.FromArgb(200, 75, 75, 75) : Color.FromArgb(75, 50, 50, 50);
        private void pbMinimize_MouseUp(object sender, MouseEventArgs e) => pbMinimize.BackColor = UseDarkmode ? Color.FromArgb(125, 75, 75, 75) : Color.FromArgb(50, 50, 50, 50);
        private void pbMinimize_MouseEnter(object sender, EventArgs e) => pbMinimize.BackColor = UseDarkmode ? Color.FromArgb(125, 75, 75, 75) : Color.FromArgb(50, 50, 50, 50);
        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = pTop.BackColor;

        #endregion

        private void pbCaptchaMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (isLoading)
                return;

            points[currentPoint] = new PointF(e.X, e.Y);
            pointsSet[currentPoint] = true;
            currentPoint++;

            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
                btnSubmit.Enabled = true;
            }

            pbCaptchaMain.Invalidate();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            btnSubmit.Enabled =
            btnReset.Enabled = false;

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(5000);

            PointF[] _points = new PointF[]
            {
                new PointF((points[0].X / ((float)pbCaptchaMain.Width)), (points[0].Y / ((float)pbCaptchaMain.Height))),
                new PointF((points[1].X / ((float)pbCaptchaMain.Width)), (points[1].Y / ((float)pbCaptchaMain.Height))),
                new PointF((points[2].X / ((float)pbCaptchaMain.Width)), (points[2].Y / ((float)pbCaptchaMain.Height)))
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;
                try
                {
                    Task<bool> task = CaptchaSolverUtils.SubmitSolution(accountInfo, _points);
                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            this.DialogResult = DialogResult.Abort;

                            _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                            startTime,
                            MK_EAM_Analytics.Data.CaptchaResult.Aborted,
                            null,
                            null,
                            null);

                            MessageBox.Show("The request timed out. Please try again later.", "Request timed out", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            this.Close();
                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    bool result = task.Result;
                    SubmitUpdateUIInvoker(result, _points);
                }
                catch (Exception ex)
                {
                    this.DialogResult = DialogResult.Abort;

                    _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                    startTime,
                    MK_EAM_Analytics.Data.CaptchaResult.Aborted,
                    null,
                    null,
                    null);

                    MessageBox.Show("An error occured while submitting the result. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    this.Close();
                    return;
                }
            }), cancellationTokenSource.Token);
        }

        private void SubmitUpdateUIInvoker(bool result, PointF[] points)
        {
            SubmitUpdateUI(result, points);
        }

        private bool SubmitUpdateUI(bool result, PointF[] points)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool, PointF[], bool>)SubmitUpdateUI, result, points);

            _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                startTime,
                result ? MK_EAM_Analytics.Data.CaptchaResult.Success : MK_EAM_Analytics.Data.CaptchaResult.Failed,
                ImageToByteArray(pbCaptchaQuestion.Image),
                ImageToByteArray(imageOrg),
                points);

            if (result)
            {
                this.DialogResult = DialogResult.OK;

                this.Close();
                return true;
            }
            pActions.Visible = false;

            RequestChallenge();

            return false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (isLoading)
                return;
            pointsSet = new bool[3] { false, false, false };
            points = new PointF[3] { new PointF(), new PointF(), new PointF() };
            currentPoint = 0;
            btnSubmit.Enabled = false;

            pbCaptchaMain.Invalidate();
        }

        private int imageSize = 24 / 2;
        private void pbCaptchaMain_Paint(object sender, PaintEventArgs e)
        {
            if (!pointsSet.Contains(true))
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            if (configuration.UseNumbered)
            {
                List<NumberImagePoint> list = new List<NumberImagePoint>();
                for (int i = 0; i < points.Length; i++)
                {
                    if (pointsSet[i])
                    {
                        list.Add(new NumberImagePoint()
                        {
                            Point = new PointF(points[i].X - imageSize, points[i].Y - imageSize),
                            Number = i + 1
                        });
                    }
                }

                foreach (NumberImagePoint nip in list)
                {
                    e.Graphics.DrawImage((Image)Properties.Resources.ResourceManager.GetObject($"_{nip.Number}_purple_24px"), nip.Point);
                }

                return;
            }
            List<RectangleF> rectangles = new List<RectangleF>();

            for (int i = 0; i < points.Length; i++)
            {
                if (pointsSet[i])
                    rectangles.Add(new RectangleF(points[i].X - 4, points[i].Y - 6, 12, 12));
            }

            using (Pen p = new Pen(pHeader.BackColor, 1f))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(75, pHeader.BackColor.R, pHeader.BackColor.G, pHeader.BackColor.B)))
            {
                e.Graphics.DrawRectangles(p, rectangles.ToArray());
                e.Graphics.FillRectangles(brush, rectangles.ToArray());
            }
        }

        private void FrmCaptchaSolver_Activated(object sender, EventArgs e)
        {
            if (accountInfo == null)
            {
                DialogResult = DialogResult.Abort;

                _ = MK_EAM_Analytics.AnalyticsClient.Instance?.AddCaptchaResult(
                DateTime.Now,
                MK_EAM_Analytics.Data.CaptchaResult.Aborted,
                null,
                null,
                null);

                this.Close();
            }
        }
        private void PbCaptchaMain_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            imageOrg = pbCaptchaMain.Image;

            if (configuration.UseGrayscale)
            {
                pbCaptchaMain.Image = GetGrayscaledImage((Bitmap)imageOrg);
            }
        }

        private void ApplyConfigurationToUI()
        {
            if (configuration == null)
            {
                configuration = new CaptchaUIConfiguration();
            }

            pbShowNumbers_MouseLeave(this, EventArgs.Empty);
            pbZoom_MouseLeave(this, EventArgs.Empty);
            pbGrayScale_MouseLeave(this, EventArgs.Empty);
        }

        private void FrmCaptchaSolver_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorDef(UseDarkmode)))
            {
                e.Graphics.DrawLine(p, 0, this.Height - 1, this.Width, this.Height - 1);
            }
        }

        private void pContent_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorDef(UseDarkmode)))
            {
                e.Graphics.DrawLine(p, 0, pContent.Height - 1, pContent.Width, pContent.Height - 1);
            }
        }

        private byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public Bitmap GetGrayscaledImage(Bitmap original)
        {
            if (original == null)
                return null;

            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {

                    //set the color matrix attribute
                    attributes.SetColorMatrix(colorMatrix);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }

        Color activated
        {
            get
            {
                return UseDarkmode ? Color.FromArgb(175, 98, 0, 238) : Color.FromArgb(150, 98, 0, 238);
            }
        }
        Color activatedHover
        {
            get
            {
                return UseDarkmode ? Color.FromArgb(225, 98, 0, 238) : Color.FromArgb(175, 98, 0, 238);
            }
        }
        Color hover
        {
            get
            {
                return UseDarkmode ? Color.FromArgb(125, 75, 75, 75) : Color.FromArgb(50, 50, 50, 50);
            }
        }

        private void pbShowNumbers_Click(object sender, EventArgs e)
        {
            configuration.UseNumbered = !configuration.UseNumbered;
            pbShowNumbers_MouseEnter(sender, e);

            pbCaptchaMain.Invalidate();

            SaveConfiguration();
        }

        private void pbShowNumbers_MouseEnter(object sender, EventArgs e)
        {
            pbShowNumbers.BackColor = configuration.UseNumbered ? activatedHover : hover;
        }

        private void pbShowNumbers_MouseLeave(object sender, EventArgs e)
        {
            pbShowNumbers.BackColor = configuration.UseNumbered ? activated : pToolsContent.BackColor;
        }

        private void pbZoom_Click(object sender, EventArgs e)
        {
            configuration.UseZoom = !configuration.UseZoom;
            pbZoom_MouseEnter(sender, e);

            SaveConfiguration();
        }

        private void pbZoom_MouseEnter(object sender, EventArgs e)
        {
            pbZoom.BackColor = configuration.UseZoom ? activatedHover : hover;
        }

        private void pbZoom_MouseLeave(object sender, EventArgs e)
        {
            pbZoom.BackColor = configuration.UseZoom ? activated : pToolsContent.BackColor;
        }

        private void pbGrayScale_Click(object sender, EventArgs e)
        {
            configuration.UseGrayscale = !configuration.UseGrayscale;
            pbGrayScale_MouseEnter(sender, e);

            pbCaptchaMain.Image = configuration.UseGrayscale ? GetGrayscaledImage((Bitmap)imageOrg) : imageOrg;

            SaveConfiguration();
        }

        private void pbGrayScale_MouseEnter(object sender, EventArgs e)
        {
            pbGrayScale.BackColor = configuration.UseGrayscale ? activatedHover : hover;
        }

        private void pbGrayScale_MouseLeave(object sender, EventArgs e)
        {
            pbGrayScale.BackColor = configuration.UseGrayscale ? activated : pToolsContent.BackColor;
        }

        private void SaveConfiguration()
        {
            try
            {
                File.WriteAllText(captchaSolverConfigurationPath, JsonConvert.SerializeObject(configuration));
            }
            catch { }
        }

        int offset = 10;
        int cutSize = 50;
        Point p = new Point(0, 0);
        private void pbCaptchaMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (pbCaptchaMain.Image == null || !configuration.UseZoom) return;

            int halfCutSize = (int)(cutSize * 0.5f);

            p.X = Math.Min(e.X <= halfCutSize ? 0 : e.X - halfCutSize, pbCaptchaMain.Width - cutSize);
            p.Y = Math.Min(e.Y <= halfCutSize ? 0 : e.Y - halfCutSize, pbCaptchaMain.Height - cutSize);

            float scale = (float)pbCaptchaMain.Image.Width / (float)pbCaptchaMain.Width;
            float scaledCutSize = cutSize * scale;

            Bitmap bmp = new Bitmap((int)scaledCutSize, (int)scaledCutSize);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(pbCaptchaMain.Image,
                    new RectangleF(0, 0, scaledCutSize, scaledCutSize),
                    new RectangleF(p.X * scale, p.Y * scale, scaledCutSize, scaledCutSize),
                    GraphicsUnit.Pixel);
            }

            PointF scaledMousePosition = new PointF(e.X * scale, e.Y * scale);
            PointF scaledImageStartPoint = new PointF(p.X * scale, p.Y * scale);

            PointF result = new PointF(
                scaledMousePosition.X - scaledImageStartPoint.X,
                scaledMousePosition.Y - scaledImageStartPoint.Y);

            if (configuration.UseZoom)
            {
                frmZoom.Image = bmp;
                frmZoom.AimLocation = result;
                Point pbScreenLocation = pbCaptchaMain.PointToScreen(Point.Empty);
                frmZoom.Location = new Point(
                        pbScreenLocation.X + e.X + offset,
                        pbScreenLocation.Y + e.Y + offset
                    );
            }
        }

        private void pbCaptchaMain_MouseEnter(object sender, EventArgs e)
        {
            if (configuration.UseZoom)
                frmZoom?.Show(this);
        }

        private void pbCaptchaMain_MouseLeave(object sender, EventArgs e)
        {
            frmZoom?.Hide();
        }

        public sealed class NumberImagePoint
        {
            public int Number { get; set; }
            public PointF Point { get; set; }
        }
    }
}
