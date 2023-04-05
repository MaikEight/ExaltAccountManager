using System;
using System.Collections.Generic;

namespace MK_EAM_Lib
{
    [Obsolete]
    [System.Serializable]
    public class DailyLoginsOLD
    {
        public List<DailyDataOLD> logins = new List<DailyDataOLD>();
        public DateTime lastUpdate;
        public bool isDone = false;
    }
}
