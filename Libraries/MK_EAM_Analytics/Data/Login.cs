
namespace MK_EAM_Analytics.Data
{
    [System.Serializable]
    public class Login
    {
        public System.Guid SessionId { get; set; }
        public string EmailHash { get; set; }
        public string ServerName { get; set; }
        public System.DateTime Time { get; set; }
    }
}
