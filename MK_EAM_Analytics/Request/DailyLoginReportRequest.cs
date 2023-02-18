
namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class DailyLoginReportRequest : AnalyticsData
    {
        public int AmountOfAccounts { get; set; }
        public int AmountOfSuccessfulLogins { get; set; }
        public int AmountOfFailedLogins { get; set; }
        public System.TimeSpan Duration { get; set; }
    }
}
