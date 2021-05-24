using System;
using System.Collections.Generic;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class DailyLogins
    {
        public List<DailyData> logins = new List<DailyData>();
        public DateTime lastUpdate;
        public bool isDone = false;
    }
}
