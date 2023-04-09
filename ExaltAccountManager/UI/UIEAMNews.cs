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

            data.Add(
                new NewsData()
                {
                    Id = Guid.NewGuid(),
                    Title = "The Exalt Account Manager news section is here 🎉",
                    Date = DateTime.Now,
                    Importance = 1,
                    newsEntries = new List<NewsEntry>()
                    {
                        new NewsEntry()
                        {
                            NewsId = Guid.NewGuid(),
                            OrderId = 1,
                            TypeId = 1,
                            UiData = new TextUIData()
                            {
                                Text  = "What are the news for?"
                            }
                        },
                        new NewsEntry()
                        {
                            NewsId = Guid.NewGuid(),
                            OrderId = 0,
                            TypeId = 100,
                            UiData = new ImageUIData()
                            {
                                ImageUrl = "https://raw.githubusercontent.com/MaikEight/ExaltAccountManager/master/ExaltAccountManager/Resources/1.png",
                                PictureBoxSizeMode = 4,
                                MaxSize = new Size(0, 200),
                                MinSize = new Size(1, 1)
                            }
                        },
                        new NewsEntry()
                        {
                            NewsId = Guid.NewGuid(),
                            OrderId = 2,
                            TypeId = 0,
                            UiData = new TextUIData()
                            {
                                Text  = "Finally the news section is getting into shape and it even got polls!\nThis feature allows for better and simpler collection of user-feedback aswell as broadcasting messages or news with every EAM-User."
                            }
                        },
                        new NewsEntry()
                        {
                            NewsId = Guid.NewGuid(),
                            OrderId = 3,
                            TypeId = 200,
                            UiData = new PollUIData()
                            {
                                Headline = "What do you think about the new polls?",
                                EntrieImageUrls = new string[]
                                {
                                    null, null, null
                                },
                                EntrieTexts = new string[]
                                {
                                    "Awesome addition",
                                    "Nice to have",
                                    "Mehh",
                                    "Unnecessary",
                                },
                                PollData = new PollData()
                                {
                                    StartDate = DateTime.Now.AddDays(-1),
                                    EndDate = DateTime.Now.AddDays(3),
                                    Entries = new int[] { 5, 2, 1, 0 },
                                    EntriesAmount = 4,
                                    Name = "Test",
                                    OwnEntry = -1,
                                    PollId = Guid.NewGuid()
                                }
                            }
                        },
                    }
                });

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

            //foreach (Control c in pNews.Controls)
            //{
            //    maxBottom = Math.Max(maxBottom, c.Bottom);
            //}
            //pNews.Height = maxBottom;

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
