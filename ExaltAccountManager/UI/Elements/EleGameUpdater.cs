using MK_EAM_Lib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleGameUpdater : UserControl
    {
        private FrmMain frm;
        private GameUpdaterUIState UIState
        {
            get => uiStateValue;
            set
            {
                uiStateValue = value;

                switch (uiStateValue)
                {
                    case GameUpdaterUIState.NotSet:
                        {

                        }
                        break;
                    case GameUpdaterUIState.NoUpdate:
                        {
                            this.Height = btnCheckForUpdate.Bottom + 17;

                            pbClose.Enabled =
                            pbClose.Visible =
                            btnCheckForUpdate.Visible =
                            btnCheckForUpdate.Enabled = true;
                            btnCheckForUpdate.Text = "Check for update";
                            lStatus.Text = "Up to date";
                            pbStatus.Image = frm.UseDarkmode ? Properties.Resources.ic_beenhere_white_36dp : Properties.Resources.ic_beenhere_black_36dp;

                            progressbar.Visible =
                            btnPerformUpdate.Enabled =
                            btnPerformUpdate.Visible =
                            btnDone.Enabled =
                            btnDone.Visible = false;

                        }
                        break;
                    case GameUpdaterUIState.UpdateAvailable:
                        {
                            this.Height = btnPerformUpdate.Bottom + 17;

                            pbClose.Enabled =
                            pbClose.Visible =
                            btnCheckForUpdate.Enabled =
                            btnCheckForUpdate.Visible =
                            btnPerformUpdate.Enabled =
                            btnPerformUpdate.Visible = true;
                            btnCheckForUpdate.Text = "Check again for update";
                            lStatus.Text = "Update available";
                            pbStatus.Image = frm.UseDarkmode ? Properties.Resources.ic_new_releases_white_36dp : Properties.Resources.ic_new_releases_black_36dp;

                            progressbar.Visible =
                            btnDone.Enabled =
                            btnDone.Visible = false;
                        }
                        break;
                    case GameUpdaterUIState.UpdateInProgress:
                        {
                            frm.HidePbShowDiscordUser();
                            this.Height = progressbar.Bottom + 17;

                            pbClose.Enabled =
                            pbClose.Visible =
                            btnCheckForUpdate.Enabled =
                            btnCheckForUpdate.Visible =
                            btnDone.Enabled =
                            btnDone.Visible =
                            btnPerformUpdate.Enabled =
                            btnPerformUpdate.Visible = false;
                            lStatus.Text = "Update in progress";

                            progressbar.Value = 0;
                            progressbar.Maximum = 100;
                            progressbar.Visible = true;
                        }
                        break;
                    case GameUpdaterUIState.UpdateDone:
                        {
                            frm.ShowPbShowDiscordUser();
                            this.Height = btnDone.Bottom + 17;

                            btnCheckForUpdate.Enabled =
                            btnCheckForUpdate.Visible =
                            btnPerformUpdate.Enabled =
                            btnPerformUpdate.Visible = false;
                            lStatus.Text = "Update finished";
                            pbStatus.Image = frm.UseDarkmode ? Properties.Resources.ic_beenhere_white_36dp : Properties.Resources.ic_beenhere_black_36dp;

                            pbClose.Enabled =
                            pbClose.Visible =
                            progressbar.Visible =
                            btnDone.Enabled =
                            btnDone.Visible = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private GameUpdaterUIState uiStateValue = GameUpdaterUIState.NotSet;
        private bool updateInProgress = false;

        public EleGameUpdater(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            pbCheck.Anchor = pbStatus.Anchor = lHeadline.Anchor;

            this.Disposed += OnDisposed;
            GameUpdater.Instance.OnUpdateRequired += OnUpdateRequiredInvoker;
            GameUpdater.Instance.OnUpdateProgressChanged += OnUpdateProgressChangedInvoker;
            GameUpdater.Instance.OnUpdateFinished += OnUpdateFinishedInvoker;

            try
            {
                if (System.IO.File.Exists(frm.lastUpdateCheckPath))
                    lLastCheck.Text = new DateTime(long.Parse(System.IO.File.ReadAllLines(frm.lastUpdateCheckPath)[0])).ToString("dd.MM.yyyy HH:mm");
                else
                    lLastCheck.Text = "Never";
            }
            catch { }

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
        }

        private void EleGameUpdater_Load(object sender, EventArgs e)
        {
            UpdateAvailable(GameUpdater.Instance.UpdateRequired);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            frm.ThemeChanged -= ApplyTheme;
            try { GameUpdater.Instance.OnUpdateRequired -= OnUpdateRequiredInvoker; } catch { }
            try { GameUpdater.Instance.OnUpdateProgressChanged -= OnUpdateProgressChangedInvoker; } catch { }
            try { GameUpdater.Instance.OnUpdateFinished -= OnUpdateFinishedInvoker; } catch { }
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
            progressbar.ProgressBackColor = def;
            progressbar.ForeColor = frm.UseDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);

            pbCheck.Image = pbHeader.Image = frm.UseDarkmode ? Properties.Resources.update_left_rotation_white_24px : Properties.Resources.update_left_rotation_24px;
            pbStatus.Image = GameUpdater.Instance.UpdateRequired ? (frm.UseDarkmode ? Properties.Resources.ic_new_releases_white_36dp : Properties.Resources.ic_new_releases_black_36dp) : (frm.UseDarkmode ? Properties.Resources.ic_beenhere_white_36dp : Properties.Resources.ic_beenhere_black_36dp);
        }

        private void OnUpdateRequiredInvoker(object sender, EventArgs e) { if (!updateInProgress) OnUpdateRequired(); }

        private bool OnUpdateRequired()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)OnUpdateRequired);

            lLastCheck.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            UpdateAvailable(GameUpdater.Instance.UpdateRequired);
            frm.ShowSnackbar(GameUpdater.Instance.UpdateRequired ? "Game-update available." : "No update available.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 10000);

            return false;
        }

        private void OnUpdateProgressChangedInvoker(object sender, EventArgs e) => OnUpdateProgressChanged();

        private bool OnUpdateProgressChanged()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)OnUpdateProgressChanged);

            progressbar.Value = Math.Min(GameUpdater.Instance.UpdateProgress, progressbar.Maximum);            

            return false;
        }

        private void OnUpdateFinishedInvoker(object sender, EventArgs e) => OnUpdateFinished();
        private bool OnUpdateFinished()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)OnUpdateFinished);

            UIState = GameUpdaterUIState.UpdateDone;
            progressbar.Value = 100;

            frm.ShowSnackbar("Game-Update successfull.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
            frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Updater", MK_EAM_Lib.LogEventType.GameUpdate, "Game-Update successfull."));
            return false;
        }

        private void UpdateAvailable(bool updateAvailable)
        {            
            UIState = updateAvailable ? GameUpdaterUIState.UpdateAvailable : GameUpdaterUIState.NoUpdate;
        }

        private void btnCheckForUpdate_Click(object sender, EventArgs e)
        {
            btnCheckForUpdate.Enabled =
            btnPerformUpdate.Enabled =
            btnDone.Enabled = false;

            if (!GameUpdater.Instance.CheckForUpdate())
            {
                btnCheckForUpdate.Enabled =
                btnPerformUpdate.Enabled = true;
            }
        }

        private void btnPerformUpdate_Click(object sender, EventArgs e)
        {
            UIState = GameUpdaterUIState.UpdateInProgress;
            GameUpdater.Instance.OnUpdateRequired -= OnUpdateRequiredInvoker;

            Application.DoEvents();
            updateInProgress = true;

            if (!GameUpdater.Instance.PerformGameUpdate())
            {
                UIState = GameUpdaterUIState.UpdateAvailable;
                updateInProgress = false;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            UIState = GameUpdaterUIState.NoUpdate;
            GameUpdater.Instance.OnUpdateRequired += OnUpdateRequiredInvoker;
            updateInProgress = false;
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

        private enum GameUpdaterUIState
        {
            NotSet,
            NoUpdate,
            UpdateAvailable,
            UpdateInProgress,
            UpdateDone
        }
    }
}
