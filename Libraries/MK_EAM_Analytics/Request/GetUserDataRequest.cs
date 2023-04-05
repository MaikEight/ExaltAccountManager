
namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public class GetUserDataRequest : AnalyticsData
    {
        public string ClientIdHash { get; set; }
    }
}
