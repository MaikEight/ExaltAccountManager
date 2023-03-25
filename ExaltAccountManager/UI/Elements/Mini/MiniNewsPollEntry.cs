using Bunifu.UI.WinForms.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements.Mini
{
    public sealed partial class MiniNewsPollEntry : UserControl
    {
        private FrmMain frm;
        private int entryNumber = -1;
        private int votes = 0;
        private int allVotes = 0;
        public bool IsOwnVote
        {
            get => isOwnVote;
            set
            {
                isOwnVote = pbOwnChoice.Visible = value;
            }
        }
        private bool isOwnVote = false;

        private string entryText = "";
        private bool reveal = false;
        private bool isFirstRevealed = false;
        private DateTime animationEndTime = DateTime.MinValue;
        private DateTime animationStartTime = DateTime.MinValue;
        private int msAnimationDuration = 500;

        public event EventHandler OnClick;
        public MiniNewsPollEntry(FrmMain _frm, int _entryNumber, int _votes, int allVotes, bool isOwnVote, string entryText, bool _reveal)
        {
            InitializeComponent();
            frm = _frm;

            this.entryNumber = _entryNumber;
            this.votes = _votes;
            this.allVotes = allVotes;
            this.IsOwnVote = isOwnVote;
            this.entryText = entryText;
            this.reveal = _reveal;

            SetReveal(reveal);

            lChoice.Text = entryText;
            lResults.Text = (allVotes == 0 ? 0 : (Math.Round((float)votes / (float)allVotes * 100f))) + "%";
            pbOwnChoice.Visible = isOwnVote;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            this.BackColor = ColorScheme.GetColorDef(frm.UseDarkmode);
            this.ForeColor = ColorScheme.GetColorFont(frm.UseDarkmode);

            pbOwnChoice.Image = frm.UseDarkmode ? Properties.Resources.Checkmark_white_28px : Properties.Resources.Checkmark_black_28px;
        }

        public int GetEntryNumber() => entryNumber;

        public void SetReveal(bool _reveal)
        {
            if (_reveal && !isFirstRevealed)
            {
                animationStartTime = DateTime.Now;
                animationEndTime = DateTime.Now.AddMilliseconds(msAnimationDuration);
                //currentFrame = 0;
                timerAnimation.Start();
            }

            pPercentage.Visible = reveal = _reveal;
            this.Invalidate();
        }

        public void UpdateVotes(int votes, int totalVotes)
        {
            this.votes = votes;
            this.allVotes = totalVotes;

            lResults.Text = (allVotes == 0 ? 0 : (Math.Round((float)votes / (float)allVotes * 100f))) + "%";
            this.Invalidate();
        }

        private void MiniNewsPollEntry_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            float percentage = (float)votes / (float)allVotes * 100f;
            if (reveal && !isFirstRevealed)
            {
                if (DateTime.Now < animationEndTime)
                {
                    TimeSpan totalTimespan = animationEndTime - animationStartTime;
                    TimeSpan elapsedTimespan = DateTime.Now - animationStartTime;

                    float elapsedPercentage = (float)elapsedTimespan.TotalMilliseconds / (float)totalTimespan.TotalMilliseconds;
                    percentage = elapsedPercentage * percentage;
                }
                else
                {
                    isFirstRevealed = true;
                }
            }
            DrawResultbar(e.Graphics, percentage);

            //Draw border
            using (Pen p = new System.Drawing.Pen(frm.UseDarkmode ? ColorScheme.GetColorSecond(frm.UseDarkmode) : ColorScheme.GetColorThird(frm.UseDarkmode), 1))
            {
                var lines = entryNumber == 0 ?
                new Point[]
                {
                        new Point(0, 0),
                        new Point(0, this.Height - 1),
                        new Point(this.Width - 1, this.Height - 1),
                        new Point(this.Width - 1, 0),
                        new Point(0, 0)
                } :
                new Point[]
                {
                    new Point(0, 0),
                        new Point(0, this.Height - 1),
                        new Point(this.Width - 1, this.Height - 1),
                        new Point(this.Width - 1, 0),
                };
                e.Graphics.DrawLines(p, lines);
            }
        }

        private void DrawResultbar(Graphics g, float percentage)
        {
            if (!reveal)
                return;
            float fillWidth = ((float)Width * percentage) / 100f;
            Color fillColor = Color.FromArgb(175, 98, 0, 238);

            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                g.FillRectangle(brush, 0, 0, fillWidth, this.Height);
            }
        }

        private void MiniNewsPollEntry_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = frm.UseDarkmode ? this.BackColor.LightenBy(3) : this.BackColor.DarkenBy(3);
        }

        private void MiniNewsPollEntry_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = ColorScheme.GetColorDef(frm.UseDarkmode);
        }

        private void MiniNewsPollEntry_Click(object sender, EventArgs e)
        {
            OnClick?.Invoke(this, e);
        }

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            if (!isFirstRevealed)
            {
                return;
            }
            timerAnimation.Stop();
        }
    }
}
