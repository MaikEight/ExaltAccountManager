using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
        private readonly Bitmap manualImage = new Bitmap(new MemoryStream(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 24, 0, 0, 0, 24, 8, 6, 0, 0, 0, 224, 119, 61, 248, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 1, 33, 73, 68, 65, 84, 72, 75, 237, 146, 79, 43, 5, 113, 20, 134, 39, 139, 123, 101, 75, 22, 247, 186, 68, 22, 62, 135, 53, 37, 159, 131, 248, 16, 190, 13, 87, 22, 40, 95, 128, 53, 27, 127, 138, 40, 86, 34, 151, 133, 238, 234, 120, 206, 156, 119, 40, 51, 114, 155, 153, 229, 60, 245, 212, 244, 158, 51, 111, 211, 239, 55, 73, 67, 45, 152, 217, 44, 246, 241, 89, 238, 227, 162, 198, 213, 160, 200, 203, 95, 241, 55, 158, 245, 180, 86, 30, 74, 252, 203, 157, 67, 156, 199, 5, 60, 242, 0, 118, 181, 86, 30, 74, 222, 163, 235, 231, 107, 121, 158, 139, 200, 222, 20, 149, 135, 146, 143, 232, 178, 174, 34, 207, 122, 17, 217, 64, 81, 121, 40, 57, 136, 174, 244, 136, 58, 216, 197, 90, 143, 104, 9, 139, 46, 249, 5, 171, 95, 178, 67, 81, 246, 155, 250, 125, 12, 112, 15, 235, 41, 111, 248, 19, 206, 184, 133, 203, 184, 131, 211, 138, 191, 241, 76, 51, 223, 105, 41, 254, 31, 150, 219, 184, 137, 79, 152, 113, 139, 107, 56, 33, 215, 241, 14, 51, 30, 113, 3, 219, 170, 41, 134, 133, 25, 188, 192, 140, 123, 188, 138, 199, 66, 46, 241, 33, 30, 83, 206, 177, 163, 186, 60, 12, 79, 210, 53, 179, 107, 92, 193, 49, 28, 199, 109, 60, 195, 79, 121, 138, 91, 232, 51, 223, 89, 197, 27, 116, 142, 85, 151, 135, 225, 48, 118, 108, 82, 209, 200, 240, 206, 84, 188, 106, 67, 69, 121, 180, 80, 25, 213, 229, 209, 188, 50, 170, 107, 24, 133, 36, 249, 2, 140, 184, 73, 199, 220, 63, 10, 87, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 }));

        public DateTime Date { get; set; }
        [DisplayName("Manually")]
        public Bitmap ManualImage
        {
            get
            {
                if (!ManualStart)
                {
                    return manualImage;
                }
                return new System.Drawing.Bitmap(15, 15);
            }
        }
        [Browsable(false)]
        public bool ManualStart { get; set; } = false;
        [Newtonsoft.Json.JsonIgnore]
        [Browsable(false)]
        public DateTime StartTime
        {
            get
            {
                if (AccountData.Count > 0)
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
                if (AccountData.Count > 0)
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
        public List<DailyAccountData> AccountData { get; set; } = new List<DailyAccountData>();

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
