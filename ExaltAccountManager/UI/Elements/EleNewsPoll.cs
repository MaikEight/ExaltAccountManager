using ExaltAccountManager.UI.Elements.Mini;
using MK_EAM_General_Services_Lib.News.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleNewsPoll : UserControl
    {
        private FrmMain frm;
        private PollUIData pollUIData;
        private List<MiniNewsPollEntry> pollEntries = new List<MiniNewsPollEntry>();
        private bool revealed = false;

        public EleNewsPoll(FrmMain _frm, PollUIData _pollUIData)
        {
            InitializeComponent();
            frm = _frm;
            pollUIData = _pollUIData;

            lQuestion.Text = pollUIData.Headline;
            EleNewsPoll_SizeChanged(this, null);

            LoadUI();

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = second;
            pContent.BackColor = second;
            lQuestion.ForeColor = font;
        }

        private void LoadUI()
        {
            int allVotes = pollUIData.PollData.Entries.Sum();
            revealed = pollUIData.PollData.OwnEntry >= 0;

            for (int i = pollUIData.PollData.Entries.Length - 1; i >= 0; i--)
            {
                MiniNewsPollEntry entry = new MiniNewsPollEntry(
                    frm,
                    i,
                    pollUIData.PollData.Entries[i],
                    allVotes,
                    pollUIData.PollData.OwnEntry == i,
                    pollUIData.EntrieTexts[i],
                    revealed)
                {
                    Dock = DockStyle.Top
                };

                entry.OnClick += Entry_OnClick;
                entry.Disposed -= Entry_OnClick;

                pollEntries.Add(entry);
                pContent.Controls.Add(entry);
            }

            this.Height = pContent.Top + pollEntries.Max(entry => entry.Bottom) + this.Padding.Bottom;
        }

        private void Entry_OnClick(object sender, EventArgs e)
        {
            MiniNewsPollEntry entry = (MiniNewsPollEntry)sender;
            if (entry.IsOwnVote)
                return;

            revealed = true;

            pollEntries.ForEach(en => { en.IsOwnVote = false; en.SetReveal(revealed); });

            entry.IsOwnVote = true;

            MK_EAM_General_Services_Lib.GeneralServicesClient.Instance.PostPoll(
                pollUIData.PollData.PollId,
                entry.GetEntryNumber(),
                frm.GetAPIClientIdHash());
        }


        private void UpdateEntryDataInvoker(PollData data)
        {
            UpdateEntryData(data);
        }

        private bool UpdateEntryData(PollData data)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<PollData, bool>)UpdateEntryData, data);

            int totalVotes = data.Entries.Sum();

            pollEntries.ForEach((entry) =>
            {
                if (data.Entries.Length > entry.GetEntryNumber())
                    entry.UpdateVotes(data.Entries[entry.GetEntryNumber()], totalVotes);
            });

            return false;
        }

        private void EleNewsPoll_SizeChanged(object sender, EventArgs e)
        {
            lQuestion.MaximumSize = new Size(this.Width, 0);
        }
    }
}
