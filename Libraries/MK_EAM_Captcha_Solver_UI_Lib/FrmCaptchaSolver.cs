using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        private DateTime startTime = DateTime.Now;

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
                ImageToByteArray(pbCaptchaMain.Image), 
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
            btnSubmit.Enabled = false;

            pbCaptchaMain.Invalidate();
        }

        private void pbCaptchaMain_Paint(object sender, PaintEventArgs e)
        {
            if (!pointsSet.Contains(true))
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            List<RectangleF> rectangles = new List<RectangleF>();

            for (int i = 0; i < points.Length; i++)
            {
                if (pointsSet[i])                
                    rectangles.Add(new RectangleF(points[i].X - 4, points[i].Y - 4, 8, 8));                   
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
    }
}
