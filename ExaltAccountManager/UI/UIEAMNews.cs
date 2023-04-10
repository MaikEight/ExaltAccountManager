using ExaltAccountManager.UI.Elements;
using MK_EAM_General_Services_Lib;
using MK_EAM_General_Services_Lib.News.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MK_EAM_Lib;

namespace ExaltAccountManager.UI
{
    public sealed partial class UIEAMNews : UserControl
    {
        private FrmMain frm;

        private List<NewsData> news = new List<NewsData>();
        private List<EleEAMNewsView> newsViews = new List<EleEAMNewsView>();

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public UIEAMNews(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);

            FetchNews(DateTime.MinValue, frm.GetAPIClientIdHash(false), 5);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = frm.UseDarkmode ? def : third;
            this.ForeColor = font;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void RenderNews(List<NewsData> data)
        {       
            int maxBottom = 0;
            int index = 0;

            foreach (Control c in pNews.Controls)
            {
                c.Dispose();
            }
            pNews.Controls.Clear();

            pNews.SuspendLayout();
            FormsUtils.SuspendDrawing(pNews);

            for (int i = data.Count - 1; i >= 0; i--)
            {
                if (i < data.Count - 1)
                {
                    Panel p = new Panel()
                    {
                        Size = new Size(this.Width, 15),
                        Dock = DockStyle.Top,
                    };
                    pNews.Controls.Add(p);
                    pNews.Controls.SetChildIndex(p, index);
                    index++;
                }

                EleEAMNewsView view = new EleEAMNewsView(frm, data[i])
                {
                    Dock = DockStyle.Top
                };
                newsViews.Add(view);
                pNews.Controls.Add(view);
                pNews.Controls.SetChildIndex(view, index);
                index++;

                maxBottom = Math.Max(maxBottom, view.Bottom);
            }

            Panel pSpacer = new Panel()
            {
                Size = new Size(this.Width, 20),
            };
            pNews.Controls.Add(pSpacer);
            pNews.Controls.SetChildIndex(pSpacer, index);

            pNews.ResumeLayout();
            FormsUtils.ResumeDrawing(pNews);

            scrollbar.BindTo(pNews);
        }

        private void FetchNews(DateTime date, string clientIdHash, int amount)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(7500);

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;
                try
                {
                    Task<List<NewsData>> task = GeneralServicesClient.Instance?.GetNews(date, clientIdHash, amount);
                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            OnNewsFetchedInvoker(new List<NewsData>());
                            frm.LogEvent(new MK_EAM_Lib.LogData(
                                "EAM News",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to fetch news: data: " + date.ToString("dd.MM.yyyy") + ", amount: " + amount));

                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    List<NewsData> result = task.Result;
                    if (result != null && result.Count > 0)
                    {
                        OnNewsFetchedInvoker(result);
                        return;
                    }

                    OnNewsFetchedInvoker(new List<NewsData>());
                    frm.LogEvent(new MK_EAM_Lib.LogData(
                                "EAM News",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to fetch news: data: " + date.ToString("dd.MM.yyyy") + ", amount: " + amount));
                }
                catch (Exception ex)
                {
                    frm.LogEvent(new MK_EAM_Lib.LogData(
                                "EAM News",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to fetch news: data: " + date.ToString("dd.MM.yyyy") + ", amount: " + amount + Environment.NewLine + "Exception: " + ex.Message));
                }
            }), cancellationTokenSource.Token);
        }

        private void OnNewsFetchedInvoker(List<NewsData> data)
        {
            OnNewsFetched(data);
        }

        private bool OnNewsFetched(List<NewsData> data)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<List<NewsData>, bool>)OnNewsFetched, data);

            news.AddRange(data);
            UpdateHasNewNews();
            RenderNews(data);

            return false;
        }

        public void UpdateHasNewNews()
        {
            if (news?.Count > 0)
            {
                frm.HasNewNews = news.Max(d => d.Date) >= frm.LastNewsViewed;
                return;
            }
            frm.HasNewNews = false;
        }
    }
}
