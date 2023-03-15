using ExaltAccountManager.UI.Elements;
using MK_EAM_Analytics;
using MK_EAM_General_Services_Lib;
using MK_EAM_General_Services_Lib.News.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public partial class UIEAMNews : UserControl
    {
        private FrmMain frm;

        private List<NewsData> news = new List<NewsData>();
        private List<EleEAMNewsView> newsViews= new List<EleEAMNewsView>();

        public UIEAMNews(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            _ = new GeneralServicesClient("https://localhost:7066/");

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);

            FetchNews(DateTime.MinValue, frm.GetAPIClientIdHash(false), 5);

            //List<NewsData> data = new List<NewsData>()
            //{
            //    new NewsData()
            //    {   
            //        Id = Guid.NewGuid(),
            //        Title = "Test",
            //        Date = DateTime.Now,
            //        Importance = 1,
            //        newsEntries = new List<NewsEntry>()
            //        {
            //            new NewsEntry()
            //            {
            //                NewsId = Guid.NewGuid(),
            //                OrderId = 0,
            //                TypeId = 1,
            //                UiData = new TextUIData()
            //                {
            //                    Text  = "Headline"
            //                }
            //            },
            //            new NewsEntry()
            //            {
            //                NewsId = Guid.NewGuid(),
            //                OrderId = 1,
            //                TypeId = 100,                          
            //                UiData = new ImageUIData()
            //                {
            //                    ImageUrl = "https://storage.ko-fi.com/cdn/useruploads/jpg_d9bc68fe-12db-48eb-a16c-b0af0776abb8cover.jpg?v=14bacd10-c3dc-48b6-ba95-0678b622bb76",                                
            //                    PictureBoxSizeMode = 4,
            //                    MaxSize = new Size(0, 200),
            //                    MinSize = new Size(1, 1)
            //                }
            //            },
            //            new NewsEntry()
            //            {
            //                NewsId = Guid.NewGuid(),
            //                OrderId = 1,
            //                TypeId = 0,
            //                UiData = new TextUIData()
            //                {
            //                    Text  = "Message here"
            //                }
            //            }
            //        }
            //    }
            //};
            //RenderNews(data);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;
            this.ForeColor = font;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void RenderNews(List<NewsData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                EleEAMNewsView view = new EleEAMNewsView(frm, data[i]);
                newsViews.Add(view);
                flow.Controls.Add(view);
                flow.SetFlowBreak(view, true);
            }
        }

        private Thread worker;
        private void FetchNews(DateTime date, string clientIdHash, int amount)
        {
            if (worker != null)
            {
                try
                {
                    worker.Abort();
                }
                catch { }
            }
            worker = new Thread(new ThreadStart(async () =>
            {
                var task = GeneralServicesClient.Instance?.GetNews(date, clientIdHash, amount);
                List<NewsData> data = await task;
                OnNewsFetchedInvoker(data);
            }));
            worker.IsBackground = true;
            worker.Start();
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
            RenderNews(data);

            return false;
        }
    }
}
