using MK_EAM_Lib;
using System;
using System.Windows.Forms;

namespace MK_EAM_Captcha_Solver_UI_Lib
{
    public static class CaptchaSolverUiUtils
    {                           
        /// <summary>
        /// Show the captcha solver form.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="owner"></param>
        /// <param name="useDarkmode"></param>
        /// <param name="LogEvent"></param>
        /// <param name="logSender"></param>
        /// <param name="accountStatsPath"></param>
        /// <param name="itemsSaveFilePath"></param>
        /// <param name="uniqueID"></param>
        /// <param name="getName"></param>
        /// <param name="doAsyncRequest"></param>
        /// <param name="callback"></param>
        /// <returns>True if the captcha solved successfully</returns>
        public static bool Show(AccountInfo info, Form owner, bool useDarkmode, Action<LogData> LogEvent, string logSender, string accountStatsPath, string itemsSaveFilePath, string uniqueID, bool getName = true, bool doAsyncRequest = true, Action<AccountInfo> callback = null)
        {
            FrmCaptchaSolver frmCaptchaSolver = new FrmCaptchaSolver(info, useDarkmode);
            frmCaptchaSolver.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = frmCaptchaSolver.ShowDialog(owner);
            
            switch (result)
            {
                case DialogResult.OK:
                    info.PerformWebrequest(owner, LogEvent, logSender, accountStatsPath, itemsSaveFilePath, uniqueID, getName, doAsyncRequest, callback);
                    return true;
                case DialogResult.Cancel:
                    return false;
                case DialogResult.Abort:
                    return false;
                default:
                    return false;
            }
        }
    }
}
