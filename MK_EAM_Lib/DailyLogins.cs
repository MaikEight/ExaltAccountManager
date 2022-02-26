using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class DailyLogins
    {
        public List<DailyData> DailyDatas { get; set; } = new List<DailyData>();
        public bool RefreshAll = false;
    }

    [System.Serializable]
    public class DailyData
    {
        public DateTime Date { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false)]
        public DateTime StartTime
        {
            get 
            {
                if(AccountData.Count > 0)
                    return AccountData.Min(a => a.StartTime);
                return Date;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false)]
        public DateTime EndTime
        {
            get 
            {
                if(AccountData.Count > 0)
                    return AccountData.Max(a => a.EndTime);
                return Date;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false)]
        public int DurationInSeconds
        {
            get
            {
                if (AccountData.Count > 0)
                    return (int)(EndTime - StartTime).TotalSeconds;
                return 0;
            }
        }
        [Browsable(false)]
        public List<DailyAccountData> AccountData { get; set;} = new List<DailyAccountData>();
        
        [Newtonsoft.Json.JsonIgnore]
        public int Success
        {
            get
            {
                if (AccountData.Count == 0)
                    return 0;
                return AccountData.Where(a => a.Success).Count();
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int Failed
        {
            get
            {
                if (AccountData.Count == 0)
                    return 0;
                return AccountData.Where(a => !a.Success).Count();
            }
        }
        [Browsable(false)]
        public int PlannedLogins { get; set; } = 0;
    }

    [System.Serializable]
    public class DailyAccountData
    {
        public string Email { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [DisplayName("Start")]
        public string StartTimeString { get => StartTime.ToString("HH:mm"); }
        [Browsable(false)]
        public DateTime StartTime { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [DisplayName("End")]
        public string EndTimeString { get => EndTime.ToString("HH:mm"); }
        [Browsable(false)]
        public DateTime EndTime { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [DisplayName("Duration")]
        public string DurationString 
        {
            get 
            {
                if (EndTime.TimeOfDay.Ticks < StartTime.TimeOfDay.Ticks)
                    return "N/A";
                return $"{((int)(EndTime - StartTime).TotalSeconds)}s";
            } 
        }
        public bool Success { get; set; } = false;
    }
}
