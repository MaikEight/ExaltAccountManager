using MK_EAM_Analytics.Data;
using System;
using System.Drawing;

namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class CaptchaResultRequest : AnalyticsData
    {
        public DateTime StartTime { get; set; }
        public CaptchaResult CaptchaResult { get; set; }
        public byte[] qimg { get; set; }
        public byte[] img { get; set; }
        public PointF[] data { get; set; }
    }
}
