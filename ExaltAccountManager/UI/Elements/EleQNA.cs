using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleQNA : UserControl
    {
        public QNA QNA
        {
            get => qna;
            set
            {
                qna = value;
                LoadUI();
            }
        }
        private QNA qna;

        private FrmMain frm;

        public EleQNA(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadowQNA.PanelColor = shadowQNA.BackColor = shadowQNA.PanelColor2 = def;
            shadowQNA.BorderColor = shadowQNA.BorderColor = second;
            shadowQNA.ShadowColor = shadowQNA.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
        }

        private void LoadUI()
        {
            lQuestion.Text = qna.Question;
            lAnswer.Text = qna.Answer;

            btnFunction.Visible = qna.Action != null;
            btnFunction.LeftIcon.Image = btnFunction.onHoverState.IconLeftImage = btnFunction.OnIdleState.IconLeftImage = qna.ButtonImage;
            btnFunction.Text = qna.ButtonText;

            spacerQuestion.Top = lQuestion.Bottom ;
            lAnswer.Top = spacerQuestion.Top + 14;

            if (btnFunction.Visible)            
                this.Height = lAnswer.Bottom + 63;            
            else
                this.Height = lAnswer.Bottom + 16;

            pbClose.Visible = qna.Type != QuestionType.Stop;
        }

        private void btnFunction_Click(object sender, EventArgs e)
        {
            if (qna.Action != null)
                qna.Action(this, null);
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
    }
}
