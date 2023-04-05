
namespace MK_EAM_Analytics.Data
{
    [System.Serializable]
    public class User
    {
        public System.Guid Id { get; set; }
        public string clientIdHash { get; set; }
        public bool LlamaFound { get; set; }
        public int AmountOfAccounts { get; set; }
        public System.DateTime LastSeen { get; set; }
    }
}
