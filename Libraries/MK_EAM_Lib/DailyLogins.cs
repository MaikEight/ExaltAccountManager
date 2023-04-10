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
    public sealed class DailyLogins
    {
        public List<DailyData> DailyDatas { get; set; } = new List<DailyData>();
        public bool RefreshAll = false;
    }


    [System.Serializable]
    public sealed class DailyData
    {
        private readonly Bitmap manualImage = new Bitmap(new MemoryStream(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 24, 0, 0, 0, 24, 8, 6, 0, 0, 0, 224, 119, 61, 248, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 1, 216, 73, 68, 65, 84, 72, 75, 221, 149, 61, 79, 21, 65, 24, 70, 47, 130, 64, 2, 118, 118, 52, 118, 242, 245, 39, 44, 8, 22, 26, 9, 104, 48, 145, 158, 4, 18, 2, 198, 63, 64, 66, 40, 76, 32, 252, 4, 44, 76, 44, 180, 161, 165, 161, 167, 68, 197, 24, 196, 218, 198, 2, 72, 80, 32, 140, 231, 153, 125, 118, 247, 94, 179, 179, 55, 16, 27, 60, 201, 201, 236, 60, 239, 59, 123, 63, 118, 118, 183, 241, 127, 19, 66, 232, 193, 25, 252, 128, 223, 240, 167, 213, 241, 123, 124, 129, 61, 110, 191, 26, 44, 124, 138, 223, 177, 29, 135, 56, 233, 101, 237, 161, 185, 19, 55, 180, 210, 124, 196, 37, 28, 193, 187, 118, 20, 95, 226, 39, 204, 89, 199, 78, 159, 38, 13, 77, 107, 177, 61, 132, 223, 56, 143, 201, 69, 170, 225, 2, 158, 161, 120, 237, 82, 53, 52, 76, 100, 125, 241, 228, 99, 142, 11, 84, 104, 30, 115, 152, 142, 163, 214, 92, 226, 99, 199, 173, 80, 232, 198, 3, 20, 179, 142, 35, 10, 170, 198, 102, 136, 230, 148, 195, 87, 188, 237, 184, 132, 240, 121, 44, 135, 176, 135, 183, 28, 71, 20, 214, 141, 130, 67, 253, 93, 249, 53, 121, 230, 184, 132, 240, 93, 86, 11, 11, 142, 10, 20, 214, 141, 57, 76, 181, 25, 196, 91, 71, 37, 132, 250, 105, 98, 200, 81, 129, 194, 186, 49, 135, 169, 118, 151, 216, 119, 84, 66, 120, 156, 213, 66, 191, 163, 2, 231, 149, 184, 37, 194, 244, 78, 150, 134, 35, 71, 37, 132, 39, 89, 45, 244, 57, 42, 112, 94, 137, 91, 34, 76, 107, 63, 160, 229, 47, 98, 220, 194, 207, 120, 95, 97, 138, 184, 216, 48, 213, 205, 40, 190, 56, 42, 33, 108, 185, 200, 140, 247, 80, 143, 138, 31, 10, 83, 196, 197, 134, 105, 237, 69, 206, 183, 169, 30, 13, 241, 238, 101, 204, 63, 36, 197, 175, 184, 24, 56, 238, 194, 253, 152, 38, 182, 105, 243, 141, 182, 232, 88, 249, 32, 234, 46, 173, 98, 219, 109, 234, 123, 149, 69, 137, 27, 77, 80, 120, 130, 186, 221, 245, 108, 25, 119, 172, 252, 1, 238, 186, 38, 46, 112, 7, 7, 92, 127, 136, 231, 168, 250, 163, 184, 40, 5, 13, 43, 40, 174, 243, 176, 91, 118, 41, 13, 77, 29, 184, 26, 219, 51, 242, 199, 245, 48, 246, 90, 237, 150, 191, 31, 215, 250, 98, 29, 62, 77, 123, 104, 158, 194, 127, 255, 194, 105, 134, 133, 122, 101, 106, 119, 189, 65, 125, 219, 83, 171, 227, 77, 156, 198, 235, 189, 50, 111, 8, 141, 198, 31, 228, 138, 174, 145, 100, 149, 7, 169, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 }));
        private readonly Bitmap automaticImage = new Bitmap(new MemoryStream(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 24, 0, 0, 0, 24, 8, 6, 0, 0, 0, 224, 119, 61, 248, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 127, 73, 68, 65, 84, 72, 75, 237, 149, 65, 10, 128, 48, 12, 4, 251, 62, 253, 255, 81, 145, 162, 239, 208, 180, 206, 45, 4, 105, 99, 17, 36, 3, 57, 148, 221, 205, 150, 94, 154, 62, 227, 4, 142, 38, 216, 30, 125, 10, 114, 239, 22, 136, 111, 150, 89, 107, 162, 143, 69, 102, 98, 157, 70, 196, 92, 109, 62, 54, 214, 105, 48, 180, 191, 39, 16, 183, 243, 232, 227, 11, 188, 176, 78, 131, 238, 134, 117, 26, 244, 241, 79, 196, 177, 25, 226, 81, 96, 67, 60, 10, 108, 136, 255, 188, 96, 191, 45, 46, 50, 235, 52, 34, 150, 15, 231, 168, 182, 62, 202, 5, 237, 15, 39, 208, 164, 116, 1, 135, 180, 137, 209, 75, 78, 65, 148, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 }));

        public DateTime Date { get; set; }
        [DisplayName("Start type")]
        public Bitmap ManualImage
        {
            get => ManualStart ? manualImage : automaticImage;
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
    public sealed class DailyAccountData
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
