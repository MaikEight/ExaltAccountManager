using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Statistics
{
    public partial class UIStatsLeft : UserControl
    {
        UIAccountViewer viewer;
        CharacterStats stats;

        List<Image> normalPots = new List<Image>()
        {
            Properties.Resources.LifePot,
            Properties.Resources.ManaPot,
            Properties.Resources.AtkPot,
            Properties.Resources.DefPot,
            Properties.Resources.SpdPot,
            Properties.Resources.DexPot,
            Properties.Resources.VitPot,
            Properties.Resources.WisPot,
        };

        List<Image> gPots = new List<Image>()
        {
            Properties.Resources.GLifePot,
            Properties.Resources.GManaPot,
            Properties.Resources.GAtkPot,
            Properties.Resources.GDefPot,
            Properties.Resources.GSpdPot,
            Properties.Resources.GDexPot,
            Properties.Resources.GVitPot,
            Properties.Resources.GWisPot,
        };

        int currentCycleIndex = 0;

        int[] pointsTomax = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        bool[] isSecond = new bool[8] { false, false, false, false, false, false, false, false };

        public UIStatsLeft(UIAccountViewer _viewer, CharacterStats _stats)
        {
            InitializeComponent();
            viewer = _viewer;
            stats = _stats;

            LoadUI();
            ApplyTheme(viewer.GetDarkmode());

            timerTogglePots.Start();
        }

        public void ApplyTheme(bool isDarkmode)
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (isDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            ApplyTheme(isDarkmode, def, second, third, font);
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            this.Visible = false;
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;

            this.ForeColor = font;

            pHeadline.BackColor = third;

            for (int i = 0; i < isSecond.Length; i++)
            {
                if (isSecond[i])
                {
                    switch (i)
                    {
                        case 0:
                            pLife.BackColor = second;
                            break;
                        case 1:
                            pMana.BackColor = second;
                            break;
                        case 2:
                            pAtk.BackColor = second;
                            break;
                        case 3:
                            pDef.BackColor = second;
                            break;
                        case 4:
                            pSpd.BackColor = second;
                            break;
                        case 5:
                            pDex.BackColor = second;
                            break;
                        case 6:
                            pVit.BackColor = second;
                            break;
                        case 7:
                            pWis.BackColor = second;
                            break;
                        default:
                            break;
                    }
                }
            }

            pbArrowLife.Image =
            pbArrowMana.Image =
            pbArrowAtk.Image =
            pbArrowDef.Image =
            pbArrowSpd.Image =
            pbArrowDex.Image =
            pbArrowVit.Image =
            pbArrowWis.Image = (isDarkmode) ? Properties.Resources.baseline_arrow_right_alt_white_24 : Properties.Resources.baseline_arrow_right_alt_black_24;

            //lLevel.BackColor = lCurrentLevel.BackColor =
            //lAlive.BackColor = lAliveFame.BackColor =
            //lClassName.BackColor = pbClass.BackColor = lHP.BackColor = lMP.BackColor = lATK.BackColor = lDEF.BackColor = lSPD.BackColor = lDEX.BackColor = lVIT.BackColor = lWIS.BackColor =
            //lExtra.BackColor = lBackpack.BackColor = lHasBackpack.BackColor = lBelt.BackColor = lHasAdventurersBelt.BackColor = pbBackpack.BackColor = pbBelt.BackColor =
            //lMaxedStats.BackColor = lXof8.BackColor = isDarkmode ? Color.FromArgb(45, 20, 20, 20) : def;

            //foreach (Panel p in this.Controls.OfType<Panel>())
            //{
            //    foreach (MaterialPanel ui in p.Controls.OfType<MaterialPanel>())
            //        ui.ApplyTheme(isDarkmode);
            //    //p.BackColor = bgColor;
            //    //foreach (Label l in p.Controls.OfType<Label>())
            //    //    l.BackColor = bgColor;
            //    //foreach (PictureBox pb in p.Controls.OfType<PictureBox>())
            //    //    pb.BackColor = bgColor;
            //}
            //foreach (MaterialPanel ui in this.Controls.OfType<MaterialPanel>())
            //    ui.ApplyTheme(isDarkmode);
            //foreach (MaterialTextPanel ui in this.Controls.OfType<MaterialTextPanel>())
            //    ui.ApplyTheme(isDarkmode);
            //foreach (MaterialSimpelTextPanel ui in this.Controls.OfType<MaterialSimpelTextPanel>())
            //    ui.ApplyTheme(isDarkmode);
            //foreach (MaterialTopAccount ui in this.Controls.OfType<MaterialTopAccount>())
            //    ui.ApplyTheme(isDarkmode);
            //foreach (MaterialRadarChars ui in this.Controls.OfType<MaterialRadarChars>())
            //    ui.ApplyTheme(isDarkmode);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
            this.Visible = true;
        }

        private void LoadUI()
        {
            if (stats != null)
            {
                pointsTomax[0] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].maxHP - stats.maxHP);
                pointsTomax[1] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].maxMP - stats.maxMP);
                pointsTomax[2] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].atk - stats.atk);
                pointsTomax[3] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].def - stats.def);
                pointsTomax[4] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].spd - stats.spd);
                pointsTomax[5] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].dex - stats.dex);
                pointsTomax[6] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].vit - stats.vit);
                pointsTomax[7] = (CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].wis - stats.wis);

                bool nextSecond = false;

                for (int i = 0; i < pointsTomax.Length; i++)
                {
                    if (pointsTomax[i] > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                lLifeCurrent.Text = stats.maxHP.ToString();
                                lLifeMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].maxHP.ToString();
                                break;
                            case 1:
                                lManaCurrent.Text = stats.maxMP.ToString();
                                lManaMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].maxMP.ToString();
                                break;
                            case 2:
                                lAtkCurrent.Text = stats.atk.ToString();
                                lAtkMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].atk.ToString();
                                break;
                            case 3:
                                lDefCurrent.Text = stats.def.ToString();
                                lDefMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].def.ToString();
                                break;
                            case 4:
                                lSpdCurrent.Text = stats.spd.ToString();
                                lSpdMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].spd.ToString();
                                break;
                            case 5:
                                lDexCurrent.Text = stats.dex.ToString();
                                lDexMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].dex.ToString();
                                break;
                            case 6:
                                lVitCurrent.Text = stats.vit.ToString();
                                lVitMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].vit.ToString();
                                break;
                            case 7:
                                lWisCurrent.Text = stats.wis.ToString();
                                lWisMax.Text = CharacterClassesUtil.dicCharClassToMaxStats[stats.charClass].wis.ToString();
                                break;
                            default:
                                break;
                        }
                        isSecond[i] = nextSecond;
                        nextSecond = !nextSecond;
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                pLife.Visible = false;
                                break;
                            case 1:
                                pMana.Visible = false;
                                break;
                            case 2:
                                pAtk.Visible = false;
                                break;
                            case 3:
                                pDef.Visible = false;
                                break;
                            case 4:
                                pSpd.Visible = false;
                                break;
                            case 5:
                                pDex.Visible = false;
                                break;
                            case 6:
                                pVit.Visible = false;
                                break;
                            case 7:
                                pWis.Visible = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
                LoadUIValues();
            }
        }

        private void LoadUIValues()
        {
            if (pLife.Visible)
                lLifeAmount.Text = $"{Math.Ceiling(((double)pointsTomax[0]) / (!togglePots.Checked ? 5d : 10d))}x";
            if (pMana.Visible)
                lManaAmount.Text = $"{Math.Ceiling(((double)pointsTomax[1]) / (!togglePots.Checked ? 5d : 10d))}x";
            if (pAtk.Visible)
                lAtkAmount.Text = $"{Math.Ceiling(((double)pointsTomax[2]) / (!togglePots.Checked ? 1d : 2d))}x";
            if (pDef.Visible)
                lDefAmount.Text = $"{Math.Ceiling(((double)pointsTomax[3]) / (!togglePots.Checked ? 1d : 2d))}x";
            if (pSpd.Visible)
                lSpdAmount.Text = $"{Math.Ceiling(((double)pointsTomax[4]) / (!togglePots.Checked ? 1d : 2d))}x";
            if (pDex.Visible)
                lDexAmount.Text = $"{Math.Ceiling(((double)pointsTomax[5]) / (!togglePots.Checked ? 1d : 2d))}x";
            if (pVit.Visible)
                lVitAmount.Text = $"{Math.Ceiling(((double)pointsTomax[6]) / (!togglePots.Checked ? 1d : 2d))}x";
            if (pWis.Visible)
                lWisAmount.Text = $"{Math.Ceiling(((double)pointsTomax[7]) / (!togglePots.Checked ? 1d : 2d))}x";
        }

        private void timerTogglePots_Tick(object sender, EventArgs e)
        {
            currentCycleIndex++;
            if (currentCycleIndex >= normalPots.Count)
                currentCycleIndex = 0;

            pbNormalPot.Image = normalPots[currentCycleIndex];
            pbGPot.Image = gPots[currentCycleIndex];
        }

        private void togglePots_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            LoadUIValues();

            pbLife.Image = togglePots.Checked ? gPots[0] : normalPots[0];
            pbMana.Image = togglePots.Checked ? gPots[1] : normalPots[1];
            pbAtk.Image = togglePots.Checked ? gPots[2] : normalPots[2];
            pbDef.Image = togglePots.Checked ? gPots[3] : normalPots[3];
            pbSpd.Image = togglePots.Checked ? gPots[4] : normalPots[4];
            pbDex.Image = togglePots.Checked ? gPots[5] : normalPots[5];
            pbVit.Image = togglePots.Checked ? gPots[6] : normalPots[6];
            pbWis.Image = togglePots.Checked ? gPots[7] : normalPots[7];
        }
    }
}
