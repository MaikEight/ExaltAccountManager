using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public sealed partial class ColorChanger : UserControl
    {
        public List<Color> colors = new List<Color>()
        {
            Color.Transparent,
            Color.Crimson,
            Color.HotPink,
            Color.DarkViolet,
            Color.SlateBlue,
            Color.DodgerBlue,
            Color.RoyalBlue,
            Color.FromArgb(0, 179, 219),
            Color.SeaGreen,
            Color.GreenYellow,
            Color.Gold,
            Color.FromArgb(50, 128, 128, 128),
        };
        private Bunifu.UI.WinForms.BunifuPictureBox pbDrawnBorder = null;
        private Bunifu.UI.WinForms.BunifuPictureBox pbTransparent = null;

        public FrmMainOLD frmOld = null;
        public FrmMain frm = null;
        public AccountUI ui = null;
        public int accountIndex = -1;

        public ColorChanger(FrmMainOLD _frm)
        {
            InitializeComponent();
            frmOld = _frm;
            LoadUIOld();
        }

        public ColorChanger(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;

            LoadUI();
        }

        private void LoadUI()
        {
            sliderRed.Size = sliderGreen.Size = sliderBlue.Size = sliderAlpha.Size = new Size(255, 24);
            tbRed.Size = tbGreen.Size = tbBlue.Size = tbAlpha.Size = new Size(45, 24);

            for (int i = 0; i < colors.Count; i++)
            {
                Bunifu.UI.WinForms.BunifuPictureBox pbColor = new Bunifu.UI.WinForms.BunifuPictureBox()
                {
                    Size = new Size(20, 20),
                    BackColor = colors[i]
                };
                pbColor.Image = pbColor.InitialImage = pbColor.BackgroundImage = pbColor.ErrorImage = null;
                pbColor.Click += pbColor_Click;
                flow.Controls.Add(pbColor);
                if (colors[i] == Color.Transparent)
                {
                    pbTransparent = pbColor;
                    pbTransparent.SizeMode = PictureBoxSizeMode.CenterImage;
                    pbTransparent.Image = frm.UseDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;
                }
            }

            for (int i = 0; i < frm.accounts.Count; i++)
                AddColorToUI(frm.accounts[i].Color);

            ApplyTheme(frm, null);
        }

        private void LoadUIOld()
        {
            sliderRed.Size = sliderGreen.Size = sliderBlue.Size = sliderAlpha.Size = new Size(255, 24);
            tbRed.Size = tbGreen.Size = tbBlue.Size = tbAlpha.Size = new Size(45, 24);

            for (int i = 0; i < colors.Count; i++)
            {
                Bunifu.UI.WinForms.BunifuPictureBox pbColor = new Bunifu.UI.WinForms.BunifuPictureBox()
                {
                    Size = new Size(20, 20),
                    BackColor = colors[i]
                };
                pbColor.Image = pbColor.InitialImage = pbColor.BackgroundImage = pbColor.ErrorImage = null;
                pbColor.Click += pbColor_Click;
                flow.Controls.Add(pbColor);
                if (colors[i] == Color.Transparent)
                {
                    pbTransparent = pbColor;
                    pbTransparent.SizeMode = PictureBoxSizeMode.CenterImage;
                    pbTransparent.Image = frmOld.useDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;
                }
            }

            for (int i = 0; i < frmOld.accounts.Count; i++)
                AddColorToUI(frmOld.accounts[i].Color);

            ApplyThemeOld();
        }

        [Obsolete]
        public void ShowUI(AccountUI _ui)
        {
            ui = _ui;

            //if (pbDrawnBorder != null)
            //    pbDrawnBorder.Paint -= pbColor_Paint;

            foreach (Bunifu.UI.WinForms.BunifuPictureBox pb in flow.Controls.OfType<Bunifu.UI.WinForms.BunifuPictureBox>())
            {
                if (pb.BackColor == ui.accountInfo.Color)
                {
                    pbDrawnBorder = pb;
                    AddColorToSlider(pbDrawnBorder.BackColor);
                    flow.Update();
                    break;
                }
            }
        }

        public void ShowUI(int index)
        {
            accountIndex = index;

            foreach (Bunifu.UI.WinForms.BunifuPictureBox pb in flow.Controls.OfType<Bunifu.UI.WinForms.BunifuPictureBox>())
            {
                if (pb.BackColor == frm.accounts[index].Color)
                {
                    pbDrawnBorder = pb;
                    AddColorToSlider(pbDrawnBorder.BackColor);
                    flow.Update();
                    break;
                }
            }

            //this.Visible = true;
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            pbClose.BackColor =
            this.BackColor = frm.UseDarkmode ? third : def;
            flow.BackColor = second;

            this.ForeColor =
            p.Color =
            separator.LineColor = separator2.LineColor =
            tbAlpha.ForeColor = tbRed.ForeColor = tbGreen.ForeColor = tbBlue.ForeColor =
            font;

            tbRed.BackColor = tbRed.OnIdleState.FillColor = tbRed.OnActiveState.FillColor = tbRed.OnDisabledState.FillColor = tbRed.OnHoverState.FillColor =
            tbGreen.BackColor = tbGreen.OnIdleState.FillColor = tbGreen.OnActiveState.FillColor = tbGreen.OnDisabledState.FillColor = tbGreen.OnHoverState.FillColor =
            tbBlue.BackColor = tbBlue.OnIdleState.FillColor = tbBlue.OnActiveState.FillColor = tbBlue.OnDisabledState.FillColor = tbBlue.OnHoverState.FillColor =
            tbAlpha.BackColor = tbAlpha.OnIdleState.FillColor = tbAlpha.OnActiveState.FillColor = tbAlpha.OnDisabledState.FillColor = tbAlpha.OnHoverState.FillColor = def;

            tbRed.OnIdleState.BorderColor = tbGreen.OnIdleState.BorderColor = tbBlue.OnIdleState.BorderColor = tbAlpha.OnIdleState.BorderColor = font;
            //lHeadline.BackColor =
            //pbClose.BackColor = third;

            pbTransparent.Image = frm.UseDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;

            this.Invalidate();
        }

        [Obsolete]
        public void ApplyThemeOld()
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (frmOld.useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            this.BackColor = def;
            flow.BackColor = second;

            this.ForeColor =
            p.Color =
            separator.LineColor = separator2.LineColor = font;

            tbRed.BackColor = tbRed.OnIdleState.FillColor = tbRed.OnActiveState.FillColor = tbRed.OnDisabledState.FillColor = tbRed.OnHoverState.FillColor =
            tbGreen.BackColor = tbGreen.OnIdleState.FillColor = tbGreen.OnActiveState.FillColor = tbGreen.OnDisabledState.FillColor = tbGreen.OnHoverState.FillColor =
            tbBlue.BackColor = tbBlue.OnIdleState.FillColor = tbBlue.OnActiveState.FillColor = tbBlue.OnDisabledState.FillColor = tbBlue.OnHoverState.FillColor =
            tbAlpha.BackColor = tbAlpha.OnIdleState.FillColor = tbAlpha.OnActiveState.FillColor = tbAlpha.OnDisabledState.FillColor = tbAlpha.OnHoverState.FillColor = def;

            tbRed.OnIdleState.BorderColor = tbGreen.OnIdleState.BorderColor = tbBlue.OnIdleState.BorderColor = tbAlpha.OnIdleState.BorderColor = font;
            lHeadline.BackColor =
            pbClose.BackColor = third;

            pbTransparent.Image = frmOld.useDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;
            pbClose.Image = frmOld.useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }

        public void AddColorToUI(Color clr)
        {
            if (colors.Contains(clr))
                return;

            colors.Add(clr);
            Bunifu.UI.WinForms.BunifuPictureBox pbColor = new Bunifu.UI.WinForms.BunifuPictureBox()
            {
                Size = new Size(20, 20),
                BackColor = clr
            };
            pbColor.Image = pbColor.InitialImage = pbColor.BackgroundImage = pbColor.ErrorImage = null;
            pbColor.Click += pbColor_Click;
            flow.Controls.Add(pbColor);
            scrollbar.BindTo(flow);
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            if ((frmOld != null && ui == null) || (frm == null && accountIndex == -1)) return;

            pbDrawnBorder = sender as Bunifu.UI.WinForms.BunifuPictureBox;

            if (frmOld != null)
                ui.ChangeColor(pbDrawnBorder.BackColor);
            else
            {
                if (accountIndex >= 0)
                {                    
                    frm.UpdateDataGridViewGroup(accountIndex, pbDrawnBorder.BackColor);
                }
                else if (accountIndex == -99) //Add new Account
                {
                    frm.UpdateAddNewUserGroup(pbDrawnBorder.BackColor, pbDrawnBorder == pbTransparent);
                }
            }

            flow.Invalidate();
            AddColorToSlider(pbDrawnBorder.BackColor);
        }

        private void AddColorToSlider(Color clr)
        {
            isChanging = true;

            sliderRed.Value = clr.R;
            tbRed.Text = clr.R.ToString();

            sliderGreen.Value = clr.G;
            tbGreen.Text = clr.G.ToString();

            sliderBlue.Value = clr.B;
            tbBlue.Text = clr.B.ToString();

            sliderAlpha.Value = clr.A;
            tbAlpha.Text = clr.A.ToString();
            btnAdd.BackColor = clr;

            isChanging = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddColorToUI(Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value));
        }

        bool isChanging = false;
        private void sliderRed_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            tbRed.Text = sliderRed.Value.ToString();
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void tbRed_TextChanged(object sender, EventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            int x = 0;
            int.TryParse(tbRed.Text, out x);
            sliderRed.Value = x;
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void sliderGreen_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            tbGreen.Text = sliderGreen.Value.ToString();
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void tbGreen_TextChanged(object sender, EventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            int x = 0;
            int.TryParse(tbGreen.Text, out x);
            sliderGreen.Value = x;
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void sliderBlue_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            tbBlue.Text = sliderBlue.Value.ToString();
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void tbBlue_TextChanged(object sender, EventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            int x = 0;
            int.TryParse(tbBlue.Text, out x);
            sliderBlue.Value = x;
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void slideAlpha_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            tbAlpha.Text = sliderAlpha.Value.ToString();
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        private void tbAlpha_TextChanged(object sender, EventArgs e)
        {
            if (isChanging) return;
            isChanging = true;

            int x = 0;
            int.TryParse(tbAlpha.Text, out x);
            sliderAlpha.Value = x;
            btnAdd.BackColor = Color.FromArgb(sliderAlpha.Value, sliderRed.Value, sliderGreen.Value, sliderBlue.Value);

            isChanging = false;
        }

        Pen p = new Pen(Color.Black);
        private void ColorChanger_Paint(object sender, PaintEventArgs e)
        {
            p.Width = 2f;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            e.Graphics.DrawArc(p, 0f, 0f, 11f, 11f, 180, 90);
            e.Graphics.DrawArc(p, (float)(this.Width - 13f), 0f, 11f, 11f, 270, 90);
            e.Graphics.DrawArc(p, (float)(this.Width - 13f), (float)(this.Height - 13.5f), 11f, 11f, 0, 90);
            e.Graphics.DrawArc(p, 0f, (float)(this.Height - 13.5f), 11f, 11f, 90, 90);

            p.Width = 1f;

            e.Graphics.DrawRectangles(p, new RectangleF[] { new RectangleF(0, 0, this.Width - 2f, this.Height - 2f) });
        }

        private void pbColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.DrawEllipse(p, new Rectangle(1, 1, 20, 20));
        }

        #region Button Close / Minimize        

        private void pbClose_Click(object sender, EventArgs e)
        {
            if (frmOld != null)
                frmOld.ShowColorChangerUI(ui, true);
            else
            {
                accountIndex = -1;
                this.Visible = false;
            }
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            if (!frm.UseDarkmode)
                pbClose.Image = Properties.Resources.ic_close_white_24dp;

            pbClose.BackColor = Color.Crimson;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            if (!frm.UseDarkmode)
                pbClose.Image = Properties.Resources.ic_close_black_24dp;

            pbClose.BackColor = this.BackColor;
        }

        #endregion

        private void flow_Paint(object sender, PaintEventArgs e)
        {
            if (pbDrawnBorder == null) return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.DrawEllipse(p, new Rectangle(pbDrawnBorder.Left - 1, pbDrawnBorder.Top - 1, pbDrawnBorder.Width + 1, pbDrawnBorder.Height + 1));
        }
    }
}
