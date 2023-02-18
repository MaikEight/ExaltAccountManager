
namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class OpenModuleRequest : AnalyticsData
    {
        public ModuleType ModuleType { get; set; }
    }

    public enum ModuleType
    {
        Statistics = 0,
        PingChecker = 1,
        VaultPeeker = 2,
        DailyLogin = 3
    }
}
