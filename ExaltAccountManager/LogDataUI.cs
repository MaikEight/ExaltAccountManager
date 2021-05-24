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

namespace ExaltAccountManager
{
    public partial class LogDataUI : UserControl
    {
        public LogData data;
        private FrmLogViewer frmLogViewer;
        private bool showDate = true;
        public LogDataUI(FrmLogViewer _frmLogViewer, LogData _data, bool _showDate = true, bool isHeadLine = false)
        {
            InitializeComponent();
            frmLogViewer = _frmLogViewer;
            data = _data;
            showDate = _showDate;

            if (!isHeadLine)
                ApplyDataToUI();
            else
            {
                lDate.Text = "Date";
                lTime.Text = "Time";
                lSender.Text = "Tool";
                lMessage.Text = "Log message";
                lEventType.Text = "Event Type";
                pEventType.Width += ((frmLogViewer.Width - 2) - this.Width);
                if (frmLogViewer.isDarkmode)
                {
                    this.BackColor = Color.FromArgb(0,0,0);                    
                }
                else
                {
                    this.BackColor = Color.FromArgb(230, 230, 230);
                }
            }
        }

        private void ApplyDataToUI()
        {
            if (data != null)
            {
                if (showDate)
                    lDate.Text = data.time.ToShortDateString();
                else
                    lDate.Visible = false;

                lTime.Text = data.time.ToShortTimeString();
                lSender.Text = data.sender;
                lMessage.Text = data.message;
                lEventType.Text = data.eventType.ToString();
            }
        }
    }
}
