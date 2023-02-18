
namespace MK_EAM_Analytics.Data
{
    [System.Serializable]
    public class Session
    {
        public System.Guid SessionId { get; set; }
        public System.Guid UserId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public bool HasEnded { get; set; }
    }
}
