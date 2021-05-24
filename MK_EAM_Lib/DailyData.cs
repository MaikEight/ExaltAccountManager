using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class DailyData
    {
        public string mail;
        public DateTime lastLogin;
        /// <summary>
        /// 1 = success; 2 = error;
        /// </summary>
        public int lastState;
    }
}
