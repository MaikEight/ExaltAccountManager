
namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class TokenRefreshRequest : AnalyticsData
    {
        public string HashOfMail { get; set; }
        public bool IsSuccess { get; set; }
    }
}
