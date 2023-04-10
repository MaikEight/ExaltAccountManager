
namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class DeleteUserDataRequest : AnalyticsData
    {
        public string ClientIdHash { get; set; }
    }
}
