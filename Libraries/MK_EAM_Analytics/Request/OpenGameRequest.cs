
namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class OpenGameRequest : AnalyticsData
    {
        public string HashOfMail { get; set; }
        public string ServerName { get; set; }
    }
}
