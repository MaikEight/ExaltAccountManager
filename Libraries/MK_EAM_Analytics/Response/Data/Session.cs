
namespace MK_EAM_Analytics.Response.Data
{
    [System.Serializable]
    public class Session
    {
        public MK_EAM_Analytics.Data.Session SessionData { get; set; }
        public System.Collections.Generic.List<MK_EAM_Analytics.Data.Login> Logins { get; set; }
        public System.Collections.Generic.List<MK_EAM_Analytics.Data.ModuleUse> ModuleUses { get; set; }
        public MK_EAM_Analytics.Data.DailyLoginReport DailyLoginReport { get; set; } = null;
    }
}
