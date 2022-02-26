using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleSnackbarOptions : UserControl
    {
        FrmMain frm;

        public Bunifu.UI.WinForms.BunifuSnackbar.Positions SnackbarPosition 
        {
            get => snackPos;
            set 
            {
                snackPos = value;
                pbPreview.Image = GetSnackbarImage();
            }
        }
        private Bunifu.UI.WinForms.BunifuSnackbar.Positions snackPos = Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight;
        private bool isInit = true;

        public EleSnackbarOptions(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            snackPos = frm.SnackbarPosition;
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

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            pbPreview.Image = GetSnackbarImage();
        }
        private void EleSnackbarOptions_Load(object sender, EventArgs e)
        {
            foreach (Bunifu.UI.WinForms.BunifuRadioButton radio in shadow.Controls.OfType<Bunifu.UI.WinForms.BunifuRadioButton>())            
                radio.Checked = false;
            
            GetBunifuRadioButton().Checked = true;
            isInit = false;
        }


        private Image GetSnackbarImage()
        {
            #region Switch snackPos

            switch (snackPos)
            {
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_TopLeft : Properties.Resources.Snackbar_TopLeft_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_TopCenter : Properties.Resources.Snackbar_TopCenter_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopRight:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_TopRight : Properties.Resources.Snackbar_TopRight_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleLeft:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_MiddleLeft : Properties.Resources.Snackbar_MiddleLeft_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_MiddleCenter : Properties.Resources.Snackbar_MiddleCenter_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleRight:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_MiddleRight : Properties.Resources.Snackbar_MiddleRight_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomLeft:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_BottomLeft : Properties.Resources.Snackbar_BottomLeft_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomCenter:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_BottomCenter : Properties.Resources.Snackbar_BottomCenter_white;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_BottomRight : Properties.Resources.Snackbar_BottomRight_white;
                default:
                    return frm.UseDarkmode ? Properties.Resources.Snackbar_BottomRight : Properties.Resources.Snackbar_BottomRight_white;
            }

            #endregion
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            frm.SnackbarPosition = SnackbarPosition;
            frm.OptionsData.snackbarPosition = (int)SnackbarPosition;
            frm.SaveOptions(frm.OptionsData);

            frm.ShowSnackbar("Snackbar position saved.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);
        }

        private Bunifu.UI.WinForms.BunifuSnackbar.Positions GetSnackbarPosition(Bunifu.UI.WinForms.BunifuRadioButton radio)
        {
            #region Switch Radio

            switch (radio.Name)
            {
                case "radioTopLeft":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft;
                case "radioTopCenter":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter;
                case "radioTopRight":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopRight;
                case "radioMiddleLeft":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleLeft;
                case "radioMiddleCenter":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter;
                case "radioMiddleRight":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleRight;
                case "radioBottomLeft":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomLeft;
                case "radioBottomCenter":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomCenter;
                case "radioBottomRight":
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight;
                default:
                    return Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight;
            }

            #endregion
        }

        private Bunifu.UI.WinForms.BunifuRadioButton GetBunifuRadioButton()
        {
            #region Switch SnackpbarPosition

            switch (SnackbarPosition)
            {
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft:
                    return radioTopLeft;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter:
                    return radioTopCenter;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopRight:
                    return radioTopRight;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleLeft:
                    return radioMiddleLeft;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter:
                    return radioMiddleCenter;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleRight:
                    return radioMiddleRight;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomLeft:
                    return radioBottomLeft;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomCenter:
                    return radioBottomCenter;
                case Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight:
                    return radioBottomRight;                
                default:
                    return radioBottomRight;
            }

            #endregion
        }

        private void radio_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {            
            if (isInit || !e.Checked) return;

            frm.SnackbarPosition = SnackbarPosition= GetSnackbarPosition(sender as Bunifu.UI.WinForms.BunifuRadioButton);

            frm.CloseAllSnackbars();
            frm.ShowSnackbar("Notification", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);
            frm.SnackbarPosition = (Bunifu.UI.WinForms.BunifuSnackbar.Positions)frm.OptionsData.snackbarPosition;
        }        
    }
}
