using System;

namespace MK_EAM_General_Services_Lib.News.Requests
{
    [System.Serializable]
    public class GetNewsRequest
    {
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public int Amount { get; set; } = 5;
    }
}
