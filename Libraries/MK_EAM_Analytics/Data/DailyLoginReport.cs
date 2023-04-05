
namespace MK_EAM_Analytics.Data
{
    [System.Serializable]
    public class DailyLoginReport
    {
        public System.Guid SessionId { get; set; }
        public int AmountOfAccounts { get; set; }
        public int Successfully { get; set; }
        public int Failed { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
    }
}
